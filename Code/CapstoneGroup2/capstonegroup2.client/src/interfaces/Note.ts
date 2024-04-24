type Note = {
    noteId: number;
    sourceId: number;
    username: string;
    noteText: string;
    noteDate: Date;
    tags: Tag[] | null;
}

type Tag = {
    tagID: number;
    tagName: string;
}

export type { Note, Tag };