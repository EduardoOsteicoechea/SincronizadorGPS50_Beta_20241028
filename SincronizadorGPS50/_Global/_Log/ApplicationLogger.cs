using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public static class ApplicationLogger
   {
      public static System.Exception ReportError(string namespaceName, string className, string methodName, System.Exception exception, string exceptionShortDescription = "Failed to end the methods execution successfully.")
      {
         string exceptionLocation = $"At:\n\n{namespaceName}\n.{className}\n.{methodName}:\n\n{exception.Message}";
         try
         {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string directoryPath = Path.GetDirectoryName(assemblyLocation);
            string filePath = Path.Combine(directoryPath, "errorLog.txt");

            using(StreamWriter writer = new StreamWriter(filePath, true))
            {
               writer.WriteLine($"Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.\nLocation: {exceptionLocation}.\nShort Description: {exceptionShortDescription}.\nMessage: {exception.Message}\n\n");
               writer.WriteLine(new string('-', 50));
            };

            throw new Exception($"{exceptionLocation}");
         }
         catch(Exception errorLogException)
         {
            new VisualizationForm(exceptionShortDescription, $"Error {errorLogException.Message}");
            throw new Exception($"{exceptionLocation}");
            //throw new Exception($"{exceptionLocation ?? ""}");
         };
      }
   }
}
