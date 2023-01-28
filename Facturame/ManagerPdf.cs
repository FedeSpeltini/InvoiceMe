using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static PdfSharpCore.Pdf.AcroForms.PdfAcroField;
using PdfSharpCore.Fonts;
using PdfSharpCore.Utils;
using System.Runtime.InteropServices;
using InvoiceMe;

namespace Facturame
{
    public class ManagerPdf
    {
        private readonly PdfDocument document;
        private readonly PdfPage page;
        private readonly XGraphics gfx;
        public decimal finalAmount;
        public ManagerPdf(PdfPage page, XGraphics gfx, PdfDocument document)
        {
            this.page = page;
            this.gfx = gfx;
            this.document = document;
        }

        public void HorizontalLine(int y)
        {
            XPen line = new XPen(XColors.DarkGreen, 3);
            gfx.DrawLine(line, 0, y, page.Width, y);
        }


        public void CreateColumnOfData(string[] data, int x, int y, int distance)
        {
            foreach(string input in data)
            {
                WriteBoldText(input, 15, x, y, new XSolidBrush(XColors.DarkBlue));
                x += distance;
            }
        }


        public int CreateColumnOfAmount(List<SubItem> subItems, int x, int y)
        {
            foreach(var subItem in subItems)
            {
                WriteText(subItem.Description, 12, x - 490, y, new XSolidBrush(XColors.Black));
                WriteText("$"+ subItem.Amount, 12, x, y, new XSolidBrush(XColors.Black));
                y += 20;
                finalAmount += subItem.Amount;
            }
            return y;
        }

        public void DigitalSign(string name, int y)
        {
            XPen line = new XPen(XColors.Black, 1);
            gfx.DrawLine(line, 390, y + 100, 500, y + 100);
            WriteText($"Aclaración: {name}", 9, 390, y + 120, new XSolidBrush(XColors.Black));
        }

        public void WriteText(string text, int size, int xAxis, int yAxis, XSolidBrush brush)
        {
            var font = new XFont("Arial", size);

            var textColor = brush;
            var layout = new XRect(xAxis, yAxis, page.Width, page.Height);
            var format = XStringFormats.TopLeft;

            gfx.DrawString(text, font, textColor, layout, format);
        }

        public void WriteBoldText(string text, int size, int xAxis, int yAxis, XSolidBrush brush)
        {
            var font = new XFont("Arial", size, XFontStyle.Bold);

            var textColor = brush;
            var layout = new XRect(xAxis, yAxis, page.Width, page.Height);
            var format = XStringFormats.TopLeft;

            gfx.DrawString(text, font, textColor, layout, format);
        }


        public void CreateHeader(string name, string company, string invoiceNumber)
        {
            WriteBoldText("FACTURA", 50, 10, 20, new XSolidBrush(XColors.Black));

            WriteText(name, 12, 10, 80, new XSolidBrush(XColors.Black));

            CreateColumnOfData(new string[] { "FACTURAR A", "N° DE FACTURA" }, 10, 200, 360);

            WriteText(invoiceNumber, 9, 500, 200, new XSolidBrush(XColors.Black));

            WriteText(company, 9, 10, 220, new XSolidBrush(XColors.Black));
            WriteText("FECHA", 12, 370, 220, new XSolidBrush(XColors.Black));
            WriteText("25/01/2023", 9, 500, 220, new XSolidBrush(XColors.Black));
        }
        public void CreatePDF(string name, string company, List<SubItem> amounts, string invoiceNumber, string path)
        {
            GlobalFontSettings.FontResolver = new FontResolver();
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            CreateHeader(name, company, invoiceNumber);

            HorizontalLine(250);
            CreateColumnOfData(new string[] { "DESCRIPCIÓN", "IMPORTE" }, 10, 260, 490);

            HorizontalLine(285);

            int yFinalValue = CreateColumnOfAmount(amounts, 500, 300);

            CreateColumnOfData(new string[] { "TOTAL", "$" + (finalAmount.ToString()) }, 370, yFinalValue + 30, 100);

            DigitalSign(name, yFinalValue + 30);


            document.Save(@$"{path}\invoice{invoiceNumber}.pdf");
        }
    }
}
