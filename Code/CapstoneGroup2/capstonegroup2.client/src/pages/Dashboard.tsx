import { useState, useEffect } from 'react';

import { Source } from '../interfaces/Source';

import SourceCard from '../components/SourceCard';

import './styles/Dashboard.css';

/**
 * Dashboard component displaying user's notes and recently shared sources.
 * 
 * @returns {JSX.Element} The rendered Dashboard component.
 */
export default function Dashboard() {

    const [sources, setSources] = useState<Source[]>([]);

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
        </div>
    );
}