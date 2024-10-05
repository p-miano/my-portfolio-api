import api from '../api/api';
import { CategoryCreateDto, CategoryReadDto } from '../types/categoryTypes';

// Get all categories
export const getCategories = async (): Promise<CategoryReadDto[]> => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.get('/categories', {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching categories', error);
        throw error;
    }
};

// Create a new category
export const createCategory = async (categoryData: CategoryCreateDto): Promise<CategoryReadDto> => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.post('/categories', categoryData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error creating category', error);
        throw error;
    }
};
