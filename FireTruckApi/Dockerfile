FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FireTruckApi/FireTruckApi.csproj", "FireTruckApi/"]
RUN dotnet restore "FireTruckApi/FireTruckApi.csproj"
COPY . .
WORKDIR "/src/FireTruckApi"
RUN dotnet build "FireTruckApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FireTruckApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FireTruckApi.dll"]
