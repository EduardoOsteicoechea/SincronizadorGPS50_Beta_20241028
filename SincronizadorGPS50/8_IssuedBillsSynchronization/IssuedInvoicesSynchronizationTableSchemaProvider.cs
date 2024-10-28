using System;
using System.Collections.Generic;
using System.Linq;

namespace SincronizadorGPS50
{
   public class IssuedInvoicesSynchronizationTableSchemaProvider : ISynchronizationTableSchemaProvider
   {      
      # region entityStaticDataProviders
      public static string EntitySynchronizationTableName { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA";
      public static List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> MainSynchronizationTuples { get; set; } = new List<(string, string, Type, string, dynamic)>()
      {
         /*0*/("ID", "Id de Sincronización", typeof(int), "INT PRIMARY KEY IDENTITY(1,1)", null),
         /*1*/("SYNC_STATUS", "Estado", typeof(string), "VARCHAR(MAX)", SynchronizationStatusOptions.Desincronizado),
      };
      public static List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> GestprojectDataTuples { get; set; } = new List<(string, string, Type, string, dynamic)>()
      {
         /*0*/("FCE_ID", "Id en Gestproject", typeof(int), "INT", null),
         /*1*/("PAR_DAO_ID", "Id de empresa", typeof(int), "INT", null),
         /*2*/("FCE_REFERENCIA", "Referencia de factura", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*3*/("FCE_FECHA", "Fecha de factura", typeof(DateTime), "DATETIME", null),
         /*4*/("PAR_CLI_ID", "Id de cliente", typeof(int), "INT", null),
         /*5*/("FCE_BASE_IMPONIBLE", "Base imponible", typeof(decimal), "DECIMAL(18,2)", null),
         /*6*/("FCE_VALOR_IVA", "IVA valor", typeof(decimal), "DECIMAL(18,2)", null),
         /*7*/("FCE_IVA", "IVA tipo", typeof(decimal), "DECIMAL(18,2)", null),
         /*8*/("FCE_VALOR_IRPF", "Retención valor", typeof(decimal), "DECIMAL(18,2)", null),
         /*9*/("FCE_IRPF", "Retención tipo", typeof(decimal), "DECIMAL(18,2)", null),
         /*10*/("FCE_TOTAL_SUPLIDO", "Total suplido", typeof(decimal), "DECIMAL(18,2)", null),
         /*11*/("FCE_TOTAL_FACTURA", "Total factura", typeof(decimal), "DECIMAL(18,2)", null),
         /*12*/("FCE_OBSERVACIONES", "Observaciones", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*13*/("FCE_IVA_IGIC", "Nombre de Iva en Gestproject", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*14*/("PAR_SUBCTA_CONTABLE", "Cuenta Contable de cliente", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*15*/("SageCompanyNumber", "Código de empresa en Sage", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*16*/("TaxCode", "Codigo de IVA en Sage", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*17*/("FCE_SUBCTA_CONTABLE", "Cuenta contable", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*18*/("SageProjectCode", "Código de proyecto en Sage", typeof(string), "VARCHAR(MAX)", string.Empty),
      };
      public static List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> SageDataTuples { get; set; } = new List<(string, string, Type, string, dynamic)>()
      {
         /*0*/("S50_CODE", "Código de Proveedor", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*0*/("S50_GUID_ID", $"Guid en Sage50", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*1*/("S50_COMPANY_GROUP_NAME", "Nombre de Grupo de Empresas en Sage50", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*2*/("S50_COMPANY_GROUP_CODE", "Código de Grupo de Empresas en Sage50", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*3*/("S50_COMPANY_GROUP_MAIN_CODE", "Código principal de Grupo de Empresas en Sage50", typeof(string), "VARCHAR(MAX)", string.Empty),
         /*4*/("S50_COMPANY_GROUP_GUID_ID", "Guid de Grupo de Empresas en Sage50", typeof(string), "VARCHAR(MAX)", string.Empty),
      };
      public static List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> MiscelaneousSynchronizationDataTuples { get; set; } = new List<(string, string, Type, string, dynamic)>()
      {
         /*0*/("LAST_UPDATE", "Última actualización", typeof(DateTime),"DATETIME DEFAULT GETDATE() NOT NULL", DateTime.Now),
         /*1*/("GP_USU_ID", "Id de Gestor en Gestproject", typeof(int), "INT", null),
         /*2*/("COMMENTS", "Comentarios", typeof(string), "VARCHAR(MAX)", string.Empty),
      };
      static ((string sageDispactcherMechanismRoute, string tableName) dispatcherAndName, List<(string name, Type type)> tableFieldsAlongTypes) SageTableDataProvider { get; set;} = 
      (
         ("gestion","c_factucom"),
         new List<(string name, Type type)>() 
         {
            /*0*/("GUID_ID",typeof(string)),
            /*1*/("EMPRESA",typeof(string)),
            /*2*/("NUMERO",typeof(string)),
            /*3*/("FECHA",typeof(DateTime)),
            /*4*/("OBRA",typeof(string)),
            /*5*/("CLIENTE",typeof(string)),
            /*6*/("IMPORTE",typeof(decimal)),
            /*7*/("TOTALDOC",typeof(decimal)),
            /*8*/("OBSERVACIO",typeof(string)),
         }
      );
      #endregion entityStaticDataProviders


      # region entityRequiredProperties

         # region entitySynchronizationTableName
         public string TableName { get; set; } = EntitySynchronizationTableName;
         # endregion entitySynchronizationTableName

         # region entityRequiredDataTuples         
         public List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> ColumnsTuplesList { get; set; } = 
         MainSynchronizationTuples
         .Concat(GestprojectDataTuples)
         .Concat(SageDataTuples)
         .Concat(MiscelaneousSynchronizationDataTuples)
         .ToList();

         public List<(string columnName, Type columnType)> SynchronizationFieldsTupleList { get; set; } =
         MainSynchronizationTuples.Select(x => (x.columnName, x.columnType))
         .Concat(SageDataTuples.Select(x => (x.columnName, x.columnType)))
         .Concat(MiscelaneousSynchronizationDataTuples.Select(x => (x.columnName, x.columnType)))
         .ToList();

         public List<(string columnName, dynamic value)> SynchronizationFieldsDefaultValuesTupleList { get; set; } =
         MainSynchronizationTuples.Select(x => (x.columnName, x.defaultValue))
         .Concat(SageDataTuples.Select(x => (x.columnName, x.defaultValue)))
         .ToList();

         public List<(string columnName, Type columnType)> GestprojectFieldsTupleList { get; set; } =
         GestprojectDataTuples.Select(x => (x.columnName, x.columnType))
         .ToList();
         
         public ((string sageDispactcherMechanismRoute, string tableName) dispatcherAndName, List<(string name, Type type)> tableFieldsAlongTypes) SageTableData { get; set;} = SageTableDataProvider;
         #endregion entityRequiredDataTuples

         #region entityMainSynchronizationProperties
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Id { get; set; } = MainSynchronizationTuples.ElementAt(0);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) SynchronizationStatus { get; set; } = MainSynchronizationTuples.ElementAt(1);
         #endregion entityMainSynchronizationProperties

         # region entityGestprojectDataProperties
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectId { get; set; } = GestprojectDataTuples.ElementAt(0);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDaoId { get; set; } = GestprojectDataTuples.ElementAt(1);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReference { get; set; } = GestprojectDataTuples.ElementAt(2);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDate { get; set; } = GestprojectDataTuples.ElementAt(3);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectCliId { get; set; } = GestprojectDataTuples.ElementAt(4);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectTaxableBase { get; set; } = GestprojectDataTuples.ElementAt(5);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectIvaValue { get; set; } = GestprojectDataTuples.ElementAt(6);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectIvaValueInEuros { get; set; } = GestprojectDataTuples.ElementAt(7);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectIrpfValue { get; set; } = GestprojectDataTuples.ElementAt(8);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectIrpfValueInEuros { get; set; } = GestprojectDataTuples.ElementAt(9);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectTotalInvoiced { get; set; } = GestprojectDataTuples.ElementAt(10);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectBillTotal { get; set; } = GestprojectDataTuples.ElementAt(11);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectBillObservations { get; set; } = GestprojectDataTuples.ElementAt(12);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectFCE_IVA_IGIC { get; set; } = 
            GestprojectDataTuples.ElementAt(13);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) AccountableSubaccount { get; set; } = 
            GestprojectDataTuples.ElementAt(14);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) SageCompanyNumber { get; set; } = 
            GestprojectDataTuples.ElementAt(15);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) SageTaxCode { get; set; } = 
            GestprojectDataTuples.ElementAt(16);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectFCE_SUBCTA_CONTABLE { get; set; } = 
            GestprojectDataTuples.ElementAt(17);
            public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) SageProjectCode { get; set; } = 
            GestprojectDataTuples.ElementAt(18);


         # endregion entityGestprojectDataProperties

         # region entitySageDataProperties
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Sage50Code { get; set; } = 
         SageDataTuples.ElementAt(0);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Sage50GuidId { get; set; } = 
         SageDataTuples.ElementAt(1);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CompanyGroupName { get; set; } = 
         SageDataTuples.ElementAt(2);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CompanyGroupCode { get; set; } = 
         SageDataTuples.ElementAt(3);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CompanyGroupMainCode { get; set; } = 
         SageDataTuples.ElementAt(4);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CompanyGroupGuidId { get; set; } = 
         SageDataTuples.ElementAt(5);


         # endregion entitySageDataProperties

         # region entityMiscelaneousSynchronizationDataProperties
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) LastUpdate { get; set; } = MiscelaneousSynchronizationDataTuples.ElementAt(0);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) ParentUserId { get; set; } = MiscelaneousSynchronizationDataTuples.ElementAt(1);
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Comments { get; set; } = MiscelaneousSynchronizationDataTuples.ElementAt(2);
         # endregion entityMiscelaneousSynchronizationDataProperties

         # region entityUnusedProperties
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectBillNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) ProjectCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) AccountableSubaccount2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CommercialName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Cif { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) PostalCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Locality { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Province { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Country { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Iva { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CtaIvRep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CtaIvSop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) Withholding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CtaReRep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) CtaReSop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) TaxType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
         public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectProId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectLastName1 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectLastName2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) FullName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public string GestprojectEntityTableName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectGroup { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillCompanyId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillProviderId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillSubaccountableAccount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillImposableBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillIvaValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillIvaPercentage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillIrpfValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillIrpfPercentage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillTotalAmmount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillObservations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedBillProjectId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailConcept { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailUnitPrice { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailUnitNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailSubtotal { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailProjectId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectReceivedInvoiceId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectInvoiceDetailIsStructural { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) InvoiceGuid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectExerciseYear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_CONCEPTO { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_PRECIO_UNIDAD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_UNIDADES { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_SUBTOTAL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectPRY_ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectFCE_ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType, string columnDefinition, dynamic defaultValue) GestprojectDFE_SUBTOTAL_BASE { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      #endregion entityUnusedProperties

      #endregion entityRequiredProperties     
   }
}
