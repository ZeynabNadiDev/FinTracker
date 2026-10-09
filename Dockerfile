FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the source code and project files
COPY . .

# Restore the API project and its referenced projects
RUN dotnet restore "Src/FinTracker.Api/FinTracker.Api.csproj"

# Publish the API project
RUN dotnet publish "Src/FinTracker.Api/FinTracker.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinTracker.Api.dll"]
