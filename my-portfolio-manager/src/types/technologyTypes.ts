// TechnologyCreateDto
export interface TechnologyCreateDto {
    name: string;
    technologyGroupId: number;
}

// TechnologyReadDto
export interface TechnologyReadDto {
    id: number;
    name: string;
    technologyGroupName: string; 
}

// TechnologyUpdateDto
export interface TechnologyUpdateDto {
    name: string;
    technologyGroupId: number;
}
