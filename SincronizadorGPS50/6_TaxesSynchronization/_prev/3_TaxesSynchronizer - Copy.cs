//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class TaxesSynchronizer : IEntitySynchronizer<GestprojectTaxModel, Sage50TaxModel>
//   {
//      public List<GestprojectTaxModel> GestprojectEntityList {get;set;} = new List<GestprojectTaxModel>();
//      public List<Sage50TaxModel> Sage50EntityList {get;set;} = new List<Sage50TaxModel> { };
//      public List<GestprojectTaxModel> UnexistingGestprojectEntityList {get;set;} = new List<GestprojectTaxModel>();
//      public List<GestprojectTaxModel> ExistingGestprojectEntityList {get;set; } = new List<GestprojectTaxModel>();
//      public List<GestprojectTaxModel> UnsynchronizedGestprojectEntityList {get;set; } = new List<GestprojectTaxModel>();
//      public bool SomeEntitiesExistsInSage50 {get;set;}
//      public bool AllEntitiesExistsInSage50 {get;set;}
//      public bool NoEntitiesExistsInSage50 {get;set;}
//      public bool UnsynchronizedEntityExists {get;set;}
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public SqlConnection Connection { get; set; }
//      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
//      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

//      public void Synchronize
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchema,
//         List<int> selectedIdList
//      )
//      {
//         try
//         {
//            GestprojectConnectionManager = gestprojectConnectionManager;
//            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
//            Sage50ConnectionManager = sage50ConnectionManager;
//            SynchronizationTableSchemaProvider = tableSchema;
//            TableSchema = tableSchema;

//            GestprojectEntityList = new List<GestprojectTaxModel>();
//            Sage50EntityList = new List<Sage50TaxModel>();
//            UnexistingGestprojectEntityList = new List<GestprojectTaxModel>();
//            ExistingGestprojectEntityList = new List<GestprojectTaxModel>();
//            UnsynchronizedGestprojectEntityList = new List<GestprojectTaxModel>();

//            StoreGestprojectEntityList
//            (
//               GestprojectConnectionManager,
//               selectedIdList,
//               SynchronizationTableSchemaProvider.TableName,
//               SynchronizationTableSchemaProvider.ColumnsTuplesList.Select(x=>(x.columnName,x.columnType)).ToList(),
//               (
//                  tableSchema.GestprojectId.ColumnDatabaseName,
//                  string.Join(",", selectedIdList)
//               )
//            );



//            new VisualizePropertiesAndValues<GestprojectTaxModel>(
//               MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//               "GestprojectEntityList",
//               GestprojectEntityList
//            );

//            StoreSage50EntityList
//            (
//               tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,
//               tableSchema.SageTableData.dispatcherAndName.tableName,
//               tableSchema.SageTableData.tableFieldsAlongTypes
//            );

//            StoreBreakDownGestprojectEntityListByStatus(GestprojectEntityList, Sage50EntityList);

//            DetermineEntitySincronizationWorkflow(UnexistingGestprojectEntityList, ExistingGestprojectEntityList, UnsynchronizedGestprojectEntityList, GestprojectEntityList);

//            ExecuteSyncronizationWorkflow
//            (
//               SomeEntitiesExistsInSage50,
//               AllEntitiesExistsInSage50,
//               NoEntitiesExistsInSage50,
//               UnsynchronizedEntityExists,
//               GestprojectConnectionManager,
//               Sage50ConnectionManager,
//               SynchronizationTableSchemaProvider,
//               UnexistingGestprojectEntityList,
//               ExistingGestprojectEntityList,
//               UnsynchronizedGestprojectEntityList,
//               GestprojectEntityList
//            );
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }

//      public void StoreGestprojectEntityList
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager, 
//         List<int> selectedIdList, 
//         string tableName, 
//         List<(string, System.Type)> fieldsToBeRetrieved, 
//         (string condition1ColumnName, string condition1Value) condition1Data
//      )
//      {
//         // Rewrite this command. It was modified, and now is loading the whole list
//         //GestprojectEntityList = new GestprojectEntities<GestprojectTaxModel>().GetAll(
//         //   gestprojectConnectionManager.GestprojectSqlConnection,
//         //   selectedIdList,
//         //   tableName,
//         //   fieldsToBeRetrieved,
//         //   condition1Data
//         //);

