#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.Props", "."]
COPY ["src/Directory.Build.Props", "src/"]
COPY ["Directory.Build.Props", "."]
COPY ["src/Domain/Directory.Build.Props", "src/Domain/"]
COPY ["nuget.config", "."]
COPY ["src/Launcher/Launcher.csproj", "src/Launcher/"]
COPY ["src/Infrastructure.EF.Outbox/Infrastructure.EF.Outbox.csproj", "src/Infrastructure.EF.Outbox/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure.EF/Infrastructure.EF.csproj", "src/Infrastructure.EF/"]
COPY ["src/Presentation/Presentation.Api.csproj", "src/Presentation/"]
RUN dotnet restore "./src/Launcher/./Launcher.csproj"
COPY . .
WORKDIR "/src/src/Launcher"
RUN dotnet build "./Launcher.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Launcher.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Launcher.dll"]