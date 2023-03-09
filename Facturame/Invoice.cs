using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMe
{
    public class Invoice
    {
        public string Name { get; set; }

        public string Company { get; set; }

        

        private string finalAmount;

        public string FinalAmount
        {
            get { return finalAmount; }
            set { finalAmount = GetFinalAmount(); }
        }


        public int InvoiceNumber { get; set; }

        public string Date { get; set; }

        public List<SubItem> SubItems { get; set; }

        public string GetFinalAmount()
        {
            decimal finalAmount = 0;
            foreach (SubItem item in SubItems)
            {
                 finalAmount += item.Amount;

            }
            return "$" + finalAmount.ToString();
        }
    }
}
