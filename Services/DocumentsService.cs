using prod_server.Entities;
using System.Reflection.Metadata;
using iText.Kernel.Pdf;
using iText.Forms;

namespace prod_server.Services
{
    public interface IDocumentsService
    {
        public Task<byte[]> CreateReceipt(Dictionary<string,string> FieldValue);
        public Task<byte[]> CreateQuote(Quote quote);
    }
    public class DocumentsService : IDocumentsService
    {
 
      
        public async Task<byte[]> CreateQuote(Quote quote)
        {

            using (var stream = new MemoryStream())
            {
                var pdf = new PdfDocument(new PdfReader("./Assets/Templates/ExampleDoc.pdf"), new PdfWriter(stream));

                // Access the form fields
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
                form.GetField("date").SetValue(DateTime.UtcNow.ToString("dd/MM/YYYY"));
                // lock pdf; Not editable.

                form.FlattenFields();
                pdf.Close();

                return stream.ToArray();
            }



        }

        public Task<byte[]> CreateReceipt(Dictionary<string, string> FieldValue)
        {
            throw new NotImplementedException();
        }
    }
}
