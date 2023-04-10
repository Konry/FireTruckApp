FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["FireTruckReactApp/FireTruckReactApp.csproj", "FireTruckReactApp/"]
RUN dotnet restore "FireTruckReactApp/FireTruckReactApp.csproj"
COPY . .
WORKDIR "/src/FireTruckReactApp"
RUN dotnet build "FireTruckReactApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FireTruckReactApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FireTruckReactApp.dll"]
