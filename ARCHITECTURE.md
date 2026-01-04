# AdFlow - Arquitetura e Componentes (Architecture and Components)

## Diagrama de Arquitetura (Architecture Diagram)

```
┌─────────────────────────────────────────────────────────────────────┐
│                         FRONTEND (Angular 21)                        │
├─────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐         │
│  │    Login     │    │  Dashboard   │    │    Models    │         │
│  │  Component   │    │  Component   │    │   (DTOs)     │         │
│  └──────┬───────┘    └──────┬───────┘    └──────────────┘         │
│         │                   │                                        │
│         └───────────┬───────┘                                        │
│                     │                                                │
│            ┌────────▼────────┐                                      │
│            │  Auth Service   │◄────┐                                │
│            │   (RxJS/HTTP)   │     │                                │
│            └────────┬────────┘     │                                │
│                     │              │                                │
│                     │         ┌────┴──────┐                         │
│                     │         │  Facebook │                         │
│                     │         │    SDK    │                         │
│                     │         └───────────┘                         │
└─────────────────────┼───────────────────────────────────────────────┘
                      │ HTTP/REST
                      │ (JWT Token)
┌─────────────────────▼───────────────────────────────────────────────┐
│                      BACKEND (.NET 10 API)                           │
├─────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  ┌───────────────────────────────────────────────────────┐          │
│  │              WebAPI Layer (Presentation)              │          │
│  │  ┌──────────────────┐   ┌──────────────────┐        │          │
│  │  │ AuthController   │   │  JWT Middleware  │        │          │
│  │  └────────┬─────────┘   └──────────────────┘        │          │
│  └───────────┼────────────────────────────────────────────┘          │
│              │                                                       │
│  ┌───────────▼────────────────────────────────────────────┐          │
│  │         Application Layer (Business Logic)            │          │
│  │  ┌──────────────────────┐   ┌────────────────────┐  │          │
│  │  │ FacebookAuthService  │   │    DTOs/Models     │  │          │
│  │  └──────────┬───────────┘   └────────────────────┘  │          │
│  │             │                                         │          │
│  │  ┌──────────▼───────────┐   ┌────────────────────┐  │          │
│  │  │ IFacebookApiClient   │   │  IJwtTokenService  │  │          │
│  │  └──────────────────────┘   └────────────────────┘  │          │
│  └───────────┬─────────────────────┬────────────────────┘          │
│              │                     │                                │
│  ┌───────────▼─────────────────────▼────────────────────┐          │
│  │       Infrastructure Layer (Data & External)         │          │
│  │  ┌───────────────────┐   ┌─────────────────────┐   │          │
│  │  │ FacebookApiClient │   │  JwtTokenService    │   │          │
│  │  └───────────────────┘   └─────────────────────┘   │          │
│  │  ┌───────────────────────────────────────────────┐  │          │
│  │  │      FacebookUserRepository                   │  │          │
│  │  └─────────────────┬─────────────────────────────┘  │          │
│  │                    │                                 │          │
│  │         ┌──────────▼──────────┐                     │          │
│  │         │   AdFlowDbContext   │                     │          │
│  │         └──────────┬──────────┘                     │          │
│  └────────────────────┼────────────────────────────────┘          │
│                       │                                            │
│  ┌────────────────────▼────────────────────────────────┐          │
│  │            Domain Layer (Core Business)             │          │
│  │  ┌──────────────────┐   ┌─────────────────────┐   │          │
│  │  │  FacebookUser    │   │  IFacebookUser      │   │          │
│  │  │    (Entity)      │   │   Repository        │   │          │
│  │  └──────────────────┘   └─────────────────────┘   │          │
│  └─────────────────────────────────────────────────────┘          │
│                       │                                            │
└───────────────────────┼────────────────────────────────────────────┘
                        │
                        ▼
              ┌─────────────────┐
              │   EF Core       │
              │   In-Memory DB  │
              └─────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                    EXTERNAL SERVICES                                 │
├─────────────────────────────────────────────────────────────────────┤
│  ┌────────────────────────────────────────────────────┐             │
│  │         Facebook Graph API                         │             │
│  │  (User Info Validation & Retrieval)               │             │
│  └────────────────────────────────────────────────────┘             │
└─────────────────────────────────────────────────────────────────────┘
```

## Fluxo de Dados - Autenticação (Data Flow - Authentication)

