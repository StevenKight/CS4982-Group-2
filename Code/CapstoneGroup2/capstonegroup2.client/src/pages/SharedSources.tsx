import React from 'react';
import SharedSourcesList from '../components/SharedSourcesList';
import { Source } from '../interfaces/Source';

interface SharedSourcesProps {
    sharedSources: Source[];
}

const SharedSources: React.FC<SharedSourcesProps> = ({ sharedSources }) => {
    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes - Shared Sources</h1>
            </div>
            {sharedSources.length > 0 ? (
                <SharedSourcesList sharedSources={sharedSources} />
            ) : (
                <p>No shared sources available.</p>
            )}
        </div>
    );
};

export default SharedSources;
