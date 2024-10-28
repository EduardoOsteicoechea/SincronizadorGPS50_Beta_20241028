using System.IO;

namespace SincronizadorGPS50.GestprojectConnector
{
   internal class LatestVersionFolderNameManager
   {
      public string Get(string folderToBeAnalizedPath, string defaultVersionValue = "12.0.0.0")
      {
         DirectoryInfo folderToBeAnalizedInformation = new DirectoryInfo( folderToBeAnalizedPath );
         if(folderToBeAnalizedInformation.Exists)
         {
            System.Collections.SortedList folders = new System.Collections.SortedList();
            foreach(DirectoryInfo directoryInfo in folderToBeAnalizedInformation.GetDirectories())
            {
               folders.Add(directoryInfo.Name, directoryInfo.Name);
            };

            if(folders.Count > 0)
            {
               return Path.Combine(folderToBeAnalizedPath, folders.GetKey(folders.Count - 1).ToString());
            }
            else
            {
               return Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
            };
         }
         else
         {
            return Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
         };
      }
   }
}
