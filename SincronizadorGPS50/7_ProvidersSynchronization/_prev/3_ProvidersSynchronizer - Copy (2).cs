//using Infragistics.Designers.SqlEditor;
//using sage.ew.db;
//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ProvidersSynchronizer : IEntitySynchronizer<GestprojectProviderModel, Sage50ProviderModel>
//   {
//      public List<GestprojectProviderModel> GestprojectEntityList {get;set;} = new List<GestprojectProviderModel>();
//      public List<Sage50ProviderModel> Sage50EntityList {get;set;} = new List<Sage50ProviderModel> { };
//      public List<GestprojectProviderModel> UnexistingGestprojectEntityList {get;set;} = new List<GestprojectProviderModel>();
//      public List<GestprojectProviderModel> ExistingGestprojectEntityList {get;set; } = new List<GestprojectProviderModel>();
//      public List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList {get;set; } = new List<GestprojectProviderModel>();
//      public bool SomeEntitiesExistsInSage50 {get;set;}
//      public bool AllEntitiesExistsInSage50 {get;set;}
//      public bool NoEntitiesExistsInSage50 {get;set;}
//      public bool UnsynchronizedEntityExists {get;set;}
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public SqlConnection Connection { get; set; }
//      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
//      public ISage50ConnectionManager SageConnectionManager { get; set; }
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
//            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchema;

//            StoreGestprojectEntityList(
//               GestprojectConnectionManager,
//               selectedIdList,
//               TableSchema.TableName,
//               TableSchema.ColumnsTuplesList.Select(x => (x.columnName, x.columnType)).ToList(),
//               (TableSchema.GestprojectId.ColumnDatabaseName, string.Join(",", selectedIdList))
//            );

//            StoreSage50EntityList(
//               tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,
//               tableSchema.SageTableData.dispatcherAndName.tableName,
//               tableSchema.SageTableData.tableFieldsAlongTypes
//            );

//            StoreBreakDownGestprojectEntityListByStatus(
//               GestprojectEntityList,
//               Sage50EntityList
//            );

//            DetermineEntitySincronizationWorkflow(
//               UnexistingGestprojectEntityList,
//               ExistingGestprojectEntityList,
//               UnsynchronizedGestprojectEntityList,
//               GestprojectEntityList
//            );

//            ExecuteSyncronizationWorkflow(
//               SomeEntitiesExistsInSage50,
//               AllEntitiesExistsInSage50,
//               NoEntitiesExistsInSage50,
//               UnsynchronizedEntityExists,
//               GestprojectConnectionManager,
//               SageConnectionManager,
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
//         GestprojectEntityList = GetSelectedGestprojectEntities(selectedIdList);
//      }

//      public List<GestprojectProviderModel> GetSelectedGestprojectEntities
//      (
//         List<int> idList
//      )
//      {
//         try
//         {
//            Connection.Open();

//            string idsString = "(";
//            for (global::System.Int32 i = 0; i < idList.Count; i++)
//            {
//               if (i != idList.Count - 1)
//                  idsString += "'" + idList[i] + "',";
//               else
//                  idsString += "'" + idList[i] + "'";
//            };            
//            idsString += ")";

//            string sqlString = $@"
//               SELECT * FROM
//                  {TableSchema.TableName}
//               WHERE
//                  ID in {idsString}
//            ;";
            
//            List<GestprojectProviderModel> entities = new List<GestprojectProviderModel>();

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@IDS", idsString);

//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     GestprojectProviderModel entity = new GestprojectProviderModel();

//                     entity.ID = FilterSelectQueryResult.Filter(typeof(int),
//                     reader.GetValue(0), "ID", -1);
//                     entity.SYNC_STATUS = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(1), "SYNC_STATUS", "");

//                     entity.PAR_ID = FilterSelectQueryResult.Filter(typeof(int),
//                     reader.GetValue(2), "PAR_ID", -1);
//                     entity.PAR_SUBCTA_CONTABLE_2 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(3), "PAR_SUBCTA_CONTABLE_2", "");
//                     entity.PAR_NOMBRE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(4), "PAR_NOMBRE", "");
//                     entity.PAR_APELLIDO_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(5), "PAR_APELLIDO_1", "");
//                     entity.PAR_APELLIDO_2 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(6), "PAR_APELLIDO_2", "");
//                     entity.NOMBRE_COMPLETO = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(7), "NOMBRE_COMPLETO", "");

//                     entity.PAR_NOMBRE_COMERCIAL = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(8), "PAR_NOMBRE_COMERCIAL", "");
//                     entity.PAR_CIF_NIF = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(9), "PAR_CIF_NIF", "");
//                     entity.PAR_DIRECCION_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(10), "PAR_DIRECCION_1", "");
//                     entity.PAR_CP_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(11), "PAR_CP_1", "");
//                     entity.PAR_LOCALIDAD_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(12), "PAR_LOCALIDAD_1", "");
//                     entity.PAR_PROVINCIA_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(13), "PAR_PROVINCIA_1", "");
//                     entity.PAR_PAIS_1 = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(14), "PAR_PAIS_1", "");

