#!/bin/bash

# Find and remove all obj and bin directories
echo "Cleaning obj and bin directories..."

# Find and remove obj directories
find . -type d -name "obj" -exec rm -rf {} +

# Find and remove bin directories
find . -type d -name "bin" -exec rm -rf {} +

echo "Clean complete!" 