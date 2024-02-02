import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

enum NoteType {
    Pdf = 1,
    Vid = 2
}

interface Note {
    id: number;
    objectLink: string;
    Note: string;
    noteType: NoteType;
}

interface AddProps {
    onAddNote: (newNote: Note) => void;
}

const Add: React.FC<AddProps> = ({ onAddNote }) => {
    const [objectLink, setObjectLink] = useState<string>('');
    const [note, setNote] = useState<string>('');
    const [noteType, setNoteType] = useState<NoteType>(NoteType.Pdf);
    const navigate = useNavigate();

    const handleAddNote = () => {
        navigate('/postauthorize'); // Remove this when API is working
        const newNote: Note = {
            id: Date.now(),
            objectLink,
            Note: note,
            noteType
        };
        
        onAddNote(newNote);
        
        navigate('/postauthorize');
    };

    return (
        <div>
            <h2>Add Note</h2>
            <form>
                <label>Object Link:</label>
                <input type="text" value={objectLink} onChange={(e) => setObjectLink(e.target.value)} />

                <label>Note:</label>
                <textarea value={note} onChange={(e) => setNote(e.target.value)} />

                <label>Note Type:</label>
                <select value={noteType} onChange={(e) => setNoteType(Number(e.target.value))}>
                    <option value={NoteType.Pdf}>Pdf</option>
                    <option value={NoteType.Vid}>Video</option>
                </select>

                <button type="button" onClick={handleAddNote}>
                    Add Note
                </button>
            </form>
        </div>
    );
};

export default Add;
