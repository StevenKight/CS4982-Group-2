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
    link: string | null;
    content: string | null;
    createdAt: Date | null;
    updatedAt: Date | null;
    authorsString: string;
    authors: string[];
    publisher: string;
    accessedAt: Date | null;
};

