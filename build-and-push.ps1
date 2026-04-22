#!/usr/bin/env pwsh
# E-Sante Docker Build and Push Script
# This script builds and pushes images to Docker Hub

param(
    [string]$DockerHubUsername = "kmarksentini",
    [string]$TagVersion = "latest"
)

$ErrorActionPreference = "Stop"

Write-Host "🐳 E-Sante Docker Build & Push Script" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Configuration
$BackendImageName = "$DockerHubUsername/esante-backend"
$FrontendImageName = "$DockerHubUsername/esante-frontend"
$BackendTag = "$BackendImageName`:$TagVersion"
$FrontendTag = "$FrontendImageName`:$TagVersion"

Write-Host "📦 Configuration:" -ForegroundColor Green
Write-Host "  Backend:  $BackendTag"
Write-Host "  Frontend: $FrontendTag"
Write-Host ""

# Build Backend
Write-Host "🔨 Building Backend Image..." -ForegroundColor Yellow
docker build -t $BackendTag -f E_santeBackend/Dockerfile E_santeBackend
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Backend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Backend built successfully!" -ForegroundColor Green
Write-Host ""

# Build Frontend
Write-Host "🔨 Building Frontend Image..." -ForegroundColor Yellow
docker build -t $FrontendTag -f E_santeFrontend_blazor/Dockerfile E_santeFrontend_blazor
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Frontend build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Frontend built successfully!" -ForegroundColor Green
Write-Host ""

# Push Backend
Write-Host "📤 Pushing Backend Image to Docker Hub..." -ForegroundColor Yellow
docker push $BackendTag
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Backend push failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Backend pushed successfully!" -ForegroundColor Green
Write-Host ""

# Push Frontend
Write-Host "📤 Pushing Frontend Image to Docker Hub..." -ForegroundColor Yellow
docker push $FrontendTag
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Frontend push failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Frontend pushed successfully!" -ForegroundColor Green
Write-Host ""

# Display summary
Write-Host "✅ Build & Push Complete!" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📍 Image URLs:" -ForegroundColor Green
Write-Host "  Backend:  https://hub.docker.com/r/$BackendImageName"
Write-Host "  Frontend: https://hub.docker.com/r/$FrontendImageName"
Write-Host ""
Write-Host "💡 To use these images:" -ForegroundColor Green
Write-Host "  docker pull $BackendTag"
Write-Host "  docker pull $FrontendTag"
Write-Host ""
