// See https://aka.ms/new-console-template for more information


using Facturame;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using System.Reflection.Metadata;


ManagerInput managerInput = new ManagerInput();
Console.WriteLine("Bienvenido a Facturame! \n");
Console.WriteLine("Para crear su factura necesitamos los siguientes datos:");


string name = managerInput.LoadStringData("Ingrese su nombre y apellido: ");

string company = managerInput.LoadStringData("Ingrese el nombre de la entidad a la cual le va a facturar: ");

List<string> amounts = new List<string>();

if(managerInput.LoadBoolData("¿Tiene varios montos a colocar en la misma factura? S/N: "))
{
    managerInput.LoadMultipleAmount(amounts);
}
else
{
    managerInput.LoadAmount(amounts);
}
string invoiceNumber = managerInput.LoadStringData("ingrese el número de factura: ");


string path = managerInput.LoadStringData("La ubicación donde desea guarda la factura: ");

GlobalFontSettings.FontResolver = new FontResolver();
var document = new PdfDocument();
var page = document.AddPage();
var gfx = XGraphics.FromPdfPage(page);
ManagerPdf manager = new ManagerPdf(page, gfx, document); 



manager.WriteBoldText("FACTURA", 50, 10, 20, new XSolidBrush(XColors.Black));

manager.WriteText(name, 12, 10, 80, new XSolidBrush(XColors.Black));

manager.CreateColumnOfData(new string[] { "FACTURAR A", "N° DE FACTURA"}, 10, 200, 360);


manager.WriteText(invoiceNumber, 9, 500, 200, new XSolidBrush(XColors.Black));


manager.WriteText(company, 9, 10, 220, new XSolidBrush(XColors.Black));
manager.WriteText("FECHA", 12, 370, 220, new XSolidBrush(XColors.Black));
manager.WriteText("25/01/2023", 9, 500, 220, new XSolidBrush(XColors.Black));

manager.HorizontalLine(250);
manager.CreateColumnOfData(new string[] { "DESCRIPCIÓN", "IMPORTE" }, 10, 260, 490);

manager.HorizontalLine(285);

int yFinalValue = manager.CreateColumnOfAmount(amounts, 500, 300);

manager.CreateColumnOfData(new string[] { "TOTAL", manager.finalAmount.ToString() }, 370, yFinalValue + 30, 100);

manager.DigitalSign(name, yFinalValue + 30);


document.Save(@$"{path}\invoice{invoiceNumber}.pdf");

Console.WriteLine("Su fatctura fue creada con éxito!");