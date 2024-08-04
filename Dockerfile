#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SalesInvoice.WebAPI/SalesInvoice.WebAPI.csproj", "SalesInvoice.WebAPI/"]
COPY ["SalesInvoice.Application/SalesInvoice.Application.csproj", "SalesInvoice.Application/"]
COPY ["SalesInvoice.Infrastructure/SalesInvoice.Infrastructure.csproj", "SalesInvoice.Infrastructure/"]
COPY ["SalesInvoice.Domain/SalesInvoice.Domain.csproj", "SalesInvoice.Domain/"]
RUN dotnet restore "./SalesInvoice.WebAPI/SalesInvoice.WebAPI.csproj"
COPY . .
WORKDIR "/src/SalesInvoice.WebAPI"
RUN dotnet build "./SalesInvoice.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SalesInvoice.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesInvoice.WebAPI.dll"]