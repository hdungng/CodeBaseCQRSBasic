# CodeBaseCQRSBasic

ASP.NET Core Web API implementing CQRS with MediatR, FluentValidation, and EF Core.

## Tech Stack
- ASP.NET Core Web API (.NET 10)
- MediatR (CQRS)
- FluentValidation
- EF Core with SQL Server (default) and MySQL provider support
- xUnit tests for handlers and validators

## Project Structure
- `CodeBaseCQRSBasic/Domain`
- `CodeBaseCQRSBasic/Commands`
- `CodeBaseCQRSBasic/Queries`
- `CodeBaseCQRSBasic/Handlers`
- `CodeBaseCQRSBasic/Controllers`
- `CodeBaseCQRSBasic/Validators`
- `CodeBaseCQRSBasic/Infrastructure`
- `CodeBaseCQRSBasic/Seed`
- `Tests/CodeBaseCQRSBasic.Tests`

## Run
1. Update connection strings in `CodeBaseCQRSBasic/appsettings.json`.
2. Set `DatabaseProvider` to `SqlServer` (default) or `MySql`.
3. Run the API:
   ```bash
   dotnet run --project CodeBaseCQRSBasic
   ```

At startup, the app seeds:
- Roles: `Admin`, `User`
- 10 users assigned to these roles.

## Test
```bash
dotnet test CodeBaseCQRSBasic.slnx
```
