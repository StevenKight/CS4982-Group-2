import { useState, useEffect } from 'react';
import { Source } from '../interfaces/Source';

import SourceCard from './SourceCard';

import './styles/SourcesGrid.css';

/**
 * React component for displaying a grid of sources with filtering options.
 *
 * @param {Object} props - The properties passed to the component.
 * @param {Source[]} props.sources - The array of sources to display in the grid.
 * @param {boolean} [props.showUser] - Flag to determine whether to show the username in the source cards.
 *
 * @returns {JSX.Element} - JSX representation of the SourcesGrid component.
 */
export default function SourcesGrid({ sources, showUser }: { sources: Source[], showUser?: boolean}) {

    const [searchTerm, setSearchTerm] = useState<string>('');
    const [filteredSources, setFilteredSources] = useState<Source[]>([]);
    const [allSources, setAllSources] = useState<Source[]>([]);

    /**
     * useEffect hook to update the allSources state when the sources prop changes.
     */
    useEffect(() => {
        setAllSources(sources);
    }, [sources]);

    /**
     * useEffect hook to filter sources based on the search term and update the filteredSources state.
     */
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
