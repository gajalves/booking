#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BooKing.Apartments/BooKing.Apartments.Api/BooKing.Apartments.Api.csproj", "src/BooKing.Apartments/BooKing.Apartments.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Api/BooKing.Generics.Api.csproj", "src/BooKing.Generics/BooKing.Generics.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/BooKing.Apartments/BooKing.Apartments.Application/BooKing.Apartments.Application.csproj", "src/BooKing.Apartments/BooKing.Apartments.Application/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Shared/BooKing.Generics.Shared.csproj", "src/BooKing.Generics/BooKing.Generics.Shared/"]
COPY ["src/BooKing.Apartments/BooKing.Apartments.Domain/BooKing.Apartments.Domain.csproj", "src/BooKing.Apartments/BooKing.Apartments.Domain/"]
COPY ["src/BooKing.Apartments/BooKing.Apartments.Infra/BooKing.Apartments.Infra.csproj", "src/BooKing.Apartments/BooKing.Apartments.Infra/"]
RUN dotnet restore "src/BooKing.Apartments/BooKing.Apartments.Api/BooKing.Apartments.Api.csproj"
COPY . .
WORKDIR "/src/src/BooKing.Apartments/BooKing.Apartments.Api"
RUN dotnet build "BooKing.Apartments.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooKing.Apartments.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooKing.Apartments.Api.dll"]