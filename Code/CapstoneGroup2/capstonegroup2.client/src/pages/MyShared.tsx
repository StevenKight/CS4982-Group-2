import SourcesGrid from "../components/SourcesGrid";

import { Source, SourceType } from "../interfaces/Source";

const dummySources: Source[] = [
    { sourceId: 1, name: "Source 1", description: "Description 1", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 2, name: "Source 2", description: "Description 2", isLink: false, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 3, name: "Source 3", description: "Description 3", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 4, name: "Source 4", description: "Description 4", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 5, name: "Source 5", description: "Description 5", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 6, name: "Source 6", description: "Description 6", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 7, name: "Source 7", description: "Description 7", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
    { sourceId: 8, name: "Source 8", description: "Description 8", isLink: true, link: "http://www.example.com", username: "StevenC", noteType: SourceType.Pdf, createdAt: new Date(), updatedAt: new Date()},
];

export default function MyShared() {
    return (
        <div>
            <h1>Dummy Shared with me</h1>
            <SourcesGrid sources={dummySources} showUser/>
        </div>
    );
}