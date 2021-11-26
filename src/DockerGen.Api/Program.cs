using DockerGen.Api;
using DockerGen.Container;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    var redis = ConnectionMultiplexer.Connect("redis-cluster:6379");
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis, "DataProtectionKeys");

    builder.Services.AddStackExchangeRedisCache(o =>
    {
        o.Configuration = "redis-cluster:6379";
        o.InstanceName = "dockergen";
    });
}
builder.Services.Configure<Config>(builder.Configuration.GetSection("DockerGen"));
builder.Services.AddCors(o =>
{
    o.AddPolicy("allowOrigins", builder =>
    {
        builder
            .WithOrigins("https://dockergen.frodehus.dev", "https://localhost:5001")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Location")
            .AllowCredentials();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddContainerService(o => o.RecipePath = builder.Configuration.GetValue<string>("DockerGen:RecipePath"));
builder.Services.AddHealthChecks();
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new ContainerConverter()));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DockerGen.Api", Version = "v1" });
    c.MapType<ContainerImage>(() =>
    {
        var s = new OpenApiSchema
        {
            Example = new OpenApiObject
            {

            }
        };
        return s;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DockerGen.Api v1"));
}


app.UseCors("allowOrigins");

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapHealthChecks("/health"));
app.MapControllers();

app.Run();
