# Fiap.Catalog

API de catálogo da plataforma. Ela gerencia os itens do catálogo, persiste os dados em **SQLite** e se integra ao **RabbitMQ** para a comunicação com os demais serviços.

## Estrutura do projeto

```
.
├── Fiap.Catalog.Api/            # API ASP.NET Core 8 (controllers e Swagger)
│   └── Dockerfile
├── Fiap.Catalog.Domain/         # Entidades e regras de domínio
├── Fiap.Catalog.Infra/          # Acesso a dados (SQLite) e catalog_data_base.db
├── docker-compose.yml           # Ambiente local (RabbitMQ + API)
└── k8s/                         # Manifestos Kubernetes
    ├── 01-secret-catalog-database.yaml
    ├── 02-deployment-catalog.yaml
    └── 03-svc-catalog-api.yaml
```

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) (execução local sem container)
- [Docker](https://www.docker.com/) e Docker Compose
- [kubectl](https://kubernetes.io/docs/tasks/tools/) e um cluster local (kind, minikube ou Docker Desktop) para o deploy em Kubernetes

## Configuração

Em variáveis de ambiente, o `:` das configurações do `appsettings` vira `__`.

| Variável | Descrição | Exemplo |
|---|---|---|
| `ASPNETCORE_URLS` | Endereço de escuta da API | `http://+:8082` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Development` |
| `ConnectionStrings__DefaultConnection` | String de conexão do SQLite | `Data Source=/app/database/catalog_data_base.db` |
| `SQLITE_URL` | Caminho do banco (injetado pelo Secret no Kubernetes) | `Data Source=/app/database/catalog_data_base.db` |
| `RabbitMQ__HostName` | Host do RabbitMQ | `rabbitmq` (Compose) / `rabbitmq-service` (Kubernetes) |
| `RabbitMQ__Port` | Porta AMQP | `5672` |
| `RabbitMQ__UserName` | Usuário do RabbitMQ | `admin` |
| `RabbitMQ__Password` | Senha do RabbitMQ | `admin1234` |
| `RabbitMQ__VirtualHost` | Virtual host | `/` |

## Executando com Docker Compose

O `docker-compose.yml` sobe o RabbitMQ (com volume e healthcheck) e a API, que só inicia depois que o RabbitMQ estiver saudável.

```bash
docker compose up --build -d
docker compose logs -f api-catalog
docker compose down            # para os containers (mantém o volume do RabbitMQ)
docker compose down -v         # para e apaga o volume do RabbitMQ
```

| Serviço | URL |
|---|---|
| Swagger da API | http://localhost:8082/swagger/index.html |
| RabbitMQ (painel) | http://localhost:15672 (usuário `admin`, senha `admin1234`) |
| RabbitMQ (AMQP) | `localhost:5672` |

## Docker

O `Dockerfile` usa **multi-stage build** (`base` → `build` → `publish` → `final`): o SDK 8.0 compila e publica, e a imagem final usa apenas o runtime ASP.NET 8.0. O contexto de build é a **raiz da solução**, porque o Dockerfile copia os projetos `Api`, `Domain` e `Infra`.

```bash
docker build -f Fiap.Catalog.Api/Dockerfile -t <usuario>/src-api-catalog:latest .
docker push <usuario>/src-api-catalog:latest
```

Detalhes da imagem:

- O arquivo `Fiap.Catalog.Infra/catalog_data_base.db` é **copiado para dentro da imagem**, em `/app/database/catalog_data_base.db`.
- A pasta `/app/database` recebe permissão para o usuário `app`, para a aplicação conseguir gravar no banco.
- A aplicação roda com o usuário não-root `app` e expõe a porta `8082`.

## Executando no Kubernetes

### Pré-requisito: RabbitMQ no cluster

Os manifestos deste projeto **não criam o RabbitMQ**. O Deployment depende do ConfigMap `configmap-rabbitmq` e do Secret `rabbitmq-credentials`, que vêm do projeto **Fiap.Notifications** (`rabbitmq-service`). Aplique-os antes, senão o pod fica em `CreateContainerConfigError`.

### Recursos

| Arquivo | Recurso | Descrição |
|---|---|---|
| `01-secret-catalog-database.yaml` | Secret `catalog-data-base-credentials` | Caminho do banco SQLite (`SQLITE_URL`) |
| `02-deployment-catalog.yaml` | Deployment `catalog-api-deployment` | API na porta 8082, com readiness probe TCP |
| `03-svc-catalog-api.yaml` | Service `catalog-api` | `ClusterIP`, porta **8092** → container 8082 |

### Aplicar

O Secret precisa existir antes do Deployment.

```bash
kubectl apply -f k8s/
kubectl get pod,svc,endpoints
```

Para remover tudo: `kubectl delete -f k8s/`.

### Comunicação entre serviços

Dentro do cluster, os serviços se encontram pelo **nome do Service**:

| Serviço | Endereço no cluster | Porta do container |
|---|---|---|
| users-api | `http://users-api:8091` | 8081 |
| catalog-api | `http://catalog-api:8092` | 8082 |
| notifications-api | `http://notifications-api:9093` | 8083 |
| payments-api | `http://payments-api:8094` | 8084 |
| rabbitmq-service | `rabbitmq-service:5672` | 5672 |

### Acessando da sua máquina

Nomes de Service só existem dentro do cluster. Para abrir no navegador:

1. Adicione o nome ao arquivo `hosts` (edite como administrador):

   ```
   # Windows: C:\Windows\System32\drivers\etc\hosts
   127.0.0.1 catalog-api
   ```

2. Mantenha um port-forward ativo, em um terminal separado:

   ```bash
   kubectl port-forward svc/catalog-api 8092:8092
   ```

3. Acesse http://catalog-api:8092/swagger/index.html.

Alternativa sem editar o `hosts`: http://127.0.0.1:8092/swagger/index.html.

### Verificação

```bash
kubectl get pod,svc,endpoints
kubectl logs deploy/catalog-api-deployment -f
```

## Problemas comuns

| Sintoma | Causa provável | O que fazer |
|---|---|---|
| Pod em `CreateContainerConfigError` | Secret ou ConfigMap não existem (inclusive `configmap-rabbitmq` e `rabbitmq-credentials`) | Aplicar o Secret e o RabbitMQ antes do Deployment |
| `DNS_PROBE_FINISHED_NXDOMAIN` ou `ping` não encontra `catalog-api` | O nome não está no `hosts` | Adicionar a entrada (veja acima) e rodar `ipconfig /flushdns` |
| `ERR_CONNECTION_REFUSED` | Port-forward parado ou nome apontando para outro IP | Subir o `port-forward` e conferir com `ping catalog-api` |
| `Unable to listen on port ...: bind` | Porta já em uso (port-forward antigo) | Encerrar o processo `kubectl` anterior |
| `Endpoints` do Service como `<none>` | Selector não bate ou pod não está `Ready` | Conferir `app: svc-catalog-api` e `kubectl describe pod` |
| Erro de acesso ao banco SQLite | Pasta `/app/database` sem permissão ou string de conexão incorreta | Conferir `SQLITE_URL` e `ConnectionStrings__DefaultConnection` |

## Pontos de atenção

- **Banco SQLite dentro do container:** o `.db` vai embutido na imagem e não há volume nem PersistentVolumeClaim. Os dados gravados se perdem quando o container ou o pod é recriado. Para persistir, monte um volume em `/app/database`.
- **Variáveis do banco:** o Dockerfile define `ConnectionStrings__DefaultConnection`, e o Kubernetes injeta `SQLITE_URL` pelo Secret. Confirme qual delas a aplicação lê e remova a que não for usada.
- **Sem espera pelo RabbitMQ no Kubernetes:** diferente do Notifications, o Deployment não tem `initContainer`. Se o pod subir antes do RabbitMQ ficar pronto, pode falhar na primeira conexão e reiniciar.
- **Senha do RabbitMQ no repositório:** o `docker-compose.yml` e o Secret do Notifications trazem `admin1234`. Serve para estudo. Em produção, crie os Secrets por `kubectl create secret` e não os versione.
- **Ambiente `Development`:** o Dockerfile e o Deployment fixam `ASPNETCORE_ENVIRONMENT=Development`. Em produção, use `Production`.
- **Imagem `:latest`:** com `imagePullPolicy: Always`, dificulta o rollback. Prefira uma tag de versão.
- **Portas diferentes:** no Compose a API responde em `8082`, e no Kubernetes o Service responde em `8092`. O container escuta `8082` nos dois casos.
- **Versão do RabbitMQ:** o Compose usa `3.11-management` e o Kubernetes (projeto Notifications) usa `3.13-management`.
- **APIS:** para simular o processo completo é preciso baixar as apis: users, payments e notifications.
