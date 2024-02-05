import React from 'react';
import { Source, SourceType } from '../interfaces/Source.ts';
import '../styles/PostAuthorize.css';

interface OwnSourcesListProps {
    ownSources: Source[];
}

const OwnSourcesList: React.FC<OwnSourcesListProps> = ({ ownSources }) => {
    const sortedOwnSources = [...ownSources].sort((a, b) => {
        return new Date(b.recentlyViewedDateTime).getTime() - new Date(a.recentlyViewedDateTime).getTime();
    });

    return (
        <div className="sources-section">
            <h2>Your Own Sources</h2>
            <ul className="own-sources-list">
                {sortedOwnSources.map((source) => (
                    <li key={source.id} className="sources-list-item">
                        <strong>{source.name}</strong> - {getSourceTypeName(source.type)}
                        <p>Link: {source.link}</p>
                        <p>Created: {source.createdDateTime}</p>
                        <p>Status: {source.status}</p>
                        <p>Recently Viewed: {source.recentlyViewedDateTime}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
};

const getSourceTypeName = (type: SourceType): string => {
    return type === SourceType.Pdf ? 'Pdf' : 'Vid';
};

export default OwnSourcesList;