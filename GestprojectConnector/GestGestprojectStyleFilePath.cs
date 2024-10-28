using Dinq.Gestproject;
using System;
using System.IO;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectConnector
{
    public class GestGestprojectStyleFilePath
    {
        private static string BaseLocalApplicationGestprojectDataFolder { get; set; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Micad\Gestproject");
        public string GestprojectDATUserSettingsFilePath { get; set; } = System.IO.Path.Combine(
            GetLatestVersionFolderName(
                BaseLocalApplicationGestprojectDataFolder
            ),
            "_USERSETTINGS.DAT"
        );
        public static string GestprojectStylesFolderPath { get; set; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Micad\Gestproject 2020\Styles");

        private static string GetLatestVersionFolderName(string folderToBeAnalizedPath, string defaultVersionValue = "12.0.0.0")
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

        public string FilePath() 
        {
            try
            {
                string styleFileName = "";

                if(System.IO.File.Exists(GestprojectDATUserSettingsFilePath))
                {
                     var userSettings = Serializer.DeserializeObject(GestprojectDATUserSettingsFilePath);
                     return "";
               //using(StreamReader reader = File.OpenText(GestprojectDATUserSettingsFilePath))
               //{
               //    string fileContent = reader.ReadToEnd();
               //    int indexOfStyleFileExtension = fileContent.IndexOf(".isl");
               //    string fileSubstringToBeAnalized = fileContent.Substring(indexOfStyleFileExtension - 40, 45);

               //    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(
               //        fileContent,
               //        @"(\w+\s*\-*\s*\w*\s*\-*\s*\w*\s*\-*\s*\w*\s*\-*\s*\w*)\.isl"
               //    );

               //    if(match.Success)
               //    {
               //        styleFileName = match.Groups[1].Value + ".isl";

               //        IsSuccessfull = true;

               //        string gestprojectStylesFolderPath = GestprojectStylesFolderPath;

               //        DisposeSensitiveData();

               //        return System.IO.Path.Combine(
               //            gestprojectStylesFolderPath,
               //            styleFileName
               //        );
               //    }
               //    else
               //    {
               //        throw new Exception("Error en la lectura del estilo actual de Gestproject");
               //    };
               //};
            }
                else
                {
                    throw new Exception("Error en la localización del registro de configuraciones de usuario en Gestproject");
                };
            }
            catch(System.Exception e)
            {
                MessageBox.Show($"Error: \n\n{e.ToString()}. \n\nProcederemos a asignar los estilos predeterminados.");

                //IsSuccessfull = true;

                string gestprojectStylesFolderPath = GestprojectStylesFolderPath;
                
                DisposeSensitiveData();

                return System.IO.Path.Combine(
                    gestprojectStylesFolderPath,
                    "default.isl"
                );
            };
        }

        private void DisposeSensitiveData() 
        {
            BaseLocalApplicationGestprojectDataFolder = "";
            GestprojectDATUserSettingsFilePath = "";
            GestprojectStylesFolderPath = "";
        }
    }
}
