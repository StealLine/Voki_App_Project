#!/bin/sh
set -e
      
# Map service name → csproj path and output dir
build_service() {
  SERVICE=$1
  case "$SERVICE" in
    auth)
      dotnet publish backend/AuthService/src/AuthService.Api/AuthService.Api.csproj -c Release -o publish/auth ;;
    albums)
      dotnet publish backend/AlbumsService/src/AlbumsService.Api/AlbumsService.Api.csproj -c Release -o publish/albums ;;
    tags)
      dotnet publish backend/TagsService/src/TagsService.Api/TagsService.Api.csproj -c Release -o publish/tags ;;
    user-profiles)
      dotnet publish backend/UserProfilesService/src/UserProfilesService.Api/UserProfilesService.Api.csproj -c Release -o publish/user-profiles ;;
    voki-comments)
      dotnet publish backend/VokiCommentsService/src/VokiCommentsService.Api/VokiCommentsService.Api.csproj -c Release -o publish/voki-comments ;;
    voki-ratings)
      dotnet publish backend/VokiRatingsService/src/VokiRatingsService.Api/VokiRatingsService.Api.csproj -c Release -o publish/voki-ratings ;;
    vokis-catalog)
      dotnet publish backend/VokisCatalogService/src/VokisCatalogService.Api/VokisCatalogService.Api.csproj -c Release -o publish/vokis-catalog ;;
    core-voki-creation)
      dotnet publish backend/VokiCreationServicesCollection/CoreVokiCreationService/src/CoreVokiCreationService.Api/CoreVokiCreationService.Api.csproj -c Release -o publish/core-voki-creation ;;
    general-voki-creation)
      dotnet publish backend/VokiCreationServicesCollection/GeneralVokiCreationService/src/GeneralVokiCreationService.Api/GeneralVokiCreationService.Api.csproj -c Release -o publish/general-voki-creation ;;
    general-voki-taking)
      dotnet publish backend/VokiTakingServicesCollection/GeneralVokiTakingService/src/GeneralVokiTakingService.Api/GeneralVokiTakingService.Api.csproj -c Release -o publish/general-voki-taking ;;
    storage)
      dotnet publish backend/VokimiStorageService/VokimiStorageService.csproj -c Release -o publish/storage ;;
    db-seeder)
      dotnet publish backend/DbSeeder/DbSeeder.csproj -c Release -o publish/db-seeder ;;
    *)
      echo "Unknown service: $SERVICE" && exit 1 ;;
  esac
}

# If SERVICE env var is set — build only that service, otherwise build all
if [ -n "$BUILD_SERVICE" ]; then
  echo "Selective build: $BUILD_SERVICE"
  build_service "$BUILD_SERVICE"
else
  echo "Full build: all services"
  for svc in auth albums tags user-profiles voki-comments voki-ratings vokis-catalog core-voki-creation general-voki-creation general-voki-taking storage db-seeder; do
    build_service "$svc"
  done
fi