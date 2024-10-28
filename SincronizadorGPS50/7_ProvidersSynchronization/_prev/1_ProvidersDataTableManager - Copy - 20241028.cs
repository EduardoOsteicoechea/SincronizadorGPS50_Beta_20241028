//using Infragistics.Designers.SqlEditor;
//using sage.ew.db;
//using SincronizadorGPS50.Sage50Connector;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ProvidersDataTableManager : IGridDataSourceGenerator<GestprojectProviderModel, Sage50ProviderModel>
//   {
//      public List<GestprojectProviderModel> GestprojectEntities { get; set; }
//      public List<Sage50ProviderModel> Sage50Entities { get; set; }
//      public List<GestprojectProviderModel> ProcessedGestprojectEntities { get; set; }
//      public DataTable DataTable { get; set; }     
//      public SqlConnection Connection { get; set; } = null;
//      public ISage50ConnectionManager SageConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

//      public System.Data.DataTable GenerateDataTable
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         try
//         {
//            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchemaProvider;

//            ManageSynchronizationTableStatus(gestprojectConnectionManager, tableSchemaProvider);
//            GetAndStoreGestprojectEntities(gestprojectConnectionManager, tableSchemaProvider);
//            RemoveDeletedEntitiesFromSynchronizationTable();
//            GetAndStoreSage50Entities(tableSchemaProvider);
//            ProccessAndStoreGestprojectEntities(
//               gestprojectConnectionManager,
//               sage50ConnectionManager,
//               tableSchemaProvider,
//               GestprojectEntities,
//               Sage50Entities
//            );
//            CreateAndDefineDataSource(tableSchemaProvider);
//            PaintEntitiesOnDataSource(tableSchemaProvider, ProcessedGestprojectEntities, DataTable);
//            return DataTable;
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

//      public void ManageSynchronizationTableStatus
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         ISynchronizationDatabaseTableManager providersSyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//         bool tableExists = providersSyncronizationTableStatusManager.TableExists(
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchemaProvider.TableName
//            );

//         if(tableExists == false)
//         {
//            providersSyncronizationTableStatusManager.CreateTable
//            (
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchemaProvider
//            );
//         };
//      }

//      public void GetAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager, 
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         GestprojectEntities = new GestprojectProvidersManager().GetAll(gestprojectConnectionManager.GestprojectSqlConnection);

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntities",
//         //   GestprojectEntities
//         //);

//         List<Sage50ProviderModel> sageProviders = GetSageProviders();

//         List<GestprojectProviderModel> gestprojectProvidersFromSageProviders = ConvertSageEntitiesToGestprojectEntities(sageProviders); 

//         List<GestprojectProviderModel> definitiveGestprojectProvidersFromSageProviders = RemoveAlreadySynchronizedSageEntitiesFromList(gestprojectProvidersFromSageProviders,GestprojectEntities );

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntities",
//         //   GestprojectEntities
//         //);

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "gestprojectProvidersFromSageProviders",
//         //   gestprojectProvidersFromSageProviders
//         //);

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "definitiveGestprojectProvidersFromSageProviders",
//         //   definitiveGestprojectProvidersFromSageProviders
//         //);

//         GestprojectEntities.AddRange(definitiveGestprojectProvidersFromSageProviders);

//         //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntities",
//         //   GestprojectEntities
//         //);
//      }

//      public List<Sage50ProviderModel> GetSageProviders()
//      {
//         try
//         {
//            List<Sage50ProviderModel> entities = new List<Sage50ProviderModel>();

//            string sqlString = $@"
//            SELECT 
//               codigo
//               ,cif
//               ,nombre
//               ,direccion
//               ,codpost
//               ,poblacion
//               ,provincia
//               ,pais
//               ,guid_id 
//            FROM 
//               {DB.SQLDatabase("gestion","proveed")}
//            ;";

//            DataTable enentiesDataTable = new DataTable();

//            DB.SQLExec(sqlString, ref enentiesDataTable);

//            if(enentiesDataTable.Rows.Count > 0)
//            {
//               for(int i = 0; i < enentiesDataTable.Rows.Count; i++)
//               {
//                  Sage50ProviderModel entity = new Sage50ProviderModel();

//                  entity.CODIGO = enentiesDataTable.Rows[i].ItemArray[0].ToString().Trim();
//                  entity.CIF = enentiesDataTable.Rows[i].ItemArray[1].ToString().Trim();
//                  entity.NOMBRE = enentiesDataTable.Rows[i].ItemArray[2].ToString().Trim();
//                  entity.DIRECCION = enentiesDataTable.Rows[i].ItemArray[3].ToString().Trim();
//                  entity.CODPOST = enentiesDataTable.Rows[i].ItemArray[4].ToString().Trim();
//                  entity.POBLACION = enentiesDataTable.Rows[i].ItemArray[5].ToString().Trim();
//                  entity.PROVINCIA = enentiesDataTable.Rows[i].ItemArray[6].ToString().Trim();
//                  entity.PAIS = enentiesDataTable.Rows[i].ItemArray[7].ToString().Trim();
//                  entity.GUID_ID = enentiesDataTable.Rows[i].ItemArray[8].ToString().Trim();

