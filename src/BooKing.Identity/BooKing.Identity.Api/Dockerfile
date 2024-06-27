#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BooKing.Identity/BooKing.Identity.Api/BooKing.Identity.Api.csproj", "src/BooKing.Identity/BooKing.Identity.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Api/BooKing.Generics.Api.csproj", "src/BooKing.Generics/BooKing.Generics.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Outbox/BooKing.Generics.Outbox.csproj", "src/BooKing.Generics/BooKing.Generics.Outbox/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Bus/BooKing.Generics.Bus.csproj", "src/BooKing.Generics/BooKing.Generics.Bus/"]
COPY ["src/BooKing.Identity/BooKing.Identity.Application/BooKing.Identity.Application.csproj", "src/BooKing.Identity/BooKing.Identity.Application/"]
COPY ["src/BooKing.Identity/BooKing.Identity.Infra/BooKing.Identity.Infra.csproj", "src/BooKing.Identity/BooKing.Identity.Infra/"]
COPY ["src/BooKing.Identity/BooKing.Identity.Domain/BooKing.Identity.Domain.csproj", "src/BooKing.Identity/BooKing.Identity.Domain/"]
RUN dotnet restore "src/BooKing.Identity/BooKing.Identity.Api/BooKing.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/BooKing.Identity/BooKing.Identity.Api"
RUN dotnet build "BooKing.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooKing.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooKing.Identity.Api.dll"]