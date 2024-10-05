export interface Project {
    id?: number; 
    title: string;
    description: string;
    githubLink?: string;
    deployedLink?: string;
    isVisible: boolean;
    startDate?: Date;
    endDate?: Date;
    difficulty: DifficultyLevel; 
    categoryId: number;
    technologyIds: number[];
}

export enum DifficultyLevel {
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3
}

export interface ProjectRead {
    id: number;
    title: string;
    description: string;
    githubLink?: string;
    deployedLink?: string;
    isVisible: boolean;
    startDate?: Date;
    endDate?: Date;
    difficultyValue: number;
    difficultyName: string;
    categoryName: string;
    technologies: TechnologyReadDto[];
}

export interface TechnologyReadDto {
    id: number;
    name: string;
}
