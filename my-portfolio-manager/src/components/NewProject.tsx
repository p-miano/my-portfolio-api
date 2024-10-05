import React, { useState, useEffect } from 'react';
import { createProject } from '../services/projectService';
import { getCategories } from '../services/categoryService';
import { getTechnologies } from '../services/technologyService';
import { getTechnologyGroups } from '../services/technologyGroupService';
import { DifficultyLevel, Project } from '../types/projectTypes';
import { CategoryReadDto } from '../types/categoryTypes';
import { TechnologyGroupReadDto } from '../types/technologyGroupTypes';
import { TechnologyReadDto } from '../types/technologyTypes';

const NewProject = () => {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [githubLink, setGithubLink] = useState('');
    const [deployedLink, setDeployedLink] = useState('');
    const [isVisible, setIsVisible] = useState(true);
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');

    const [categoryId, setCategoryId] = useState<number | undefined>(undefined);
    const [technologyGroupId, setTechnologyGroupId] = useState<number | undefined>(undefined);
    const [technologyIds, setTechnologyIds] = useState<number[]>([]);
    const [difficulty, setDifficulty] = useState<DifficultyLevel | undefined>(undefined);

    // Explicitly define the types for categories, technology groups, and technologies
    const [categories, setCategories] = useState<CategoryReadDto[]>([]);
    const [technologyGroups, setTechnologyGroups] = useState<TechnologyGroupReadDto[]>([]);
    const [allTechnologies, setAllTechnologies] = useState<TechnologyReadDto[]>([]);
    const [technologies, setTechnologies] = useState<TechnologyReadDto[]>([]);

    // Fetch categories, technology groups, and technologies
    useEffect(() => {
        const fetchData = async () => {
            try {
                const categoryData = await getCategories();
                setCategories(categoryData); // Ensure this is properly set

                const technologyGroupData = await getTechnologyGroups();
                setTechnologyGroups(technologyGroupData);

                const technologyData = await getTechnologies();
                setAllTechnologies(technologyData); // Populate all technologies
            } catch (error) {
                console.error('Error fetching data', error);
            }
        };
        fetchData();
    }, []);

    // Filter technologies by selected technology group
    useEffect(() => {
        if (technologyGroupId) {
            const selectedGroup = technologyGroups.find(group => group.id === technologyGroupId);
            if (selectedGroup) {
                const filteredTechnologies = allTechnologies.filter(tech => tech.technologyGroupName === selectedGroup.name);
                setTechnologies(filteredTechnologies); // Apply the filter
            }
        } else {
            setTechnologies([]); // Clear when no group selected
        }
    }, [technologyGroupId, allTechnologies, technologyGroups]);

    // Handle form submission
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const newProject: Project = {
                title,
                description,
                githubLink,
                deployedLink,
                isVisible,
                startDate: startDate ? new Date(startDate) : undefined,
                endDate: endDate ? new Date(endDate) : undefined,
                categoryId: categoryId || 0,
                technologyIds,
                difficulty: difficulty ?? DifficultyLevel.Beginner,
            };
            await createProject(newProject);
            window.location.href = '/projects';
        } catch (error) {
            console.error('Error creating project:', error);
        }
    };

    return (
        <div className="container mt-5">
            <h2>Add New Project</h2>
            <form onSubmit={handleSubmit}>
                {/* Title Input */}
                <div className="mb-3">
                    <label className="form-label">Title</label>
                    <input
                        type="text"
                        className="form-control"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        required
                    />
                </div>

                {/* Description Input */}
                <div className="mb-3">
                    <label className="form-label">Description</label>
                    <textarea
                        className="form-control"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        required
                    ></textarea>
                </div>

                {/* Github Link Input */}
                <div className="mb-3">
                    <label className="form-label">GitHub Link</label>
                    <input
                        type="text"
                        className="form-control"
                        value={githubLink}
                        onChange={(e) => setGithubLink(e.target.value)}
                    />
                </div>

                {/* Deployed Link Input */}
                <div className="mb-3">
                    <label className="form-label">Deployed Link</label>
                    <input
                        type="text"
                        className="form-control"
                        value={deployedLink}
                        onChange={(e) => setDeployedLink(e.target.value)}
                    />
                </div>

                {/* Is Visible Checkbox */}
                <div className="mb-3">
                    <label className="form-label">Is Visible</label>
                    <input
                        type="checkbox"
                        className="form-check-input"
                        checked={isVisible}
                        onChange={(e) => setIsVisible(e.target.checked)}
                    />
                </div>

                {/* Start Date Input */}
                <div className="mb-3">
                    <label className="form-label">Start Date</label>
                    <input
                        type="date"
                        className="form-control"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                </div>

                {/* End Date Input */}
                <div className="mb-3">
                    <label className="form-label">End Date</label>
                    <input
                        type="date"
                        className="form-control"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                </div>

                {/* Category Dropdown */}
                <div className="mb-3">
                    <label className="form-label">Category</label>
                    <select
                        className="form-control"
                        value={categoryId || ''}
                        onChange={(e) => setCategoryId(Number(e.target.value))}
                    >
                        <option value="">Select Category</option>
                        {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                                {category.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Technology Group Dropdown */}
                <div className="mb-3">
                    <label className="form-label">Technology Group</label>
                    <select
                        className="form-control"
                        value={technologyGroupId || ''}
                        onChange={(e) => setTechnologyGroupId(Number(e.target.value))}
                    >
                        <option value="">Select Technology Group</option>
                        {technologyGroups.map((group) => (
                            <option key={group.id} value={group.id}>
                                {group.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Technology Multi-Select */}
                <div className="mb-3">
                    <label className="form-label">Technologies</label>
                    <select
                        multiple
                        className="form-control"
                        value={technologyIds.map(String)} // Convert number[] to string[]
                        onChange={(e) =>
                            setTechnologyIds(
                                Array.from(e.target.selectedOptions, (option) =>
                                    Number(option.value)
                                )
                            )
                        }
                    >
                        {technologies.map((technology) => (
                            <option key={technology.id} value={technology.id.toString()}>
                                {technology.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Difficulty Dropdown */}
                <div className="mb-3">
                    <label className="form-label">Difficulty Level</label>
                    <select
                        className="form-control"
                        value={difficulty || ''}
                        onChange={(e) => setDifficulty(Number(e.target.value))}
                    >
                        <option value="">Select Difficulty</option>
                        <option value={DifficultyLevel.Beginner}>Beginner</option>
                        <option value={DifficultyLevel.Intermediate}>Intermediate</option>
                        <option value={DifficultyLevel.Advanced}>Advanced</option>
                    </select>
                </div>

                {/* Submit Button */}
                <button type="submit" className="btn btn-primary">Add Project</button>
            </form>
        </div>
    );
};

export default NewProject;
