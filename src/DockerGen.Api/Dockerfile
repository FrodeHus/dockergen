#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG CERTPASS=thisisnotit
WORKDIR /src
RUN dotnet dev-certs https -ep ./certificate.pfx -p $CERTPASS
COPY DockerGen.Shared/DockerGen.Shared.csproj DockerGen.Shared/
COPY DockerGen.Api/DockerGen.Api.csproj DockerGen.Api/
RUN dotnet restore "DockerGen.Api/DockerGen.Api.csproj"
COPY DockerGen.Shared/ DockerGen.Shared/
COPY DockerGen.Api/ DockerGen.Api/
WORKDIR /src/DockerGen.Api
RUN dotnet build "DockerGen.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerGen.Api.csproj" -c Release -o /app/publish

FROM base AS final
ARG CERTPASS=thisisnotit
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/DockerGen.Api/recipes recipes/
COPY --from=build /src/certificate.pfx .
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERTPASS
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENTRYPOINT ["dotnet", "DockerGen.Api.dll"]