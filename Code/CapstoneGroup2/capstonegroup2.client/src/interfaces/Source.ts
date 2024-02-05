// Source.ts
export enum SourceType {
    Pdf = 1,
    Vid = 2,
}

export type Source = {
    id: number;
    name: string;
    type: SourceType;
    link: string;
    createdDateTime: string;
    status: 'own' | 'shared';
    recentlyViewedDateTime: string;
};

export const getSourceTypeName = (type: SourceType): string => {
    return type === SourceType.Pdf ? 'Pdf' : 'Vid';
};
