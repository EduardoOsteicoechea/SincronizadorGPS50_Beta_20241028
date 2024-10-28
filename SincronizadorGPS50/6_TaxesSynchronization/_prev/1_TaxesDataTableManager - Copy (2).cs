//﻿using Infragistics.Designers.SqlEditor;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class TaxesDataTableManager : IGridDataSourceGenerator<GestprojectTaxModel, Sage50TaxModel>
//   {
//      public List<GestprojectTaxModel> GestprojectEntities { get; set; }
//      public List<Sage50TaxModel> Sage50Entities { get; set; }
//      public List<GestprojectTaxModel> ProcessedGestprojectEntities { get; set; }
//      public DataTable DataTable { get; set; }

//      public System.Data.DataTable GenerateDataTable
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider
//      )
//      {
//         try
//         {
//            //MessageBox.Show("This is an application stopper to verify workflow results after synchronization and before table regeneration.");
//            ManageSynchronizationTableStatus(gestprojectConnectionManager, tableSchemaProvider);
//            GetAndStoreGestprojectEntities(gestprojectConnectionManager, tableSchemaProvider);
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
//         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

//         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
//               gestprojectConnectionManager.GestprojectSqlConnection,
//               tableSchemaProvider.TableName
//            );

//         if(tableExists == false)
//         {
//            entitySyncronizationTableStatusManager.CreateTable
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
//         /////////////////////////////////
//         // Declare a new list to avoid duplication on table refresh
//         /////////////////////////////////
         
//         GestprojectEntities = new List<GestprojectTaxModel>();
//         GestprojectEntities.Clear();

//         /////////////////////////////////
//         // Get all Gestproject Database entities
//         /////////////////////////////////

//         GestprojectEntities = this.GetGestprojectTaxes(gestprojectConnectionManager.GestprojectSqlConnection);
         
//         /////////////////////////////////
//         // Determine if Gestproject entity was deleted after last syncronization (if synchronization occurred)
//         /////////////////////////////////
         
//         List<GestprojectTaxModel> deletedEntitiesInGestproject = this.GetDeletedEntitiesInGestproject(gestprojectConnectionManager, tableSchemaProvider, GestprojectEntities);

//         /////////////////////////////////
//         // If any, delete from the synchronization table, the items that were deleted from Gestproject
//         /////////////////////////////////

//         this.DeleteFromSynchronizationTable(gestprojectConnectionManager, tableSchemaProvider, deletedEntitiesInGestproject);

//         /////////////////////////////////
//         /// Get all the repeteable fields of the Gestproject entities list 
//         /// To check if the sage entities match with some of them
//         /////////////////////////////////

//         var subaccountableAccountList = GestprojectEntities.Select(x=>x.IMP_SUBCTA_CONTABLE);
//         var subaccountableAccount2List = GestprojectEntities.Select(x=>x.IMP_SUBCTA_CONTABLE_2);
//         var taxValueList = GestprojectEntities.Select(x=>x.IMP_VALOR);
//         var taxNameList = GestprojectEntities.Select(x=>x.IMP_DESCRIPCION);
         
//         /////////////////////////////////
//         // Get all Sage entities
//         /////////////////////////////////

//         List<Sage50TaxModel> sage50Entities = new GetSage50Taxes().Entities;
         
//         /////////////////////////////////
//         // Check if the sage entites already exist in Gestproject
//         /////////////////////////////////
         
//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntities",
//         //   GestprojectEntities
//         //);
            
//         foreach(Sage50TaxModel sageEntity in sage50Entities)
//         {
//            //new VisualizePropertiesAndValues<Sage50TaxModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "sageEntity",
//            //   sageEntity
//            //);

//            bool sageEntityPropertiesAreFoundInGestproject = false;
//            bool sageEntityExistsInGestproject = false;

//            bool sageEntityIsIva = sageEntity.IMP_TIPO == "IVA";

//            bool SageIvaRepExistInGestproject = subaccountableAccountList.Contains(sageEntity.CTA_IV_REP);
//            bool SageIvaSopExistInGestproject = subaccountableAccount2List.Contains(sageEntity.CTA_IV_SOP);
//            bool SageIvaValueExistInGestproject = taxValueList.Contains(Convert.ToDecimal(sageEntity.IVA));

