# GitHub Actions Deployment Setup

## Overview
This guide explains how to set up the GitHub Actions CI/CD pipeline for building and pushing Docker images to Docker Hub.

## Prerequisites

1. **GitHub Repository** - Push code to GitHub
2. **Docker Hub Account** - To host your Docker images
3. **GitHub Secrets** - Store credentials securely

## Setup Instructions

### Step 1: Generate Docker Hub Credentials

1. Go to [Docker Hub](https://hub.docker.com)
2. Log in with your account
3. Go to **Account Settings** → **Security**
4. Click **New Access Token**
5. Name it: `github-actions-token`
6. Copy the token (save it securely)

### Step 2: Add GitHub Secrets

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Add two secrets:

   **Secret 1: DOCKERHUB_USERNAME**
   - Name: `DOCKERHUB_USERNAME`
   - Value: Your Docker Hub username (e.g., `kmarksentini`)

   **Secret 2: DOCKERHUB_TOKEN**
   - Name: `DOCKERHUB_TOKEN`
   - Value: Your Docker Hub access token (from Step 1)

### Step 3: Push to GitHub

```powershell
# Configure Git
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"

# Add files
git add .

# Commit
git commit -m "Add GitHub Actions CI/CD pipeline"

# Push to main branch
git push origin main
```

## Workflow Triggers

The pipeline automatically runs on:

- ✅ Push to `main` branch
- ✅ Push to `develop` branch
- ✅ Pull requests to `main` branch
- ✅ Manual trigger (Workflow dispatch)

## Workflow Steps

1. **Checkout Code** - Pulls your repository
2. **Setup Docker Buildx** - Advanced Docker build capabilities
3. **Login to Docker Hub** - Authenticates using secrets
4. **Build & Push Backend** - Builds and pushes backend image
5. **Build & Push Frontend** - Builds and pushes frontend image
6. **Test** - Tests images using docker-compose
7. **Notify** - Reports success or failure

## Image Tags

Images are tagged automatically:
- `latest` - Latest on main branch
- `develop` - Latest develop branch
- `main` - Main branch tag
- `sha-<commit>` - Specific commit SHA
- `v1.0.0` - Semantic version (if using git tags)

## Monitoring

### View Workflow Runs
1. Go to your GitHub repo
2. Click **Actions** tab
3. Select **Build and Push Docker Images**
4. View logs in real-time

### Check Docker Hub
- Visit: `https://hub.docker.com/u/kmarksentini`
- See all pushed images and tags

## Troubleshooting

### Workflow Fails to Start
- Ensure `.github/workflows/deploy.yml` is committed and pushed
- Check that secrets are added correctly

### Docker Push Fails
```
ERROR: failed to connect to the docker API
```
**Solution:** Verify `DOCKERHUB_USERNAME` and `DOCKERHUB_TOKEN` are correct

### Image Not Appearing on Docker Hub
- Check workflow logs for errors
- Verify Docker Hub credentials are valid
- Ensure `push: true` is set in build step

### How to Debug
1. Add to workflow for more verbose output:
   ```yaml
   - name: Debug
     run: echo "Backend image tag - ${{ steps.meta-backend.outputs.tags }}"
   ```

## Security Best Practices

✅ **DO:**
- Store credentials in GitHub Secrets
- Use short-lived access tokens
- Regularly rotate tokens
- Limit token permissions

❌ **DON'T:**
- Commit credentials to repository
- Share tokens in messages
- Use personal credentials
- Store passwords in code

## Advanced Configuration

### Add Slack Notifications
```yaml
- name: Notify Slack
  uses: 8398a7/action-slack@v3
  if: always()
  with:
    status: ${{ job.status }}
    webhook_url: ${{ secrets.SLACK_WEBHOOK }}
```

### Add SonarQube Code Quality
```yaml
- name: SonarQube Scan
  uses: sonarsource/sonarcloud-github-action@master
  env:
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
    SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
```

### Schedule Builds
Add to top of workflow:
```yaml
on:
  schedule:
    - cron: '0 2 * * *'  # Daily at 2 AM UTC
```

## Useful Commands

### View Workflow Status
```bash
# Requires GitHub CLI
gh workflow list
gh workflow view "Build and Push Docker Images"
```

### Manually Trigger Workflow
```bash
gh workflow run deploy.yml
```

### View Recent Runs
```bash
gh run list -w deploy.yml
```

---

For more information, see:
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Docker Build Push Action](https://github.com/docker/build-push-action)
- [Docker Hub Documentation](https://docs.docker.com/docker-hub/)
