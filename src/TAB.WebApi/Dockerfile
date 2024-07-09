#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TAB.WebApi/TAB.WebApi.csproj", "src/TAB.WebApi/"]
COPY ["src/TAB.Infrastructure/TAB.Infrastructure.csproj", "src/TAB.Infrastructure/"]
COPY ["src/TAB.Application/TAB.Application.csproj", "src/TAB.Application/"]
COPY ["src/TAB.Contracts/TAB.Contracts.csproj", "src/TAB.Contracts/"]
COPY ["src/TAB.Domain/TAB.Domain.csproj", "src/TAB.Domain/"]
COPY ["src/TAB.Persistence/TAB.Persistence.csproj", "src/TAB.Persistence/"]
RUN dotnet restore "src/TAB.WebApi/TAB.WebApi.csproj"
COPY . .
WORKDIR "/src/src/TAB.WebApi"
RUN dotnet build "TAB.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TAB.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TAB.WebApi.dll"]