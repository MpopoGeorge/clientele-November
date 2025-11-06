#!/bin/bash
# Regression Test Script for Clientele Projects
# This script runs all projects and tests to ensure everything works correctly

echo "=========================================="
echo "Clientele Projects - Regression Testing"
echo "=========================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

PASSED=0
FAILED=0

# Function to test a project build
test_build() {
    local project_name=$1
    local project_path=$2
    
    echo -e "${YELLOW}Testing build: $project_name${NC}"
    if dotnet build "$project_path" --verbosity quiet > /dev/null 2>&1; then
        echo -e "${GREEN}✓ $project_name builds successfully${NC}"
        ((PASSED++))
        return 0
    else
        echo -e "${RED}✗ $project_name build failed${NC}"
        ((FAILED++))
        return 1
    fi
}

# Function to test a test project
test_tests() {
    local test_name=$1
    local test_path=$2
    
    echo -e "${YELLOW}Running tests: $test_name${NC}"
    if dotnet test "$test_path" --verbosity quiet --no-build > /dev/null 2>&1; then
        echo -e "${GREEN}✓ $test_name tests passed${NC}"
        ((PASSED++))
        return 0
    else
        echo -e "${RED}✗ $test_name tests failed${NC}"
        ((FAILED++))
        return 1
    fi
}

echo "Step 1: Building all projects..."
echo "-----------------------------------"

# Build all main projects
test_build "LibraryManagementSystem" "LibraryManagementSystem/LibraryManagementSystem/LibraryManagementSystem.csproj"
test_build "FactorialCalculator" "FactorialCalculator/FactorialCalculator/FactorialCalculator.csproj"
test_build "ProducerConsumerPattern" "ProducerConsumerPattern/ProducerConsumerPattern/ProducerConsumerPattern.csproj"
test_build "EntityFrameworkCRUD" "EntityFrameworkCRUD/EntityFrameworkCRUD.csproj"
test_build "ProductAPI" "ProductAPI/ProductAPI.csproj"
test_build "WeatherAPI" "WeatherAPI/WeatherAPI.csproj"

echo ""
echo "Step 2: Building all test projects..."
echo "-----------------------------------"

# Build all test projects
test_build "EntityFrameworkCRUD.Tests" "EntityFrameworkCRUD.Tests/EntityFrameworkCRUD.Tests.csproj"
test_build "ProductAPI.Tests" "ProductAPI.Tests/ProductAPI.Tests.csproj"
test_build "LibraryManagementSystem.Tests" "LibraryManagementSystem.Tests/LibraryManagementSystem.Tests.csproj"
test_build "WeatherAPI.Tests" "WeatherAPI.Tests/WeatherAPI.Tests.csproj"
test_build "FactorialCalculator.Tests" "FactorialCalculator.Tests/FactorialCalculator.Tests.csproj"
test_build "ProducerConsumerPattern.Tests" "ProducerConsumerPattern.Tests/ProducerConsumerPattern.Tests.csproj"

echo ""
echo "Step 3: Running all unit tests..."
echo "-----------------------------------"

# Run all tests
test_tests "EntityFrameworkCRUD.Tests" "EntityFrameworkCRUD.Tests/EntityFrameworkCRUD.Tests.csproj"
test_tests "ProductAPI.Tests" "ProductAPI.Tests/ProductAPI.Tests.csproj"
test_tests "LibraryManagementSystem.Tests" "LibraryManagementSystem.Tests/LibraryManagementSystem.Tests.csproj"
test_tests "WeatherAPI.Tests" "WeatherAPI.Tests/WeatherAPI.Tests.csproj"
test_tests "FactorialCalculator.Tests" "FactorialCalculator.Tests/FactorialCalculator.Tests.csproj"
test_tests "ProducerConsumerPattern.Tests" "ProducerConsumerPattern.Tests/ProducerConsumerPattern.Tests.csproj"

echo ""
echo "=========================================="
echo "Regression Test Summary"
echo "=========================================="
echo -e "${GREEN}Passed: $PASSED${NC}"
echo -e "${RED}Failed: $FAILED${NC}"
echo ""

if [ $FAILED -eq 0 ]; then
    echo -e "${GREEN}✓ All regression tests passed!${NC}"
    exit 0
else
    echo -e "${RED}✗ Some regression tests failed!${NC}"
    exit 1
fi