```
┌─────────┐          ┌──────────┐          ┌─────────┐          ┌──────────┐
│ Browser │          │  Angular │          │ .NET API│          │ Facebook │
└────┬────┘          └────┬─────┘          └────┬────┘          └────┬─────┘
     │                    │                     │                     │
     │  1. Click Login    │                     │                     │
     ├───────────────────►│                     │                     │
     │                    │                     │                     │
     │                    │  2. Open FB Popup   │                     │
     │                    ├────────────────────────────────────────►  │
     │                    │                     │                     │
     │                    │  3. User Authorizes │                     │
     │                    │◄────────────────────────────────────────  │
     │                    │   (Access Token)    │                     │
     │                    │                     │                     │
     │                    │  4. POST /auth/facebook-login             │
     │                    ├────────────────────►│                     │
     │                    │   {accessToken}     │                     │
     │                    │                     │                     │
     │                    │                     │  5. Validate Token  │
     │                    │                     ├────────────────────►│
     │                    │                     │                     │
     │                    │                     │  6. User Info       │
     │                    │                     │◄────────────────────┤
     │                    │                     │                     │
     │                    │                     │  7. Save/Update User│
     │                    │                     │  in Database        │
     │                    │                     │                     │
     │                    │  8. JWT Token       │                     │
     │                    │◄────────────────────┤                     │
     │                    │   {user, token}     │                     │
     │                    │                     │                     │
     │  9. Store & Redirect                    │                     │
     │◄───────────────────┤                     │                     │
     │   to Dashboard     │                     │                     │
     │                    │                     │                     │
```

## Princípios da Clean Architecture Aplicados

### 1. Separação de Camadas (Layer Separation)
- **Domain**: Núcleo do negócio, sem dependências externas
- **Application**: Casos de uso e lógica de aplicação
- **Infrastructure**: Implementações concretas e acesso a dados
- **WebAPI**: Interface com o mundo externo (HTTP)

### 2. Dependency Inversion (Inversão de Dependências)
```
WebAPI ──depends──> Application ──depends──> Domain
   │                    │
   │                    │
   └──depends──> Infrastructure
                        │
                        └──depends──> Application
                                     └──depends──> Domain
```

### 3. Interfaces e Abstrações
- `IFacebookUserRepository`: Abstração do repositório
- `IFacebookApiClient`: Abstração da API externa
- `IJwtTokenService`: Abstração do serviço de tokens

## Componentes Principais (Main Components)

### Backend

#### 1. Domain Layer
```csharp
// Entities
- FacebookUser: Entidade de usuário do Facebook

// Interfaces
- IFacebookUserRepository: Contrato do repositório
```

#### 2. Application Layer
```csharp
// Services
- FacebookAuthService: Lógica de autenticação

// DTOs
- FacebookUserDto: Transferência de dados do usuário
- FacebookLoginRequest: Request de login
- FacebookLoginResponse: Response de login

// Interfaces
- IFacebookAuthService: Contrato do serviço de auth
- IFacebookApiClient: Contrato do cliente Facebook
- IJwtTokenService: Contrato do serviço JWT
```

#### 3. Infrastructure Layer
```csharp
// Data
- AdFlowDbContext: Contexto do EF Core

// Repositories
- FacebookUserRepository: Implementação do repositório

// Services
- FacebookApiClient: Cliente da API do Facebook
- JwtTokenService: Geração de tokens JWT
```

#### 4. WebAPI Layer
```csharp
// Controllers
- AuthController: Endpoints de autenticação

// Configuration
- Program.cs: Setup de DI, CORS, JWT
```

### Frontend

#### 1. Components
```typescript
// Login Component
- Gerencia o fluxo de login com Facebook
- Exibe interface de autenticação

// Dashboard Component
- Exibe informações do usuário autenticado
- Permite logout
```

#### 2. Services
```typescript
// Auth Service
- Integração com Facebook SDK
- Comunicação com API backend
- Gerenciamento de estado de autenticação
- Armazenamento de tokens
```

#### 3. Models
```typescript
// Interfaces TypeScript
- FacebookUser: Modelo do usuário
- FacebookLoginRequest: Request de login
- FacebookLoginResponse: Response de login
```

## Segurança Implementada (Implemented Security)

1. **JWT Authentication**: Tokens assinados com chave secreta
2. **Token Expiration**: Tokens expiram em 7 dias
3. **CORS Configuration**: Restrito ao domínio do frontend
4. **Facebook Token Validation**: Validação via Graph API
5. **HTTPS Ready**: Configurado para usar HTTPS em produção

## Tecnologias e Padrões (Technologies and Patterns)

### Padrões de Design (Design Patterns)
- **Repository Pattern**: Abstração de acesso a dados
- **Dependency Injection**: Injeção de dependências nativa
- **Service Layer**: Lógica de negócio isolada
- **DTO Pattern**: Transferência de dados otimizada

### Bibliotecas e Frameworks
- **Backend**: ASP.NET Core, EF Core, JWT
- **Frontend**: Angular 21, RxJS, TypeScript
- **External**: Facebook SDK, Facebook Graph API

## Escalabilidade e Manutenção (Scalability and Maintenance)

### Vantagens da Arquitetura
✅ Fácil substituição de componentes  
✅ Testes unitários simplificados  
✅ Baixo acoplamento entre camadas  
✅ Alta coesão dentro das camadas  
✅ Fácil adição de novos provedores de auth  
✅ Código limpo e organizado  

### Próximas Melhorias
- [ ] Adicionar cache (Redis)
- [ ] Implementar event sourcing
- [ ] Adicionar message queue
- [ ] Implementar CQRS pattern
- [ ] Adicionar monitoring e logging
