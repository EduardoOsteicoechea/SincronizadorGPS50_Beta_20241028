using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
    internal static class DataHolder
    {
        public static List<string> Sage50InstallationsList { get; set; } = new List<String>();
        public static string Sage50LocalTerminalPath { get; set; } = "";
        public static string Sage50Username { get; set; } = "";
        public static string Sage50Password { get; set; } = "";
        public static string Sage50CompanyNumber { get; set; } = "";
        public static bool ConnectedToSage50 { get; set; } = false;
        public static LinkSage50 Sage50ConnectionObjectInstance { get; set; } = null;
        public static DataTable Sage50CompanyGroupsDataTable { get; set; } = null;
        public static List<SincronizadorGPS50.Sage50Connector.CompanyGroup> Sage50CompanyGroupsList { get; set; } = new List<SincronizadorGPS50.Sage50Connector.CompanyGroup>();
        public static string Sage50SelectedCompanyGroupName { get; set; } = "";
        public static string Sage50SelectedCompanyGroupCode { get; set; } = "";
        public static string Sage50SelectedCompanyGroupMainCode { get; set; } = "";

        // ConnectToGestprojectDatabase
        // ConnectToGestprojectDatabase
        // ConnectToGestprojectDatabase
        // ConnectToGestprojectDatabase
        // ConnectToGestprojectDatabase

        public static string WindowsIdentityDomainName { get; set; } = "";
        public static string WindowsIdentityUserName { get; set; } = "";
        public static List<string> GestprojectVersionNames { get; set; } = new List<string>();
        public static string GestprojectVersionName { get; set; } = "";
        public static string GestprojectConnectionString { get; set; } = "";
        //public static SqlConnection GestprojectSQLConnection { get; set; } = null;
        public static List<int> GestprojectClientIdList { get; set; } = new List<int>();
        public static List<int> GestprojectProviderIdList { get; set; } = new List<int>();
        public static List<GestprojectClient> GestprojectClientClassList { get; set; } = new List<GestprojectClient>();
        internal static List<GestprojectClient> GestprojectSynchronizableClientClassList { get; set; } = new List<GestprojectClient>();
        public static DataTable GestprojectClientsTable { get; set; } = null;






        public static List<Sage50Client> Sage50ClientClassList { get; set; } = new List<Sage50Client>();
        public static List<string> Sage50ClientCodeList { get; set; } = new List<string>();
        public static List<string> Sage50ClientGUID_IDList { get; set; } = new List<string>();
        public static List<string> Sage50CIFList { get; set; } = new List<string>();
        public static DataTable ClientsSynchronizationTable { get; set; } = null;

        public static List<int> ListOfSelectedClientIdInTable { get; set; } = new List<int>();




    }
}
