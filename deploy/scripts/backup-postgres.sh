#!/usr/bin/env bash
# Backup do banco do ComandaX (dump comprimido em ./backups).
# Uso: ./deploy/scripts/backup-postgres.sh [diretorio-destino]
set -euo pipefail

cd "$(dirname "$0")/../.."
export COMPOSE_FILE=docker-compose.yml

BACKUP_DIR="${1:-./backups}"
mkdir -p "$BACKUP_DIR"

# shellcheck disable=SC1091
set -a; source .env; set +a

DB_USER="${POSTGRES_USER:-comandax}"
DB_NAME="${POSTGRES_DB:-comandax}"
STAMP="$(date +%Y%m%d-%H%M%S)"
OUT="$BACKUP_DIR/comandax-$STAMP.sql.gz"

echo "==> Gerando dump de '$DB_NAME'..."
docker compose exec -T db pg_dump -U "$DB_USER" -d "$DB_NAME" | gzip > "$OUT"

# pg_dump falhando ainda produziria um .gz pequeno e válido; conferir o tamanho
# evita guardar um backup vazio achando que deu certo.
SIZE=$(wc -c < "$OUT")
if [ "$SIZE" -lt 1000 ]; then
    echo "ERRO: backup suspeito (${SIZE} bytes). Confira 'docker compose ps db'." >&2
    rm -f "$OUT"
    exit 1
fi

echo "==> Backup salvo: $OUT ($(du -h "$OUT" | cut -f1))"

# Retenção: mantém os 14 dumps mais recentes.
ls -1t "$BACKUP_DIR"/comandax-*.sql.gz 2>/dev/null | tail -n +15 | xargs -r rm --
echo "==> Backups mantidos: $(ls -1 "$BACKUP_DIR"/comandax-*.sql.gz 2>/dev/null | wc -l)"
