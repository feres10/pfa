# Configuration du Stack Monitoring E-Sante

## Vue d'ensemble

Ce guide explique comment configurer et utiliser Prometheus et Grafana pour monitorer votre application E-Sante.

## Structure du Stack

### Services inclus:
- **Prometheus** - Base de données time-series pour stocker les métriques
- **Grafana** - Plateforme de visualisation et d'alertes
- **Node Exporter** - Exporte les métriques système du serveur
- **cAdvisor** - Monitore les conteneurs Docker

## Installation des dépendances

### Backend (.NET)

Vous devez installer le package Prometheus client:

```bash
cd E_santeBackend
dotnet add package prometheus-net.AspNetCore
```

Le code a déjà été configuré dans `Program.cs`.

## Démarrage du Stack Monitoring

### Option 1: Démarrer tout ensemble (recommandé)

```bash
# Terminal 1: Démarrer l'application principale
docker-compose up -d

# Terminal 2: Démarrer le monitoring stack
cd monitoring
docker-compose up -d
```

### Option 2: Avec Compose V2 (une seule commande)

```bash
docker-compose -f docker-compose.yml -f monitoring/docker-compose.yml up -d
```

## Accès aux services

Une fois démarrés, accédez à:

- **Prometheus**: http://localhost:9090
  - Utilisé pour interroger les métriques brutes
  - Page d'accueil: http://localhost:9090
  - Métriques du backend: http://localhost:9090/api/v1/query?query=http_requests_total

- **Grafana**: http://localhost:3000
  - Utilisateur par défaut: `admin`
  - Mot de passe par défaut: `admin`
  - Dashboard disponible: "E-Sante System Monitoring"

- **Node Exporter**: http://localhost:9100/metrics
  - Métriques système brutes

- **cAdvisor**: http://localhost:8080
  - Interface web pour les métriques conteneurs

## Configuration Prometheus

Le fichier `prometheus/prometheus.yml` configure les jobs de scraping:

### Jobs configurés:

1. **prometheus** - Monitore Prometheus lui-même
2. **node-exporter** - Métriques système (CPU, RAM, Disk)
3. **cadvisor** - Métriques des conteneurs Docker
4. **esante-backend** - Votre API .NET (endpoint `/metrics`)
5. **esante-frontend** - Votre application Blazor
6. **postgres** - Base de données PostgreSQL (optionnel)

### Métriques exposées par le backend:

```
http_request_duration_seconds - Durée des requêtes HTTP
http_requests_total - Nombre total de requêtes
http_requests_created - Timestamp de création des métriques
process_* - Métriques du processus .NET
dotnet_* - Métriques .NET
```

## Alertes configurées

Le fichier `prometheus/alert.rules.yml` définit les alertes:

- **HighCPUUsage** - CPU > 80% pendant 5 minutes
- **HighMemoryUsage** - RAM > 85% pendant 5 minutes
- **HighDiskUsage** - Disque > 80% pendant 5 minutes
- **ContainerDown** - Un conteneur E-Sante est arrêté
- **HighResponseTime** - Temps de réponse p95 > 1 seconde
- **HighErrorRate** - Taux d'erreur 5xx > 5%

## Dashboards Grafana

### Dashboard par défaut: "E-Sante System Monitoring"

Contient les panneaux suivants:

1. **CPU Usage (%)** - Gauge montrant l'utilisation CPU
2. **Memory Usage (%)** - Gauge montrant l'utilisation RAM
3. **HTTP Requests Rate** - Graphique des requêtes par seconde
4. **Response Time (p95, p99)** - Percentiles de temps de réponse
5. **Backend Status** - Indicateur UP/DOWN du backend
6. **Frontend Status** - Indicateur UP/DOWN du frontend
7. **Prometheus Status** - Indicateur UP/DOWN de Prometheus

### Créer un nouveau dashboard:

1. Allez à http://localhost:3000/dashboard
2. Cliquez sur "Create" > "Dashboard"
3. Sélectionnez "Prometheus" comme datasource
4. Ajoutez vos panneaux avec les requêtes PromQL

## Requêtes PromQL utiles

```promql
# Taux de requêtes par seconde
rate(http_requests_total[5m])

# Temps de réponse moyen (5 minutes)
rate(http_request_duration_seconds_sum[5m]) / rate(http_request_duration_seconds_count[5m])

# 95ème percentile de temps de réponse
histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m]))

# Taux d'erreur 5xx
rate(http_requests_total{status=~"5.."}[5m]) / rate(http_requests_total[5m])

# CPU usage (en %)
100 - (avg(rate(node_cpu_seconds_total{mode="idle"}[5m])) * 100)

# Memory usage (en %)
(1 - (node_memory_MemAvailable_bytes / node_memory_MemTotal_bytes)) * 100

# Status des conteneurs
up{job=~"esante-.*"}
```

## Configuration du datasource Prometheus dans Grafana

Le datasource est configuré automatiquement:
- URL: http://prometheus:9090
- Access: Proxy
- Timeout: 60s

Si vous devez le modifier manuellement:
1. Allez à Configuration > Data Sources
2. Cherchez "Prometheus"
3. Modifiez les paramètres

## Persistence des données

- **Prometheus**: Données stockées dans le volume `prometheus_data` (15 jours de rétention)
- **Grafana**: Dashboards et datasources stockés dans le volume `grafana_data`

## Nettoyage

Pour arrêter le stack monitoring:

```bash
cd monitoring
docker-compose down

# Supprimer aussi les volumes (attention: supprime les données)
docker-compose down -v
```

## Dépannage

### Prometheus ne scrape pas le backend

1. Vérifiez que le backend répond à `/metrics`:
   ```bash
   curl http://backend:8080/metrics
   ```

2. Vérifiez les logs de Prometheus:
   ```bash
   docker logs esante_prometheus
   ```

3. Vérifiez la configuration dans `prometheus.yml`

### Grafana ne se connecte pas à Prometheus

1. Vérifiez que Prometheus est en cours d'exécution:
   ```bash
   docker ps | grep prometheus
   ```

2. Testez la connectivité:
   ```bash
   docker exec esante_grafana curl http://prometheus:9090
   ```

### Métriques manquantes

1. Assurez-vous que vous avez installé le package NuGet:
   ```bash
   dotnet list package | grep prometheus
   ```

2. Reconstruisez et relancez le backend

## Prochaines étapes

1. Customizer les dashboards selon vos besoins
2. Ajouter des alertes pour les KPIs importants
3. Intégrer avec des services d'alertes (Slack, Teams, etc.)
4. Ajouter des exporters supplémentaires (PostgreSQL, Redis, etc.)

## Ressources

- [Documentation Prometheus](https://prometheus.io/docs/)
- [Documentation Grafana](https://grafana.com/docs/)
- [Prometheus .NET Client](https://github.com/prometheus-net/prometheus-net)
- [PromQL Queries](https://prometheus.io/docs/prometheus/latest/querying/basics/)
