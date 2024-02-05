import OwnSourcesList from '../components/OwnedSourcesList';
import SharedSourcesList from '../components/SharedSourcesList';
import { mockOwnSources, mockSharedSources } from '../MockSources.ts';
import '../styles/PostAuthorize.css'
function PostAuthorize() {
    const ownSources = mockOwnSources;
    const sharedSources = mockSharedSources;

    return (
        <div className='page-content'>
            <div style={{ display: 'flex' }}>
                <h1>Docunotes</h1>
            </div>
            <OwnSourcesList ownSources={ownSources} />
            <SharedSourcesList sharedSources={sharedSources} />
        </div>
    );
}

export default PostAuthorize;