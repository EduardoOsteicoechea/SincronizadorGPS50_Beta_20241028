using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class VisualizePropertiesAndValues<T>
   {
      public VisualizePropertiesAndValues(string location, string title, T entity)
      {
         string content = "";
         content += location;
         content += "\n\n";
         content += "-----------------------------";
         content += "\n\n";

         foreach(var item in entity.GetType().GetProperties())
         {
            content += item.Name + ": " + item.GetValue(entity) + "\n";
         };

         content += "\n";
         content += "-----------------------------";
         content += "\n";

         new VisualizationForm(title, content);
      }
      public VisualizePropertiesAndValues(string location, string title, List<T> entityList)
      {
         int counter = 1;

         string content = "";
         content += location;
         content += "\n\n";
         content += "-----------------------------";
         content += "\n\n";
         content += "Total Count: " + entityList.Count;
         content += "\n\n";
         content += "-----------------------------";
         content += "\n";

         foreach(T entity in entityList)
         {
            content += counter + "." + "\n\n";
            foreach(var item in entity.GetType().GetProperties())
            {
               content += item.Name + ": " + item.GetValue(entity) + "\n";
            };
            content += "\n";
            content += "-----------------------------";
            content += "\n";

            counter++;
         };

         new VisualizationForm(title, content);
      }

      public VisualizePropertiesAndValues(string location, string title, List<int> entityList)
      {
         int counter = 1;

         string content = "";
         content += location;
         content += "\n\n";
         content += "-----------------------------";
         content += "\n\n";
         content += "Total Count: " + entityList.Count;
         content += "\n\n";
         content += "-----------------------------";
         content += "\n";

         foreach(int entity in entityList)
         {
            content += counter + "." + "\n\n";
            content += "value: " + entity + "\n";
            content += "\n";
            content += "-----------------------------";
            content += "\n";

            counter++;
         };

         new VisualizationForm(title, content);
      }
   }
}
