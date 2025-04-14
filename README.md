
# Setup
## ensure docker 
https://docs.docker.com/desktop/

## run kubernetes
follow instructions here https://minikube.sigs.k8s.io/docs/start

## get lens for k8s
https://k8slens.dev/download

## deploy open-match
```cmd
helm repo add open-match https://open-match.dev/chart/stable
helm repo update
helm install open-match --create-namespace --namespace open-match open-match/open-match \
  --set open-match-customize.enabled=true \
  --set open-match-customize.evaluator.enabled=true \
  --set open-match-override.enabled=true \
  --set query.replicas=1 \
  --set frontend.replicas=1 \
  --set backend.replicas=1 \
```

## Build Images
```cmd
docker build -f .\src\backend\Gg.Demo.Backend.Host\Dockerfile -t gg-demo-backend:latest .; minikube image load gg-demo-backend:latest

docker build -f .\src\backend\Gg.Demo.FrontEnd.Host\Dockerfile -t gg-demo-frontend:latest .; minikube image load gg-demo-frontend:latest

docker build -f .\src\backend\Gg.Demo.Matchmaking.Director\Dockerfile -t gg-demo-mm-director:latest .; minikube image load gg-demo-mm-director:latest

docker build -f .\src\backend\Gg.Demo.Matchmaking.Director\Dockerfile -t gg-demo-mm-function:latest .; minikube image load gg-demo-mm-function:latest
```

## Deployment

### Prerequisites

- Minikube installed and running
- Helm installed
- Docker installed and configured to use Minikube's Docker daemon

### Building Docker Images

First, build the Docker images for all services:

```bash
# Build backend image
docker build -t gg-demo-backend:latest -f src/backend/Gg.Demo.Backend.Host/Dockerfile .; minikube image load gg-demo-backend:latest

# Build frontend image
docker build -t gg-demo-frontend:latest -f src/backend/Gg.Demo.FrontEnd.Host/Dockerfile .; minikube image load gg-demo-frontend:latest

# Build director image
docker build -t gg-demo-director:latest -f src/backend/Gg.Demo.Matchmaking.Director/Dockerfile .
```

### Deploying with Helm

1. Add the OpenMatch Helm repository and install OpenMatch:

```bash
helm repo add open-match https://open-match.dev/chart/stable
helm repo update
helm install open-match open-match/open-match \
  --create-namespace \
  --namespace open-match \
  --set open-match-override.enabled=true \
  --set open-match-override.open-match-core.enabled=true
```

2. Deploy the Backend service:

```bash
helm install backend deployment/helm/backend -f deployment/minikube/backend-values.yaml --create-namespace --namespace demo-game
```

3. Deploy the Frontend service:

```bash
helm install frontend deployment/helm/frontend -f deployment/minikube/frontend-values.yaml --create-namespace --namespace demo-game
```

4. Deploy the Matchmaking Director service:

```bash
helm install director deployment/helm/mm-director -f deployment/minikube/mm-director-values.yaml --create-namespace --namespace demo-matchamaker
```

5. Deploy the Matchmaker Function service:

```bash
helm install director deployment/helm/mm-function -f deployment/minikube/mm-function-values.yaml --create-namespace --namespace demo-matchamaker
```

### Accessing the Services

After deployment, you can access the services using port-forwarding:

```bash
# Forward frontend service
kubectl port-forward service/frontend 5000:80 -n game-demo

# Forward backend service
kubectl port-forward service/backend 5001:80 -n game-demo

# Forward OpenMatch dashboard
kubectl port-forward service/open-match-dashboard 51500:51500 -n open-match
```

### Updating Deployments

To update a deployment after making changes:

```bash
# Update backend
helm upgrade backend deployment/helm/backend -f deployment/minikube/backend-values.yaml

# Update frontend
helm upgrade frontend deployment/helm/frontend -f deployment/minikube/frontend-values.yaml

# Update director
helm upgrade director deployment/helm/mm-director -f deployment/minikube/mm-director-values.yaml
```

### Uninstalling

To uninstall all components:

```bash
# Uninstall game services
helm uninstall backend -n demo-game
helm uninstall frontend -n demo-game
helm uninstall mm-director -n demo-matchmaker
helm uninstall mm-function -n demo-matchmaker

# Uninstall OpenMatch
helm uninstall open-match -n open-match
``` 