//            bool SageIrpfRepExistInGestproject = subaccountableAccountList.Contains(sageEntity.CTA_RE_REP);
//            bool SageIrpfSopExistInGestproject = subaccountableAccount2List.Contains(sageEntity.CTA_RE_SOP);
//            bool SageIrpfValueExistInGestproject = taxValueList.Contains(Convert.ToDecimal(sageEntity.RETENCION));

//            bool SageEntityNameExistInGestproject = taxNameList.Contains(sageEntity.NOMBRE);

//            if(sageEntityIsIva)
//            {
//               sageEntityPropertiesAreFoundInGestproject = 
//                  SageIvaRepExistInGestproject 
//                  && 
//                  SageIvaSopExistInGestproject 
//                  &&
//                  SageIvaValueExistInGestproject
//                  && 
//                  SageEntityNameExistInGestproject;

//               if(sageEntityPropertiesAreFoundInGestproject)
//               {
//                  //MessageBox.Show(sageEntity.NOMBRE + " was found in the Gestproject entity List");
                 
//                  GestprojectTaxModel correspondingGestprojectEntity = GestprojectEntities.FirstOrDefault(
//                     entity => 
//                     entity.IMP_SUBCTA_CONTABLE == sageEntity.CTA_IV_REP
//                     &&
//                     entity.IMP_SUBCTA_CONTABLE_2 == sageEntity.CTA_IV_SOP
//                     &&
//                     entity.IMP_VALOR == Convert.ToDecimal(sageEntity.IVA)
//                     &&
//                     entity.IMP_DESCRIPCION == sageEntity.NOMBRE
//                  );

//                  sageEntityExistsInGestproject = correspondingGestprojectEntity != null;
                  
//                  //MessageBox.Show("sageEntityExistsInGestproject: " + sageEntityExistsInGestproject);
//               }
               
//            }
//            else
//            {
//               sageEntityPropertiesAreFoundInGestproject = 
//                  SageIrpfRepExistInGestproject 
//                  && 
//                  SageIrpfSopExistInGestproject 
//                  && 
//                  SageIrpfValueExistInGestproject
//                  && 
//                  SageEntityNameExistInGestproject;
               

//               if(sageEntityPropertiesAreFoundInGestproject)
//               {
//                  //MessageBox.Show(sageEntity.NOMBRE + " was found in the Gestproject entity List");

//                  GestprojectTaxModel correspondingGestprojectEntity = GestprojectEntities.FirstOrDefault(
//                     entity => 
//                     entity.IMP_SUBCTA_CONTABLE == sageEntity.CTA_RE_REP
//                     &&
//                     entity.IMP_SUBCTA_CONTABLE_2 == sageEntity.CTA_RE_SOP
//                     &&
//                     entity.IMP_VALOR == sageEntity.RETENCION
//                     &&
//                     entity.IMP_DESCRIPCION == sageEntity.NOMBRE
//                  );

//                  sageEntityExistsInGestproject = correspondingGestprojectEntity != null;
                  
//                  //MessageBox.Show("sageEntityExistsInGestproject: " + sageEntityExistsInGestproject);
//               }
//            };

//            //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//            //   "GestprojectEntities",
//            //   GestprojectEntities
//            //);

//            if(sageEntityExistsInGestproject == false)
//            {
//               GestprojectTaxModel gestprojectTaxModel = new GestprojectTaxModel();

//               gestprojectTaxModel.IMP_ID = 0;
//               gestprojectTaxModel.IMP_TIPO = sageEntity.IMP_TIPO;
//               gestprojectTaxModel.IMP_DESCRIPCION = sageEntity.NOMBRE.Trim();

