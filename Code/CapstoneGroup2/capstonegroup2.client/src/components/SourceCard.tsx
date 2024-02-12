
import { useNavigate } from 'react-router-dom';
import { Source } from '../interfaces/Source';

import './styles/SourceCard.css';

export default function SourceCard({ source, showUser, showDate }: 
    { source: Source, showUser?: boolean, showDate?: boolean}) {

    const navigate = useNavigate();

    const onSourceClick = () => {
        if (!showUser) {
            navigate(`/my-source/${source.sourceId}`);
        }
        else {
            console.log('SourceCard: Cannot navigate to source notes for shared source yet.');
        }
    }
        
    return (
        <div className='source' onClick={onSourceClick}>
            <h4>{source.name}</h4>
            <p>{source.description}</p>
            <p>{source.link}</p>
            {(showDate && !showUser && source.updatedAt) && <p>Updated on: {source.updatedAt.toDateString()}</p>}
            {/* FIXME: Instead of created at for shared, make it shared date */}
            {(showDate && showUser && source.createdAt) && <p>Shared on: {source.createdAt.toDateString()}</p>} 
            {showUser && <p>Shared by: {source.username}</p>}
        </div>
    );
}