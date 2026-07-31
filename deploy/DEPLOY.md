# Deploy do ComandaX em produção

Guia para publicar o ComandaX na mesma VPS que já hospeda o Agende, o Dopabet e
a ChaLive (Nginx + Docker + Certbot no host), migrando do Render.

## Arquitetura

Mesmo modelo da ChaLive: **tudo roda em Docker Compose** e o Nginx do host só
faz TLS e proxy reverso. Deploy vira um comando só.

```
Internet ──HTTPS──► Nginx do host (TLS/certbot, vhost comandax.conf)
                         │ proxy → 127.0.0.1:8110
                         ▼
                 [container comandax-web]  nginx:alpine
                   ├─ serve o build do Angular (SPA)
                   ├─ /graphql ──proxy──►┐
                   └─ /api/*   ──proxy──►│ rede interna do Compose
                                         ▼
                 [container comandax-api]  .NET 8 (GraphQL/Hot Chocolate), porta 8080 interna
                                         │
                                         ▼
                 [container comandax-db]  postgres:17 (volume pgdata)
```

Pontos importantes:

- **Dois repositórios git, sem um terceiro para deploy** — diferente dos
  outros projetos, `ComandaX-API` e `comandax-front` são repositórios
  separados clonados lado a lado. Os arquivos de orquestração
  (`docker-compose.prod.yml`, `.env.example`, `deploy/`) moram **dentro do
  repo da API**, na raiz. O compose builda a API a partir de `.` e o front a
  partir de `../comandax-front` (repo irmão). O `deploy.sh` dá `git pull` nos
  dois repositórios.
- **O banco não é exposto** — o serviço `db` não publica porta nenhuma; só a
  API o alcança pela rede interna. Para inspecionar:
  `docker compose exec db psql -U comandax -d comandax`.
- **Porta 8110 no loopback** — o container web só escuta em `127.0.0.1`; quem
  atende a internet é o Nginx do host. 8080 é do `agende-api`, 8090 do
  `dopabet-web`, 8100 da `chalive`.
- **Mesma origem, sem CORS** — o front chama `/graphql` como caminho relativo e
  o nginx do container repassa para a API. A lista `Cors:AllowedOrigins` do
  `appsettings.json` só importa se algum cliente externo consumir a API direto.
- **Migrations no boot** — a API aplica as migrations do EF ao subir, então o
  deploy não tem passo manual de banco. O `deploy.sh` faz backup antes,
  justamente por causa disso.
- **Login com Google** — o domínio de produção precisa estar autorizado no
  Google Cloud Console (Credentials → OAuth 2.0 Client → Authorized JavaScript
  origins). Sem isso o botão de login não renderiza.

## Arquivos de deploy

Todos vivem dentro do repo `ComandaX-API` (não há um terceiro repo só de
deploy):

| Arquivo                               | Função                                                |
| -------------------------------------- | ----------------------------------------------------- |
| `ComandaX-API/Dockerfile`              | Build multi-stage da API (.NET 8, não-root, curl)     |
| `comandax-front/Dockerfile`            | Build do Angular + nginx servindo o SPA (repo irmão)  |
| `comandax-front/nginx.conf`            | Nginx interno: SPA + proxy `/graphql` e `/api`        |
| `comandax-front/security-headers.inc`  | CSP e cabeçalhos de segurança do SPA                  |
| `ComandaX-API/docker-compose.prod.yml` | Orquestra `db` + `api` + `web`                        |
| `ComandaX-API/.env.example`            | Modelo do `.env` (porta, banco, segredos)             |
| `ComandaX-API/deploy/nginx/comandax.conf` | Vhost do Nginx do **host** (TLS via certbot)       |
| `ComandaX-API/deploy/deploy.sh`        | Script de atualização (backup + pull + build + up)    |
| `ComandaX-API/deploy/scripts/backup-postgres.sh`  | Dump comprimido em `./backups` (retém os 14 últimos) |
| `ComandaX-API/deploy/scripts/restore-postgres.sh` | Restaura um dump (destrutivo, pede confirmação)      |

