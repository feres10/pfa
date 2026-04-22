# E-Sante Platform - Docker Compose Setup

## Overview
This docker-compose configuration sets up the complete E-Sante Platform with:
- **PostgreSQL 16** - Database
- **E-Sante Backend** - ASP.NET Core API (Port 5139)
- **E-Sante Frontend** - Blazor Web App (Port 5159)

## Prerequisites
- Docker Engine 20.10+
- Docker Compose 2.0+

## Quick Start

### 1. Build and Start All Services
```bash
docker-compose up -d --build
```

### 2. Check Service Status
```bash
docker-compose ps
```

### 3. View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres
```

## Accessing Services

| Service | URL | Purpose |
|---------|-----|---------|
| Frontend | http://localhost:5159 | Web Application |
| Backend API | http://localhost:5139 | API Endpoints |
| Database | localhost:5432 | PostgreSQL Connection |

## Database Information

- **Host**: postgres (within container network)
- **Port**: 5432
- **Database**: EHealthDb
- **Username**: postgres
- **Password**: Feres12345!

## Configuration

### Environment Variables
Modify `docker-compose.yml` to change:
- Database credentials
- JWT settings
- API URLs
- Environment profiles

### Database Persistence
PostgreSQL data is stored in the `postgres_data` volume and persists across container restarts.

## Common Commands

### Stop Services
```bash
docker-compose down
```

### Stop and Remove Volumes (Reset Database)
```bash
docker-compose down -v
```

### Rebuild Services
```bash
docker-compose build --no-cache
```

### Run in Foreground (View Logs Directly)
```bash
docker-compose up
```

### Execute Commands in Container
```bash
# Backend
docker-compose exec backend /bin/bash

# Frontend
docker-compose exec frontend /bin/bash

# Database
docker-compose exec postgres psql -U postgres -d EHealthDb
```

## Troubleshooting

### Port Already in Use
If ports 5139, 5159, or 5432 are already in use, modify the port mappings in `docker-compose.yml`:
```yaml
ports:
  - "NEW_PORT:8080"  # for backend/frontend
  - "NEW_PORT:5432"  # for postgres
```

### Database Connection Issues
Ensure PostgreSQL is healthy:
```bash
docker-compose exec postgres pg_isready -U postgres
```

### Clear Everything and Restart
```bash
docker-compose down -v
docker system prune -a
docker-compose up -d --build
```

## Performance Tips

1. **Use BuildKit** for faster builds:
   ```bash
   DOCKER_BUILDKIT=1 docker-compose build
   ```

2. **Resource Limits** - Edit docker-compose.yml to add:
   ```yaml
   services:
     backend:
       deploy:
         resources:
           limits:
             cpus: '0.5'
             memory: 512M
   ```

3. **Production Considerations**:
   - Change JWT secret key
   - Use environment-specific configurations
   - Enable HTTPS with proper certificates
   - Implement backup strategy for postgres_data volume

## Security Notes

⚠️ **Important**: The credentials in `docker-compose.yml` are for development only.

For production:
- Use Docker secrets management
- Implement container registry authentication
- Use environment variable files with proper permissions
- Enable network policies and firewalls
- Regularly update base images

## Network Architecture

All services communicate through the `esante_network` bridge network:
- Services can resolve each other by hostname
- Backend connects to `postgres:5432` (not localhost)
- Frontend connects to `backend:8080` (not localhost)

---
For more information, refer to [Docker Documentation](https://docs.docker.com/) and [Docker Compose Documentation](https://docs.docker.com/compose/)
