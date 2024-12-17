# Étape de base pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copie uniquement le fichier .csproj pour restaurer les dépendances
COPY ["BasketAPI.csproj", "./"]

# Restaurer les dépendances
RUN dotnet restore "BasketAPI.csproj"

# Copier tous les fichiers du projet dans l'image Docker
COPY . .

# Changer de répertoire pour le dossier du projet
WORKDIR "/src"

# Construire le projet
RUN dotnet build "BasketAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Étape de publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BasketAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Étape finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasketAPI.dll"]
