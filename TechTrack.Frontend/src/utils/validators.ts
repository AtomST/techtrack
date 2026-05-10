export const validators = {
  email: (email: string): boolean => {
    const re = /^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$/;
    return re.test(email);
  },

  phone: (phone: string): boolean => {
    const re = /^\+?[\d\s-]{10,}$/;
    return re.test(phone);
  },

  notEmpty: (value: string): boolean => {
    return value.trim().length > 0;
  },

  minLength: (value: string, length: number): boolean => {
    return value.length >= length;
  },

  maxLength: (value: string, length: number): boolean => {
    return value.length <= length;
  },

  isNumber: (value: string): boolean => {
    return /^\d+$/.test(value);
  },

  isUUID: (value: string): boolean => {
    const re = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return re.test(value);
  },

  isDate: (value: string): boolean => {
    return !isNaN(Date.parse(value));
  },

  isFutureDate: (value: string): boolean => {
    return new Date(value) > new Date();
  },

  isPastDate: (value: string): boolean => {
    return new Date(value) < new Date();
  },
};
