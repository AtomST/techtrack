import axios from 'axios';
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
    
    // Store tokens and user data for refresh recovery
    if (typeof window !== 'undefined') {
      sessionStorage.setItem('accessToken', accessToken);
      sessionStorage.setItem('user', JSON.stringify(user));
      sessionStorage.setItem('userEmail', credentials.email);
      sessionStorage.setItem('userFullName', user.fullName || ''); // Save for refresh
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
      sessionStorage.setItem('userEmail', data.email); // For refresh recovery
      sessionStorage.setItem('userFullName', data.FullName); // For refresh recovery
    }
    
    return { user, accessToken };
  },

  async refresh(): Promise<string> {
    console.log('[Auth Service] Starting refresh...');
    
    try {
      const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:8080';
      const refreshUrl = `${API_URL}/api/auth/refresh`;
      
      console.log('[Auth Service] Refresh URL:', refreshUrl);
      
      // IMPORTANT: Use vanilla axios, NOT apiClient, to avoid triggering interceptor
      const response = await axios.post<AuthResponse>(
        refreshUrl,
        {},
        {
          withCredentials: true, // Send httpOnly cookie
          headers: {
            'Content-Type': 'application/json',
          },
        }
      );
      
      console.log('[Auth Service] Refresh response:', {
        status: response.status,
        statusCode: response.data?.statusCode,
        hasAccessToken: !!response.data?.data?.accessToken,
      });
      
      if (response.data.statusCode !== 200) {
        throw new Error('Token refresh failed - invalid status code');
      }

      const accessToken = response.data.data.accessToken;
      
      if (!accessToken) {
        throw new Error('Token refresh failed - no accessToken in response');
      }
      
      console.log('[Auth Service] Got new accessToken:', accessToken.substring(0, 20) + '...');
      
      if (typeof window !== 'undefined') {
        sessionStorage.setItem('accessToken', accessToken);
        console.log('[Auth Service] Saved accessToken to sessionStorage');
        
        // Decode token and reconstruct user
        const email = sessionStorage.getItem('userEmail') || '';
        const fullName = sessionStorage.getItem('userFullName') || '';
        
        console.log('[Auth Service] Reconstructing user with email:', email);
        
        const user = getUserFromToken(accessToken, email);
        if (user) {
          user.fullName = fullName;
          sessionStorage.setItem('user', JSON.stringify(user));
          console.log('[Auth Service] User reconstructed:', user);
        } else {
          console.error('[Auth Service] Failed to decode token');
        }
      }
      
      console.log('[Auth Service] Refresh successful!');
      return accessToken;
    } catch (error: any) {
      console.error('[Auth Service] Refresh error:', error.message, error.response?.data);
      
      // Clear tokens on refresh failure
      if (typeof window !== 'undefined') {
        sessionStorage.removeItem('accessToken');
        sessionStorage.removeItem('user');
        sessionStorage.removeItem('userEmail');
        sessionStorage.removeItem('userFullName');
      }
      
      // Re-throw to let caller handle
      throw new Error(`Refresh token invalid or expired: ${error.message}`);
    }
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
        sessionStorage.removeItem('userEmail');
        sessionStorage.removeItem('userFullName');
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
        sessionStorage.removeItem('userEmail');
        sessionStorage.removeItem('userFullName');
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
