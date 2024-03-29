import { useState, useEffect } from 'react';

import { Source, SourceType } from '../interfaces/Source';

import SourceCard from '../components/SourceCard';

import './styles/Dashboard.css';

const dummySharedSources: Source[] = [
    { sourceId: 1, name: "Source 1", description: "Description 1", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 2, name: "Source 2", description: "Description 2", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 3, name: "Source 3", description: "Description 3", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 4, name: "Source 4", description: "Description 4", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 5, name: "Source 5", description: "Description 5", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 6, name: "Source 6", description: "Description 6", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 7, name: "Source 7", description: "Description 7", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
    { sourceId: 8, name: "Source 8", description: "Description 8", isLink: true, link: "http://www.example.com", type: "Pdf", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date(), file: null},
];

/**
 * Dashboard component displaying user's notes and recently shared sources.
 * 
 * @returns {JSX.Element} The rendered Dashboard component.
 */
export default function Dashboard() {

    const [sources, setSources] = useState<Source[]>([]);
    const [shared, setShared] = useState<Source[]>([]);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    /**
     * Fetch user's sources and recently shared sources on component mount.
     */
    useEffect(() => {
        setLoading(true);
        const username = localStorage.getItem('username');

        if (username) {
            fetch(`/source/${username}`)
                .then((response) => {
                    if (!response.ok) {
                        throw new Error('Error fetching sources');
                    }
                    return response.json();
                })
                .then((data) => {
                    setSources(data);
                    setLoading(false);
                })
                .catch(() => {
                    setError('Error fetching sources');
                });

            setShared(dummySharedSources);
        }
    }, []);

    if (error) {
        return (
            <div>
                <h1>Dashboard</h1>
                <p>{error}</p>
            </div>
        );
    }

    return (
        <div>
            <h1>Dashboard</h1>
            <div>
                {/* TODO: Make this recent notes */}
                <h3>User Notes</h3>
                {
                    !loading && sources.length === 0 ?
                        <p className='dashboard-no-data'>No notes found.</p> :
                        !loading ?
                            <div className='dashboard-notes-row'>
                                {
                                    sources.map((source) => {
                                        return (
                                            <SourceCard key={source.sourceId} source={source} showDate />
                                        );
                                    })
                                }
                            </div> :
                            <p>Loading...</p>
                }
            </div>
            <div>
                <h3>Dummy Recently Shared with you</h3>
                {
                    shared.length !== 0 || !loading ?
                    <div className='dashboard-notes-row'>
                        {
                            shared.map((source) => {
                                return (
                                    <SourceCard key={source.sourceId} source={source} showUser showDate/>
                                );
                            })
                        }
                    </div> :
                    <p>Loading...</p>
                }
            </div>
        </div>
    );
}