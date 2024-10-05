import axios from 'axios';
import { Project, ProjectRead } from '../types/projectTypes'; // Assuming these types are defined elsewhere

// Retrieve all projects
export const getProjects = async (): Promise<ProjectRead[]> => {
    try {
        const token = localStorage.getItem('authToken'); // Get the token from localStorage
        const response = await axios.get('https://localhost:7051/api/projects', {
            headers: {
                Authorization: `Bearer ${token}`, // Include the token in the Authorization header
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching projects', error);
        throw error;
    }
};

// Fetch a single project by its ID
export const getProjectById = async (id: number): Promise<ProjectRead> => {
    try {
        const token = localStorage.getItem('authToken'); // Get the token
        const response = await axios.get(`https://localhost:7051/api/projects/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`, // Include the token in the request
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching project', error);
        throw error;
    }
};

// Create a new project
export const createProject = async (projectData: Project): Promise<Project> => {
    try {
        const token = localStorage.getItem('authToken'); // Get the token
        const response = await axios.post('https://localhost:7051/api/projects', projectData, {
            headers: {
                Authorization: `Bearer ${token}`, // Include the token in the request
            },
        });
        return response.data;
    } catch (error) {
        console.error('Error creating project', error);
        throw error;
    }
};

// Update an existing project
export const updateProject = async (id: number, projectData: Project): Promise<void> => {
    try {
        const token = localStorage.getItem('authToken'); // Get the token
        await axios.put(`https://localhost:7051/api/projects/${id}`, projectData, {
            headers: {
                Authorization: `Bearer ${token}`, // Include the token in the request
            },
        });
    } catch (error) {
        console.error('Error updating project', error);
        throw error;
    }
};

// Delete a project
export const deleteProject = async (id: number): Promise<void> => {
    try {
        const token = localStorage.getItem('authToken'); // Get the token
        await axios.delete(`https://localhost:7051/api/projects/${id}`, {
            headers: {
                Authorization: `Bearer ${token}`, // Include the token in the request
            },
        });
    } catch (error) {
        console.error('Error deleting project', error);
        throw error;
    }
};
