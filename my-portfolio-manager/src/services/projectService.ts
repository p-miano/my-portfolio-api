import api from '../api/api';
import { Project } from '../types/projectTypes';


// Fetch all projects for the logged-in user
export const getProjects = async (): Promise<Project[]> => {
    try {
        const response = await api.get('/projects');
        return response.data;
    } catch (error) {
        console.error('Error fetching projects', error);
        throw error;
    }
};

// Fetch a single project by its ID
export const getProjectById = async (id: number): Promise<Project> => {
    try {
        const response = await api.get(`/projects/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching project', error);
        throw error;
    }
};

// Create a new project
export const createProject = async (projectData: Project): Promise<Project> => {
    try {
        const response = await api.post('/projects', projectData);
        return response.data;
    } catch (error) {
        console.error('Error creating project', error);
        throw error;
    }
};

// Update an existing project
export const updateProject = async (id: number, projectData: Project): Promise<void> => {
    try {
        await api.put(`/projects/${id}`, projectData);
    } catch (error) {
        console.error('Error updating project', error);
        throw error;
    }
};

// Delete a project
export const deleteProject = async (id: number): Promise<void> => {
    try {
        await api.delete(`/projects/${id}`);
    } catch (error) {
        console.error('Error deleting project', error);
        throw error;
    }
};
