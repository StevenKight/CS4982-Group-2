// Source.ts
export enum SourceType {
    Pdf = 1,
    Vid = 2,
}

export const getSourceTypeName = (type: SourceType): string => {
    return type === SourceType.Pdf ? 'Pdf' : 'Vid';
};

export type Source = {
    sourceId: number;
    username: string;
    type: string;
    noteType: SourceType;
    name: string;
    description: string;
    isLink: boolean;
    link: string;
    createdAt: Date | null;
    updatedAt: Date | null;
    file: File | null;
};

