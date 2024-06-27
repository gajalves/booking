#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BooKing.Outbox/BooKing.Outbox.Service/BooKing.Outbox.Service.csproj", "src/BooKing.Outbox/BooKing.Outbox.Service/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Outbox/BooKing.Generics.Outbox.csproj", "src/BooKing.Generics/BooKing.Generics.Outbox/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Bus/BooKing.Generics.Bus.csproj", "src/BooKing.Generics/BooKing.Generics.Bus/"]
RUN dotnet restore "src/BooKing.Outbox/BooKing.Outbox.Service/BooKing.Outbox.Service.csproj"
COPY . .
WORKDIR "/src/src/BooKing.Outbox/BooKing.Outbox.Service"
RUN dotnet build "BooKing.Outbox.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooKing.Outbox.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooKing.Outbox.Service.dll"]