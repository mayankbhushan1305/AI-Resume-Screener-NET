using UglyToad.PdfPig;

namespace ResumeScreeningAPI.Services;

public class ResumeService
{
    public string ExtractTextFromPdf(Stream pdfStream)
    {
        var text = "";
        using (var pdf = PdfDocument.Open(pdfStream))
        {
            foreach (var page in pdf.GetPages())
            {
                text += page.Text + " ";
            }
        }
        return text.Trim();
    }
}