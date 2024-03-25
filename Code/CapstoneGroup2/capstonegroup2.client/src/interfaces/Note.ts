type Note = {
    noteId: number;
    sourceId: number;
    username: string;
    noteText: string;
    noteDate: Date;
    tags: Tag[];
}

type Tag = {
    tagId: number;
    tagName: string;
}

export type { Note, Tag };