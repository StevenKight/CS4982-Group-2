import { Source } from "../interfaces/Source";

/**
 * PdfViewer component for rendering PDF content.
 * 
 * @param {Object} props - The component properties.
 * @param {Source} props.pdf - The source containing PDF information.
 * @returns {JSX.Element} The rendered PdfViewer component.
 */
function PdfViewer({ pdf }: { pdf: Source }) {

    const loadPdf = () => {
        let objectURL = '';
        if (!pdf.isLink && pdf.content) {
            const base64String = pdf.content;

            // Decode the Base64 string to a Uint8Array
            const binaryString = atob(base64String);
            const bytes = new Uint8Array(binaryString.length);
            for (let i = 0; i < binaryString.length; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            const blob = new Blob([bytes], { type: 'application/pdf' });
            objectURL = URL.createObjectURL(blob);
        }
        else if (pdf.isLink && pdf.link) {
            objectURL = pdf.link;
        }

        return (
            <iframe
                src={objectURL}
                height="100%"
                width="100%"
            />
        );
    }

    return (
        <div>
            {
                loadPdf()
            }
        </div>
    );
}
export default PdfViewer;