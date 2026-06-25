#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["FamUnion.WebAuth/FamUnion.WebAuth.csproj", "FamUnion.WebAuth/"]
COPY ["FamUnion.Core/FamUnion.Core.csproj", "FamUnion.Core/"]
RUN dotnet restore "FamUnion.WebAuth/FamUnion.WebAuth.csproj"
COPY . .
WORKDIR "/src/FamUnion.WebAuth"
RUN dotnet build "FamUnion.WebAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamUnion.WebAuth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamUnion.WebAuth.dll"]