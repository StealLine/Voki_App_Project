#!/bin/sh
set -e

dotnet publish backend/AuthService/src/AuthService.Api/AuthService.Api.csproj -c Release -o publish/auth
dotnet publish backend/AlbumsService/src/AlbumsService.Api/AlbumsService.Api.csproj -c Release -o publish/albums
dotnet publish backend/TagsService/src/TagsService.Api/TagsService.Api.csproj -c Release -o publish/tags
dotnet publish backend/UserProfilesService/src/UserProfilesService.Api/UserProfilesService.Api.csproj -c Release -o publish/user-profiles
dotnet publish backend/VokiCommentsService/src/VokiCommentsService.Api/VokiCommentsService.Api.csproj -c Release -o publish/voki-comments
dotnet publish backend/VokiRatingsService/src/VokiRatingsService.Api/VokiRatingsService.Api.csproj -c Release -o publish/voki-ratings
dotnet publish backend/VokisCatalogService/src/VokisCatalogService.Api/VokisCatalogService.Api.csproj -c Release -o publish/vokis-catalog
dotnet publish backend/VokiCreationServicesCollection/CoreVokiCreationService/src/CoreVokiCreationService.Api/CoreVokiCreationService.Api.csproj -c Release -o publish/core-voki-creation
dotnet publish backend/VokiCreationServicesCollection/GeneralVokiCreationService/src/GeneralVokiCreationService.Api/GeneralVokiCreationService.Api.csproj -c Release -o publish/general-voki-creation
dotnet publish backend/VokiTakingServicesCollection/GeneralVokiTakingService/src/GeneralVokiTakingService.Api/GeneralVokiTakingService.Api.csproj -c Release -o publish/general-voki-taking
dotnet publish backend/VokimiStorageService/VokimiStorageService.csproj -c Release -o publish/storage
dotnet publish backend/DbSeeder/DbSeeder.csproj -c Release -o publish/db-seeder