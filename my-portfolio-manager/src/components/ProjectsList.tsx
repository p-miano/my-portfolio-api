import React, { useEffect, useState } from 'react';
import { getProjects } from '../services/projectService';
import { Project } from '../types/projectTypes';

const ProjectsList: React.FC = () => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    // Fetch the projects when the component mounts
    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const data = await getProjects();
                setProjects(data);
            } catch (error) {
                setError('Failed to fetch projects.');
            } finally {
                setLoading(false);
            }
        };

        fetchProjects();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div>
            <h1>Projects List</h1>
            <ul>
                {projects.map((project) => (
                    <li key={project.title}>
                        <strong>{project.title}</strong> - {project.description}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ProjectsList;
