using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using prod_server.Entities;
using System.Reflection.Metadata;

namespace prod_server.Services
{
    public interface IDocumentsService
    {
        public Task<byte[]> CreateReceipt(Dictionary<string,string> FieldValue);
        public Task<byte[]> CreateQuote(Quote quote);
    }
    public class DocumentsService : IDocumentsService
    {
        public async Task<byte[]> CreateReceipt(Dictionary<string, string> FieldValue)
        {
            PdfDocument pdf = PdfReader.Open("receipt.pdf", PdfDocumentOpenMode.Modify);

            PdfAcroForm form = pdf.AcroForm;

            foreach (var item in FieldValue)
            {
                form.Fields[item.Key].Value = new PdfString(item.Value);
            }

            var generatedPdfByteArray = new byte[] { };
            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                generatedPdfByteArray = stream.ToArray();
            }

            return generatedPdfByteArray;
        }
      
        public async Task<byte[]> CreateQuote(Quote quote)
        {
            PdfDocument? pdf = PdfReader.Open("./Assets/Templates/ExampleDoc.pdf", PdfDocumentOpenMode.Modify);

            if (pdf == null) throw new Exception("Could not open PDF");

            PdfAcroForm form = pdf.AcroForm;



            // Convert the DateTime to a string representation compatible with the field format
            string dateValue = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");

            // Set the value of the form field
            form.Fields["date"].Value = new PdfString(dateValue);


            var generatedPdfByteArray = new byte[] { };
            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                generatedPdfByteArray = stream.ToArray();
            }

            return generatedPdfByteArray;




        }
    }
}
