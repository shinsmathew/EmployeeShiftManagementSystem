@echo off
setlocal enabledelayedexpansion

REM === Set project root directory ===
set ROOTDIR=C:\Users\shins\source\repos\Good Projects\EmployeeShiftManagementSystem
cd /d "%ROOTDIR%"

echo Creating solution and folders...

dotnet new sln -n EmployeeShiftManagementSystem
if not exist src mkdir src
cd src

echo Creating main projects...
dotnet new webapi -n EmployeeShiftManagementSystem.API --framework net8.0
dotnet new classlib -n EmployeeShiftManagementSystem.Core --framework net8.0
dotnet new classlib -n EmployeeShiftManagementSystem.Application --framework net8.0
dotnet new classlib -n EmployeeShiftManagementSystem.Infrastructure --framework net8.0

cd ..
if not exist tests mkdir tests
cd tests

echo Creating test projects...
dotnet new xunit -n EmployeeShiftManagementSystem.UnitTests --framework net8.0
dotnet new xunit -n EmployeeShiftManagementSystem.IntegrationTests --framework net8.0

cd ..

echo Adding projects to solution...
dotnet sln add src/EmployeeShiftManagementSystem.API/EmployeeShiftManagementSystem.API.csproj
dotnet sln add src/EmployeeShiftManagementSystem.Core/EmployeeShiftManagementSystem.Core.csproj
dotnet sln add src/EmployeeShiftManagementSystem.Application/EmployeeShiftManagementSystem.Application.csproj
dotnet sln add src/EmployeeShiftManagementSystem.Infrastructure/EmployeeShiftManagementSystem.Infrastructure.csproj
dotnet sln add tests/EmployeeShiftManagementSystem.UnitTests/EmployeeShiftManagementSystem.UnitTests.csproj
dotnet sln add tests/EmployeeShiftManagementSystem.IntegrationTests/EmployeeShiftManagementSystem.IntegrationTests.csproj

echo Adding project references...
dotnet add src/EmployeeShiftManagementSystem.API/EmployeeShiftManagementSystem.API.csproj reference src/EmployeeShiftManagementSystem.Application/EmployeeShiftManagementSystem.Application.csproj

dotnet add src/EmployeeShiftManagementSystem.Application/EmployeeShiftManagementSystem.Application.csproj reference src/EmployeeShiftManagementSystem.Core/EmployeeShiftManagementSystem.Core.csproj
dotnet add src/EmployeeShiftManagementSystem.Application/EmployeeShiftManagementSystem.Application.csproj reference src/EmployeeShiftManagementSystem.Infrastructure/EmployeeShiftManagementSystem.Infrastructure.csproj

dotnet add src/EmployeeShiftManagementSystem.Infrastructure/EmployeeShiftManagementSystem.Infrastructure.csproj reference src/EmployeeShiftManagementSystem.Core/EmployeeShiftManagementSystem.Core.csproj

dotnet add tests/EmployeeShiftManagementSystem.UnitTests/EmployeeShiftManagementSystem.UnitTests.csproj reference src/EmployeeShiftManagementSystem.API/EmployeeShiftManagementSystem.API.csproj
dotnet add tests/EmployeeShiftManagementSystem.UnitTests/EmployeeShiftManagementSystem.UnitTests.csproj reference src/EmployeeShiftManagementSystem.Application/EmployeeShiftManagementSystem.Application.csproj
dotnet add tests/EmployeeShiftManagementSystem.IntegrationTests/EmployeeShiftManagementSystem.IntegrationTests.csproj reference src/EmployeeShiftManagementSystem.API/EmployeeShiftManagementSystem.API.csproj

echo Installing NuGet packages (pinned to latest .NET 8 versions)...

REM --- API Packages ---
cd src/EmployeeShiftManagementSystem.API
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package StackExchange.Redis --version 2.7.33
dotnet add package Serilog.AspNetCore --version 8.0.1
dotnet add package Swashbuckle.AspNetCore --version 6.6.2

REM --- Application Packages ---
cd ../EmployeeShiftManagementSystem.Application
dotnet add package MediatR --version 12.1.1
dotnet add package AutoMapper --version 13.0.1
dotnet add package FluentValidation --version 11.8.0

REM --- Infrastructure Packages ---
cd ../EmployeeShiftManagementSystem.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0

REM --- Test Projects ---
cd ../../tests/EmployeeShiftManagementSystem.UnitTests
dotnet add package Moq --version 4.20.70
dotnet add package FluentAssertions --version 6.12.0
dotnet add package xunit.runner.visualstudio --version 2.8.2

cd ../EmployeeShiftManagementSystem.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.0
dotnet add package FluentAssertions --version 6.12.0

REM --- Final Steps ---
cd "%ROOTDIR%"
dotnet restore
dotnet build

echo.
echo ✅ Setup Complete for EmployeeShiftManagementSystem (.NET 8) with pinned package versions!
pause
