@REM Initialize API (script for Windows)
dotnet ef migrations add SetupMigrations -o Persistence/Migrations
start chrome "https://localhost:7043/swagger/index.html"
dotnet run