#!/usr/bin/env bash
# Restaura um dump gerado por backup-postgres.sh.
# Uso: ./deploy/scripts/restore-postgres.sh backups/comandax-20260729-120000.sql.gz
#
# DESTRUTIVO: apaga o schema atual antes de restaurar.
set -euo pipefail

cd "$(dirname "$0")/../.."
export COMPOSE_FILE=docker-compose.prod.yml

DUMP="${1:?uso: restore-postgres.sh <arquivo.sql.gz>}"
[ -f "$DUMP" ] || { echo "Arquivo não encontrado: $DUMP" >&2; exit 1; }

# shellcheck disable=SC1091
set -a; source .env; set +a

DB_USER="${POSTGRES_USER:-comandax}"
DB_NAME="${POSTGRES_DB:-comandax}"

echo "!! Isto APAGA todos os dados de '$DB_NAME' e restaura '$DUMP'."
read -r -p "Digite o nome do banco para confirmar: " CONFIRM
[ "$CONFIRM" = "$DB_NAME" ] || { echo "Cancelado."; exit 1; }

# A API mantém conexões abertas; pare antes de recriar o schema.
echo "==> Parando a API..."
docker compose stop api

echo "==> Recriando o schema..."
docker compose exec -T db psql -U "$DB_USER" -d "$DB_NAME" \
    -c "DROP SCHEMA public CASCADE; CREATE SCHEMA public;"

echo "==> Restaurando..."
gunzip -c "$DUMP" | docker compose exec -T db psql -U "$DB_USER" -d "$DB_NAME"

echo "==> Subindo a API..."
docker compose up -d api

echo "==> Restauração concluída."
