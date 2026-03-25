#!/bin/bash
# ──────────────────────────────────────────────────────────────
# deploy-swarm.sh – Build, push, and deploy to Docker Swarm
# Usage:
#   ./deploy-swarm.sh              # first deploy or full redeploy
#   ./deploy-swarm.sh update       # rolling update only
# ──────────────────────────────────────────────────────────────

set -euo pipefail

STACK_NAME="ebay"
COMPOSE_FILE="docker-compose.prod.yml"
REGISTRY="${REGISTRY:-localhost:5000}"
TAG="${TAG:-latest}"

export REGISTRY TAG

echo "══════════════════════════════════════════════"
echo "  eBay Clone – Swarm Deployment"
echo "  Registry : $REGISTRY"
echo "  Tag      : $TAG"
echo "══════════════════════════════════════════════"

# ── Step 0: Ensure Swarm mode is active ───────────────────
if ! docker info --format '{{.Swarm.LocalNodeState}}' 2>/dev/null | grep -q active; then
    echo "⚙  Initializing Docker Swarm..."
    docker swarm init --advertise-addr $(hostname -I | awk '{print $1}') 2>/dev/null || true
fi

# ── Step 1: Build images ─────────────────────────────────
echo ""
echo "🔨 Building images..."
docker compose -f "$COMPOSE_FILE" build

# ── Step 2: Tag & push to registry (if not localhost) ────
if [[ "$REGISTRY" != "localhost:5000" ]]; then
    echo ""
    echo "📦 Pushing images to $REGISTRY..."
    docker compose -f "$COMPOSE_FILE" push
fi

# ── Step 3: Deploy / Update the stack ─────────────────────
echo ""
if [[ "${1:-}" == "update" ]]; then
    echo "🔄 Rolling update..."
    docker stack deploy -c "$COMPOSE_FILE" "$STACK_NAME" --with-registry-auth
else
    echo "🚀 Deploying stack '$STACK_NAME'..."
    docker stack deploy -c "$COMPOSE_FILE" "$STACK_NAME" --with-registry-auth
fi

# ── Step 4: Show status ──────────────────────────────────
echo ""
echo "📊 Stack services:"
docker stack services "$STACK_NAME"

echo ""
echo "✅ Deployment complete!"
echo ""
echo "Useful commands:"
echo "  docker stack services $STACK_NAME          # list services"
echo "  docker service logs ${STACK_NAME}_api -f   # follow API logs"
echo "  docker service scale ${STACK_NAME}_api=3   # scale API to 3"
echo "  docker stack rm $STACK_NAME                # tear down"
