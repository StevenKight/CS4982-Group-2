import { useEffect, useState } from "react";
import SourcesGrid from "../components/SourcesGrid";

import { Source, SourceType } from "../interfaces/Source";

import "./styles/MyShared.css";

const dummySources: Source[] = [
    { sourceId: 1, name: "Source 1", description: "Description 1", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 2, name: "Source 2", description: "Description 2", isLink: false, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 3, name: "Source 3", description: "Description 3", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 4, name: "Source 4", description: "Description 4", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 5, name: "Source 5", description: "Description 5", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 6, name: "Source 6", description: "Description 6", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 7, name: "Source 7", description: "Description 7", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 8, name: "Source 8", description: "Description 8", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
];

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
            setSharedSources(dummySources); // TODO: Replace with fetch call
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
        <>
            <div>
                <div id="shared-sources-heading">
                    <h1>Shared Sources</h1>
                </div>
                <SourcesGrid sources={sharedSources}/>
            </div>
        </>
    );
}