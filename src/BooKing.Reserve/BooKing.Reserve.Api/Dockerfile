#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BooKing.Reserve/BooKing.Reserve.Api/BooKing.Reserve.Api.csproj", "src/BooKing.Reserve/BooKing.Reserve.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Api/BooKing.Generics.Api.csproj", "src/BooKing.Generics/BooKing.Generics.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/BooKing.Reserve/BooKing.Reserve.Application/BooKing.Reserve.Application.csproj", "src/BooKing.Reserve/BooKing.Reserve.Application/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Shared/BooKing.Generics.Shared.csproj", "src/BooKing.Generics/BooKing.Generics.Shared/"]
COPY ["src/BooKing.Reserve/BooKing.Reserve.Domain/BooKing.Reserve.Domain.csproj", "src/BooKing.Reserve/BooKing.Reserve.Domain/"]
COPY ["src/BooKing.Reserve/BooKing.Reserve.Infra/BooKing.Reserve.Infra.csproj", "src/BooKing.Reserve/BooKing.Reserve.Infra/"]
RUN dotnet restore "src/BooKing.Reserve/BooKing.Reserve.Api/BooKing.Reserve.Api.csproj"
COPY . .
WORKDIR "/src/src/BooKing.Reserve/BooKing.Reserve.Api"
RUN dotnet build "BooKing.Reserve.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooKing.Reserve.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooKing.Reserve.Api.dll"]