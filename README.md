# AdFlow

AdFlow is a full-stack application built with Angular 21 and .NET 10, featuring Facebook authentication using Clean Architecture principles.

## Architecture

### Backend (.NET 10 API)
The backend follows Clean Architecture with the following layers:

- **AdFlow.Domain**: Entities and repository interfaces
- **AdFlow.Application**: Business logic, services, and DTOs
- **AdFlow.Infrastructure**: Data access, external API clients, and service implementations
- **AdFlow.WebAPI**: REST API controllers and configuration

### Frontend (Angular 21)
The frontend is built with Angular 21 (standalone components) and includes:

- Facebook authentication integration
- Login and Dashboard components
- Authentication service
- Route guards

## Prerequisites

- .NET 10 SDK
- Node.js (v20+)
- npm
- Facebook Developer App (for Facebook authentication)

## Setup Instructions

### 1. Facebook App Configuration

1. Go to [Facebook Developers](https://developers.facebook.com/)
2. Create a new app or use an existing one
3. Add "Facebook Login" product to your app
4. Configure OAuth redirect URIs:
   - `http://localhost:4200/`
5. Note your App ID for later use

### 2. Backend Setup

Navigate to the project root:

```bash
cd /path/to/AdFlow
```

Build the solution:

```bash
dotnet build
```

Run the API:

```bash
cd src/AdFlow.WebAPI
dotnet run
```

The API will be available at `http://localhost:5065`

### 3. Frontend Setup

Navigate to the client folder:

```bash
cd client
```

Install dependencies:

```bash
npm install
```

Update the Facebook App ID in `src/environments/environment.ts`:

```javascript
export const environment = {
  production: false,
  facebookAppId: 'YOUR_ACTUAL_FACEBOOK_APP_ID', // Replace with your actual Facebook App ID
  apiUrl: 'http://localhost:5065/api'
};
```

**Note:** Never commit your actual Facebook App ID to version control in production. Use environment variables or build-time configuration.

Run the development server:

```bash
npm start
```

The application will be available at `http://localhost:4200`

## Features

- **Facebook Authentication**: Users can log in using their Facebook account
- **JWT Token Management**: Secure token-based authentication
- **User Profile**: Display Facebook user information
- **Clean Architecture**: Separation of concerns with proper layer boundaries
- **In-Memory Database**: Uses EF Core In-Memory database for development

## API Endpoints

- `POST /api/auth/facebook-login`: Login with Facebook access token
- `GET /api/auth/user/{id}`: Get user by ID (requires authentication)
- `GET /api/auth/users`: Get all users (requires authentication)

## Project Structure

```
AdFlow/
├── src/
│   ├── AdFlow.Domain/          # Entities and interfaces
│   ├── AdFlow.Application/     # Business logic and DTOs
│   ├── AdFlow.Infrastructure/  # Data access and external services
│   └── AdFlow.WebAPI/          # API controllers
├── client/                     # Angular 21 frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/    # Login and Dashboard components
│   │   │   ├── services/      # Authentication service
│   │   │   └── models/        # TypeScript interfaces
│   │   └── index.html         # Facebook SDK integration
└── AdFlow.sln                 # Solution file
```

## Technologies Used

### Backend
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core (In-Memory)
- JWT Bearer Authentication
- Clean Architecture

### Frontend
- Angular 21
- TypeScript
- RxJS
- Facebook SDK for JavaScript

## Development Notes

- The application uses an in-memory database for development. For production, configure a proper database provider.
- JWT secret key should be stored securely (e.g., in environment variables or Azure Key Vault).
- Facebook App ID should be configured properly for your domain in production.
- CORS is configured to allow requests from `http://localhost:4200`.

## License

This project is licensed under the MIT License.