//                  entities.Add(entity);
//               };
//            };
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
//         };
//      }


//      public List<GestprojectProviderModel> ConvertSageEntitiesToGestprojectEntities
//      (
//         List<Sage50ProviderModel> sageEntitiesList
//      )
//      {
//         List<GestprojectProviderModel> gestprojectEntities = new List<GestprojectProviderModel>();
//         foreach (Sage50ProviderModel sageEntity in sageEntitiesList)
//         {
//            GestprojectProviderModel gestprojectEntity = new GestprojectProviderModel();
            
//            gestprojectEntity.PAR_ID = -1;
//            gestprojectEntity.PAR_SUBCTA_CONTABLE_2 = sageEntity.CODIGO;
//            gestprojectEntity.PAR_NOMBRE = sageEntity.NOMBRE;
//            gestprojectEntity.PAR_CIF_NIF = sageEntity.CIF;
//            gestprojectEntity.PAR_DIRECCION_1 = sageEntity.DIRECCION;
//            gestprojectEntity.PAR_CP_1 = sageEntity.CODPOST;
//            gestprojectEntity.PAR_LOCALIDAD_1 = sageEntity.POBLACION;
//            gestprojectEntity.PAR_PROVINCIA_1 = sageEntity.PROVINCIA;
//            gestprojectEntity.PAR_PAIS_1 = sageEntity.PAIS;

//            gestprojectEntity.S50_CODE = sageEntity.CODIGO;
//            gestprojectEntity.S50_GUID_ID = sageEntity.GUID_ID;

//            gestprojectEntities.Add(gestprojectEntity);
//         };
//         return gestprojectEntities;
//      }


//      public void RemoveDeletedEntitiesFromSynchronizationTable
//      ()
//      {
//         try
//         {
//            List<GestprojectProviderModel> synchronizationTableEntities = GetSynchronizationTableEntities();
//            List<GestprojectProviderModel> deletedEntities = GetSynchronizationTableDeletedEntities(synchronizationTableEntities);

//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "synchronizationTableEntities",
//            //   synchronizationTableEntities
//            //);
//            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "deletedEntities",
//            //   deletedEntities
//            //);

//            if(deletedEntities.Count > 0)
//            {
//               foreach(GestprojectProviderModel entitiy in deletedEntities)
//               {
//                  DeleteEntityFromSynchronizationTable(entitiy);
//               };
//            };
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

//      public List<GestprojectProviderModel> GetSynchronizationTableEntities
//      ()
//      {
//         try
//         {
//            Connection.Open();

//            List<GestprojectProviderModel> entities = new List<GestprojectProviderModel>();

//            string sqlString = $@"SELECT * FROM {TableSchema.TableName};";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while (reader.Read()) 
//                  {
//                     GestprojectProviderModel entity = new GestprojectProviderModel();

//                     entity.ID = FilterSelectQueryResult.Filter(typeof(int), 
//                     reader.GetValue(0),"ID",-1);
//                     entity.SYNC_STATUS = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(1),"SYNC_STATUS","");

//                     entity.PAR_ID = FilterSelectQueryResult.Filter(typeof(int), 
//                     reader.GetValue(2),"PAR_ID",-1);
//                     entity.PAR_SUBCTA_CONTABLE_2 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(3),"PAR_SUBCTA_CONTABLE_2","");
//                     entity.PAR_NOMBRE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(4),"PAR_NOMBRE","");
//                     entity.PAR_APELLIDO_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(5),"PAR_APELLIDO_1","");
//                     entity.PAR_APELLIDO_2 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(6),"PAR_APELLIDO_2","");
//                     entity.NOMBRE_COMPLETO = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(7),"NOMBRE_COMPLETO","");
   
//                     entity.PAR_NOMBRE_COMERCIAL = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(8),"PAR_NOMBRE_COMERCIAL","");
//                     entity.PAR_CIF_NIF = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(9),"PAR_CIF_NIF","");
//                     entity.PAR_DIRECCION_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(10),"PAR_DIRECCION_1","");
//                     entity.PAR_CP_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(11),"PAR_CP_1","");
//                     entity.PAR_LOCALIDAD_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(12),"PAR_LOCALIDAD_1","");
//                     entity.PAR_PROVINCIA_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(13),"PAR_PROVINCIA_1","");
//                     entity.PAR_PAIS_1 = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(14),"PAR_PAIS_1","");

