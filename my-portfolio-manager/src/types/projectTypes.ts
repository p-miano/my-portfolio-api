export interface Project {
    title: string;
    description: string;
    githubLink?: string;
    deployedLink?: string;
    isVisible: boolean;
    startDate?: Date;
    endDate?: Date;
    difficulty: number;
    categoryId: number;
    technologyIds: number[];
}