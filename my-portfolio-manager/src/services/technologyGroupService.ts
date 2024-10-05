import api from '../api/api';
import { TechnologyGroupCreateDto, TechnologyGroupReadDto } from '../types/technologyGroupTypes';

// Get all technology groups
export const getTechnologyGroups = async (): Promise<TechnologyGroupReadDto[]> => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.get('/technologygroups', {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching technology groups', error);
        throw error;
    }
};

// Create a new technology group
export const createTechnologyGroup = async (groupData: TechnologyGroupCreateDto): Promise<TechnologyGroupReadDto> => {
    try {
        const token = localStorage.getItem('authToken');
        const response = await api.post('/technologygroups', groupData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error creating technology group', error);
        throw error;
    }
};