Os scripts em `deploy/` já exportam `COMPOSE_FILE=docker-compose.prod.yml`
antes de chamar `docker compose`, então funcionam sozinhos. Para comandos
manuais (seções abaixo), rode uma vez por sessão de shell, dentro de
`~/comandax/ComandaX-API`:

```bash
export COMPOSE_FILE=docker-compose.prod.yml
```

ou passe `-f docker-compose.prod.yml` em cada comando.

## Primeiro deploy (passo a passo)

### 1. DNS

Crie um registro **A** do domínio apontando para o IP da VPS. Aguarde propagar
antes de rodar o certbot.

### 2. Montar a estrutura na VPS

```bash
ssh usuario@sua-vps
mkdir -p ~/comandax && cd ~/comandax
git clone <url-do-repo-da-api> ComandaX-API
git clone <url-do-repo-do-front> comandax-front
cd ComandaX-API
```

Os arquivos de orquestração (`docker-compose.prod.yml`, `.env.example`,
`deploy/`) já vêm com o clone do `ComandaX-API` — não precisa copiar nada à
parte nem criar um repo extra. Dali em diante, todo comando deste guia roda
de dentro de `~/comandax/ComandaX-API`.

```bash
chmod +x deploy/deploy.sh deploy/scripts/*.sh
export COMPOSE_FILE=docker-compose.prod.yml
```

### 3. Configurar o `.env`

```bash
cp .env.example .env
nano .env
```

Gere os segredos:

```bash
openssl rand -base64 24   # POSTGRES_PASSWORD
openssl rand -base64 48   # Jwt__Key
```

Preencha também `RESEND_APITOKEN` (envio do recibo por e-mail; sem ele o app
funciona, mas o envio de e-mail falha).

> Atenção: `POSTGRES_PASSWORD` só tem efeito quando o volume do banco é criado
> pela primeira vez. Trocar depois não muda a senha do banco existente.

### 4. Subir os containers

```bash
docker compose up -d --build
```

O primeiro build demora alguns minutos (SDK do .NET + `npm ci`). Verifique:

```bash
docker compose ps                          # db e api devem ficar "healthy"
curl http://127.0.0.1:8110/                # HTML do SPA
curl -X POST http://127.0.0.1:8110/graphql \
  -H 'Content-Type: application/json' \
  -d '{"query":"{ __typename }"}'          # prova que o proxy alcança a API
```

Se a API não subir, quase sempre é `.env`: `docker compose logs api`.

### 5. Configurar o Nginx do host

```bash
sudo cp deploy/nginx/comandax.conf /etc/nginx/sites-available/comandax.conf
sudo nano /etc/nginx/sites-available/comandax.conf   # troque SEU_DOMINIO
sudo ln -s /etc/nginx/sites-available/comandax.conf /etc/nginx/sites-enabled/
sudo nginx -t && sudo systemctl reload nginx
```

Teste em `http://SEU_DOMINIO` (ainda sem HTTPS).

### 6. HTTPS com Certbot

```bash
sudo certbot --nginx -d SEU_DOMINIO
```

A renovação automática já fica agendada (`sudo certbot renew --dry-run`).

### 7. Autorizar o domínio no Google

