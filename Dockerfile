FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY src/DockerGen/DockerGen.csproj .
RUN dotnet restore "DockerGen.csproj"
COPY src/DockerGen/ .
RUN dotnet build "DockerGen.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerGen.csproj" -c Release -o /publish
RUN openssl req -x509 -nodes -days 365 \
    -subj  "/C=NO/ST=NA/O=Frode Hus/CN=dockergen.frodehus.dev" \
     -newkey rsa:2048 -keyout /publish/nginx-selfsigned.key \
     -out /publish/nginx-selfsigned.crt;

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /publish/nginx-selfsigned.crt /etc/ssl/certs/nginx-selfsigned.crt
COPY --from=publish /publish/nginx-selfsigned.key /etc/ssl/private/nginx-selfsigned.key 
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf