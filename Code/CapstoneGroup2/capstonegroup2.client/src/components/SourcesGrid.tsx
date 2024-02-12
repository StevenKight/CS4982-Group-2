
import { useState, useEffect } from 'react';
import { Source } from '../interfaces/Source';

import SourceCard from './SourceCard';

import './styles/SourcesGrid.css';

export default function SourcesGrid({ sources, showUser }: { sources: Source[], showUser?: boolean}) {

    const [searchTerm, setSearchTerm] = useState<string>('');
    const [filteredSources, setFilteredSources] = useState<Source[]>([]);
    const [allSources, setAllSources] = useState<Source[]>([]);

    useEffect(() => {
        setAllSources(sources);
    }, [sources]);

    useEffect(() => {
        const filtered = allSources.filter((source) => {
            return source.name.toLowerCase().includes(searchTerm.toLowerCase());
        });

        setFilteredSources(filtered);
    }, [searchTerm, allSources]);

    return (
        <div>
            <div id="source-filter-options">
                <label htmlFor='search'>Filter by name:</label>
                <input type='text' placeholder='Enter a search term...' name="search"
                    value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
            </div>
            <div className='sources-grid'>
                {
                    filteredSources.map((source) => {
                        return (
                            <SourceCard key={source.sourceId} source={source} showUser={showUser} />
                        );
                    })
                }
            </div>
        </div>
    );
}