//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntityList",
//         //   GestprojectEntityList
//         //);

         
//         try
//         {
//            Connection.Open();

//            string idsString = "(";
//            for (global::System.Int32 i = 0; i < selectedIdList.Count; i++)
//            {
//               if (i != selectedIdList.Count - 1)
//                  idsString += "'" + selectedIdList[i] + "',";
//               else
//                  idsString += "'" + selectedIdList[i] + "'";
//            };            
//            idsString += ")";

//            string sqlString = $@"
//               SELECT * FROM
//                  {TableSchema.TableName}
//               WHERE
//                  ID in {idsString}
//            ;";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     GestprojectTaxModel entity = new GestprojectTaxModel();

//                     entity.ID = FilterSelectQueryResult.Filter(typeof(int),
//                     reader.GetValue(0), "ID", -1);
//                     entity.SYNC_STATUS = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(1), "SYNC_STATUS", "");

//                     entity.IMP_ID = FilterSelectQueryResult.Filter(typeof(int), 
//                     reader.GetValue(2), "IMP_ID", -1);
//                     entity.IMP_TIPO = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(3), "IMP_TIPO", "");
//                     entity.IMP_NOMBRE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(4), "IMP_NOMBRE", "");
//                     entity.IMP_DESCRIPCION = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(5), "IMP_DESCRIPCION", "");
//                     entity.IMP_VALOR = FilterSelectQueryResult.Filter(typeof(decimal), 
//                     reader.GetValue(6), "IMP_VALOR", 0);
//                     entity.IMP_SUBCTA_CONTABLE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(7), "IMP_SUBCTA_CONTABLE", "");
//                     entity.IMP_SUBCTA_CONTABLE_2 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(8), "IMP_SUBCTA_CONTABLE_2", "");

//                     entity.S50_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(9), "S50_CODE", "");
//                     entity.S50_GUID_ID = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(10), "S50_GUID_ID", "");
//                     entity.S50_COMPANY_GROUP_NAME = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(11), "S50_COMPANY_GROUP_NAME", "");
//                     entity.S50_COMPANY_GROUP_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(12), "S50_COMPANY_GROUP_CODE", "");
//                     entity.S50_COMPANY_GROUP_MAIN_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(13), "S50_COMPANY_GROUP_MAIN_CODE", "");
//                     entity.S50_COMPANY_GROUP_GUID_ID = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(14), "S50_COMPANY_GROUP_GUID_ID", "");

//                     entity.LAST_UPDATE = FilterSelectQueryResult.Filter(typeof(DateTime),
//                     reader.GetValue(15), "LAST_UPDATE", DateTime.Now);

//                     entity.GP_USU_ID = FilterSelectQueryResult.Filter(typeof(int),
//                     reader.GetValue(16), "GP_USU_ID", -1);
//                     entity.COMMENTS = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(17), "COMMENTS", "");

//                     GestprojectEntityList.Add(entity);
//                  }
//               };
//            }
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            Connection.Close();
//         };
//      }

//      public void StoreSage50EntityList
//      (
//         string sageDispactcherMechanismRoute,
//         string tableName,
//         List<(string, System.Type)> tableFieldsAlongTypes
//      )
//      {
//         //Sage50EntityList = new Sage50Entities<Sage50TaxModel>().GetAll(
//         //   sageDispactcherMechanismRoute,
//         //   tableName,
//         //   tableFieldsAlongTypes
//         //);
         
//         Sage50EntityList = new GetSage50Taxes().Entities;
         
//         //MessageBox.Show("Sage50EntityList.Count: " + Sage50EntityList.Count);
//      }
      
