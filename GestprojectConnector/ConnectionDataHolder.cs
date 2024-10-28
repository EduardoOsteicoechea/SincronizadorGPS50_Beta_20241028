using System;
using System.IO;

namespace SincronizadorGPS50.GestprojectConnector
{

    internal static class ConnectionDataHolder
    {
        public static string WindowsIdentityDomainName { get; set; } = null;
        public static string WindowsIdentityUserName { get; set; } = null;



        private static string BaseCommonApplicationGestprojectDataFolder { get; set; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Micad\Gestproject");

        public static string GestprojectXMLConfigurationFilePath { get; set; } = System.IO.Path.Combine(
            GetLatestVersionFolderName(
                BaseCommonApplicationGestprojectDataFolder
            ),
            "Gestproject.config.xml"
        );


        internal static string Server { get; set; } = "";
        internal static string DatabaseInstance { get; set; } = "";
        internal static string DatabaseName { get; set; } = "";
        internal static string DatabaseUser { get; set; } = "";
        private static string _DatabasePassword { get; set; } = "";
        internal static string DatabasePassword
        {
            get { return _DatabasePassword; }
        }
        internal static string AskForServer { get; set; } = "";
        internal static string LastServer { get; set; } = "";

        internal static void RecordPasswordFromXML(string encryptedPassword)
        {
            _DatabasePassword = Encryptor.UnEncrypt(encryptedPassword);
        }



        public static string GestprojectConnectionString { get; set; } = null;




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




        internal static void DisposeSensitiveData() 
        {
            WindowsIdentityDomainName= "";
            WindowsIdentityUserName  = "";
            BaseCommonApplicationGestprojectDataFolder  = "";
            Server = "";
            DatabaseInstance = "";
            DatabaseName = "";
            DatabaseUser = "";
            _DatabasePassword  = "";
            AskForServer  = "";
            LastServer = "";
            GestprojectConnectionString = "";
        }
    }
}
