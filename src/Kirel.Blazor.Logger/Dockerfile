# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Kirel.Blazor.Logger/Kirel.Blazor.Logger.csproj .
RUN dotnet restore Kirel.Blazor.Logger.csproj
COPY . .
RUN dotnet build "Kirel.Blazor.Logger/Kirel.Blazor.Logger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.Blazor.Logger/Kirel.Blazor.Logger.csproj" -c Release -o /app/publish

WORKDIR /app/publish
RUN ls -l

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY blazorLoggerNginx.conf /etc/nginx/nginx.conf
