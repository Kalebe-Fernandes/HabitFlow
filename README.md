# 🎯 HabitFlow - Sistema de Rastreamento de Hábitos com Gamificação

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Aplicação mobile-first desenvolvida em .NET MAUI com Blazor Hybrid para ajudar usuários brasileiros a construir e manter hábitos positivos através de rastreamento, visualizações avançadas e gamificação ética.

---

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Executando o Projeto](#executando-o-projeto)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Roadmap](#roadmap)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

---

## 🚀 Sobre o Projeto

O HabitFlow é uma solução completa para rastreamento de hábitos que combina:

- ✅ **Rastreamento Intuitivo**: Interface simples para marcar conclusões diárias
- 📊 **Analytics Avançado**: Gráficos Burndown/Burnup, estatísticas detalhadas
- 🎮 **Gamificação Ética**: XP, níveis, medalhas e moedas virtuais
- 🎯 **Metas de Longo Prazo**: Associe múltiplos hábitos a objetivos maiores
- 📱 **Mobile-First**: Desenvolvido com .NET MAUI para Android e iOS
- 🔐 **Segurança**: Autenticação JWT, conformidade LGPD

### Funcionalidades Principais

**MVP (V1):**
- Cadastro e login com email/senha
- CRUD completo de hábitos
- Conclusão de hábitos com cálculo automático de streaks
- Dashboard com progresso diário
- Sistema de XP, níveis e badges
- Metas de longo prazo
- Gráficos Burndown/Burnup
- Loja virtual com moedas
- Biblioteca de templates de hábitos
- Exportação de dados (CSV, JSON, PDF)

**Futuro (V2/V3):**
- Login social (Google, Apple, Facebook)
- Features sociais (grupos, desafios, rankings)
- IA preditiva e insights
- Integrações (Google Fit, Apple Health)

---

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** combinada com **Domain-Driven Design (DDD)** e **CQRS**:

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│         (HabitFlow.API)                 │
│  • Controllers REST                     │
│  • Middleware Pipeline                  │
│  • Swagger/Scalar Documentation         │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│       Application Layer                 │
│       (HabitFlow.Application)           │
│  • Commands & Queries (CQRS)            │
│  • Handlers (MediatR)                   │
│  • FluentValidation                     │
│  • DTOs & Mappings (Mapster)            │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Domain Layer                   │
│          (HabitFlow.Domain)             │
│  • Aggregates & Entities                │
│  • Value Objects                        │
│  • Domain Events                        │
│  • Business Rules                       │
│  • Repository Interfaces                │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│      Infrastructure Layer               │
│      (HabitFlow.Infrastructure)         │
│  • EF Core (DbContext)                  │
│  • Repository Implementations           │
│  • External Services                    │
│  • Migrations                           │
└─────────────────────────────────────────┘
```

### Bounded Contexts

O domínio está organizado em 7 bounded contexts:

1. **Users**: Autenticação, perfis, configurações
2. **Habits**: CRUD de hábitos, conclusões, streaks
3. **Goals**: Metas de longo prazo
4. **Gamification**: XP, níveis, badges, moedas, loja
5. **Social**: Grupos, desafios, rankings (V2)
6. **Analytics**: Métricas, relatórios, gráficos
7. **Notifications**: Lembretes, alertas de conquistas

---

## 🛠️ Tecnologias

### Backend
- **.NET 10.0** - Framework principal
- **ASP.NET Core Web API** - API REST
- **Entity Framework Core 10.0** - ORM
- **SQL Server 2022** - Banco de dados
- **MediatR 12.x** - CQRS pattern
- **FluentValidation 11.x** - Validações
- **Mapster 7.x** - Object mapping
- **Serilog 3.x** - Logging estruturado

### Frontend Mobile
- **.NET MAUI 10.0** - Framework multiplataforma
- **Blazor Hybrid** - UI framework
- **MudBlazor 7.x** - Biblioteca de componentes (Material Design)
- **Refit 7.x** - Cliente HTTP type-safe

### Testes
- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions
- **NSubstitute** - Mocking
- **Coverlet** - Code coverage

### Desenvolvimento Local
- **SQL Server 2022 Developer Edition** - Banco de dados local
- **IMemoryCache** - Cache em memória
- **IHostedService** - Background jobs
- **Serviços Mock** - Email, Storage, Notificações

---

## 📋 Pré-requisitos

### Obrigatório

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (17.8 ou superior)
  - Workloads necessários:
    - ASP.NET and web development
    - .NET Multi-platform App UI development
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022 Developer Edition](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/)

### Opcional (mas recomendado)

- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) ou
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download)
- [Postman](https://www.postman.com/downloads/) - Para testar API

---

## ⚙️ Instalação

### 1. Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/HabitFlow.git
cd HabitFlow
```

### 2. Verificar Instalações

```bash
# Verificar .NET SDK
dotnet --version
# Deve retornar: 10.0.x

# Verificar SQL Server
# No Windows: Services > SQL Server (MSSQLSERVER) deve estar "Running"
```

### 3. Restaurar Pacotes NuGet

```bash
# Na raiz do solution
dotnet restore
```

### 4. Configurar Banco de Dados Local

#### Opção A: Integrated Security (Windows Authentication)

```bash
# A connection string padrão já está configurada para isso
# Nenhuma ação necessária
```

#### Opção B: SQL Server Authentication

Se preferir usar SQL Authentication, edite `src/HabitFlow.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HabitFlowDB;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=true"
  }
}
```

### 5. Aplicar Migrations

```bash
cd src/HabitFlow.API

# Aplicar migrations para criar o banco
dotnet ef database update

# Verificar se foi criado
dotnet ef migrations list
```

### 6. (Opcional) Seed de Dados

```bash
# Executar com flag de seed
dotnet run --seed-data
```

Isso criará:
- Categorias padrão de hábitos
- Badges do sistema
- Templates de hábitos populares

---

## 🚀 Executando o Projeto

### Método 1: Visual Studio (Recomendado)

1. Abrir `HabitFlow.sln` no Visual Studio 2022

2. **Configurar Multiple Startup Projects:**
   - Clique com botão direito na Solution
   - Properties → Multiple startup projects
   - Marque como "Start":
     - `HabitFlow.API`
     - `HabitFlow.MauiApp` (opcional)
   - Apply → OK

3. **Pressionar F5 ou clicar em "Start"**

### Método 2: .NET CLI

**Terminal 1 - API:**
```bash
cd src/HabitFlow.API
dotnet run
# API rodando em: https://localhost:7001
# Swagger UI: https://localhost:7001/swagger
```

**Terminal 2 - Mobile App:**
```bash
cd src/HabitFlow.MauiApp
dotnet build -t:Run -f net10.0-android
# ou
dotnet build -t:Run -f net10.0-windows
```

### Testar a API

1. Abrir navegador em: `https://localhost:7001/swagger`
2. Testar endpoint de Health Check: `GET /health`
3. Deve retornar status 200 OK

---

## 📁 Estrutura do Projeto

```
HabitFlow/
├── src/
│   ├── HabitFlow.API/                      # 🌐 Web API
│   │   ├── Controllers/                    # REST endpoints
│   │   ├── Middleware/                     # Exception handling, logging
│   │   ├── Extensions/                     # DI setup
│   │   └── Program.cs                      # Startup
│   │
│   ├── HabitFlow.Application/              # 🧠 Application Layer
│   │   ├── Features/                       # Vertical slices (CQRS)
│   │   │   ├── Users/
│   │   │   ├── Habits/
│   │   │   ├── Goals/
│   │   │   └── Gamification/
│   │   ├── Common/
│   │   │   ├── Behaviors/                  # MediatR pipeline
│   │   │   ├── Interfaces/
│   │   │   └── Models/
│   │   └── DependencyInjection.cs
│   │
│   ├── HabitFlow.Domain/                   # 💎 Domain Layer
│   │   ├── Users/                          # User aggregate
│   │   ├── Habits/                         # Habit aggregate
│   │   ├── Goals/                          # Goal aggregate
│   │   ├── Gamification/                   # UserLevel aggregate
│   │   ├── Common/                         # Base classes
│   │   └── Repositories/                   # Interfaces
│   │
│   ├── HabitFlow.Infrastructure/           # 🔧 Infrastructure Layer
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs     # EF Core context
│   │   │   ├── Configurations/             # Entity configs
│   │   │   └── Migrations/                 # DB migrations
│   │   ├── Repositories/                   # Repository implementations
│   │   ├── Services/                       # External services
│   │   │   ├── Email/
│   │   │   ├── Storage/
│   │   │   └── Notifications/
│   │   └── BackgroundJobs/
│   │
│   ├── HabitFlow.Contracts/                # 📄 Shared DTOs
│   │   ├── Habits/
│   │   ├── Users/
│   │   ├── Goals/
│   │   └── Common/
│   │
│   └── HabitFlow.MauiApp/                  # 📱 Mobile App
│       ├── Pages/                          # Blazor pages
│       ├── Components/                     # Reusable components
│       ├── Services/                       # HTTP services
│       └── Platforms/                      # Platform-specific
│           ├── Android/
│           ├── iOS/
│           └── Windows/
│
├── tests/
│   ├── HabitFlow.UnitTests/                # Unit tests
│   └── HabitFlow.IntegrationTests/         # Integration tests
│
├── docs/                                   # 📚 Documentação
│   ├── architecture/
│   ├── requirements/
│   └── api/
│
├── .gitignore
├── .editorconfig
├── Directory.Build.props
├── README.md
└── HabitFlow.sln                           # Solution principal
```

---

## 🔧 Configurações Importantes

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HabitFlowDB;Integrated Security=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "SuperSecretKeyForDevelopmentOnly_MinimumLengthRequired32Characters!",
    "Issuer": "HabitFlow.API",
    "Audience": "HabitFlow.MauiApp",
    "ExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "FileStorage": {
    "Type": "Local",
    "LocalPath": "C:\\HabitFlowData"
  },
  "EmailSettings": {
    "Type": "Mock"
  },
  "NotificationSettings": {
    "Type": "Mock"
  }
}
```

### User Secrets (Produção)

Para configurações sensíveis em produção, use User Secrets:

```bash
cd src/HabitFlow.API
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "YourProductionSecretKey"
```

---

## 🧪 Executando Testes

```bash
# Todos os testes
dotnet test

