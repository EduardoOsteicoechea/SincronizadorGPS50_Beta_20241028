using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml;

namespace SincronizadorGPS50
{
   public class GestprojectConnectionManager2 : IGestprojectConnectionManager
   {
      private static string BaseCommonApplicationGestprojectDataFolder { get; set; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Micad\Gestproject");

      public static string GestprojectXMLConfigurationFilePath { get; set; } = System.IO.Path.Combine(
         new GetLatestVersionFolderName(
               BaseCommonApplicationGestprojectDataFolder
         ).FolderName,
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
      public System.Data.SqlClient.SqlConnection GestprojectSqlConnection { get; set; }
      public SynchronizerUserRememberableDataModel GestprojectUserRememberableData { get; set; }
      public static string WindowsIdentityDomainName { get; set; } = null;
      public static string WindowsIdentityUserName { get; set; } = null;
      public static string MicrosoftSQLServerfolderPath { get; set; } = null;
      public static List<string> DatabaseInstancesNames { get; set; } = new List<string>();
      public static List<string> DatabaseVersionNames { get; set; } = new List<string>();
      public LocalDeviceUserData GestprojectLocalDeviceUserData { get; set; }
      public GestprojectConnectionManager2()
      {
         try
         {
            GetLocalDeviceData();
            GetConnectionData();
            GenerateConnectionString();
            ValidateDatabaseConnectionString();
            GestprojectSqlConnection = new SqlConnection(GestprojectConnectionString);
            GestprojectUserRememberableData = ManageRememberableUserData.GetSynchronizerUserRememberableDataForConnection(
               GestprojectDataHolder.GestprojectDatabaseConnection
            );
            GestprojectLocalDeviceUserData = new GestprojectLocalDeviceUserData().Get();

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

      public void GetLocalDeviceData()
      {
         try
         {
            string MicrosoftSQLServerfolderPath =
            Environment.GetEnvironmentVariable("ProgramW6432") + @"\" + "Microsoft SQL Server";

            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            string userName = identity.Name;
            string[] userNameParts = userName.Split('\\');

            if(userNameParts.Length == 1)
            {
               string WindowsIdentityUserName = Environment.UserName;
            }
            else
            {
               WindowsIdentityDomainName = userNameParts[0];
               WindowsIdentityUserName = userNameParts[1];
            };

            if(WindowsIdentityUserName == "")
            {
               MessageBox.Show("No logramos encontrar el nombre de usuario.\n\nContacte al proveedor para más información.");
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

      public void GetConnectionData()
      {
         try
         {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(GestprojectXMLConfigurationFilePath);

            string xmlContent = xmlDocument.OuterXml;

            Server = xmlDocument.SelectSingleNode("/configuration/conexion/Servidor").InnerText;
            DatabaseInstance = xmlDocument.SelectSingleNode("/configuration/conexion/Instancia").InnerText;
            DatabaseName = xmlDocument.SelectSingleNode("/configuration/conexion/NombreBD").InnerText;
            DatabaseUser = xmlDocument.SelectSingleNode("/configuration/conexion/Usuario").InnerText;
            RecordPasswordFromXML(xmlDocument.SelectSingleNode("/configuration/conexion/Password").InnerText);
            AskForServer = xmlDocument.SelectSingleNode("/configuration/conexion/AskServerAtStartup").InnerText;
            LastServer = xmlDocument.SelectSingleNode("/configuration/conexion/LastServer").InnerText;
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

      public void GenerateConnectionString()
      {
         try
         {
            string connectionString = "";
            connectionString += $@"Data Source={Server.Trim()}\{DatabaseInstance.Trim().ToUpper()};";
            connectionString += $"Initial Catalog={DatabaseName.Trim()};";
            connectionString += $"Persist Security Info=True;";
            connectionString += $"User ID={DatabaseUser.Trim()};";
            connectionString += $"Password={DatabasePassword.Trim()};";
            connectionString += $"Connection Timeout=5;";

            GestprojectConnectionString = connectionString;

            connectionString = null;
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

      public void ValidateDatabaseConnectionString()
      {
         try
         {
            using(SqlConnection testConnection = new SqlConnection(GestprojectConnectionString))
            {
               testConnection.Open();
               testConnection.Close();
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
