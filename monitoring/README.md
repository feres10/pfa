# E-Sante Monitoring Stack

Stack de monitoring pour l'application E-Sante avec **Prometheus** et **Grafana**.

## Structure

```
monitoring/
├── docker-compose.yml          # Configuration Docker des services
├── prometheus/
│   ├── prometheus.yml          # Configuration Prometheus (scrape jobs)
│   └── alert.rules.yml         # Règles d'alertes
└── grafana/
    └── provisioning/
        ├── datasources/
        │   └── prometheus.yml  # Configuration du datasource Prometheus
        └── dashboards/
            ├── dashboard.yml   # Configuration des dashboards
            └── esante-dashboard.json  # Dashboard principal
```

## Démarrage rapide

### Mode 1: Ligne de commande

```bash
# Démarrer seulement le monitoring
cd monitoring
docker-compose up -d

# Accéder à Grafana
# http://localhost:3000
# Login: admin / admin
```

### Mode 2: Avec le script PowerShell

```powershell
# Démarrer tout avec monitoring
.\start-monitoring.ps1 -Monitoring

# Voir les logs
.\start-monitoring.ps1 -Logs -Monitoring

# Arrêter
.\start-monitoring.ps1 -Down
```

## Services inclus

| Service | Port | URL |
|---------|------|-----|
| Prometheus | 9090 | http://localhost:9090 |
| Grafana | 3000 | http://localhost:3000 |
| Node Exporter | 9100 | http://localhost:9100 |
| cAdvisor | 8080 | http://localhost:8080 |

## Configuration Prometheus

Le fichier `prometheus/prometheus.yml` scrape les métriques de:
- Backend API (endpoint `/metrics`)
- Frontend (santé générale)
- Node Exporter (CPU, RAM, Disk)
- cAdvisor (métriques conteneurs Docker)
- PostgreSQL (optionnel)

## Dashboards Grafana

Le dashboard `esante-dashboard.json` est provisionné automatiquement avec:
- CPU/Memory gauges
- HTTP requests rate
- Response time (p95, p99)
- Container status indicators

## Alertes

Les alertes sont définies dans `prometheus/alert.rules.yml`:
- CPU > 80% pendant 5 min
- RAM > 85% pendant 5 min
- Disque > 80%
- Conteneurs arrêtés
- Erreurs 5xx élevées

## Pour plus d'infos

Consultez le fichier [MONITORING_SETUP.md](../MONITORING_SETUP.md) à la racine du projet.
