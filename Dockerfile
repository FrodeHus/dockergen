FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY src/DockerGen/DockerGen.csproj .
RUN dotnet restore "DockerGen.csproj"
COPY src/DockerGen/ .
RUN dotnet build "DockerGen.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerGen.csproj" -c Release -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
RUN openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /etc/ssl/private/nginx-selfsigned.key -out /etc/ssl/certs/nginx-selfsigned.crt
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf