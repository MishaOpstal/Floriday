#!/bin/bash
set -e

# Ensure the mount point for named volume exists
mkdir -p /app/node_modules

cd /app

echo "📦 Checking node_modules..."
if [ ! -d node_modules ] || [ -z "$(ls -A node_modules)" ]; then
    echo "📦 node_modules missing or empty. Installing..."
    npm install
else
    echo "🔍 Verifying installed packages..."
    npm install --prefer-offline
fi

echo "🚀 Starting NPM dev server..."
exec npm run dev
