import React from 'react';
import OwnSourcesList from '../components/OwnedSourcesList';
import { Source } from '../interfaces/Source';

interface MySourcesProps {
    ownSources: Source[];
}

const MySources: React.FC<MySourcesProps> = ({ ownSources }) => {
    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes - My Sources</h1>
            </div>
            <OwnSourcesList ownSources={ownSources} />
        </div>
    );
};

export default MySources;
