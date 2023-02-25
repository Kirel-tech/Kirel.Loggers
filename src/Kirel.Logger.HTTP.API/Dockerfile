# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/Kirel.Logger.HTTP.API/Kirel.Logger.HTTP.API.csproj"
WORKDIR "/src/Kirel.Logger.HTTP.API"
RUN dotnet build "Kirel.Logger.HTTP.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.Logger.HTTP.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kirel.Logger.HTTP.API.dll"]
