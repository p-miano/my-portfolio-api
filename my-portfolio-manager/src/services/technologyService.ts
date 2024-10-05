import api from '../api/api';
import { TechnologyCreateDto } from '../types/technologyTypes'; // Create a type for Technology
import axios from 'axios';

// Get all technologies
export const getTechnologies = async () => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.get('/technologies', {
            headers: {
                Authorization: `Bearer ${token}`, // Attach the token
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching technologies', error);
        throw error;
    }
};

export const getTechnologiesByGroup = async (groupId: number) => {
    const token = localStorage.getItem('authToken');
    const response = await axios.get(`https://localhost:7051/api/technologygroups/${groupId}/technologies`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    return response.data;
};

// Create a new technology
export const createTechnology = async (technologyData: TechnologyCreateDto) => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.post('/technologies', technologyData, {
            headers: {
                Authorization: `Bearer ${token}`, // Attach the token
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error creating technology', error);
        throw error;
    }
};
