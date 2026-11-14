# RaceDay - PROG6212
Full working API + MVC + Tests + Docker + Azure Blob.

## Requirements
.NET 8 SDK, SQL Server (LocalDB on Windows, or the Docker container below on Mac/Linux), VS Code with C# Dev Kit.

## First-time setup
```
dotnet dev-certs https --trust
dotnet tool install --global dotnet-ef --version 8.*
dotnet restore
```

### Database
Option A (zero setup): just run the API in Development. It creates `RaceDayDB` and seeds the 3 categories automatically.

Option B (migrations, for the Part 2 evidence): run these BEFORE the first run of the API:
```
dotnet ef migrations add InitialCreate --project RaceDay.API
dotnet ef database update --project RaceDay.API
```
If the API already created the database (Option A), drop it first: `dotnet ef database drop --project RaceDay.API`.

Mac/Linux: start SQL Server in Docker and change `DefaultConnection` in `RaceDay.API/appsettings.json`:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name raceday-sql -d mcr.microsoft.com/mssql/server:2022-latest
Server=localhost,1433;Database=RaceDayDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true
```

## Run
```
dotnet build
dotnet test
dotnet run --project RaceDay.API     # https://localhost:7001/swagger
dotnet run --project RaceDay.MVC     # https://localhost:7002
```
Or in VS Code: Run and Debug (Ctrl+Shift+D) > **API + MVC** > F5.

## Features
- Session Auth with BCrypt
- Organiser only can create events (403 for Participant)
- Enrol with duplicate check (409)
- Results show Position out of TotalFinishers e.g. 47th out of 312
- Azure Blob for banner upload (falls back to `RaceDay.API/wwwroot/banners` if Azure/Azurite is not running)
- Role-based navigation in the MVC layout
- Docker for the MVC app

## Banner upload without Azure
Install the Azurite VS Code extension, run **Azurite: Start**, and keep `UseDevelopmentStorage=true`. For real Azure, put your connection string in `RaceDay.API/appsettings.json` under `AzureBlob`.

## Docker (MVC)
The container cannot reach `https://localhost:7001`, so start the API on plain HTTP first:
```
dotnet run --project RaceDay.API --urls http://0.0.0.0:5000
cd RaceDay.MVC
docker build -t raceday-mvc .
docker run -p 8080:8080 -e ApiBaseUrl=http://host.docker.internal:5000/ raceday-mvc
```
Open http://localhost:8080

## Submission extras (add yourself)
- `docs/ERD.png`, `docs/ci-green.png` (screenshot of the green GitHub Actions run)
- YouTube links (Unlisted) for the Part 2 and Part 3 videos
- 20+ meaningful commits per part

Note: the API trusts the `X-User-Id` / `X-User-Role` headers sent by the MVC app. That is fine for coursework, but a real system needs JWT tokens.
