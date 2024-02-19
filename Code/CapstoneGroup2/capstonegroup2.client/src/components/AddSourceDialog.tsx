import { useState } from 'react';

import './styles/AddSourceDialog.css';

import { Source } from '../interfaces/Source';

// const fileTypeMaps = [
//     {type: SourceType.Vid, extensions: ['mp4', 'avi', 'mov']},
//     {type: SourceType.Pdf, extensions: ['pdf']},
// ]

export default function AddSourceDialog({ id, onAdd }: { id: string, onAdd: () => void }) {

    const [isLink, setIsLink] = useState(false);

    const [file, setFile] = useState(null as File | null);

    const [authors, setAuthors] = useState([] as string[]);
    const [selectedAuthor, setSelectedAuthor] = useState(null as string | null);

    const closeDialog = () => {
        // Close the dialog
        const dialog = document.getElementById('add-source-dialog') as HTMLDialogElement;
        if (dialog) {
            dialog.close();
        }

        var authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        var selected = authorOptions.querySelector('.selected');
        if (selected) {
            selected.classList.remove('selected');
        }

        setIsLink(false);
        setFile(null);
        setAuthors([]);
        setSelectedAuthor(null);

        // Reset the form
        const form = document.querySelector('.add-source-dialog') as HTMLFormElement;
        form.reset();
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const form = e.currentTarget;
        const inputs = form.querySelectorAll('input');

        const data: { [key: string]: any } = {}; // Explicitly define the type of data

        inputs.forEach(input => {
            if (input.type === 'radio') return;

            if (input.type === 'file' && file) {
                data[input.id] = file;
            }
            else if (input.id) {
                data[input.id] = input.value;
            }
        });

        const source = {
            type: 'Pdf',
            name: data['name'],
            description: data['description'],
            isLink: isLink,
            authorsString: authors.join('|'),
            publisher: data['publisher'],
            accessedAt: data['accessedAt'],
        } as Source;

        if (isLink) {
            source.link = data['link'];
        }
        else {
            const arrayBuffer = await file?.arrayBuffer();
            if (arrayBuffer) {
                const bytes = new Uint8Array(arrayBuffer);
                const base64String = btoa(String.fromCharCode(...bytes));
                source.content = base64String;
            }
        }

        const username = localStorage.getItem('username');
        if (username) {
            source.username = username;

            fetch(`/source/${username}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(source),
                })
                .then(response => {
                    if (response.status === 200) {
                        setTimeout(() => {
                            onAdd();
                            closeDialog();
                        }, 250);
                    }
                    else {
                        throw Error("Could not add source");
                    }
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }
    }

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        
        const file = e.currentTarget.files?.item(0);
        if (file) {
            setFile(file);
        }
    }

    const handleAuthorClick = (e: React.MouseEvent<HTMLInputElement>) => {
        const author = e.currentTarget;
        
        var authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        var selected = authorOptions.querySelector('.selected');
        if (selected) {
            selected.classList.remove('selected');
        }

        setSelectedAuthor(author.value); // TODO: Change to author name
        author.classList.toggle('selected');
    }

    const handleAddAuthor = (e: React.MouseEvent<HTMLButtonElement>) => {

        e.preventDefault();

        // Add an author
        var authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        var selected = authorOptions.querySelector('.selected');
        if (selected) {
            selected.classList.remove('selected');
        }

        var authorName = prompt('Enter the author name');
        if (!authorName) return;

        const updatedAuthors = authors.concat(authorName);
        setAuthors(updatedAuthors);
    }

    const handleRemoveAuthor = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        
        // Remove an author
        const updatedAuthors = authors.filter(author => author !== selectedAuthor);
        setAuthors(updatedAuthors);
    }

    return (
        <dialog id={id} className='add-source-dialog'>
            
            <form className='add-source-dialog-form' onSubmit={handleSubmit}>
                <div className='add-source-dialog-heading'>
                    <h2>Add a source</h2>
                    <button onClick={closeDialog}>Close</button>
                </div>

                <label htmlFor='name'>Name</label>
                <input type='text' id='name' placeholder='Enter a name for the source...' required/>

                <label htmlFor='description'>Description</label>
                <input type='text' id='description' placeholder='Enter a description...' />

                <div className='add-source-dialog-linkable-row'>
                    <div>
                        <label htmlFor='isLinkLink'>Link</label>
                        <input type="radio" id="isLinkLink" name="isLink" value="Link" 
                            required onClick={() => {setIsLink(true); setFile(null);}}/>
                    </div>

                    <div>
                        <label htmlFor='isLinkUpload'>Upload</label>
                        <input type="radio" id="isLinkUpload" name="isLink" value="Upload" 
                            onClick={() => setIsLink(false)} defaultChecked/>
                    </div>
                </div>

                {
                    isLink ? 
                        <>
                            <label htmlFor='link'>Link</label>
                            <input type='text' id='link' placeholder='Enter a link...' required={isLink}/>
                        </> :
                        <>
                            {/* TODO: Add more file types based on source type selection */}
                            <label htmlFor='file'>File</label>
                            <input type='file' id='file' accept='application/pdf'
                                required={!isLink} onChange={(e) => handleFileChange(e)}/>
                        </>
                }

                <div className='add-source-dialog-author-heading'>
                    <label>Authors:</label>
                    <div style={{display: 'flex'}}>
                        <button onClick={handleAddAuthor}>+</button>
                        <button onClick={handleRemoveAuthor} disabled={selectedAuthor === null}>-</button>
                    </div>
                </div>
                <div className='add-source-dialog-author-selector'>
                    {
                        authors.map((author, index) => {
                            return (
                                <input key={index} type='text' id={'author-' + index} onClick={handleAuthorClick} value={author} />
                            );
                        })
                    }
                </div>

                <label htmlFor='publisher'>Publisher</label>
                <input type='text' id='publisher' placeholder='Enter the publisher...' />

                <label htmlFor='accessedAt'>Accessed At</label>
                <input type='date' id='accessedAt'/>

                <div className='add-source-dialog-finish-row'>
                    <input type='submit' value='Add'/>
                    <input type='reset' value='Cancel' onClick={closeDialog}/>
                </div>
            </form>
           
        </dialog>
    );
}