# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/Kirel.HttpLogger.API/Kirel.HttpLogger.API.csproj"
WORKDIR "/src/Kirel.HttpLogger.API"
RUN dotnet build "Kirel.HttpLogger.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.HttpLogger.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kirel.HttpLogger.API.dll"]
