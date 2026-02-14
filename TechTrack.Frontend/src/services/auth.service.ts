import { apiClient } from '@/lib/api-client';
import { LoginRequest, RegisterRequest, AuthResponse, User, UserRole } from '@/types';

// Helper function to decode JWT token
function decodeToken(token: string): any {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (error) {
    console.error('Error decoding token:', error);
    return null;
  }
}

// Helper function to create User object from token
function getUserFromToken(token: string, email: string): User | null {
  const decoded = decodeToken(token);
  if (!decoded) return null;

  return {
    id: decoded.nameid || '',
    email: email,
    fullName: '', // Will be updated from profile if needed
    companyId: decoded.company_id || undefined,
    role: (decoded.role as UserRole) || UserRole.UNDEFINED,
  };
}

export const authService = {
  async login(credentials: LoginRequest): Promise<{ user: User; accessToken: string }> {
    const response = await apiClient.post<AuthResponse>('/api/auth/login', credentials, {
      withCredentials: true, // Important! Allows httpOnly cookies
    });
    
    if (response.data.statusCode !== 200) {
      throw new Error('Login failed');
    }

    const accessToken = response.data.data.accessToken;
    const user = getUserFromToken(accessToken, credentials.email);

    if (!user) {
      throw new Error('Invalid token');
    }
    
    // Store ONLY accessToken in memory/sessionStorage (not localStorage for security)
    // httpOnly refresh token is automatically stored in cookies by browser
    if (typeof window !== 'undefined') {
      sessionStorage.setItem('accessToken', accessToken);
      sessionStorage.setItem('user', JSON.stringify(user));
    }
    
    return { user, accessToken };
  },

  async register(data: RegisterRequest): Promise<{ user: User; accessToken: string }> {
    const response = await apiClient.post<AuthResponse>('/api/auth/register', data, {
      withCredentials: true, // Important! Allows httpOnly cookies
    });
    
    if (response.data.statusCode !== 200) {
      throw new Error('Registration failed');
    }

    const accessToken = response.data.data.accessToken;
    const user = getUserFromToken(accessToken, data.email);

    if (!user) {
      throw new Error('Invalid token');
    }

    // Update user's fullName from registration data
    user.fullName = data.FullName;
    
    // Store in sessionStorage
    if (typeof window !== 'undefined') {
      sessionStorage.setItem('accessToken', accessToken);
      sessionStorage.setItem('user', JSON.stringify(user));
    }
    
    return { user, accessToken };
  },

  async refresh(): Promise<string> {
    // Refresh token is automatically sent via httpOnly cookie
    const response = await apiClient.post<AuthResponse>('/api/auth/refresh', {}, {
      withCredentials: true, // Send httpOnly cookie
    });
    
    if (response.data.statusCode !== 200) {
      throw new Error('Token refresh failed');
    }

    const accessToken = response.data.data.accessToken;
    
    if (typeof window !== 'undefined') {
      sessionStorage.setItem('accessToken', accessToken);
    }
    
    return accessToken;
  },

  async logout(): Promise<void> {
    try {
      await apiClient.post('/api/auth/logout', {}, {
        withCredentials: true, // Send httpOnly cookie to invalidate it
      });
    } finally {
      if (typeof window !== 'undefined') {
        sessionStorage.removeItem('accessToken');
        sessionStorage.removeItem('user');
      }
    }
  },

  async logoutAll(): Promise<void> {
    try {
      await apiClient.post('/api/auth/logout-all', {}, {
        withCredentials: true,
      });
    } finally {
      if (typeof window !== 'undefined') {
        sessionStorage.removeItem('accessToken');
        sessionStorage.removeItem('user');
      }
    }
  },

  getCurrentUser(): User | null {
    if (typeof window === 'undefined') return null;
    const userStr = sessionStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  getAccessToken(): string | null {
    if (typeof window === 'undefined') return null;
    return sessionStorage.getItem('accessToken');
  },

  isAuthenticated(): boolean {
    if (typeof window === 'undefined') return false;
    const token = sessionStorage.getItem('accessToken');
    if (!token) return false;

    // Check if token is expired
    const decoded = decodeToken(token);
    if (!decoded || !decoded.exp) return false;

    const currentTime = Math.floor(Date.now() / 1000);
    return decoded.exp > currentTime;
  },
};
