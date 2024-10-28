using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
    internal static class GestprojectDatabase
    {
        public static string WindowsIdentityDomainName { get; set; } = null;
        public static string WindowsIdentityUserName { get; set; } = null;
        public static string MicrosoftSQLServerfolderPath { get; set; } =
        Environment.GetEnvironmentVariable("ProgramW6432") + @"\" + "Microsoft SQL Server";
        public static List<string> DatabaseInstancesNames { get; set; } = new List<string>();
        public static List<string> DatabaseVersionNames { get; set; } = new List<string>();
        public static System.Data.SqlClient.SqlConnection Connect()
        {
            // GetUserDeviceData
            // GetUserDeviceData
            // GetUserDeviceData
            // GetUserDeviceData

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string userName = identity.Name;
            string[] userNameParts = userName.Split('\\');

            if(userNameParts.Length == 1)
            {
                WindowsIdentityUserName = Environment.UserName;
            }
            else
            {
                WindowsIdentityDomainName = userNameParts[0];
                WindowsIdentityUserName = userNameParts[1];
            };

            if(WindowsIdentityDomainName != null && WindowsIdentityUserName != null)
            {
                DataHolder.WindowsIdentityDomainName = WindowsIdentityDomainName;
                DataHolder.WindowsIdentityUserName = WindowsIdentityUserName;
            }
            else if(WindowsIdentityUserName != null && WindowsIdentityDomainName == "")
            {
                DataHolder.WindowsIdentityUserName = WindowsIdentityUserName;
            }
            else if(WindowsIdentityUserName == "")
            {
                MessageBox.Show("No logramos encontrar el nombre de usuario.\n\nContacte al proveedor para más información.");
            };

            // GetGestprojectSQLServerInstances
            // GetGestprojectSQLServerInstances
            // GetGestprojectSQLServerInstances
            // GetGestprojectSQLServerInstances
            // GetGestprojectSQLServerInstances

            if(Directory.Exists(MicrosoftSQLServerfolderPath))
            {
                string[] folderNames = Directory.GetDirectories(MicrosoftSQLServerfolderPath);
                string aa = "";

                for(global::System.Int32 i = 0; i < folderNames.Length; i++)
                {
                    string folderName = folderNames[i];

                    if(folderName.Contains("GESTPROJECT"))
                    {
                        string gestprojectDatabaseInstanceName = folderName.Split('\\').Last();
                        DatabaseInstancesNames.Add(gestprojectDatabaseInstanceName);

                        string gestprojectDatabaseInstanceProgramVersionName = gestprojectDatabaseInstanceName.Split('.').FirstOrDefault(part => part.Contains("GESTPROJECT"));

                        DatabaseVersionNames.Add(gestprojectDatabaseInstanceProgramVersionName);
                    };
                }

                if(DatabaseVersionNames.Count > 0)
                {
                    DataHolder.GestprojectVersionNames = DatabaseVersionNames;
                    DataHolder.GestprojectVersionName = DatabaseVersionNames.Last();
                }
                else
                {
                    MessageBox.Show("No hay servidores de Gestproject en la carpeta de Microsoft SQL Server.\n\nVerifica si Gestproject está instalado en tu dispositivo.");
                };

                for(global::System.Int32 i = 0; i < DatabaseVersionNames.Count; i++)
                {
                    aa += DatabaseVersionNames[i] + "\n";
                }
            }
            else
            {
                MessageBox.Show("La carpeta de Microsoft SQL Server no existe en tu dispositivo.\n\nVerifica si Microsoft SQL Server está instalado.");
            }

            // CreateGestprojectConnectionString
            // CreateGestprojectConnectionString
            // CreateGestprojectConnectionString
            // CreateGestprojectConnectionString
            // CreateGestprojectConnectionString


            string serverName = "";
            if(DataHolder.WindowsIdentityDomainName != null)
            {
                serverName += DataHolder.WindowsIdentityDomainName + "\\" + DataHolder.GestprojectVersionName;
            }
            else
            {
                serverName += DataHolder.WindowsIdentityUserName + "\\" + DataHolder.GestprojectVersionName;
            };

            string connectionString = "";
            connectionString += $"Server={serverName};";
            connectionString += $"Database={DataHolder.GestprojectVersionName};";
            connectionString += $"Trusted_Connection=true;";

            DataHolder.GestprojectConnectionString = connectionString;

            // ConnectToGestprojectDatabase and Return SqlConnectionObject
            // ConnectToGestprojectDatabase and Return SqlConnectionObject
            // ConnectToGestprojectDatabase and Return SqlConnectionObject
            // ConnectToGestprojectDatabase and Return SqlConnectionObject

            return new System.Data.SqlClient.SqlConnection(DataHolder.GestprojectConnectionString);
        }
    }
}
