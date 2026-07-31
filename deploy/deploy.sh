#!/usr/bin/env bash
# Deploy/atualização do ComandaX na VPS.
# Uso: ./deploy/deploy.sh [--no-backup]   (a partir de qualquer diretório)
#
# Diferente do ChaLive, o ComandaX são DOIS repositórios git (API e front)
# clonados lado a lado. Este script mora dentro do repo da API e orquestra
# os dois: docker-compose.prod.yml (também neste repo) builda a API a partir
# daqui e o front a partir de "../comandax-front".
set -euo pipefail

cd "$(dirname "$0")/.."
export COMPOSE_FILE=docker-compose.prod.yml

[ -f .env ] || { echo "ERRO: .env não encontrado. Copie de .env.example." >&2; exit 1; }

# A API roda as migrations no boot; um backup antes torna o rollback possível
# se uma migration se comportar de forma inesperada com os dados reais.
if [ "${1:-}" != "--no-backup" ] && docker compose ps --status running db | grep -q comandax-db; then
    echo "==> Backup do banco antes de migrar..."
    ./deploy/scripts/backup-postgres.sh
fi

echo "==> Atualizando código (git pull nos dois repositórios)..."
git pull --ff-only
git -C ../comandax-front pull --ff-only

echo "==> Build das imagens..."
docker compose build --pull

echo "==> Subindo containers..."
docker compose up -d

echo "==> Limpando imagens antigas..."
docker image prune -f

echo "==> Status:"
docker compose ps

echo "==> Deploy concluído."
