# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/FinTracker.Api/FinTracker.Api.csproj", "Src/FinTracker.Api/"]
COPY ["Src/Modules/Budget/Budget.Composition/Budget.Composition.csproj", "Src/Modules/Budget/Budget.Composition/"]
COPY ["Src/Modules/Budget/Budget.Application/Budget.Application.csproj", "Src/Modules/Budget/Budget.Application/"]
COPY ["Src/Modules/Budget/Budget.Infrastructure/Budget.Infrastructure.csproj", "Src/Modules/Budget/Budget.Infrastructure/"]
COPY ["Src/Modules/Budget/Budget.Domain/Budget.Domain.csproj", "Src/Modules/Budget/Budget.Domain/"]
COPY ["Src/Shared/FinTracker.SharedKernel/FinTracker.SharedKernel.csproj", "Src/Shared/FinTracker.SharedKernel/"]
COPY ["Src/Modules/Budget/Budget.Presentation/Budget.Presentation.csproj", "Src/Modules/Budget/Budget.Presentation/"]
COPY ["Src/Modules/Category/Category/Category.csproj", "Src/Modules/Category/Category/"]
COPY ["Src/Modules/Identity/Identity.Composition/Identity.Composition.csproj", "Src/Modules/Identity/Identity.Composition/"]
COPY ["Src/Modules/Identity/Identity.Application/Identity.Application.csproj", "Src/Modules/Identity/Identity.Application/"]
COPY ["Src/Modules/Identity/Identity.Domain/Identity.Domain.csproj", "Src/Modules/Identity/Identity.Domain/"]
COPY ["Src/Modules/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Src/Modules/Identity/Identity.Infrastructure/"]
COPY ["Src/Modules/Identity/Identity.Presentation/Identity.Presentation.csproj", "Src/Modules/Identity/Identity.Presentation/"]
COPY ["Src/Modules/Notification/Notification.Composition/Notification.Composition.csproj", "Src/Modules/Notification/Notification.Composition/"]
COPY ["Src/Modules/Notification/Notification.Application/Notification.Application.csproj", "Src/Modules/Notification/Notification.Application/"]
COPY ["Src/Modules/Notification/Notification.Domain/Notification.Domain.csproj", "Src/Modules/Notification/Notification.Domain/"]
COPY ["Src/Modules/Notification/Notification.Infrastructure/Notification.Infrastructure.csproj", "Src/Modules/Notification/Notification.Infrastructure/"]
COPY ["Src/Modules/Notification/Notification.Presentation/Notification.Presentation.csproj", "Src/Modules/Notification/Notification.Presentation/"]
COPY ["Src/Modules/Report/Report.Application/Report.csproj", "Src/Modules/Report/Report.Application/"]
COPY ["Src/Modules/Transaction/Transaction.Infrastructure/Transaction.Infrastructure.csproj", "Src/Modules/Transaction/Transaction.Infrastructure/"]
COPY ["Src/Modules/Transaction/Transaction.Domain/Transaction.Domain.csproj", "Src/Modules/Transaction/Transaction.Domain/"]
COPY ["Src/Modules/Transaction/Transaction.Composition/Transaction.Composition.csproj", "Src/Modules/Transaction/Transaction.Composition/"]
COPY ["Src/Modules/Transaction/Transaction.Application/Transaction.Application.csproj", "Src/Modules/Transaction/Transaction.Application/"]
COPY ["Src/Modules/Transaction/Transaction.Presentation/Transaction.Presentation.csproj", "Src/Modules/Transaction/Transaction.Presentation/"]
COPY ["Src/Modules/Wallet/Wallet.Composition/Wallet.Composition.csproj", "Src/Modules/Wallet/Wallet.Composition/"]
COPY ["Src/Modules/Wallet/Wallet.Application/Wallet.Application.csproj", "Src/Modules/Wallet/Wallet.Application/"]
COPY ["Src/Modules/Wallet/Wallet.Domain/Wallet.Domain.csproj", "Src/Modules/Wallet/Wallet.Domain/"]
COPY ["Src/Modules/Wallet/Wallet.Infrastructure/Wallet.Infrastructure.csproj", "Src/Modules/Wallet/Wallet.Infrastructure/"]
COPY ["Src/Modules/Wallet/Wallet.Presentation/Wallet.Presentation.csproj", "Src/Modules/Wallet/Wallet.Presentation/"]
RUN dotnet restore "./Src/FinTracker.Api/FinTracker.Api.csproj"
COPY . .
WORKDIR "/src/Src/FinTracker.Api"
RUN dotnet build "./FinTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinTracker.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTracker.Api.dll"]