using DockerGen.Container;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddContainerService();
builder.Services.AddHealthChecks();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new ContainerConverter());
});
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


app.UseCors(c =>
{
    c.AllowAnyMethod()
    .AllowAnyOrigin()
    .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
});
app.MapControllers();

app.Run();
