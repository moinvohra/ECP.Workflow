using System.IO;

namespace ECP.Rendering.PDF
{
    public interface IHtmlToPdfRenderer
    {
        void ConvertHtmlToPdf(Stream outputStream, ExportParameters exportParameters);
    }
}
