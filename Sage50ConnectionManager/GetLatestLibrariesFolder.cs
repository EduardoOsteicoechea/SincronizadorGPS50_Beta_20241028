using System;
using System.IO;
using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   internal static class Sage50LibrariesFolder
   {
      internal static string Get(string terminalPath)
      {
         try
         {
            DirectoryInfo folderToBeAnalizedInformation = new DirectoryInfo(terminalPath);

            if(folderToBeAnalizedInformation.Exists)
            {
               System.Collections.SortedList folders = new System.Collections.SortedList();

               foreach(DirectoryInfo directoryInfo in folderToBeAnalizedInformation.GetDirectories())
               {
                  if(directoryInfo.Name.StartsWith("50."))
                  {
                     folders.Add(directoryInfo.Name, directoryInfo.Name);
                  };
               };

               if(folders.Count > 0)
               {
                  return Path.Combine(terminalPath, folders.GetKey(folders.Count - 1).ToString());
               }
               else
               {
                  throw new Exception("No existen carpetas de librerías cuyos nombres comienzen con \"50.\".");
               };
            }
            else
            {
               throw new Exception("La ruta del terminal especificada no existe.");
            };
         }
         catch(System.Exception ex)
         {
            MessageBox.Show(ex.Message);
            return null;
         };
      }
   }
}
