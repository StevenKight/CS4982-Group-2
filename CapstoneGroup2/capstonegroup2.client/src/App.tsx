import { useEffect, useState } from 'react';
import './App.css';

import ReactPlayer from 'react-player';

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

function App() {
    const [notes, setNotes] = useState<Note[]>();
    const [selectedNote, setSelectedNote] = useState<Note>();

    useEffect(() => {
        populateNoteData();
    }, []);

    const contents = notes === undefined
        ? <p><em>Loading...</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Note Type</th>
                </tr>
            </thead>
            <tbody>
                {
                    notes.map(note =>
                        <tr key={note.id} onClick={() => setSelectedNote(note)}>
                            <td>{note.id}</td>
                            <td>{NoteType[note.noteType]}</td>
                        </tr>
                    )
                }
            </tbody>
        </table>;

    return (
        <div style={{display: "flex"}}>
            <div style={{textAlign: "left", marginRight: "4em"}}>
                <h1 id="tabelLabel">Notes Prototype</h1>
                <p>This component demonstrates fetching data from the server <br/> and database.</p>
                <div className='table-container'>
                    {contents}
                </div>
            </div>
            <div>
                {
                    selectedNote !== undefined
                        ? <NoteDisplay {...selectedNote} />
                        : <p><em>Select a note to view</em></p>
                }
            </div>
        </div>
    );

    async function populateNoteData() {
        const response = await fetch('notes');
        const data = await response.json();
        setNotes(data);
    }
}

function NoteDisplay(note: Note) {

    const PdfNoteDisplay = (note: Note) => <embed src={note.objectLink} />;

    const VideoNoteDisplay = (note: Note) => <ReactPlayer url={note.objectLink} controls/>;

    return (
        <div>
            {
                note.noteType === NoteType.Pdf
                    ? <PdfNoteDisplay {...note} />
                    : <VideoNoteDisplay {...note} />
            }
        </div>
    );
}

export default App;