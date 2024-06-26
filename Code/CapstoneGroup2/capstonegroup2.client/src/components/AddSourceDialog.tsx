import { useEffect, useState } from 'react';

import './styles/AddSourceDialog.css';

import { Source, SourceType, getSourceTypeName } from '../interfaces/Source';

const fileTypeMaps = [
    { type: SourceType.Vid, extensions: ['mp4', 'avi', 'mov'] },
    { type: SourceType.Pdf, extensions: ['pdf'] },
]

/**
 * React component for adding a new source.
 *
 * @param {string} id - The unique identifier for the dialog.
 * @param {() => void} onAdd - Callback function invoked when a source is successfully added.
 * @returns {JSX.Element} - JSX representation of the AddSourceDialog component.
 */
export default function AddSourceDialog({ id, onAdd }: { id: string, onAdd: () => void }) {

    const [isLink, setIsLink] = useState(false);

    const [file, setFile] = useState(null as File | null);
    const [fileBytes, setFileBytes] = useState(null as string | null);

    const [sourceType, setSourceType] = useState(null as SourceType | null);
    const [authors, setAuthors] = useState([] as string[]);
    const [selectedAuthor, setSelectedAuthor] = useState(null as string | null);

    useEffect(() => {
        const dialog = document.getElementById(id) as HTMLDialogElement;
        const dialogBackground = document.getElementsByClassName('add-source-dialog-background')[0] as HTMLElement;
        
        // Create a new MutationObserver instance
        const observer = new MutationObserver((mutationsList, observer) => {
            if (mutationsList.length > 0 
                && mutationsList.some((mutation) => mutation.type === 'attributes') 
                && mutationsList.some((mutation) => mutation.attributeName === 'open')) {
                
                    dialogBackground.classList.toggle('open');
            }
        });

        // Start observing the dialog element
        observer.observe(dialog, { attributes: true, childList: true, subtree: true });

        // Stop observing when the component unmounts
        return () => {
            observer.disconnect();
        };
    }, []);

    /**
     * Closes the dialog and resets the form state.
     */
    const closeDialog = () => {
        // Close the dialog
        const dialog = document.getElementById('add-source-dialog') as HTMLDialogElement;
        if (dialog) {
            dialog.close();
        }

        const authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        const selected = authorOptions.querySelector('.selected');
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

    /**
     * Handles the form submission, processes form data, and triggers API request to add a new source.
     *
     * @param {React.FormEvent<HTMLFormElement>} e - The form submission event.
     * @returns {Promise<void>} - A Promise resolving after the form submission is processed.
     */
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

        if (sourceType == null){
            alert("Please select a file type.");
            return;
        }

        if (data['accessedAt'] == "") {
            data['accessedAt'] = null;
        }

        const source = {
            type: getSourceTypeName(sourceType),
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
            source.content = fileBytes;
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

    /**
     * Handles file input change and updates the file state.
     *
     * @param {React.ChangeEvent<HTMLInputElement>} e - The file input change event.
     */
    const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();

        const file = e.currentTarget.files?.item(0);
        if (file) {
            setFile(file);

            const arrayBuffer = await file?.arrayBuffer();
            if (arrayBuffer) {
                const bytes = new Uint8Array(arrayBuffer);
                const base64String = btoa(
                    bytes.reduce((data, byte) => 
                        data + String.fromCharCode(byte), ''
                    )
                  );
                setFileBytes(base64String);
            }
        }
    }

    /**
     * Determines the 'accept' attribute for the file input based on the selected source type.
     *
     * @returns {string} - The 'accept' attribute value containing MIME types or file extensions.
     */
    const getFileAcceptAttribute = () => {
        const fileTypeMap = fileTypeMaps.find((map) => map.type == sourceType);
        if (fileTypeMap) {
            return fileTypeMap.extensions.map((ext) => `.${ext}`).join(',');
        }
        return '*/*';
    }

    /**
     * Handles author click and updates the selected author state.
     *
     * @param {React.MouseEvent<HTMLInputElement>} e - The author click event.
     */
    const handleAuthorClick = (e: React.MouseEvent<HTMLInputElement>) => {
        const author = e.currentTarget;
        
        const authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        const selected = authorOptions.querySelector('.selected');
        if (selected) {
            selected.classList.remove('selected');
        }

        setSelectedAuthor(author.value); // TODO: Change to author name
        author.classList.toggle('selected');
    }

    /**
     * Adds a new author to the authors state.
     *
     * @param {React.MouseEvent<HTMLButtonElement>} e - The add author button click event.
     */
    const handleAddAuthor = (e: React.MouseEvent<HTMLButtonElement>) => {

        e.preventDefault();

        // Add an author
        const authorOptions = document.querySelector('.add-source-dialog-author-selector');

        if (!authorOptions) return;

        const selected = authorOptions.querySelector('.selected');
        if (selected) {
            selected.classList.remove('selected');
        }

        const authorName = prompt('Enter the author name');
        if (!authorName) return;

        const updatedAuthors = authors.concat(authorName);
        setAuthors(updatedAuthors);
    }

    /**
     * Removes the selected author from the authors state.
     *
     * @param {React.MouseEvent<HTMLButtonElement>} e - The remove author button click event.
     */
    const handleRemoveAuthor = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();

        // Remove an author
        const updatedAuthors = authors.filter(author => author !== selectedAuthor);
        setAuthors(updatedAuthors);
    }

    return (
        <>
            <div className='add-source-dialog-background'/>

            <dialog id={id} className='add-source-dialog'>
                <form className='add-source-dialog-form' onSubmit={handleSubmit}>
                    <div className='add-source-dialog-heading'>
                        <h2>Add a source</h2>
                        <p className='add-source-dialog-info'>* - Required Field</p>
                        <button onClick={closeDialog}>Close</button>
                    </div>

                    <label htmlFor='name'>* Name</label>
                    <input type='text' id='name' placeholder='Enter a name for the source...' required />
    
                    <label htmlFor='description'>Description</label>
                    <input type='text' id='description' placeholder='Enter a description...' />
    
                    <div className='add-source-dialog-row'>
                        <div className='add-source-dialog-column'>
                            <div>
                                <label htmlFor='isLinkLink'>* Linked File</label>
                                <input type="radio" id="isLinkLink" name="isLink" value="Link"
                                    required onClick={() => { setIsLink(true); setFile(null); }} />
                            </div>
        
                            <div>
                                <label htmlFor='isLinkUpload'>* Upload File</label>
                                <input type="radio" id="isLinkUpload" name="isLink" value="Upload"
                                    onClick={() => setIsLink(false)} defaultChecked />
                            </div>
                        </div>
        
                        <div className='add-source-dialog-column'>
                            <label htmlFor='filetype'>* File type:</label>
                            <select name="filetype" id="filetype" required
                                onChange={
                                    (e) => {
                                        const selectedType = parseInt(e.currentTarget.value);
                                        setSourceType(selectedType);
                                    }
                                }>
                                <option selected disabled>Select a file type...</option>
                                <option value={SourceType.Pdf}>PDF</option>
                                <option value={SourceType.Vid}>Video</option>
                            </select>
                        </div>
                    </div>
    
                    {
                        isLink ?
                            <>
                                <label htmlFor='link'>* Link</label>
                                <input type='text' id='link' placeholder='Enter a link...' required={isLink} />
                            </> :
                            <>
                                <label htmlFor='file'>* File</label>
                                <input
                                    type='file'
                                    id='file'
                                    accept={getFileAcceptAttribute()}
                                    required={!isLink}
                                    onChange={(e) => handleFileChange(e)}
                                />
                            </>
                    }
    
                    <div className='add-source-dialog-row'>
                        <div className='add-source-dialog-column'>
                            <div className='add-source-dialog-author-heading'>
                                <label>Authors:</label>
                                <div style={{ display: 'flex' }}>
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
                        </div>
        
                        <div className='add-source-dialog-column'>
                            <label htmlFor='publisher'>Publisher</label>
                            <input type='text' id='publisher' placeholder='Enter the publisher...' />
            
                            <label htmlFor='accessedAt'>Accessed At</label>
                            <input type='date' id='accessedAt' />
                        </div>
                    </div>
    
                    <div className='add-source-dialog-finish-row'>
                        <input type='submit' value='Add' />
                        <input type='reset' value='Cancel' onClick={closeDialog} />
                    </div>
                </form>
    
            </dialog>
        </>
    );
}