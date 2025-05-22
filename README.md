# AppTaskManager

Projeto desenvolvido como teste técnico para a empresa **TaskManager**. O sistema realiza o **CRUD de Tarefas** utilizando Blazor no front-end e arquitetura moderna no back-end.

## 🛠️ Tecnologias Utilizadas

- **Front-end**: Blazor WebAssembly
- **Back-end**: ASP.NET Core 8.0 (API)
- **Banco de dados**: SQL Server (externo)
- **Arquitetura**: DDD, CQRS, Event Sourcing (mínimo viável)
- **Validações**: FluentValidation
- **Testes unitários**: xUnit + Moq
- **Containerização**: Docker e Docker Compose

---

## 🔧 Rodando com Docker

### Pré-requisitos
- Docker instalado na máquina

### Passos:

```bash
git clone https://github.com/diogocorte19932607/AppTaskManager.git
cd AppTaskManager
docker-compose up --build
```

Acesse a API em: [http://localhost:5000](http://localhost:5000)

---

## ⚙️ Funcionalidades

- Cadastro de Tarefas com:
  - Title
  - Description
  - TaskStatus
  - CreatedAt

  ---

## 🔍 Regras de Negócio

- Autenticação
- WT (usuário/senha fixos)

---

## 🧪 Testes

- Executar os testes com:

```bash
dotnet test TaskManager.Domain.Tests
```

---

## 📝 Observações

- Blazor foi utilizado no lugar de Angular por decisão técnica.
- Projeto com foco em legibilidade, arquitetura limpa e boas práticas.