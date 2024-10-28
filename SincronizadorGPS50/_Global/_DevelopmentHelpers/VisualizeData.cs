using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class VisualizeData<T>
   {

      // SingleItemPrinter
      public VisualizeData(string location, string listTitle, T item) 
      {
         string sb = "";
         sb += location + "\n\n";
         sb += listTitle + "\n\n";
         sb += item.ToString();
         new VisualizationForm(listTitle, sb);
      }

      // ListPrinter
      public VisualizeData(string location, string listTitle, IEnumerable<T> list, PropertyInfo propertyInfo = null) 
      {
         string sb = "";
         int counter = 1;
         sb += location + "\n\n";
         sb += listTitle + "\n\n";
         foreach (T item in list)
         {
            string printableCounterValue = "";

            if(counter.ToString().Length < 2)
            {
               printableCounterValue = "0" + counter;
            }
            else
            {
               printableCounterValue = counter.ToString();
            };
            
            sb += printableCounterValue + " --- ";

            if (propertyInfo == null)
            {
               sb += item.ToString() + "\n";
            }
            else
            {
               sb += propertyInfo.GetValue(item) + "\n";
            };

            counter++;
         };
         new VisualizationForm(listTitle, sb);
      }
   }
}
