# Base runtime
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

# Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BooKing.Generics/BooKing.Infra.PreBase/BooKing.Infra.PreBase.csproj", "src/BooKing.Generics/BooKing.Infra.PreBase/"]
RUN dotnet restore "./src/BooKing.Generics/BooKing.Infra.PreBase/BooKing.Infra.PreBase.csproj"
COPY . .
WORKDIR "/src/src/BooKing.Generics/BooKing.Infra.PreBase"
RUN dotnet build "./BooKing.Infra.PreBase.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BooKing.Infra.PreBase.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooKing.Infra.PreBase.dll"]