//      public void StoreBreakDownGestprojectEntityListByStatus
//      (
//         List<GestprojectTaxModel> GestprojectEntityList, 
//         List<Sage50TaxModel> Sage50EntityList
//      )
//      {
//         foreach(var gestprojectEntity in GestprojectEntityList)
//         //for(int i = 0; i < GestprojectEntityList.Count; i++)
//         {
//            bool existInSage50 = false;
//            bool GestprojectEntityHasSageCodeValue = gestprojectEntity.S50_CODE != "";
//            bool GestprojectEntityHasUnsynchronizedStatus = gestprojectEntity.SYNC_STATUS != "Sincronizado";

//            foreach(var sageEntity in Sage50EntityList)
//            {
//               bool entitiesNamesMatch = gestprojectEntity.IMP_DESCRIPCION == sageEntity.NOMBRE;
//               bool entitiesCodesMatch = gestprojectEntity.S50_CODE == sageEntity.CODIGO;
//               bool entityHasCompanyGroupGuidIdValue = gestprojectEntity.S50_COMPANY_GROUP_GUID_ID != "";
//               bool entitiesGuidIdsMatch = gestprojectEntity.S50_GUID_ID == sageEntity.GUID_ID;

//               if( 
//                  entitiesNamesMatch 
//                  && 
//                  entitiesCodesMatch 
//                  && 
//                  entitiesGuidIdsMatch 
//                  && 
//                  entityHasCompanyGroupGuidIdValue 
//                  && 
//                  GestprojectEntityHasSageCodeValue
//               )
//               {
//                  ExistingGestprojectEntityList.Add(gestprojectEntity);
//                  existInSage50 = true;
//                  break;
//               };               
//            };

//            if(existInSage50 == false)
//            {
//               UnexistingGestprojectEntityList.Add(gestprojectEntity);
//            };

//            if(GestprojectEntityHasUnsynchronizedStatus && GestprojectEntityHasSageCodeValue)
//            {
//               UnsynchronizedGestprojectEntityList.Add(gestprojectEntity);
//            };
//         };
//      }

//      public void DetermineEntitySincronizationWorkflow
//      (
//         List<GestprojectTaxModel> UnexistingGestprojectEntityList, 
//         List<GestprojectTaxModel> ExistingGestprojectEntityList, 
//         List<GestprojectTaxModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectTaxModel> GestprojectEntityList
//      )
//      {
//         SomeEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count > 0;
//         AllEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == GestprojectEntityList.Count;
//         NoEntitiesExistsInSage50 = ExistingGestprojectEntityList.Count == 0;
//         UnsynchronizedEntityExists = UnsynchronizedGestprojectEntityList.Count > 0;
//      }

//      public void ExecuteSyncronizationWorkflow
//      (
//         bool SomeEntitiesExistsInSage50, 
//         bool AllEntitiesExistsInSage50, 
//         bool NoEntitiesExistsInSage50, 
//         bool UnsynchronizedEntityExists, 
//         IGestprojectConnectionManager gestprojectConnectionManager, 
//         ISage50ConnectionManager sage50ConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchema, 
//         List<GestprojectTaxModel> unexistingGestprojectEntityList, 
//         List<GestprojectTaxModel> existingGestprojectEntityList, 
//         List<GestprojectTaxModel> unsynchronizedGestprojectEntityList,
//         List<GestprojectTaxModel> gestprojectEntityList
//      )
//      {
//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "unexistingGestprojectEntityList",
//         //   unexistingGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "existingGestprojectEntityList",
//         //   existingGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "unsynchronizedGestprojectEntityList",
//         //   unsynchronizedGestprojectEntityList
//         //);
//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "gestprojectEntityList",
//         //   gestprojectEntityList
//         //);

//         new TaxSynchronizationWorkflow(
//            gestprojectConnectionManager,
//            sage50ConnectionManager.CompanyGroupData,
//            tableSchema,
//            gestprojectEntityList,
//            unsynchronizedGestprojectEntityList,
//            Sage50EntityList
//         );

//         /////////////////////////////////////////
//         // Clear Lists to avoid repetition
//         /////////////////////////////////////////

//         gestprojectEntityList.Clear();
//         unsynchronizedGestprojectEntityList.Clear();
//         Sage50EntityList.Clear();
//      }
//   }
//}
