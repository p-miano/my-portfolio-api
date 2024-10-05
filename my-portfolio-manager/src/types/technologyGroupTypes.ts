// TechnologyGroupCreateDto
export interface TechnologyGroupCreateDto {
    name: string;
}

// TechnologyGroupReadDto
export interface TechnologyGroupReadDto {
    id: number;
    name: string;
    technologies: TechnologyReadDto[]; // A list of technologies belonging to the group
}

// TechnologyGroupUpdateDto
export interface TechnologyGroupUpdateDto {
    name: string;
}