//                     entity.S50_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(15), "S50_CODE", "");
//                     entity.S50_GUID_ID = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(16), "S50_GUID_ID", "");
//                     entity.S50_COMPANY_GROUP_NAME = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(17), "S50_COMPANY_GROUP_NAME", "");
//                     entity.S50_COMPANY_GROUP_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(18), "S50_COMPANY_GROUP_CODE", "");
//                     entity.S50_COMPANY_GROUP_MAIN_CODE = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(19), "S50_COMPANY_GROUP_MAIN_CODE", "");
//                     entity.S50_COMPANY_GROUP_GUID_ID = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(20), "S50_COMPANY_GROUP_GUID_ID", "");

//                     entity.LAST_UPDATE = FilterSelectQueryResult.Filter(typeof(DateTime),
//                     reader.GetValue(21), "LAST_UPDATE", DateTime.Now);

//                     entity.GP_USU_ID = FilterSelectQueryResult.Filter(typeof(int),
//                     reader.GetValue(22), "GP_USU_ID", -1);
//                     entity.COMMENTS = FilterSelectQueryResult.Filter(typeof(string),
//                     reader.GetValue(23), "COMMENTS", "");

//                     entities.Add(entity);
//                  }
//               };
//            }

//            return entities;
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
//         string sqlString = $@"
//            SELECT 
//               CODIGO
//               ,CIF
//               ,NOMBRE
//               ,DIRECCION
//               ,CODPOST
//               ,POBLACION
//               ,PROVINCIA
//               ,PAIS
//               ,GUID_ID
//            FROM 
//               {DB.SQLDatabase("gestion","proveed")}
//         ;";

//         DataTable entityDataTable = new DataTable();

//         DB.SQLExec(sqlString, ref entityDataTable);

//         if(entityDataTable.Rows.Count > 0)
//         {
//            for(int i = 0; i < entityDataTable.Rows.Count; i++)
//            {
//               DataRow dataRow = entityDataTable.Rows[i];
//               Sage50ProviderModel entity = new Sage50ProviderModel();

//               entity.CODIGO = dataRow.ItemArray[0].ToString().Trim();
//               entity.CIF = dataRow.ItemArray[1].ToString().Trim();
//               entity.NOMBRE = dataRow.ItemArray[2].ToString().Trim();
//               entity.DIRECCION = dataRow.ItemArray[3].ToString().Trim();
//               entity.CODPOST = dataRow.ItemArray[4].ToString().Trim();
//               entity.POBLACION = dataRow.ItemArray[5].ToString().Trim();
//               entity.PROVINCIA = dataRow.ItemArray[6].ToString().Trim();
//               entity.PAIS = dataRow.ItemArray[7].ToString().Trim();
//               entity.GUID_ID = dataRow.ItemArray[8].ToString().Trim();

//               Sage50EntityList.Add(entity);
//            };
//         };
//      }
      
//      public void StoreBreakDownGestprojectEntityListByStatus
//      (
//         List<GestprojectProviderModel> GestprojectEntityList, 
//         List<Sage50ProviderModel> Sage50EntityList
//      )
//      {
//         for(int i = 0; i < GestprojectEntityList.Count; i++)
//         {
//            var gestprojectEntity = GestprojectEntityList[i];
//            bool exists = false;

//            foreach (var sageEntitiy in Sage50EntityList)
//            {
//               if( gestprojectEntity.S50_CODE == sageEntitiy.GUID_ID )
//               {
//                  ExistingGestprojectEntityList.Add(gestprojectEntity);
//                  exists = true;
//                  break;
//               };
//            };

//            if(!exists)
//            {
//               UnexistingGestprojectEntityList.Add(gestprojectEntity);
//            };

//            bool gestprojectEntityHasSynchronizedStatus = gestprojectEntity.SYNC_STATUS == "Sincronizado";
//            bool gestprojectEntityHasSageCode = gestprojectEntity.S50_CODE != "";

//            if(
//               gestprojectEntityHasSynchronizedStatus == false 
//               && 
//               gestprojectEntityHasSageCode == true
//            )
//            {
//               UnsynchronizedGestprojectEntityList.Add(gestprojectEntity);
//            };
//         };
//      }

//      public void DetermineEntitySincronizationWorkflow
//      (
//         List<GestprojectProviderModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProviderModel> ExistingGestprojectEntityList, 
//         List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProviderModel> GestprojectEntityList
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
//         IGestprojectConnectionManager GestprojectConnectionManager, 
//         ISage50ConnectionManager Sage50ConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchemaProvider, 
//         List<GestprojectProviderModel> UnexistingGestprojectEntityList, 
//         List<GestprojectProviderModel> ExistingGestprojectEntityList, 
//         List<GestprojectProviderModel> UnsynchronizedGestprojectEntityList,
//         List<GestprojectProviderModel> GestprojectEntityList
//      )
//      {         
//         try
//         {
//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "UnexistingGestprojectEntityList",
//            //   UnexistingGestprojectEntityList
//            //);
//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "ExistingGestprojectEntityList",
//            //   ExistingGestprojectEntityList
//            //);
//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "UnsynchronizedGestprojectEntityList",
//            //   UnsynchronizedGestprojectEntityList
//            //);
//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "GestprojectEntityList",
//            //   GestprojectEntityList
//            //);

//            new UnexsistingProvidersSynchronizationWorkflow().Execute
//            (
//               this.GestprojectConnectionManager,
//               this.SageConnectionManager,
//               this.UnexistingGestprojectEntityList,
//               this.TableSchema
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
//         }
//      }
//   }
//}
