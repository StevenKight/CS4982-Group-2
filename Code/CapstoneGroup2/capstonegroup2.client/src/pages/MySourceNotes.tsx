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
    }
        
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
    }

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
            <div>
                <h2>Notes</h2>
                <ul>
                    {
                        notes.map((note) => {
                            return (
                                <li key={note.noteId}>
                                    <p>{note.noteText}</p>
                                    <p>Created at: {note.noteDate?.toString()}</p>
                                </li>
                            );
                        })
                    }
                </ul>
            </div>
        </div>
    );
}