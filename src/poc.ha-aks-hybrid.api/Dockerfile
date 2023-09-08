#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["poc.ha-aks-hybrid.api/poc.ha-aks-hybrid.api.csproj", "poc.ha-aks-hybrid.api/"]
COPY ["poc.ha-aks-hybrid.core/poc.ha-aks-hybrid.core.csproj", "poc.ha-aks-hybrid.core/"]
RUN dotnet restore "poc.ha-aks-hybrid.api/poc.ha-aks-hybrid.api.csproj"
COPY . .
WORKDIR "/src/poc.ha-aks-hybrid.api"
RUN dotnet build "poc.ha-aks-hybrid.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "poc.ha-aks-hybrid.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc.ha-aks-hybrid.api.dll"]