//               if(sageEntity.IMP_TIPO == "IVA")
//               {
//                  gestprojectTaxModel.IMP_NOMBRE = $"{sageEntity.IMP_TIPO} {sageEntity.IVA.ToString().Split(',')[0]}";
//                  gestprojectTaxModel.IMP_VALOR = Convert.ToDecimal(sageEntity.IVA);
//                  gestprojectTaxModel.IMP_SUBCTA_CONTABLE = sageEntity.CTA_IV_REP;
//                  gestprojectTaxModel.IMP_SUBCTA_CONTABLE_2 = sageEntity.CTA_IV_SOP;
//               }
//               else
//               {
//                  gestprojectTaxModel.IMP_NOMBRE = $"{sageEntity.IMP_TIPO} {sageEntity.RETENCION.ToString().Split(',')[0]}";
//                  gestprojectTaxModel.IMP_VALOR = sageEntity.RETENCION;
//                  gestprojectTaxModel.IMP_SUBCTA_CONTABLE = sageEntity.CTA_RE_REP;
//                  gestprojectTaxModel.IMP_SUBCTA_CONTABLE_2 = sageEntity.CTA_RE_SOP;
//               };

//               gestprojectTaxModel.S50_CODE = sageEntity.CODIGO;
//               gestprojectTaxModel.S50_GUID_ID = sageEntity.GUID_ID;

//               GestprojectEntities.Add(gestprojectTaxModel);
//            };               
//         };

//         //MessageBox.Show("Final iteration");

//         //new VisualizePropertiesAndValues<GestprojectTaxModel>(
//         //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
//         //   "GestprojectEntities",
//         //   GestprojectEntities
//         //);
//      }

//      public List<GestprojectTaxModel> GetGestprojectTaxes
//      (
//         System.Data.SqlClient.SqlConnection connection
//      )
//      {
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//               SELECT
//                  IMP_ID
//                  ,IMP_TIPO
//                  ,IMP_NOMBRE
//                  ,IMP_DESCRIPCION
//                  ,IMP_VALOR
//                  ,IMP_SUBCTA_CONTABLE
//                  ,IMP_SUBCTA_CONTABLE_2
//               FROM
//                  IMPUESTO_CONFIG
//            ;";

//            List<GestprojectTaxModel> entitiesList = new List<GestprojectTaxModel>();

//            using(SqlCommand command = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     GestprojectTaxModel entity = new GestprojectTaxModel();

//                     int IMP_ID_ordinal = 0;
//                     int IMP_TIPO_ordinal = 1;
//                     int IMP_NOMBRE_ordinal = 2;
//                     int IMP_DESCRIPCION_ordinal = 3;
//                     int IMP_VALOR_ordinal = 4;
//                     int IMP_SUBCTA_CONTABLE_ordinal = 5;
//                     int IMP_SUBCTA_CONTABLE_2_ordinal = 6;

//                     entity.IMP_ID = Convert.ToInt32(
//                        reader.GetValue(IMP_ID_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_ID_ordinal) : 0
//                     );
//                     entity.IMP_TIPO = Convert.ToString(
//                        reader.GetValue(IMP_TIPO_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_TIPO_ordinal) : ""
//                     );
//                     entity.IMP_NOMBRE = Convert.ToString(
//                        reader.GetValue(IMP_NOMBRE_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_NOMBRE_ordinal) : ""
//                     );
//                     entity.IMP_DESCRIPCION = Convert.ToString(
//                        reader.GetValue(IMP_DESCRIPCION_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_DESCRIPCION_ordinal) : ""
//                     );
//                     entity.IMP_VALOR = Convert.ToDecimal(
//                        reader.GetValue(IMP_VALOR_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_VALOR_ordinal) : null
//                     );
//                     entity.IMP_SUBCTA_CONTABLE = Convert.ToString(
//                        reader.GetValue(IMP_SUBCTA_CONTABLE_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_SUBCTA_CONTABLE_ordinal) : ""
//                     );
//                     entity.IMP_SUBCTA_CONTABLE_2 = Convert.ToString(
//                        reader.GetValue(IMP_SUBCTA_CONTABLE_2_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(IMP_SUBCTA_CONTABLE_2_ordinal) : ""
//                     );

//                     entitiesList.Add(entity);
//                  };
//               };
//            };

//            return entitiesList;
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
//            connection.Close();
//         };
//      }


//      public List<GestprojectTaxModel> GetDeletedEntitiesInGestproject
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectTaxModel> GestprojectEntities
//      )
//      {
//         SqlConnection connection = gestprojectConnectionManager.GestprojectSqlConnection;
//         try
//         {
//            connection.Open();

