# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Kirel.Logger.HTTP.Blazor.GUI/Kirel.Logger.HTTP.Blazor.GUI.csproj .
RUN dotnet restore Kirel.Logger.HTTP.Blazor.GUI.csproj
COPY . .
RUN dotnet build "Kirel.Logger.HTTP.Blazor.GUI/Kirel.Logger.HTTP.Blazor.GUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.Logger.HTTP.Blazor.GUI/Kirel.Logger.HTTP.Blazor.GUI.csproj" -c Release -o /app/publish

WORKDIR /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY docker/gui.nginx.conf /etc/nginx/nginx.conf