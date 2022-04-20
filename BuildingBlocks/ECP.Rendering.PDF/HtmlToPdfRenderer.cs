using HiQPdf;
using Microsoft.Extensions.Options;
using System.IO;

namespace ECP.Rendering.PDF
{
    public class HtmlToPdfRenderer : IHtmlToPdfRenderer
    {
        private readonly LicenceKeySettings _licencekeyOptions;
        public HtmlToPdfRenderer(IOptions<LicenceKeySettings> licencekeyOptions)
        {
            _licencekeyOptions = licencekeyOptions.Value;
        }
        public void ConvertHtmlToPdf(Stream outputStream, ExportParameters exportParameters)
        {
            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            // set LicenceKey
            htmlToPdfConverter.SerialNumber = _licencekeyOptions.LicenceKey;

            // set the default header of the document
            if (!string.IsNullOrEmpty(exportParameters.headerContents))
                SetHeader(htmlToPdfConverter.Document, exportParameters.headerContents);

            // set the footer of the document
            if (!string.IsNullOrEmpty(exportParameters.footerContents))
                SetFooter(htmlToPdfConverter.Document, exportParameters.footerContents);

            // convert the HTML to PDF
            htmlToPdfConverter.ConvertHtmlToStream
                (exportParameters.templateContents, null, outputStream);
        }

        private void SetHeader(PdfDocumentControl htmlToPdfDocument, string headerhtml)
        {
            // enable header display
            htmlToPdfDocument.Header.Enabled = true;

            // layout HTML in header
            PdfHtml headerHtml = new PdfHtml(headerhtml, null);
            headerHtml.FitDestHeight = true;
            htmlToPdfDocument.Header.Layout(headerHtml);
        }
        private void SetFooter(PdfDocumentControl htmlToPdfDocument, string footerhtml)
        {
            // enable footer display
            htmlToPdfDocument.Footer.Enabled = true;

            // layout HTML in footer
            PdfHtml footerHtml = new PdfHtml(footerhtml, null);
            footerHtml.FitDestHeight = true;
            htmlToPdfDocument.Footer.Layout(footerHtml);

            // add page numbers in HTML - more flexible but less efficient than text version

            PdfHtmlWithPlaceHolders htmlWithPageNumbers = new PdfHtmlWithPlaceHolders(5, 50 - 20,
            "Page <span style=\"font-size: 16px; color: blue; font-style: italic; font-weight: bold\">{CrtPage}</span> of <span style=\"font-size: 16px; color: green; font-weight: bold\">{PageCount}</span>", null);
            htmlToPdfDocument.Footer.Layout(htmlWithPageNumbers);
        }
    }
}
