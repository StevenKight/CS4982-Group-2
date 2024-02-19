import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Source, SourceType } from '../interfaces/Source';
import { Note } from '../interfaces/Note';

import './styles/MySourceNotes.css';

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
        var newNoteTextarea = document.getElementById('new-note-text') as HTMLTextAreaElement;
        var newNoteText = newNoteTextarea.value;

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

    const cancelNote = () => {
        var newNoteTextarea = document.getElementById('new-note-text') as HTMLTextAreaElement;
        newNoteTextarea.value = '';
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
                <div className='my-source-notes-content-source'>
                    {
                        source?.noteType === SourceType.Pdf ? 
                            <PdfViewer pdf={source}/> : 
                            <h3>Source type not supported yet.</h3>
                    }
                </div>
            </div>
        </div>
    );
}

function PdfViewer({ pdf }: { pdf: Source }) {

    const loadPdf = () => {
        var objectURL = '';
        if (!pdf.isLink && pdf.content) {
            const base64String = pdf.content;

            // Decode the Base64 string to a Uint8Array
            const binaryString = atob(base64String);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            const blob = new Blob([bytes], { type: 'application/pdf' });
            objectURL = URL.createObjectURL(blob);
        }
        else if (pdf.isLink && pdf.link) {
            objectURL = pdf.link;
        }

        return (
            <iframe
                src={objectURL}
                height="100%"
                width="100%"
            />
        );
    }

    return (
        <div>
            {
                loadPdf()
            }
        </div>
    );
}