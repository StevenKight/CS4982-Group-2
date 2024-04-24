import { useEffect, useState } from "react";

import { Source } from "../interfaces/Source";

import SourcesGrid from "../components/SourcesGrid";

import './styles/MySources.css';
import AddSourceDialog from "../components/AddSourceDialog";
import { Tag } from "../interfaces/Note";
import TagDropdown from "../components/TagDropdown";

/**
 * MySources component displaying a user's sources.
 * 
 * @returns {JSX.Element} The rendered MySources component.
 */
export default function MySources() {
    const [sources, setSources] = useState<Source[]>([]);
    const[tags,setTags] = useState<Tag[]>([])

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    /**
     * Effect hook to fetch and update user's sources.
     */
    useEffect(() => {
        getSources();
        getTags();
    }, []);

    /**
     * Function to fetch user's sources from the server.
     */
    const getSources = () => {
        setLoading(true);
        const username = localStorage.getItem('username');

        if (username) {
            fetch(`/source/${username}`)
                .then((response) => response.json())
                .then((data) => {
                    setSources(data);
                    setLoading(false);
                })
                .catch(() => {
                    setError('Error fetching sources');
                });
        }
    }
    const getTags = () => {
        setLoading(true);
        const username = localStorage.getItem('username');
        if  (username) {
            fetch(`/tag/${username}`).then((response)=> response.json())
            .then((data)=>{
                setTags(data);
                setLoading(false);
            })
        }
    }
    const getSourcesByTag = (tags:Tag[] | null) =>{
        if (tags == null){
            getSources();
            return;
        }
        setLoading(true);
        const username = localStorage.getItem('username');

        if (username) {
                fetch(`source/Tag/${username}`,{
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(tags),
                })
                .then((response)=>response.json())
                .then((data) =>{
                    setSources(data);
                    setLoading(false);
                    console.log(data);
                });
        }
    }
    

    /**
     * Function to open the Add Source dialog.
     */
    const addSource = () => {
        const dialog = document.getElementById('add-source-dialog') as HTMLDialogElement;
        if (dialog) {
            dialog.showModal();
        }
    }

    if (error) {
        return (
            <div>
                <div id="my-sources-heading">
                    <h1>My Sources</h1>
                </div>
                <p>{error}</p>
            </div>
        );
    }

    if (sources.length === 0 && loading) {
        return (
            <div>
                <div id="my-sources-heading">
                    <h1>My Sources</h1>
                </div>
                <p>loading sources...</p>
            </div>
        );
    }

    if (sources.length === 0 && !loading) {
        return (
            <div>
                <AddSourceDialog id="add-source-dialog" onAdd={getSources}/>
                <div id="my-sources-heading">
                    <h1>My Sources</h1>
                    <button aria-label="Add Source"
                        onClick={addSource}>
                        +
                    </button>
                </div>
                <p>No sources found</p>
            </div>
        );
    }

    return (
        <>
            <div style={{maxHeight:"40px"}}>
            <AddSourceDialog id="add-source-dialog" onAdd={getSources}/>
            </div>
            <div>
                <div id="my-sources-heading">
                    <h1>My Sources</h1>
                    {
                        !loading ? 
                            <button aria-label="Add Source"
                                onClick={addSource}>
                                +
                            </button> 
                            : null
                    }
                </div>
                <TagDropdown tags={tags}  getSources ={getSourcesByTag}></TagDropdown>
                <SourcesGrid sources={sources}/>
            </div>
        </>
    );
}