# F5-CS-CampanhasApi

Microsserviço de campanhas e doações da plataforma **Conexão Solidária** (Hackathon FIAP Pós Tech, Fase 5). Repositório-irmão de [conexao-solidaria](https://github.com/Agonxx/conexao-solidaria), que concentra a documentação e as decisões do projeto.

## Responsabilidades

- CRUD de campanhas para o `GestorONG` (Título, Descrição, DataInicio, DataFim, MetaFinanceira, Status `Ativa`/`Concluida`/`Cancelada`)
- Painel de transparência público: campanhas `Ativa` com Título, Meta e Valor Arrecadado
- Doação do `Doador` logado (`IdCampanha` + `ValorDoacao`), só para campanha `Ativa`
- A doação **não** atualiza o `ValorArrecadado`: grava a `Doacao` e publica `DoacaoRecebidaEvent` no RabbitMQ. Quem atualiza o valor é o `F5-CS-DoacaoWorker`

O JWT é gerado pelo [F5-CS-UsersApi](https://github.com/Agonxx/F5-CS-UsersApi); aqui ele só é validado (mesma `JwtSettings:SecretKey`).

## Stack

.NET 9, EF Core + SQL Server, MassTransit + RabbitMQ, JWT Bearer, Swagger, Prometheus (`/metrics`).

## Como rodar localmente

Pré-requisito: Docker Desktop.

```
docker compose up -d --build
```

Sobe a API (porta 5002), o SQL Server (1433) e o RabbitMQ (5672, painel em `http://localhost:15672`, guest/guest). Swagger em `http://localhost:5002/swagger`. O banco `CampanhasDB` é criado na primeira execução.

Para obter um token, suba também o UsersApi (mesma `SecretKey`) e use `POST /api/Usuario/Auth`.

## Endpoints

| Método | Rota | Acesso |
|---|---|---|
| GET | `/api/Campanha/Transparencia` | Público |
| GET | `/api/Campanha/GetAll` | `GestorONG` |
| GET | `/api/Campanha/GetById/{id}` | `GestorONG` |
| POST | `/api/Campanha/Criar` | `GestorONG` |
| PUT | `/api/Campanha/Atualizar/{id}` | `GestorONG` |
| POST | `/api/Doacao/Doar` | `Doador` — `{ "idCampanha", "valorDoacao" }` |
| GET | `/api/Doacao/MinhasDoacoes` | `Doador` |

## Contrato do evento

`DoacaoRecebidaEvent` (`namespace Shared.Contracts.Events`), classe duplicada em cada repo. O MassTransit roteia pelo nome completo do tipo, então o namespace precisa ser idêntico no Worker (há teste de `FullName`).

## Testes

```
dotnet test CampanhasApi.Tests/CampanhasApi.Tests.csproj
```
