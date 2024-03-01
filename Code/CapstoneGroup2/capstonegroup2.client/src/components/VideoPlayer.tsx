import ReactPlayer from "react-player";
import { Source } from "../interfaces/Source";

/**
 * VideoPlayer component for rendering video content.
 * 
 * @param {Object} props - The component properties.
 * @param {Source} props.video - The source containing video information.
 * @returns {JSX.Element} The rendered VideoPlayer component.
 */
function VideoPlayer({ video }: { video: Source }) {

    const loadVideo = () => {
        let sourceURL = '';

        if (!video.isLink && video.content) {
            // Assuming video.content contains the base64-encoded video data
            const base64String = video.content;

            // Decode the Base64 string to a Blob
            const blob = b64toBlob(base64String, 'video/mp4');
            sourceURL = URL.createObjectURL(blob);
        }
        else if (video.isLink && video.link) {
            sourceURL = video.link;
        }

        return (
            <ReactPlayer
                url={sourceURL}
                controls={true}
                width="100%"
                height="100%"
            />
        );
    }

    // Function to convert Base64 string to Blob
    const b64toBlob = (b64Data: string, contentType: string = '', sliceSize: number = 512) => {
        const byteCharacters = atob(b64Data);
        const byteArrays: Uint8Array[] = [];

        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);
            const byteNumbers = new Array(slice.length);

            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }

        return new Blob(byteArrays, { type: contentType });
    }

    return (
        <div>
            {
                loadVideo()
            }
        </div>
    );
}

export default VideoPlayer;