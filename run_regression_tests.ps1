# Regression Test Script for Clientele Projects (PowerShell)
# This script runs all projects and tests to ensure everything works correctly

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Clientele Projects - Regression Testing" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

$Passed = 0
$Failed = 0

# Function to test a project build
function Test-Build {
    param(
        [string]$ProjectName,
        [string]$ProjectPath
    )
    
    Write-Host "Testing build: $ProjectName" -ForegroundColor Yellow
    $result = dotnet build $ProjectPath --verbosity quiet 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ $ProjectName builds successfully" -ForegroundColor Green
        $script:Passed++
        return $true
    } else {
        Write-Host "✗ $ProjectName build failed" -ForegroundColor Red
        $script:Failed++
        return $false
    }
}

# Function to test a test project
function Test-Tests {
    param(
        [string]$TestName,
        [string]$TestPath
    )
    
    Write-Host "Running tests: $TestName" -ForegroundColor Yellow
    $result = dotnet test $TestPath --verbosity quiet --no-build 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ $TestName tests passed" -ForegroundColor Green
        $script:Passed++
        return $true
    } else {
        Write-Host "✗ $TestName tests failed" -ForegroundColor Red
        $script:Failed++
        return $false
    }
}

Write-Host "Step 1: Building all projects..." -ForegroundColor Cyan
Write-Host "-----------------------------------" -ForegroundColor Cyan

# Build all main projects
Test-Build "LibraryManagementSystem" "LibraryManagementSystem\LibraryManagementSystem\LibraryManagementSystem.csproj"
Test-Build "FactorialCalculator" "FactorialCalculator\FactorialCalculator\FactorialCalculator.csproj"
Test-Build "ProducerConsumerPattern" "ProducerConsumerPattern\ProducerConsumerPattern\ProducerConsumerPattern.csproj"
Test-Build "EntityFrameworkCRUD" "EntityFrameworkCRUD\EntityFrameworkCRUD.csproj"
Test-Build "ProductAPI" "ProductAPI\ProductAPI.csproj"
Test-Build "WeatherAPI" "WeatherAPI\WeatherAPI.csproj"

Write-Host ""
Write-Host "Step 2: Building all test projects..." -ForegroundColor Cyan
Write-Host "-----------------------------------" -ForegroundColor Cyan

# Build all test projects
Test-Build "EntityFrameworkCRUD.Tests" "EntityFrameworkCRUD.Tests\EntityFrameworkCRUD.Tests.csproj"
Test-Build "ProductAPI.Tests" "ProductAPI.Tests\ProductAPI.Tests.csproj"
Test-Build "LibraryManagementSystem.Tests" "LibraryManagementSystem.Tests\LibraryManagementSystem.Tests.csproj"
Test-Build "WeatherAPI.Tests" "WeatherAPI.Tests\WeatherAPI.Tests.csproj"
Test-Build "FactorialCalculator.Tests" "FactorialCalculator.Tests\FactorialCalculator.Tests.csproj"
Test-Build "ProducerConsumerPattern.Tests" "ProducerConsumerPattern.Tests\ProducerConsumerPattern.Tests.csproj"

Write-Host ""
Write-Host "Step 3: Running all unit tests..." -ForegroundColor Cyan
Write-Host "-----------------------------------" -ForegroundColor Cyan

# Run all tests
Test-Tests "EntityFrameworkCRUD.Tests" "EntityFrameworkCRUD.Tests\EntityFrameworkCRUD.Tests.csproj"
Test-Tests "ProductAPI.Tests" "ProductAPI.Tests\ProductAPI.Tests.csproj"
Test-Tests "LibraryManagementSystem.Tests" "LibraryManagementSystem.Tests\LibraryManagementSystem.Tests.csproj"
Test-Tests "WeatherAPI.Tests" "WeatherAPI.Tests\WeatherAPI.Tests.csproj"
Test-Tests "FactorialCalculator.Tests" "FactorialCalculator.Tests\FactorialCalculator.Tests.csproj"
Test-Tests "ProducerConsumerPattern.Tests" "ProducerConsumerPattern.Tests\ProducerConsumerPattern.Tests.csproj"

Write-Host ""
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Regression Test Summary" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "Passed: $Passed" -ForegroundColor Green
Write-Host "Failed: $Failed" -ForegroundColor Red
Write-Host ""

if ($Failed -eq 0) {
    Write-Host "All regression tests passed!" -ForegroundColor Green
    exit 0
} else {
    Write-Host "Some regression tests failed!" -ForegroundColor Red
    exit 1
}

