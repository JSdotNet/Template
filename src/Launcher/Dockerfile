#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Directory.Build.Props", "src/"]
COPY ["Directory.Build.Props", "."]
COPY ["src/Launcher/Launcher.csproj", "src/Launcher/"]
RUN dotnet restore "src/Launcher/Launcher.csproj"
COPY . .
WORKDIR "/src/src/Launcher"
RUN dotnet build "Launcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Launcher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Launcher.dll"]