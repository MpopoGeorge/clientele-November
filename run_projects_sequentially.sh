#!/bin/bash
# Run all projects sequentially for manual testing

echo "=========================================="
echo "Running Projects Sequentially"
echo "=========================================="
echo ""
echo "Press Ctrl+C to stop each project when done testing"
echo ""

# Function to run a project
run_project() {
    local project_name=$1
    local project_path=$2
    
    echo "=========================================="
    echo "Running: $project_name"
    echo "=========================================="
    echo "Press Ctrl+C to stop and move to next project"
    echo ""
    
    cd "$project_path" || exit 1
    dotnet run
    cd - > /dev/null || exit 1
    
    echo ""
    echo "Stopped: $project_name"
    echo ""
    read -p "Press Enter to continue to next project..."
    echo ""
}

# Run projects sequentially
run_project "LibraryManagementSystem" "LibraryManagementSystem/LibraryManagementSystem"
run_project "FactorialCalculator" "FactorialCalculator/FactorialCalculator"
run_project "ProducerConsumerPattern" "ProducerConsumerPattern/ProducerConsumerPattern"
run_project "EntityFrameworkCRUD" "EntityFrameworkCRUD"
run_project "ProductAPI" "ProductAPI"
run_project "WeatherAPI" "WeatherAPI"

echo "=========================================="
echo "All projects completed!"
echo "=========================================="

