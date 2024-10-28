using System;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class CustomerSyncronizationTableSchema
   {
      //public  string TableName { get; set; } = "INT_SAGE_SINC_CLIENTE";
      public  string TableName { get; set; } = "INT_SAGE_SINC_CLIENTES";

      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) SynchronizationTableClientIdColumn { get; set; } = ("ID", "Id de Sincronización", typeof(int));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) SynchronizationStatusColumn { get; set; } = ("SYNC_STATUS", "Estado", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientIdColumn { get; set; } = ("PAR_ID", "Id de Cliente en Gestproject", typeof(int));





      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientAccountableSubaccountColumn { get; set; } = ("PAR_SUBCTA_CONTABLE", "Subcuenta contable", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientNameColumn { get; set; } = ("NOMBRE_COMPLETO", "Nombre", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientCommercialNameColumn { get; set; } = ("PAR_NOMBRE_COMERCIAL", "Nombre comercial", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientCIFNIFColumn { get; set; } = ("PAR_CIF_NIF", "CIF - NIF", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientAddressColumn { get; set; } = ("PAR_DIRECCION_1", "Dirección", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientPostalCodeColumn { get; set; } = ("PAR_CP_1", "Código postal", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientLocalityColumn { get; set; } = ("PAR_LOCALIDAD_1", "Localidad", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientProvinceColumn { get; set; } = ("PAR_PROVINCIA_1", "Provincia", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientCountryColumn { get; set; } = ("PAR_PAIS_1", "País", typeof(string));



      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientCodeColumn { get; set; } = ("S50_CLIENT_CODE", "Código de cliente", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientGuidIdColumn { get; set; } = ("S50_CLIENT_GUID_ID", "Guid de Cliente en Sage50", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientCompanyGroupNameColumn { get; set; } = ("S50_CLIENT_COMPANY_GROUP_NAME", "Nombre de Grupo de Empresas en Sage50", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientCompanyGroupCodeColumn { get; set; } = ("S50_CLIENT_COMPANY_GROUP_CODE", "Código de Grupo de Empresas en Sage50", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientCompanyGroupMainCodeColumn { get; set; } = ("S50_CLIENT_COMPANY_GROUP_MAIN_CODE", "Código principal de Grupo de Empresas en Sage50", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ClientCompanyGroupGuidIdColumn { get; set; } = ("S50_CLIENT_COMPANY_GROUP_GUID_ID", "Guid de Grupo de Empresas en Sage50", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) CommentsColumn { get; set; } = ("COMMENTS", "Comentarios", typeof(string));
      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) ClientLastUpdateTerminalColumn { get; set; } = ("LAST_UPDATE", "Última actualización", typeof(DateTime));



      public  (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectClientParentUserIdColumn { get; set; } = ("GP_USU_ID", "Id de Gestor en Gestproject", typeof(int));
   }
}
