import { useEffect, useState } from "react";

import { Source } from "../interfaces/Source";

import SourcesGrid from "../components/SourcesGrid";

import './styles/MySources.css';
import AddSourceDialog from "../components/AddSourceDialog";

/**
 * MySources component displaying a user's sources.
 * 
 * @returns {JSX.Element} The rendered MySources component.
 */
export default function MySources() {
    const [sources, setSources] = useState<Source[]>([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    /**
     * Effect hook to fetch and update user's sources.
     */
    useEffect(() => {
        getSources();
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
            <AddSourceDialog id="add-source-dialog" onAdd={getSources}/>
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
                <SourcesGrid sources={sources}/>
            </div>
        </>
    );
}