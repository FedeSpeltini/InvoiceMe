// See https://aka.ms/new-console-template for more information


using Facturame;
using InvoiceMe;
using Newtonsoft.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using System;
using System.Reflection.Metadata;

Console.WriteLine("Bienvenido a Facturame! \n");

string jsonString = File.ReadAllText("E:\\Fede\\Programacion\\Proyectos\\NetCore\\Facturame\\Facturame\\data.json");


Invoice invoice = JsonConvert.DeserializeObject<Invoice>(jsonString);

invoice.FinalAmount = invoice.GetFinalAmount();


string path = "E:\\Fede\\Facturass";


GlobalFontSettings.FontResolver = new FontResolver();
var document = new PdfDocument();
var page = document.AddPage();
var gfx = XGraphics.FromPdfPage(page);
ManagerPdf manager = new ManagerPdf(page, gfx, document); 



manager.WriteBoldText("FACTURA", 50, 10, 20, new XSolidBrush(XColors.Black));

manager.WriteText(invoice.Name, 12, 10, 80, new XSolidBrush(XColors.Black));

manager.CreateColumnOfData(new string[] { "FACTURAR A", "N° DE FACTURA"}, 10, 200, 360);


manager.WriteText(invoice.Number.ToString(), 9, 500, 200, new XSolidBrush(XColors.Black));


manager.WriteText(invoice.Company, 9, 10, 220, new XSolidBrush(XColors.Black));
manager.WriteText("FECHA", 12, 370, 220, new XSolidBrush(XColors.Black));
manager.WriteText("25/01/2023", 9, 500, 220, new XSolidBrush(XColors.Black));

manager.HorizontalLine(250);
manager.CreateColumnOfData(new string[] { "DESCRIPCIÓN", "IMPORTE" }, 10, 260, 490);

manager.HorizontalLine(285);

int yFinalValue = manager.CreateColumnOfAmount(invoice.SubItems, 500, 300);

manager.CreateColumnOfData(new string[] { "TOTAL", invoice.FinalAmount }, 370, yFinalValue + 30, 100);

manager.DigitalSign(invoice.Name, yFinalValue + 30);


document.Save(@$"{path}\invoice{invoice.Number}.pdf");

Console.WriteLine("Su fatctura fue creada con éxito!");