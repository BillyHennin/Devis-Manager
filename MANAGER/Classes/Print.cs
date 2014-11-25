using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace MANAGER.Classes
{
    class Print
    {
        private Estimate estimate;

        public Print(List<Merchandise> merchandises)
        {
            estimate = new Estimate(merchandises);

            var nbMarchandise = estimate.GetList.Count;
            Console.WriteLine("iText Demo");
            for (var i = 0; i < nbMarchandise; i++)
            {

            }
            

        }
    }
}
