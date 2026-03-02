# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y --no-install-recommends \
    chromium \
    fonts-liberation \
    libasound2 \
    libatk-bridge2.0-0 \
    libatk1.0-0 \
    libcairo2 \
    libcups2 \
    libdrm2 \
    libfontconfig1 \
    libfreetype6 \
    libgbm1 \
    libgtk-3-0 \
    libnss3 \
    libpng16-16 \
    libx11-6 \
    libxcomposite1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxrandr2 \
    libxrender1 \
    libxshmfence1 \
    wget \
    && rm -rf /var/lib/apt/lists/* \
    && ln -sf /usr/bin/chromium /usr/bin/chromium-browser

ENV EBAYCLONE_CHROMIUM_EXECUTABLE_PATH=/usr/bin/chromium \
    EBAYCLONE_DISABLE_CHROMIUM_DOWNLOAD=true

WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PRN232_EbayClone/PRN232_EbayClone.Api/PRN232_EbayClone.Api.csproj", "PRN232_EbayClone/PRN232_EbayClone.Api/"]
COPY ["PRN232_EbayClone/PRN232_EbayClone.Application/PRN232_EbayClone.Application.csproj", "PRN232_EbayClone/PRN232_EbayClone.Application/"]
COPY ["PRN232_EbayClone/PRN232_EbayClone.Domain/PRN232_EbayClone.Domain.csproj", "PRN232_EbayClone/PRN232_EbayClone.Domain/"]
COPY ["PRN232_EbayClone/PRN232_EbayClone.Infrastructure/PRN232_EbayClone.Infrastructure.csproj", "PRN232_EbayClone/PRN232_EbayClone.Infrastructure/"]
RUN dotnet restore "./PRN232_EbayClone/PRN232_EbayClone.Api/PRN232_EbayClone.Api.csproj"
COPY . .
WORKDIR "/src/PRN232_EbayClone/PRN232_EbayClone.Api"
RUN dotnet build "./PRN232_EbayClone.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PRN232_EbayClone.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRN232_EbayClone.Api.dll"]