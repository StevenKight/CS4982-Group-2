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

    const closeDialog = () => {
        // Close the dialog
        const dialog = document.getElementById('add-source-dialog') as HTMLDialogElement;
        if (dialog) {
            dialog.close();
        }

        setIsLink(false);

        // Reset the form
        const form = document.querySelector('.add-source-dialog') as HTMLFormElement;
        form.reset();
    }

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
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
            name: data.name,
            description: data.description,
            isLink: isLink,
            link: data.link,
            file: file,
            type: 'Pdf', // TODO: Add a way to select the source type
        } as Source;

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
        const file = e.currentTarget.files?.item(0);
        if (file) {
            setFile(file);
        }
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

                <label htmlFor='tags'>Tags</label>
                <input type='text' id='tags' placeholder='Enter tags...' />

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
                            <input type='file' id='file' accept='.pdf'
                                required={!isLink} onChange={handleFileChange}/>
                        </>
                }

                <div className='add-source-dialog-finish-row'>
                    <input type='submit' value='Add'/>
                    <input type='reset' value='Cancel' onClick={closeDialog}/>
                </div>
            </form>
           
        </dialog>
    );
}