No [Google Cloud Console](https://console.cloud.google.com/apis/credentials),
abra o OAuth 2.0 Client usado pelo app e adicione
`https://SEU_DOMINIO` em **Authorized JavaScript origins**.

### 8. Migrar os dados do Render (se houver)

```bash
# Na máquina local, com a connection string do Render:
pg_dump "<CONNECTION_STRING_DO_RENDER>" | gzip > comandax-render.sql.gz
scp comandax-render.sql.gz usuario@sua-vps:~/comandax/ComandaX-API/backups/

# Na VPS:
./deploy/scripts/restore-postgres.sh backups/comandax-render.sql.gz
```

Depois de validar, desligue os serviços no Render para não pagar à toa.

### 9. Validar

- Abrir `https://SEU_DOMINIO`, entrar com Google.
- Criar uma comanda, adicionar itens, fechar a comanda.
- Cadastrar produto/categoria/mesa em Gestão.
- Enviar um recibo por e-mail (prova do `RESEND_APITOKEN`).
- Abrir Relatórios e conferir os números.

## Atualizações (deploys seguintes)

```bash
cd ~/comandax/ComandaX-API
./deploy/deploy.sh
```

O script faz backup do banco, `git pull` nos dois repositórios, rebuilda as
imagens, recria só o que mudou e limpa imagens órfãs. Use
`./deploy/deploy.sh --no-backup` para pular o backup.

Há uma janela de alguns segundos de indisponibilidade enquanto os containers
reiniciam — prefira horários fora do pico do restaurante (meio da tarde).

## Backup e restauração

O `deploy.sh` já faz backup a cada deploy. Para um backup agendado diário:

```bash
crontab -e
# Backup do ComandaX todo dia às 4h
0 4 * * * cd ~/comandax/ComandaX-API && ./deploy/scripts/backup-postgres.sh >> ~/comandax-backup.log 2>&1
```

Os dumps ficam em `~/comandax/ComandaX-API/backups` (os 14 mais recentes).
**Copie-os para fora da VPS** — backup no mesmo disco não protege contra perda
da máquina.

Restaurar:

```bash
./deploy/scripts/restore-postgres.sh backups/comandax-20260729-040000.sql.gz
```

## Operação

Rode a partir de `~/comandax/ComandaX-API`, com `COMPOSE_FILE=docker-compose.prod.yml`
exportado na sessão (passo 2) — senão, adicione `-f docker-compose.prod.yml`
em cada comando abaixo.

```bash
docker compose logs -f api      # logs da API (requests, GraphQL, erros)
docker compose logs -f web      # logs do nginx interno
docker compose ps               # status e health
docker compose restart api      # reinicia só a API
docker compose down             # derruba tudo (o volume do banco PERMANECE)
docker compose up -d            # sobe de novo sem rebuild
docker compose exec db psql -U comandax -d comandax   # console do banco
```

> `docker compose down -v` apaga os volumes — **isso destrói o banco**. Nunca
> use em produção.

## Troubleshooting

| Sintoma                                  | Causa provável                                | Correção                                                              |
| ---------------------------------------- | --------------------------------------------- | --------------------------------------------------------------------- |
| API não sobe / reinicia em loop          | `.env` incompleto (`Jwt__Key` vazio) ou senha do banco divergente do volume | `docker compose logs api`; ver nota sobre `POSTGRES_PASSWORD` acima |
| `502 Bad Gateway`                        | Containers fora do ar ou porta errada         | `docker compose ps`; confira `COMANDAX_WEB_PORT` no `.env` vs. vhost  |
| Porta 8110 em uso                        | Conflito com outro serviço da VPS             | Mude `COMANDAX_WEB_PORT` no `.env` e no vhost                         |
| Botão do Google não aparece              | Domínio não autorizado no Google Console      | Passo 7                                                               |
| SPA abre, mas login falha com CORS       | Front apontando para URL antiga do Render     | Confirme `apiUrl: '/graphql'` no `environment.prod.ts` e rebuild `web`|
| Recibo por e-mail não chega              | `RESEND_APITOKEN` vazio ou domínio não verificado no Resend | Preencha o token e reinicie: `docker compose up -d api` |
| `429 Too Many Requests`                  | Rate limiting do nginx do host                | Esperado sob abuso; ajuste `rate=20r/s` no vhost se atingir uso real  |
| Build falha em `npm ci`                  | `package-lock.json` fora de sincronia         | Na máquina de dev: `npm install` e commite o lockfile                 |
