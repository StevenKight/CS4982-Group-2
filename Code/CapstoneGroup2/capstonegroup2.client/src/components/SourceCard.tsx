import { useNavigate } from 'react-router-dom';
import { Source } from '../interfaces/Source';

import './styles/SourceCard.css';

/**
 * React component for displaying a source card.
 *
 * @param {Object} props - The properties passed to the component.
 * @param {Source} props.source - The source data to display.
 * @param {boolean} [props.showUser] - Flag to determine whether to show the username.
 * @param {boolean} [props.showDate] - Flag to determine whether to show the source date.
 *
 * @returns {JSX.Element} - JSX representation of the SourceCard component.
 */
export default function SourceCard({ source, showUser, showDate }: 
    { source: Source, showUser?: boolean, showDate?: boolean}) {

    const navigate = useNavigate();

    /**
     * Handles the click event on the source card.
     * Navigates to the detailed view of the source.
     */
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
            <div style={{
                    display: 'flex', 
                    alignItems: 'center', 
                    justifyContent: 'space-between',
                    marginRight: '1em'
                }}>
                <h4>{source.name}</h4>
                <small>{source.type}</small>
            </div>
            <p>{source.description}</p>
            {/*{(showDate && !showUser && source.updatedAt) && <p>Updated on: {source.updatedAt.toDateString()}</p>}*/}
            {/* FIXME: Instead of created at for shared, make it shared date */}
            {(showDate && showUser && source.createdAt) && <p>Shared on: {source.createdAt.toDateString()}</p>} 
            {showUser && <p>Shared by: {source.username}</p>}
        </div>
    );
}