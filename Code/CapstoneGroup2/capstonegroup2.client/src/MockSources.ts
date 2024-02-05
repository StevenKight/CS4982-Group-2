import { Source, SourceType } from './interfaces/Source';

const mockOwnSources: Source[] = [
    {
        id: 1,
        name: 'Sample PDF 1',
        type: SourceType.Pdf,
        link: 'https://example.com/sample-pdf-1.pdf',
        createdDateTime: '2022-03-01T10:30:00',
        status: 'own',
        recentlyViewedDateTime: '2022-03-15T14:45:00',
    },
    {
        id: 2,
        name: 'Sample Video 1',
        type: SourceType.Vid,
        link: 'https://example.com/sample-video-1.mp4',
        createdDateTime: '2022-03-05T08:15:00',
        status: 'own',
        recentlyViewedDateTime: '2022-03-18T12:00:00',
    },
];

const mockSharedSources: Source[] = [
    {
        id: 3,
        name: 'Shared PDF 1',
        type: SourceType.Pdf,
        link: 'https://example.com/shared-pdf-1.pdf',
        createdDateTime: '2022-03-10T13:00:00',
        status: 'shared',
        recentlyViewedDateTime: '2022-03-20T09:30:00',
    },
    {
        id: 4,
        name: 'Shared Video 1',
        type: SourceType.Vid,
        link: 'https://example.com/shared-video-1.mp4',
        createdDateTime: '2022-03-12T16:45:00',
        status: 'shared',
        recentlyViewedDateTime: '2022-03-22T15:15:00',
    },
];

export { mockOwnSources, mockSharedSources };
