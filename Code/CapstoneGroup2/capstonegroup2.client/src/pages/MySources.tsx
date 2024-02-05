import React from 'react';
import OwnSourcesList from '../components/OwnedSourcesList';
import { Source } from '../interfaces/Source';

const MySources: React.FC = ({}) => {

    const [sources, setSources] = React.useState<Source[]>([]);

    React.useEffect(() => {
        fetch('/source/' + localStorage.getItem('username'))
            .then((res) => res.json())
            .then((data) => {
                setSources(data);
            });
    }, []);

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes - My Sources</h1>
            </div>
            {
                sources.length > 0 ? 
                    <OwnSourcesList ownSources={sources} /> : 
                    <p>No sources available.</p>
            }
        </div>
    );
};

export default MySources;
