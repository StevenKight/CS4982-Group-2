
import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Source, SourceType } from '../interfaces/Source';
import { Note, Tag } from '../interfaces/Note';

import './styles/MySourceNotes.css';
import VideoPlayer from '../components/VideoPlayer';
import PdfViewer from '../components/PdfViewer';

const useForceUpdate = () => useState()[1] as () => void;

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
                    <NotesDisplay notes={notes} onDelete={getNotes} />
                </div>
                </div>
            </div>
        </div>
    );
}

function NotesDisplay({ notes, onDelete }: { notes: Note[], onDelete: () => void}) {
    
    const forceUpdate = useForceUpdate();

    const [selectedNote, setSelectedNote] = useState<Note | null>(null);

    function selectedNoteHandler() {
        notes.forEach((note) => {
            if (note.noteId === selectedNote?.noteId) {
                note.noteText = selectedNote?.noteText || '';
                note.tags = selectedNote?.tags || [];
            }
        });

        forceUpdate();
    }

    function deleteNote() {
        setSelectedNote(null);
        onDelete();
    }

    return (
        <div>
            <h2>Notes</h2>
            <div className='notes-display'>
                <NotesList notes={notes} selectedNote={selectedNote} setSelectedNote={setSelectedNote} />
                <NoteDisplay selectedNote={selectedNote} 
                    onChange={selectedNoteHandler} onDelete={deleteNote}/>
            </div>
        </div>
    );
}

function NotesList({ notes, selectedNote, setSelectedNote }: { notes: Note[], selectedNote: Note | null, setSelectedNote: (note: Note) => void }) {

    useEffect(() => {
        clearSelected();

        if (selectedNote) {
            const selectedElement = document.getElementById(`note-${selectedNote.noteId}`);
            if (selectedElement) {
                selectedElement.classList.add('selected');
            }
        }
    }, [selectedNote]);

    function clearSelected() {
        const selectedElements = document.getElementsByClassName('selected');
        for (let i = 0; i < selectedElements.length; i++) {
            if (selectedElements[i].nodeName === 'P') {
                selectedElements[i].classList.remove('selected');
            }
        }
    }

    function selectNote(event: React.MouseEvent<HTMLParagraphElement>, note: Note) {
        setSelectedNote(note);

        event.preventDefault();
        event.stopPropagation();

        clearSelected();
        event.currentTarget.classList.add('selected');
    }

    return (
        <div>
            <ul>
                {
                    notes.map((note: Note) => (
                        <p key={note.noteId} id={`note-${note.noteId}`}
                            onClick={(e) => selectNote(e, note)}>
                            {note.noteText}
                        </p>
                    ))
                }
            </ul>
        </div>
    );
}

function NoteDisplay({ selectedNote, onChange, onDelete }: { selectedNote: Note | null, onChange: () => void, onDelete: () => void}) {

    const forceUpdate = useForceUpdate();

    const [selectedTag, setSelectedTag] = useState<Tag | null>(null);
    const [noteText, setNoteText] = useState(selectedNote?.noteText || 'Please select a note to view its content.');

    useEffect(() => {
        setNoteText(selectedNote?.noteText || 'Please select a note to view its content.');
    }, [selectedNote]);

    function checkSelectedTag() {
        const selectedElements = document.getElementsByClassName('selected');
        let selectedTag = null;
        for (let i = 0; i < selectedElements.length; i++) {
            if (selectedElements[i].nodeName === 'LI') {
                selectedTag = selectedElements[i].textContent;
            }
        }
        return selectedTag !== null;
    }

    function deleteNote() {
        if (!selectedNote || selectedNote == null) {
            alert('Please select a note to remove');
            return;
        }

        fetch(`/notes/${selectedNote.noteId}`, {
            method: 'DELETE'
        })
            .then((response) => {
                if (response.ok) {
                    console.log('Note deleted');
                    setSelectedTag(null);
                    onDelete();
                }
                else {
                    throw new Error('Error deleting source');
                }
            });
    }

    function addTag() {
        if (!selectedNote || selectedNote == null) {
            alert('Please select a note to add a tag');
            return;
        }

        var tagName = prompt('Enter tag name');
        
        if (tagName === null || tagName === '' || /^\s*$/.test(tagName)) {
            alert('Tag name cannot be empty');
            return;
        }

        if (selectedNote.tags.find(tag => tag.tagName === tagName)) {
            alert('Tag already exists');
            return;
        }

        selectedNote.tags.push({
            tagId: Math.floor(Math.random() * 1000),
            tagName: tagName
        });

        updateNote();
        forceUpdate();
    }

    function deleteTag() {
        if (!selectedNote) {
            alert('Please select a note to delete a tag');
            return;
        }

        if (!checkSelectedTag()) {
            alert('Please select a tag to delete');
            setSelectedTag(null);
            return;
        }

        selectedNote.tags = selectedNote?.tags.filter(tag => tag.tagName !== selectedTag?.tagName) || [];
        console.log(selectedNote.tags);
        updateNote();
    }

    function updateNote() {
        const username = localStorage.getItem('username');

        if (!selectedNote || !username) {
            alert('Error updating note');
            return;
        }

        selectedNote.noteText = noteText;
        
        fetch(`/notes/${username}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(selectedNote),
            })
            .then((response) => {
                if (response.ok) {
                    setSelectedTag(null);
                    forceUpdate();
                    onChange();
                } else {
                    alert('Error updating note');
                }
            })
            .catch(() => {
                alert('Error updating note');
            });
    }

    function selectTag(event: React.MouseEvent<HTMLLIElement>, tag: Tag) {
        setSelectedTag(tag);

        event.preventDefault();
        event.stopPropagation();

        const selected = document.getElementsByClassName('selected');
        for (let i = 0; i < selected.length; i++) {
            if (selected[i].nodeName === 'LI') {
                selected[i].classList.remove('selected');
            }
        }

        event.currentTarget.classList.add('selected');
    }

    return (
        <div className='note-display'>
            <button disabled={!selectedNote}
                onClick={deleteNote} style={{marginTop: '-2em', marginBottom: '1em'}}>
                Delete Note
            </button>
            <textarea value={
                    selectedNote ? noteText : 'Please select a note to view its content.'
                } readOnly={!selectedNote} onChange={(e) => setNoteText(e.target.value)} 
                onBlur={updateNote}/>

            <div className='note-tags-options'>
                <button disabled={!selectedNote}
                    onClick={addTag}>
                    Add Tag
                </button>
                <button disabled={!selectedNote || !selectedTag}
                    
                    onClick={deleteTag}>
                    Delete Tag
                </button>
            </div>
            <ul>
                {
                    selectedNote ? selectedNote.tags.map((tag: Tag) => (
                        <li key={tag.tagId} onClick={(e) => selectTag(e, tag)}>
                            {tag.tagName}
                        </li>
                    )) : null
                }
            </ul>
        </div>
    );
}