# Apenas unit tests
dotnet test tests/HabitFlow.UnitTests/

# Com cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

---

## 🗺️ Roadmap

### ✅ Fase 0 - Setup (Completo)
- [x] Estrutura de projetos
- [x] Configuração de ambiente
- [x] CI/CD pipeline básico

### 🚧 Fase 1 - MVP Core (Em Desenvolvimento)
- [ ] Sprint 1: Users & Authentication
- [ ] Sprint 2-3: Habits CRUD
- [ ] Sprint 4-5: Gamification
- [ ] Sprint 6-7: Goals & Analytics
- [ ] Sprint 8-9: Polish & Launch Prep

### 📅 Fase 2 - V2 Growth (Planejado)
- [ ] Features sociais
- [ ] Login social
- [ ] Integrações externas
- [ ] Analytics avançado

### 🔮 Fase 3 - V3 Scale (Futuro)
- [ ] IA preditiva
- [ ] Microservices
- [ ] Multi-região
- [ ] Features enterprise

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor, leia as [guidelines de contribuição](CONTRIBUTING.md) antes de submeter pull requests.

### Processo

1. Fork o projeto
2. Crie uma feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

### Padrões de Código

- Siga as convenções do `.editorconfig`
- Todos os commits devem passar nos testes
- Cobertura de código >80% para novas features
- Documentação XML para APIs públicas

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 📧 Contato

**HabitFlow Team**
- Website: https://habitflow.com.br
- Email: contato@habitflow.com.br
- GitHub: [@seu-usuario](https://github.com/seu-usuario)

---

## 🙏 Agradecimentos

- [.NET Foundation](https://dotnetfoundation.org/)
- [MudBlazor](https://mudblazor.com/)
- [Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture)

---

**Desenvolvido com ❤️ no Brasil 🇧🇷**
