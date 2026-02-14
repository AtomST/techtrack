# API Integration Guide

## Обновленная структура API

### Аутентификация

#### Регистрация
**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "email": "user@example.com",
  "FullName": "Иван Иванов",
  "phoneNumber": "+79996663333",
  "Password": "securePassword123"
}
```

**Response:**
```json
{
  "statusCode": 200,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

#### Вход
**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "securePassword123"
}
```

**Response:**
```json
{
  "statusCode": 200,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### JWT Token Structure

Токен содержит следующие claims:
```json
{
  "nameid": "019c5728-61d2-7b1d-bbe6-356894db9f35",
  "role": "Undefined",
  "company_id": "",
  "nbf": 1770989113,
  "exp": 1770989713,
  "iat": 1770989113
}
```

- `nameid` - ID пользователя
- `role` - роль пользователя (Admin, Manager, Technician, Viewer, Undefined)
- `company_id` - ID компании пользователя (может быть пустым)
- `exp` - время истечения токена (Unix timestamp)

## Изменения в Frontend

### 1. Типы (types/index.ts)

Обновлены типы для соответствия новой структуре API.

### 2. Сервис аутентификации (services/auth.service.ts)

Добавлены функции для работы с JWT:

- `decodeToken()` - декодирование JWT токена
- `getUserFromToken()` - извлечение данных пользователя из токена

Теперь данные пользователя извлекаются из JWT токена, а не приходят в ответе.

### 3. API клиент (lib/api-client.ts)

Упрощен interceptor для 401 ошибок - при 401 пользователь перенаправляется на /login.

## Примеры использования

### Регистрация
```typescript
const registerData = {
  email: 'newuser@example.com',
  FullName: 'Новый Пользователь',
  phoneNumber: '+79991234567',
  Password: 'SecurePass123'
};

const { user, accessToken } = await authService.register(registerData);
```

### Вход
```typescript
const loginData = {
  email: 'user@example.com',
  password: 'SecurePass123'
};

const { user, accessToken } = await authService.login(loginData);
```
