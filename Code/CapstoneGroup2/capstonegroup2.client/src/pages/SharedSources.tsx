import React from 'react';
import SharedSourcesList from '../components/SharedSourcesList';
import { Source } from '../interfaces/Source';

const SharedSources: React.FC = ({}) => {

    const [sources, setSources] = React.useState<Source[]>([]);

    React.useEffect(() => {
        setSources([]);
    }, []);

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes - Shared Sources</h1>
            </div>
            {
                sources.length > 0 ? 
                    <SharedSourcesList sharedSources={sources} /> : 
                    <p>No shared sources available.</p>
            }
        </div>
    );
};

export default SharedSources;
