# Vokimi Application & Deployment Files

## About

#### This application was developed by [SaYMooN0](https://github.com/SaYMooN0?utm_source=chatgpt.com).

#### All required instructions for deployment can be found in this [repo](https://github.com/StealLine/DEVOPS_WORKFLOW_MICROSERVICES)

## Documentation Overview

This repository contains the **Vokimi** application along with all required configuration and deployment files.


# About vokimi

Vokimi is a web application for interacting with quiz-tests (similar to the ones on uquiz.com), which are called Vokis here. Users can create Vokis alone or in team, take them, leave unfinished and then continue later in the same session, collect into albums, rate and manage (i.e. track statistics after publication).

## Backend Architecture:
The Vokimi backend is 11 microservices on C# Asp.Net with Minimal Api.  

10 Main services: 
1)   **AlbumsService**
2)   **AuthService**
3)   **TagsService**
4)   **UserProfilesService**
5)   **VokiCommentsService**
6)   **CoreVokiCreationService**
7)   **GeneralVokiCreationService**
8)   **VokiRatingsService**
9)   **VokisCatalogService**
10)   **GeneralVokiTakingService**

---
 these are applications implemented according to DDD and Clean Architecture principles. Each service is divided into 4 layers: .Api, .Infrastructure, .Application, .Domain, and has its own PostgreSql database, which it interacts with through EfCore. All integration events are sent through RabbitMQ, which the services connect to using MassTransit. Services that need to work with S3 use AWSSDK.S3

there is also
11)   **VokimiStorageService** 
which is used for compressing image and audio files through FFmpeg and uploading them to S3 as temp files.
## Backend stack:
- **C#**, Asp.Net, Minimal Api 
- **PostgreSql** (with access through EfCore)
- **S3** (through the AWSSDK.S3 library)
- **RabbitMQ** (through the MassTransit library)
- **FFmpeg** (through the Xabe.FFmpeg library)
- + libraries: MailKit, Scrutor
- + custom mediator and event publisher for domain events
## Frontend stack:
- **Svelte 5** / **SvelteKit 2** with server side rendering where possible
- **node.js**
- **npm**
- **vite**
- + libraries: runed, svelte-sonner, svelte-relative-time

---

# Vokimi — Configuration Files Reference
 
---
 
## `build_script.sh`
 
A shell script that chooses the appropriate services to build and runs the appropriate build commands.
 
**Behavior:**
 
- If the environment variable `BUILD_SERVICE` is set, only that service is built.
- Otherwise, all 12 services are built sequentially.
**Supported service names** (passed via `BUILD_SERVICE`):
 
| Value | Output directory |
|---|---|
| `auth` | `publish/auth` |
| `albums` | `publish/albums` |
| `tags` | `publish/tags` |
| `user-profiles` | `publish/user-profiles` |
| `voki-comments` | `publish/voki-comments` |
| `voki-ratings` | `publish/voki-ratings` |
| `vokis-catalog` | `publish/vokis-catalog` |
| `core-voki-creation` | `publish/core-voki-creation` |
| `general-voki-creation` | `publish/general-voki-creation` |
| `general-voki-taking` | `publish/general-voki-taking` |
| `storage` | `publish/storage` |
| `db-seeder` | `publish/db-seeder` |
 
**Usage:**
 
```sh
# Build all services
./build_script.sh
 
# Build a single service
BUILD_SERVICE=auth ./build_script.sh
```
 
---
 
## `compose-preview.yml`
 
Docker Compose file for the **preview** environment. Deployed once per commit on a staging server behind Traefik.
 
### Infrastructure services
 
| Service | Image | Notes |
|---|---|---|
| `postgres` | `postgres:16` | Shared database for all microservices. |
| `rabbitmq` | `rabbitmq:3-management` | Message broker. |
| `minio` | `minio/minio` | S3-compatible object storage. Exposes UI at `yourdomain/minio-console`. |
| `minio-init` | `minio/mc` | One-shot initializer: creates the `vokimi-storage` bucket and pre-populates default assets. |
 
### `db-seeder`
 
Runs migrations for all service databases via connection strings injected as environment variables. It runs with `command: ["clear"]` and `restart: "no"`, meaning it exits after seeding and is not restarted.
 
### Backend services
 
All backend services share the following common environment variables:
 
| Variable | Description |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | Set to `Production` |
| `ConnectionStrings__<ServiceName>Db` | PostgreSQL connection string for that service's own database |
| `MessageBroker__Host` / `Username` / `Password` | RabbitMQ connection |
| `MessageBroker__RetryCount` / `RetryIntervalSeconds` | Message retry policy (3 retries, 5 s interval) |
| `ServiceName` | Logical name used internally |
| `FrontendUrl` | Base URL of the frontend (used for CORS and email links) |
| `JwtTokenConfig__*` | JWT tokens |
 
Services that handle file uploads additionally receive S3 configuration:
 
| Variable | Description |
|---|---|
| `S3__AccessKey` / `SecretKey` | MinIO credentials |
| `S3__ServiceURL` | MinIO endpoint URL |
| `S3__MainBucket__Name` | Always `vokimi-storage` |
| `S3__ForcePathStyle` | `true` (required for MinIO) |
 
`auth-service` additionally receives email configuration (`EmailServiceConfig__*`).
 
`vokimi-storage-service` additionally sets `FfmpegPath: /usr/bin`.
 
### Frontend & Nginx
 
| Service | Description |
|---|---|
| `frontend` | Node.js app. Depends on no backend services directly. |
| `nginx` | Reverse proxy. Carries Traefik labels that expose the deployment at `<CI_COMMIT_SHORT_SHA>.<DOMAIN>` over HTTPS. HTTP Basic Auth is enforced via `PREVIEW_PASS_HASH`. Depends on all backend services and `frontend`. |
 
Nginx is connected to two networks: the internal compose network and an external Traefik network (`DEPLOY_NETWORK_NAME`).
 
`nginx-preview.conf` and `nginx-prod.conf` simply route requests coming from Traefik to the appropriate service — nothing special there.
 
---
 
## `compose-prod.yml`
 
Structurally identical to `compose-preview.yml` with some differences:
 
- The Nginx Traefik labels use a fixed domain targeting the production hostname.
- Instead of MinIO, you should use an S3-compatible cloud provider like AWS S3 or similar.
- There are also containers named `backup-*` that perform daily backups into your S3 storage and retain them for 7 days before deletion. You are free to modify this.
---
 
## Dockerfiles
 
Each Dockerfile simply builds the appropriate service. `Dockerfile.backend` is similar for almost all services except the storage service. `Dockerfile.frontend` and `Dockerfile.backend` are also pretty straightforward. The only thing worth mentioning is that all of them run as the `nobody` user for extra safety.
 
---
 
## `.gitlab-ci.yml`
 
The entire CI pipeline is defined in an external shared project [CI_CD_VOKIMI](https://github.com/StealLine/CI_CD_Configuration_Vokimi). This file simply includes it. All build, test, and deploy jobs are maintained centrally there.