//            string sqlString = $@"
//               SELECT
//                  ID
//                  ,IMP_ID
//                  ,SYNC_STATUS
//               FROM
//                  {tableSchemaProvider.TableName}
//            ;";

//            List<GestprojectTaxModel> deletedEntitiesList = new List<GestprojectTaxModel>();
//            List<int?> existingEntitiesInGestprojectIds = GestprojectEntities.Select(
//               entity => entity.IMP_ID
//            ).ToList();

//            using(SqlCommand command = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  int ID_ordinal = 0;
//                  int IMP_ID_ordinal = 1;
//                  int SYNC_STATUS_ordinal = 2;

//                  while(reader.Read())
//                  {  
//                     string synchronizationStatus = Convert.ToString(reader.GetValue(SYNC_STATUS_ordinal));
//                     int currentEntityGestprojectId = Convert.ToInt32(reader.GetValue(IMP_ID_ordinal));

//                     if(
//                        synchronizationStatus == SynchronizationStatusOptions.Sincronizado
//                        &&
//                        existingEntitiesInGestprojectIds.Contains(currentEntityGestprojectId) == false
//                     )
//                     {
//                        GestprojectTaxModel entity = new GestprojectTaxModel();

//                        entity.ID = Convert.ToInt32(
//                           reader.GetValue(ID_ordinal).GetType() != typeof(DBNull) ? reader.GetValue(ID_ordinal) : 0
//                        );

//                        deletedEntitiesList.Add(entity);
//                     };
//                  };
//               };
//            };

//            return deletedEntitiesList;
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
//            connection.Close();
//         };
//      }

//      public void DeleteFromSynchronizationTable
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectTaxModel> entitiesToDelete
//      )
//      {
//         SqlConnection connection = gestprojectConnectionManager.GestprojectSqlConnection;
         
//         foreach(GestprojectTaxModel entity in entitiesToDelete)
//         {
//            try
//            {
//               connection.Open();

//               string sqlString = $@"
//                  DELETE FROM
//                     {tableSchemaProvider.TableName}
//                  WHERE
//                     ID=@ID
//               ;";

//               using(SqlCommand command = new SqlCommand(sqlString, connection))
//               {
//                  command.Parameters.AddWithValue("@ID",entity.ID);
//                  command.ExecuteNonQuery();
//               };               
//            }
//            catch(System.Exception exception)
//            {
//               throw ApplicationLogger.ReportError(
//                  MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//                  MethodBase.GetCurrentMethod().DeclaringType.Name,
//                  MethodBase.GetCurrentMethod().Name,
//                  exception
//               );
//            }
//            finally
//            {
//               connection.Close();
//            };
//         };
//      }


//      public void GetAndStoreSage50Entities 
//      ( 
//         ISynchronizationTableSchemaProvider tableSchemaProvider 
//      )
//      {
//         Sage50Entities = new GetSage50Taxes().Entities;
//         //new VisualizePropertiesAndValues<Sage50TaxModel>("Sage50Entities", Sage50Entities);
//      }

//      public void ProccessAndStoreGestprojectEntities
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectTaxModel> GestprojectEntities,
//         List<Sage50TaxModel> Sage50Entities
//      )
//      {

//         ISynchronizableEntityProcessor<GestprojectTaxModel, Sage50TaxModel> gestprojectProvidersProcessor = new GestprojectTaxesProcessor();
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
//         IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
//         DataTable = entityDataTableGenerator.CreateDataTable(tableSchemaProvider.ColumnsTuplesList);
//      }

//      public void PaintEntitiesOnDataSource
//      (
//         ISynchronizationTableSchemaProvider tableSchemaProvider,
//         List<GestprojectTaxModel> ProcessedGestprojectEntities,
//         DataTable dataTable
//      )
//      {
//         ISynchronizableEntityPainter<GestprojectTaxModel> entityPainter = new EntityPainter<GestprojectTaxModel>();
//         entityPainter.PaintEntityListOnDataTable(
//            ProcessedGestprojectEntities,
//            dataTable,
//            tableSchemaProvider.ColumnsTuplesList
//         );
//      }
//   }
//}