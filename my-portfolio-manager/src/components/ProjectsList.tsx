// ProjectsList.tsx
import React, { useState, useEffect } from 'react';
import { getProjects } from '../services/projectService'; // Import your project service

const ProjectsList = () => {
    const [projects, setProjects] = useState([]);

    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const data = await getProjects();
                setProjects(data);
            } catch (error) {
                console.error('Error fetching projects:', error);
            }
        };

        fetchProjects();
    }, []);

    return (
        <div className="container mt-5">
            <h2>Your Projects</h2>
            <div className="row">
                {projects.map((project) => (
                    <div key={project.id} className="col-md-4">
                        <div className="card mb-3">
                            <div className="card-body">
                                <h5 className="card-title">{project.title}</h5>
                                <p className="card-text">{project.description}</p>
                                <a href={project.githubLink} className="btn btn-primary">GitHub</a>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ProjectsList;
