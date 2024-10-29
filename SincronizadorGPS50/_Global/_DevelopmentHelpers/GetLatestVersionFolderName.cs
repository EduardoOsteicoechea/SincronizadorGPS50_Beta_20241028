using System.IO;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class GetLatestVersionFolderName
   {
      public string FolderName { get; set; } = "";
      public GetLatestVersionFolderName(string folderToBeAnalizedPath, string defaultVersionValue = "12.0.0.0")
      {
         try
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
                  FolderName = Path.Combine(folderToBeAnalizedPath, folders.GetKey(folders.Count - 1).ToString());
               }
               else
               {
                  FolderName = Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
               };
            }
            else
            {
               FolderName = Path.Combine(folderToBeAnalizedPath, defaultVersionValue);
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }
   }
}
