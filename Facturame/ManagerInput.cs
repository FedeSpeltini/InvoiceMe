using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Facturame
{
    internal class ManagerInput
    {
        public bool YesNoValidator(string input)
        {
            if(input == "S")
            {
                return true;
            }
            else if (input == "N")
            {
                return false;
            }
            else
            {
                return LoadBoolData("Disculpe, no comprendo. Responda con S si tiene más de un monto a cargar o N si es solo uno");
            }
        }

        public List<string> LoadAmount(List<string> amounts)
        {

            amounts.Add(LoadStringData("ingrese el monto a facturar: "));
            return amounts;
        }

        public List<string> LoadMultipleAmount(List<string> amounts)
        {
            bool next = true;
            while (next)
            {
                amounts.Add(LoadStringData("ingrese el monto a facturar: "));

                next = LoadBoolData("Desea cargar otro monto? S/N: ");
            }
            return amounts;
        }

        public string LoadStringData(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }


        public bool LoadBoolData(string text)
        {
            Console.WriteLine(text);
            string response = Console.ReadLine();

            return YesNoValidator(response);
        }
    }


}
