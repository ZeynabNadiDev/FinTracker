import axios from 'axios';

// Get base URL from environment variables or fallback to local URL
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5020/api';

export const registerUser = async (userData) => {
    try {
        const response = await axios.post(`${API_BASE_URL}/Auth/register`, userData);
        return response.data;
    } catch (error) {
        const errorMessage = error.response?.data?.message || error.message || 'Registration failed';
        throw new Error(errorMessage);
    }
};
