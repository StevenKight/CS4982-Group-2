import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Source } from '../interfaces/Source';
import { Note } from '../interfaces/Note';

import './styles/MySourceNotes.css';

export default function MySourceNotes() {
    const navigate = useNavigate();
    const { sourceid } = useParams();

    const [source, setSource] = useState<Source>();
    const [notes, setNotes] = useState<Note[]>([]);

    const [newNoteText, setNewNoteText] = useState<string>('');

    const [newTagText, setNewTagText] = useState<string>('');

    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        getSource();
    }, [sourceid]);

    const getSource = () => {
        const username = localStorage.getItem('username');

        if (username) {
            fetch(`/source/${sourceid}-${username}`)
                .then((response) => response.json())
                .then((data) => {
                    setSource(data);
                    getNotes();
                })
                .catch(() => {
                    setError('Error fetching sources');
                });
        }
    };

    const getNotes = () => {
        const username = localStorage.getItem('username');

        if (username) {
            fetch(`/notes/${sourceid}-${username}`)
                .then((response) => response.json())
                .then((data) => {
                    setNotes(data);
                })
                .catch(() => {
                    setError('Error fetching notes');
                });
        }
    };

    const saveNote = () => {
        if (newNoteText.trim() === '') {
            setError('Note text cannot be empty, note will not be added');
            return;
        }

        const username = localStorage.getItem('username');

        if (username) {
            const tagsArray = newTagText.split(',').map(tag => tag.trim());

            fetch(`/notes/${username}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    sourceId: sourceid,
                    username: username,
                    noteText: newNoteText,
                    tagsString: tagsArray.join(','), 
                    noteDate: new Date().toISOString(), 
                }),
            })
                .then((response) => {
                    if (response.ok) {
                        setNewNoteText('');
                        setNewTagText('');
                        setError(null);
                        getNotes();
                    } else {
                        throw new Error("Error saving note's data, the note will not be added");
                    }
                })
                .catch(() => {
                    setError('Error connecting to our server to save the note, the note will not be added');
                });
        }
    };


    const cancelNote = () => {
        setNewNoteText('');
        setNewTagText('');
        setError(null);
    };

    const deleteSource = () => {
        const username = localStorage.getItem('username');

        if (username) {
            fetch(`/source/${sourceid}`, {
                method: 'DELETE'
            })
                .then((response) => {
                    if (response.ok) {
                        console.log('Source deleted');
                        navigate('/my-sources');
                    }
                    else {
                        throw new Error('Error deleting source');
                    }
                })
                .catch(() => {
                    setError('Error deleting source');
                });
        }
    }

    if (error) {
        return (
            <div>
                <h1>Unable to load source {sourceid}</h1>
                <p>{error}</p>
            </div>
        );
    }

    return (
        <div id='my-source-notes'>
            <div id='my-source-notes-heading'>
                <div>
                    <h1>{source?.name}</h1>
                    <p>{source?.description}</p>
                </div>
                <div>
                    <button onClick={deleteSource}>
                        Delete Source
                    </button>
                </div>
            </div>
            <p>{source?.link}</p>
            <p>Created at: {source?.createdAt?.toString()}</p>
            {source?.updatedAt && <p>Updated at: {source?.updatedAt?.toString()}</p>}
            <div id='add-note-section'>
                <h2>Add Note</h2>
                <textarea
                    value={newNoteText}
                    onChange={(e) => setNewNoteText(e.target.value)}
                    placeholder="Enter your note here..."
                />
                <div id='add-tag-section'>
                    <input
                        type="text"
                        value={newTagText}
                        onChange={(e) => setNewTagText(e.target.value)}
                        placeholder="Add tags (comma-separated)..."
                    />
                </div>
                <div>
                    <button onClick={saveNote}>Save</button>
                    <button onClick={cancelNote}>Cancel</button>
                </div>
            </div>
            <h2>Notes</h2>
            <ul>
                {notes.map((note) => (
                    <li key={note.noteId}>
                        <p>{note.noteText}</p>
                        <p>Tags: {note.tagsString}</p>
                        <p>Created at: {note.noteDate?.toString()}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
}