//                     entity.S50_CODE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(15),"S50_CODE","");
//                     entity.S50_GUID_ID = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(16),"S50_GUID_ID","");
//                     entity.S50_COMPANY_GROUP_NAME = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(17),"S50_COMPANY_GROUP_NAME","");
//                     entity.S50_COMPANY_GROUP_CODE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(18),"S50_COMPANY_GROUP_CODE","");
//                     entity.S50_COMPANY_GROUP_MAIN_CODE = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(19),"S50_COMPANY_GROUP_MAIN_CODE","");
//                     entity.S50_COMPANY_GROUP_GUID_ID = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(20),"S50_COMPANY_GROUP_GUID_ID","");

//                     entity.LAST_UPDATE = FilterSelectQueryResult.Filter(typeof(DateTime), 
//                     reader.GetValue(21),"LAST_UPDATE",DateTime.Now);

//                     entity.GP_USU_ID = FilterSelectQueryResult.Filter(typeof(int), 
//                     reader.GetValue(22),"GP_USU_ID",-1);
//                     entity.COMMENTS = FilterSelectQueryResult.Filter(typeof(string), 
//                     reader.GetValue(23),"COMMENTS","");

//                     entities.Add(entity);
//                  }
//               }
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
      
//      public List<GestprojectProviderModel> RemoveAlreadySynchronizedSageEntitiesFromList
//      (
//         List<GestprojectProviderModel> sagesEndpointEntities,
//         List<GestprojectProviderModel> gestprojectsEndpointEntities
//      )
//      {
//         try
//         {
//            List<GestprojectProviderModel> processedEntities = new List<GestprojectProviderModel>();

//            foreach (GestprojectProviderModel entity in sagesEndpointEntities)
//            {
//               if(EntityExistsInList(entity,gestprojectsEndpointEntities) == false)
//               {
//                  processedEntities.Add( entity );
//               };
//            };

//            return processedEntities;
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
            
//      public List<GestprojectProviderModel> GetSynchronizationTableDeletedEntities
//      (
//         List<GestprojectProviderModel> synchronizationTableEntities
//      )
//      {
//         try
//         {
//            List<GestprojectProviderModel> deletedEntities = new List<GestprojectProviderModel>();

//            foreach (GestprojectProviderModel entity in synchronizationTableEntities)
//            {
//               if(EntityExistsInList(entity,GestprojectEntities) == false)
//               {
//                  deletedEntities.Add( entity );
//               };
//            };

//            return deletedEntities;
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
      
//      public bool EntityExistsInList
//      (
//         GestprojectProviderModel entity,
//         List<GestprojectProviderModel> list
//      )
//      {
//         try
//         {
//            return list.Any(
//               listEntity 
//                  =>
//               listEntity.PAR_SUBCTA_CONTABLE_2 == entity.PAR_SUBCTA_CONTABLE_2
//                  &&
//               listEntity.PAR_NOMBRE == entity.PAR_NOMBRE
//                  &&
//               listEntity.PAR_CIF_NIF == entity.PAR_CIF_NIF
//                  &&
//               listEntity.PAR_DIRECCION_1 == entity.PAR_DIRECCION_1
//                  &&
//               //listEntity.PAR_LOCALIDAD_1 == entity.PAR_LOCALIDAD_1
//               //   &&
//               //listEntity.PAR_PROVINCIA_1 == entity.PAR_PROVINCIA_1
//               //   &&
//               listEntity.PAR_CP_1 == entity.PAR_CP_1
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

//      public void DeleteEntityFromSynchronizationTable
//      (
//         GestprojectProviderModel entitiy
//      )
//      {
//         try
//         {
//            Connection.Open();

//            string sqlString = $@"
//               DELETE FROM
//                  {TableSchema.TableName}
//               WHERE
//                  ID=@ID
//            ;";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@ID",entitiy.ID);
//               command.ExecuteNonQuery();
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
      

//      public void GetAndStoreSage50Entities
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         Sage50Entities = new GetSage50Providers().Entities;
//      }

//      public void ProccessAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectProviderModel> GestprojectEntities,
//         List<Sage50ProviderModel> Sage50Entities
//      )
//      {
//         ISynchronizableEntityProcessor<GestprojectProviderModel, Sage50ProviderModel> gestprojectProvidersProcessor = new GestprojectProvidersProcessor();
//         ProcessedGestprojectEntities = gestprojectProvidersProcessor.ProcessEntityList(
//            gestprojectConnectionManager.GestprojectSqlConnection,
//            sage50ConnectionManager,
//            tableSchemaProvider,
//            GestprojectEntities,
//            Sage50Entities
//         );
//      }

//      public void CreateAndDefineDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         IDataTableGenerator providersDataTableGenerator = new SyncrhonizationDataTableGenerator();
//         DataTable = providersDataTableGenerator.CreateDataTable(tableSchemaProvider.ColumnsTuplesList);
//      }

//      public void PaintEntitiesOnDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectProviderModel> ProcessedGestprojectEntities,
//         DataTable dataTable
//      )
//      {
//         ISynchronizableEntityPainter<GestprojectProviderModel> entityPainter = new EntityPainter<GestprojectProviderModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}
