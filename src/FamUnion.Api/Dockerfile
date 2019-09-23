FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["FamUnion.Api/FamUnion.Api.csproj", "FamUnion.Api/"]
COPY ["FamUnion.Infrastructure/FamUnion.Infrastructure.csproj", "FamUnion.Infrastructure/"]
COPY ["FamUnion.Core/FamUnion.Core.csproj", "FamUnion.Core/"]
COPY ["StatesAndProvinces.NetStandard/StatesAndProvinces.NetStandard.csproj", "StatesAndProvinces.NetStandard/"]
RUN dotnet restore "FamUnion.Api/FamUnion.Api.csproj"
COPY . .
WORKDIR "/src/FamUnion.Api"
RUN dotnet build "FamUnion.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FamUnion.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FamUnion.Api.dll"]