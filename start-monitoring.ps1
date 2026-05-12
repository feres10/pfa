#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Script de démarrage du stack complet E-Sante avec monitoring

.DESCRIPTION
    Ce script démarre l'application E-Sante et le stack de monitoring (Prometheus + Grafana)

.PARAMETER Monitoring
    Démarrer aussi le stack de monitoring (Prometheus + Grafana)

.PARAMETER Down
    Arrêter tous les services

.PARAMETER Logs
    Afficher les logs en temps réel

.EXAMPLE
    .\start-monitoring.ps1 -Monitoring
    Démarre l'application et le monitoring

    .\start-monitoring.ps1 -Down
    Arrête tous les services

    .\start-monitoring.ps1 -Logs
    Affiche les logs en direct
#>

param(
    [switch]$Monitoring,
    [switch]$Down,
    [switch]$Logs,
    [switch]$Build
)

$ErrorActionPreference = "Stop"

function Write-Header {
    param([string]$Message)
    Write-Host "`n========================================" -ForegroundColor Cyan
    Write-Host $Message -ForegroundColor Cyan
    Write-Host "========================================`n" -ForegroundColor Cyan
}

function Test-Docker {
    Write-Header "Vérification de Docker..."
    
    try {
        $null = docker --version
        Write-Host "✓ Docker est installé" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Docker n'est pas installé ou n'est pas dans le PATH" -ForegroundColor Red
        exit 1
    }

    try {
        $null = docker ps
        Write-Host "✓ Docker daemon est en cours d'exécution" -ForegroundColor Green
    }
    catch {
        Write-Host "✗ Docker daemon n'est pas en cours d'exécution" -ForegroundColor Red
        exit 1
    }
}

function Start-Services {
    Write-Header "Démarrage des services..."

    Write-Host "Démarrage de l'application principale..." -ForegroundColor Yellow
    docker-compose up -d
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "✗ Erreur lors du démarrage des services" -ForegroundColor Red
        exit 1
    }

    Write-Host "✓ Services principaux démarrés" -ForegroundColor Green

    if ($Monitoring) {
        Write-Host "`nDémarrage du stack de monitoring..." -ForegroundColor Yellow
        Push-Location monitoring
        docker-compose up -d
        Pop-Location
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "✗ Erreur lors du démarrage du monitoring" -ForegroundColor Red
            exit 1
        }

        Write-Host "✓ Stack de monitoring démarré" -ForegroundColor Green
    }
}

function Stop-Services {
    Write-Header "Arrêt des services..."

    if ($Monitoring) {
        Write-Host "Arrêt du monitoring..." -ForegroundColor Yellow
        Push-Location monitoring
        docker-compose down
        Pop-Location
    }

    Write-Host "Arrêt des services principaux..." -ForegroundColor Yellow
    docker-compose down

    Write-Host "✓ Tous les services sont arrêtés" -ForegroundColor Green
}

function Show-Logs {
    Write-Header "Logs en temps réel..."
    
    if ($Monitoring) {
        Write-Host "`nDémarrage en mode logs avec monitoring" -ForegroundColor Yellow
        docker-compose -f docker-compose.yml -f monitoring/docker-compose.yml logs -f
    }
    else {
        Write-Host "`nDémarrage en mode logs" -ForegroundColor Yellow
        docker-compose logs -f
    }
}

function Show-Status {
    Write-Header "Status des services"
    
    docker-compose ps
    
    if ($Monitoring) {
        Write-Host "`nMonitoring Status:" -ForegroundColor Cyan
        Push-Location monitoring
        docker-compose ps
        Pop-Location
    }
}

function Show-URLs {
    Write-Header "Accès aux services"
    
    Write-Host "Application principale:"
    Write-Host "  Backend API:  http://localhost:5139" -ForegroundColor Green
    Write-Host "  Frontend:     http://localhost:5159" -ForegroundColor Green
    Write-Host "  PostgreSQL:   localhost:5432" -ForegroundColor Green
    
    if ($Monitoring) {
        Write-Host "`nMonitoring:"
        Write-Host "  Prometheus:   http://localhost:9090" -ForegroundColor Green
        Write-Host "  Grafana:      http://localhost:3000" -ForegroundColor Green
        Write-Host "    Login:      admin / admin" -ForegroundColor Yellow
        Write-Host "  Node Exporter: http://localhost:9100/metrics" -ForegroundColor Green
        Write-Host "  cAdvisor:     http://localhost:8080" -ForegroundColor Green
    }
}

# Main logic
if (-not $Build -and -not $Down -and -not $Logs) {
    Test-Docker
    Start-Services
    Show-Status
    Show-URLs
    Write-Host "`nUtilisez -Logs pour voir les logs en temps réel" -ForegroundColor Yellow
    Write-Host "Utilisez -Down pour arrêter les services" -ForegroundColor Yellow
}
elseif ($Build) {
    Write-Header "Build de l'application..."
    
    Test-Docker
    
    if ($Monitoring) {
        Write-Host "Build des images Docker..." -ForegroundColor Yellow
        docker-compose build
        docker-compose -f monitoring/docker-compose.yml build
    }
    else {
        docker-compose build
    }
    
    Write-Host "✓ Build complété" -ForegroundColor Green
}
elseif ($Down) {
    Stop-Services
}
elseif ($Logs) {
    if ($Monitoring) {
        Start-Services -Monitoring
    }
    else {
        Start-Services
    }
    Show-Logs
}

Write-Host "`nDone! ✓" -ForegroundColor Green
