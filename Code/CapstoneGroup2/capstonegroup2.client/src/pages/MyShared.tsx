import { useEffect, useState } from "react";
import { Source } from "../interfaces/Source"
import SourcesGrid from "../components/SourcesGrid";

/**
 * MyShared component displaying sources shared with the current user.
 * 
 * @returns {JSX.Element} The rendered MyShared component.
 */
export default function MyShared() {

    const [sharedSources, setSharedSources] = useState<Source[]>([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    /**
     * Effect hook to fetch and update user's sources.
     */
    useEffect(() => {
        getSharedSources();
    }, []);

    /**
     * Function to fetch user's sources from the server.
     */
    const getSharedSources = () => {
        setLoading(true);
        const username = localStorage.getItem('username');

        if (username) {
            setSharedSources([]); // TODO: Replace with fetch call
        }
        else {
            setError('No user logged in');
        }

        setLoading(false);
    }

    if (error) {
        return (
            <div>
                <div id="shared-sources-heading">
                    <h1>Shared Sources</h1>
                </div>
                <p>{error}</p>
            </div>
        );
    }

    if (sharedSources.length === 0 && loading) {
        return (
            <div>
                <div id="shared-sources-heading">
                    <h1>Shared Sources</h1>
                </div>
                <p>loading sources...</p>
            </div>
        );
    }

    if (sharedSources.length === 0 && !loading) {
        return (
            <div>
                <div id="shared-sources-heading">
                    <h1>Shared Sources</h1>
                </div>
                <p>No sources found</p>
            </div>
        );
    }

    return (
        <div>
            <h1>Dummy Shared with me</h1>
            <SourcesGrid sources={sharedSources} showUser/>
        </div>
    );
}