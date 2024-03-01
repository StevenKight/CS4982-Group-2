import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Source, SourceType } from '../interfaces/Source';
import { Note } from '../interfaces/Note';

import './styles/MySourceNotes.css';
import VideoPlayer from '../components/VideoPlayer';
import PdfViewer from '../components/PdfViewer';

/**
 * MySourceNotes component displaying notes for a specific source.
 * 
 * @returns {JSX.Element} The rendered MySourceNotes component.
 */
export default function MySourceNotes() {
    const navigate = useNavigate();
    const { sourceid } = useParams();

    const [source, setSource] = useState<Source>();
    const [notes, setNotes] = useState<Note[]>([]);

    const [newTagText, setNewTagText] = useState<string>('');

    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        getSource();
    }, [sourceid]);

    /**
     * Function to retrieve the source information and associated notes.
     */
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

    /**
     * Function to retrieve notes associated with the current source.
     */
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

    /**
     * Function to save a new note.
     */
    const saveNote = () => {
        const newNoteTextarea = document.getElementById('new-note-text') as HTMLTextAreaElement;
        const newNoteText = newNoteTextarea.value;

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
                        newNoteTextarea.value = '';
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

    /**
     * Function to cancel creating a new note.
     */
    const cancelNote = () => {
        const newNoteTextarea = document.getElementById('new-note-text') as HTMLTextAreaElement;
        newNoteTextarea.value = '';
        setNewTagText('');
        setError(null);
    };

    /**
     * Function to delete the current source.
     */
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
            <div className='my-source-notes-container'>
            <div className='my-source-notes-content-source'>
                    {
                        source?.noteType === SourceType.Pdf || source?.noteType === SourceType.Vid ?
                            source?.noteType === SourceType.Pdf ?
                                <PdfViewer pdf={source} /> :
                                <VideoPlayer video={source} /> :
                            <h3>Source type not supported yet.</h3>


                    }
                </div>
            <div className='my-source-notes-content'>
                <div className='my-source-notes-content-notes'>
                    <div className='my-source-notes-content-add-note-section'>
                        <h2>Add Note</h2>
                        <div className='new-note'>
                            <textarea
                                id='new-note-text'
                                placeholder="Enter your note here..."
                            />
                            <div className='new-note-options'>
                                <button onClick={saveNote}>Save</button>
                                <button onClick={cancelNote}>Cancel</button>
                            </div>
                        </div>
                    </div>
                    <h2>Notes</h2>
                    <ul>
                        {
                            notes.map((note) => (
                                <li key={note.noteId}>
                                    <p>{note.noteText}</p>
                                </li>
                            ))
                        }
                    </ul>
                </div>
                </div>
            </div>
        </div>
    );
}