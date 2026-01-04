# AdFlow - Guia de Configuração (Setup Guide)

## Visão Geral (Overview)
AdFlow é uma aplicação full-stack que demonstra autenticação via Facebook usando Angular 21 no frontend e .NET 10 com Clean Architecture no backend.

## Estrutura do Projeto (Project Structure)

### Backend (.NET 10)
```
src/
├── AdFlow.Domain/          # Camada de Domínio - Entidades e Interfaces
├── AdFlow.Application/     # Camada de Aplicação - Casos de Uso e DTOs
├── AdFlow.Infrastructure/  # Camada de Infraestrutura - Acesso a Dados
└── AdFlow.WebAPI/          # Camada de API - Controllers e Configuração
```

### Frontend (Angular 21)
```
client/
├── src/app/
│   ├── components/    # Componentes (Login, Dashboard)
│   ├── services/      # Serviços (Auth)
│   └── models/        # Modelos TypeScript
```

## Pré-requisitos (Prerequisites)

- .NET 10 SDK
- Node.js v20+
- npm 10+
- Conta de desenvolvedor Facebook (Facebook Developer Account)

## Configuração do App Facebook (Facebook App Setup)

1. Acesse [Facebook Developers](https://developers.facebook.com/)
2. Crie um novo app ou use um existente
3. Adicione o produto "Facebook Login"
4. Configure as URIs de redirecionamento OAuth:
   - `http://localhost:4200/`
5. Anote o App ID para configuração

## Instalação e Execução (Installation and Running)

### 1. Backend (.NET API)

```bash
# Navegue até o diretório raiz
cd /path/to/AdFlow

# Compile a solução
dotnet build

# Execute a API
cd src/AdFlow.WebAPI
dotnet run
```

A API estará disponível em: `http://localhost:5065`

### 2. Frontend (Angular)

```bash
# Navegue até o diretório do cliente
cd client

# Instale as dependências
npm install

# IMPORTANTE: Configure o Facebook App ID
# Edite: src/environments/environment.ts
# Substitua facebookAppId por seu App ID real

# Execute o servidor de desenvolvimento
npm start
```

A aplicação estará disponível em: `http://localhost:4200`

## Endpoints da API (API Endpoints)

### Autenticação (Authentication)
- `POST /api/auth/facebook-login` - Login com token do Facebook
  - Body: `{ "accessToken": "string" }`
  - Response: `{ "user": {...}, "token": "jwt-token" }`

### Usuários (Users) - Requer Autenticação
- `GET /api/auth/user/{id}` - Buscar usuário por ID
- `GET /api/auth/users` - Listar todos os usuários

## Características Implementadas (Implemented Features)

✅ Autenticação via Facebook  
✅ Geração e validação de JWT tokens  
✅ Clean Architecture (separação de camadas)  
✅ Entity Framework Core (In-Memory Database)  
✅ Angular 21 com Standalone Components  
✅ Roteamento com guards  
✅ Serviço de autenticação centralizado  
✅ Interface de usuário responsiva  

## Tecnologias Utilizadas (Technologies Used)

### Backend
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- JWT Bearer Authentication
- Clean Architecture Pattern

### Frontend
- Angular 21
- TypeScript
- RxJS
- Facebook SDK for JavaScript
- CSS3

## Estrutura da Clean Architecture (Clean Architecture Structure)

### Camada de Domínio (Domain Layer)
- `FacebookUser`: Entidade principal
- `IFacebookUserRepository`: Interface do repositório

### Camada de Aplicação (Application Layer)
- `FacebookAuthService`: Lógica de autenticação
- `IFacebookApiClient`: Interface para API do Facebook
- `IJwtTokenService`: Interface para geração de tokens
- DTOs: `FacebookUserDto`, `FacebookLoginRequest`, `FacebookLoginResponse`

### Camada de Infraestrutura (Infrastructure Layer)
- `FacebookUserRepository`: Implementação do repositório
- `FacebookApiClient`: Cliente da API do Facebook
- `JwtTokenService`: Geração de tokens JWT
- `AdFlowDbContext`: Contexto do banco de dados

### Camada de API (API Layer)
- `AuthController`: Endpoints de autenticação
- Configuração de JWT e CORS
- Injeção de dependências

## Considerações de Segurança (Security Considerations)

⚠️ **Para Produção (For Production):**
- Configure um banco de dados real (PostgreSQL, SQL Server, etc.)
- Armazene secrets em variáveis de ambiente ou Azure Key Vault
- Configure HTTPS
- Atualize as configurações de CORS para o domínio de produção
- Configure o Facebook App para o domínio de produção

## Fluxo de Autenticação (Authentication Flow)

1. Usuário clica em "Login with Facebook" no Angular
2. SDK do Facebook abre popup de autenticação
3. Após autorização, obtém access token do Facebook
4. Angular envia access token para API (.NET)
5. API valida token com Facebook Graph API
6. API cria/atualiza usuário no banco de dados
7. API gera JWT token
8. Angular armazena JWT e redireciona para dashboard
9. Requisições subsequentes incluem JWT no header

## Próximos Passos (Next Steps)

- [ ] Adicionar testes unitários
- [ ] Adicionar testes de integração
- [ ] Implementar refresh tokens
- [ ] Adicionar mais provedores de autenticação
- [ ] Implementar roles e permissions
- [ ] Adicionar logging estruturado
- [ ] Configurar CI/CD

## Licença (License)
MIT License
