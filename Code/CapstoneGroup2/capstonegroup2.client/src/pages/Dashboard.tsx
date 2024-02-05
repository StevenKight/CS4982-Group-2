import React from 'react';
import OwnSourcesList from '../components/OwnedSourcesList';
import SharedSourcesList from '../components/SharedSourcesList';
import { Source } from '../interfaces/Source';

import '../styles/PostAuthorize.css';

function PostAuthorize() {
    const [ownSources, setOwnSources] = React.useState<Source[]>([]);
    const [sharedSources, setSharedSources] = React.useState<Source[]>([]);

    React.useEffect(() => {
        fetch('/source/' + localStorage.getItem('username'))
            .then((res) => res.json())
            .then((data) => {
                setOwnSources(data);
            });
        setSharedSources([]);
    }, []);

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes</h1>
            </div>
            {
                ownSources.length > 0 ? 
                    <OwnSourcesList ownSources={ownSources} /> : 
                    <p>No sources available.</p>
            }
            {
                sharedSources.length > 0 ? 
                    <SharedSourcesList sharedSources={sharedSources} /> : 
                    <p>No shared sources available.</p>
            }
        </div>
    );
}

export default PostAuthorize;