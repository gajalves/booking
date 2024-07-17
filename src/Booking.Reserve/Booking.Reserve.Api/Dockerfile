#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Booking.Reserve/Booking.Reserve.Api/Booking.Reserve.Api.csproj", "src/Booking.Reserve/Booking.Reserve.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Api/BooKing.Generics.Api.csproj", "src/BooKing.Generics/BooKing.Generics.Api/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Infra/BooKing.Generics.Infra.csproj", "src/BooKing.Generics/BooKing.Generics.Infra/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Domain/BooKing.Generics.Domain.csproj", "src/BooKing.Generics/BooKing.Generics.Domain/"]
COPY ["src/Booking.Reserve/Booking.Reserve.Application/Booking.Reserve.Application.csproj", "src/Booking.Reserve/Booking.Reserve.Application/"]
COPY ["src/BooKing.Generics/BooKing.Generics.Shared/BooKing.Generics.Shared.csproj", "src/BooKing.Generics/BooKing.Generics.Shared/"]
COPY ["src/Booking.Reserve/Booking.Reserve.Domain/Booking.Reserve.Domain.csproj", "src/Booking.Reserve/Booking.Reserve.Domain/"]
COPY ["src/Booking.Reserve/Booking.Reserve.Infra/Booking.Reserve.Infra.csproj", "src/Booking.Reserve/Booking.Reserve.Infra/"]
RUN dotnet restore "src/Booking.Reserve/Booking.Reserve.Api/Booking.Reserve.Api.csproj"
COPY . .
WORKDIR "/src/src/Booking.Reserve/Booking.Reserve.Api"
RUN dotnet build "Booking.Reserve.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Booking.Reserve.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.Reserve.Api.dll"]