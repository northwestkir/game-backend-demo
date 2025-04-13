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
