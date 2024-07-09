#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TAB.WebApi/TAB.WebApi.csproj", "TAB.WebApi/"]
COPY ["TAB.Infrastructure/TAB.Infrastructure.csproj", "TAB.Infrastructure/"]
COPY ["TAB.Application/TAB.Application.csproj", "TAB.Application/"]
COPY ["TAB.Contracts/TAB.Contracts.csproj", "TAB.Contracts/"]
COPY ["TAB.Domain/TAB.Domain.csproj", "TAB.Domain/"]
COPY ["TAB.Persistence/TAB.Persistence.csproj", "TAB.Persistence/"]
RUN dotnet restore "TAB.WebApi/TAB.WebApi.csproj"
COPY . .
WORKDIR "/src/TAB.WebApi"
RUN dotnet build "TAB.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TAB.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TAB.WebApi.dll"]