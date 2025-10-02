# 🏥 ClinicMVP

Sistema de gestão de clínica médica desenvolvido com .NET 8, Blazor WebAssembly e PostgreSQL.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Executando o Projeto](#executando-o-projeto)
- [API Endpoints](#api-endpoints)
- [Funcionalidades](#funcionalidades)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Configuração](#configuração)
- [Deploy](#deploy)
- [Contribuição](#contribuição)
- [Licença](#licença)

## 🎯 Visão Geral

O ClinicMVP é um sistema completo para gestão de clínicas médicas, oferecendo funcionalidades para gerenciamento de pacientes, médicos e consultas. O sistema foi desenvolvido seguindo as melhores práticas de desenvolvimento, com arquitetura separada entre frontend e backend.

### ✨ Características Principais

- **CRUD Completo**: Gerenciamento de pacientes, médicos e consultas
- **Auditoria Automática**: Log de todas as alterações no banco de dados
- **Soft Delete**: Exclusão lógica de registros
- **Interface Moderna**: Blazor WebAssembly com Bootstrap
- **API RESTful**: Documentação automática com Swagger
- **Logs Estruturados**: Serilog para monitoramento
- **Docker Ready**: Configuração para containerização

## 🏗️ Arquitetura

```
┌─────────────────┐    HTTP/REST    ┌─────────────────┐
│                 │ ◄─────────────► │                 │
│  Blazor Web     │                 │  ASP.NET Core   │
│  Assembly       │                 │  Web API        │
│  (Frontend)     │                 │  (Backend)      │
│                 │                 │                 │
└─────────────────┘                 └─────────────────┘
                                              │
                                              │ EF Core
                                              ▼
                                    ┌─────────────────┐
                                    │                 │
                                    │   PostgreSQL    │
                                    │   Database      │
                                    │                 │
                                    └─────────────────┘
```

## 🛠️ Tecnologias

### Backend
- **.NET 8.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **Serilog** - Logging estruturado
- **Swagger/OpenAPI** - Documentação da API

### Frontend
- **Blazor WebAssembly** - Framework SPA
- **Bootstrap 5** - Framework CSS
- **HttpClient** - Comunicação com API

### Infraestrutura
- **Docker** - Containerização
- **Docker Compose** - Orquestração
- **Git** - Controle de versão

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (opcional)
- [Git](https://git-scm.com/downloads)

## 🚀 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/clinicmvp.git
cd clinicmvp
```

### 2. Configure o banco de dados
```bash
# Crie o banco de dados PostgreSQL
createdb clinicmvp

# Ou use Docker
docker run --name clinic-postgres -e POSTGRES_DB=clinicmvp -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=TMKTtmkt123 -p 5432:5432 -d postgres:15
```

### 3. Configure a string de conexão
Edite o arquivo `backend/ClinicService/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=clinicmvp;Username=postgres;Password=TMKTtmkt123;"
  }
}
```

## ▶️ Executando o Projeto

### Opção 1: Execução Manual

#### Backend (API)
```bash
cd backend/ClinicService
dotnet restore
dotnet run
```
A API estará disponível em: `http://localhost:7235`

#### Frontend (Blazor)
```bash
cd frontend/ClinicFrontend
dotnet restore
dotnet run
```
O frontend estará disponível em: `http://localhost:5001`

### Opção 2: Docker Compose
```bash
# Na raiz do projeto
docker-compose up -d
```

## 📚 API Endpoints

### Pacientes
- `GET /api/Pacientes` - Listar pacientes
- `GET /api/Pacientes/{id}` - Obter paciente por ID
- `POST /api/Pacientes` - Criar paciente
- `PUT /api/Pacientes/{id}` - Atualizar paciente
- `DELETE /api/Pacientes/{id}` - Excluir paciente (soft delete)

### Médicos
- `GET /api/Medicos` - Listar médicos
- `GET /api/Medicos/{id}` - Obter médico por ID
- `POST /api/Medicos` - Criar médico
- `PUT /api/Medicos/{id}` - Atualizar médico
- `DELETE /api/Medicos/{id}` - Excluir médico

### Consultas
- `GET /api/Consultas` - Listar consultas
- `GET /api/Consultas/{id}` - Obter consulta por ID
- `POST /api/Consultas` - Criar consulta
- `PUT /api/Consultas/{id}` - Atualizar consulta
- `DELETE /api/Consultas/{id}` - Excluir consulta

### Auditoria
- `GET /api/Audit` - Listar logs de auditoria

## 🎯 Funcionalidades

### ✅ Implementadas
- [x] CRUD de Pacientes
- [x] CRUD de Médicos
- [x] CRUD de Consultas
- [x] Sistema de Auditoria
- [x] Soft Delete
- [x] Filtros de busca
- [x] Validação de formulários
- [x] Logs estruturados
- [x] Documentação Swagger
- [x] CORS configurado

### 🚧 Em Desenvolvimento
- [ ] Autenticação e Autorização
- [ ] Testes automatizados
- [ ] Paginação
- [ ] Cache Redis
- [ ] Monitoramento com Health Checks

### 📋 Planejadas
- [ ] Relatórios
- [ ] Notificações
- [ ] Integração com sistemas externos
- [ ] App mobile
- [ ] Dashboard analytics

## 📁 Estrutura do Projeto

```
ClinicMVP/
├── backend/
│   └── ClinicService/                 # API Backend
│       ├── Controllers/               # Controllers da API
│       ├── Data/                      # Contexto do Entity Framework
│       ├── DTOs/                      # Data Transfer Objects
│       ├── Interceptors/              # Interceptors para auditoria
│       ├── Models/                    # Modelos de domínio
│       ├── Services/                  # Serviços de negócio
│       ├── Migrations/                # Migrações do banco
│       └── Logs/                      # Logs da aplicação
├── frontend/
│   └── ClinicFrontend/                # Blazor WebAssembly
│       ├── Pages/                     # Páginas Razor
│       ├── Services/                  # Serviços HTTP
│       ├── Models/                    # Modelos do frontend
│       ├── Layout/                    # Layouts e componentes
│       └── wwwroot/                   # Arquivos estáticos
├── infra/
│   └── docker-compose.yml             # Configuração Docker
├── .gitignore                         # Arquivos ignorados pelo Git
└── README.md                          # Este arquivo
```

## ⚙️ Configuração

### Variáveis de Ambiente

#### Backend
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=clinicmvp;Username=postgres;Password=TMKTtmkt123;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### Frontend
```json
{
  "ApiBaseUrl": "http://localhost:7235/"
}
```

### Configurações de Desenvolvimento
- **API**: `http://localhost:7235`
- **Frontend**: `http://localhost:5001`
- **Swagger**: `http://localhost:7235/swagger`

## 🐳 Deploy

### Docker Compose
```yaml
version: "3.9"
services:
  postgres:
    image: postgres:15
    environment:
      POSTGRES_DB: clinicmvp
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: TMKTtmkt123
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  api:
    build: ./backend/ClinicService
    ports:
      - "5000:80"
    depends_on:
      - postgres
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=clinicmvp;Username=postgres;Password=TMKTtmkt123;

  frontend:
    build: ./frontend/ClinicFrontend
    ports:
      - "80:80"
    depends_on:
      - api

volumes:
  postgres_data:
```

### Comandos de Deploy
```bash
# Build e execução
docker-compose up -d

# Logs
docker-compose logs -f

# Parar
docker-compose down
```

## 🧪 Testes

### Executar Testes
```bash
# Backend
cd backend/ClinicService
dotnet test

# Frontend
cd frontend/ClinicFrontend
dotnet test
```

### Cobertura de Testes
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📊 Monitoramento

### Logs
- **Localização**: `backend/ClinicService/Logs/`
- **Formato**: JSON estruturado
- **Rotação**: Diária

### Métricas
- **Health Checks**: `/health`
- **Swagger**: `/swagger`
- **Logs**: Serilog com diferentes níveis

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Padrões de Código
- **C#**: Seguir convenções da Microsoft
- **Blazor**: Componentes reutilizáveis
- **API**: RESTful com documentação Swagger
- **Commits**: Conventional Commits

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📞 Suporte

- **Email**: suporte@clinicmvp.com
- **Issues**: [GitHub Issues](https://github.com/seu-usuario/clinicmvp/issues)
- **Documentação**: [Wiki](https://github.com/seu-usuario/clinicmvp/wiki)

## 🙏 Agradecimentos

- [Microsoft](https://microsoft.com) - .NET e Blazor
- [PostgreSQL](https://postgresql.org) - Banco de dados
- [Bootstrap](https://getbootstrap.com) - Framework CSS
- [Serilog](https://serilog.net) - Logging estruturado

---

**Desenvolvido com ❤️ usando .NET 8 e Blazor WebAssembly**
