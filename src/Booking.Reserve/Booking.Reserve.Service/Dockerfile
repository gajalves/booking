#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Booking.Reserve/Booking.Reserve.Service/Booking.Reserve.Service.csproj", "src/Booking.Reserve/Booking.Reserve.Service/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Bus/BooKing.Generics.Bus.csproj", "src/BooKing.Generics/BooKing.Generics.Bus/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Outbox/BooKing.Generics.Outbox.csproj", "src/BooKing.Generics/BooKing.Generics.Outbox/"]
COPY ["src/Booking.Reserve/Booking.Reserve.Domain/Booking.Reserve.Domain.csproj", "src/Booking.Reserve/Booking.Reserve.Domain/"]
COPY ["src/Booking.Reserve/Booking.Reserve.Infra/Booking.Reserve.Infra.csproj", "src/Booking.Reserve/Booking.Reserve.Infra/"]
RUN dotnet restore "src/Booking.Reserve/Booking.Reserve.Service/Booking.Reserve.Service.csproj"
COPY . .
WORKDIR "/src/src/Booking.Reserve/Booking.Reserve.Service"
RUN dotnet build "Booking.Reserve.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.Reserve.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.Reserve.Service.dll"]