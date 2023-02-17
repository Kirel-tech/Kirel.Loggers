# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/Kirel.MessageLogger.API/Kirel.MessageLogger.API.csproj"
WORKDIR "/src/Kirel.MessageLogger.API"
RUN dotnet build "Kirel.MessageLogger.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.MessageLogger.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kirel.MessageLogger.API.dll"]