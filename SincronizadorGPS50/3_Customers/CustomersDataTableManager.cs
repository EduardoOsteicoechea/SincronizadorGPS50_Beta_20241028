using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class CustomersDataTableManager : IGridDataSourceGenerator<SynchronizableCustomerModel, SageCustomerModel>
   {
      public string EntityTypeNameSingularArticle { get; set; } = "el";
      public string EntityTypeNamePluralArticle { get; set; } = "los";
      public string EntityTypeNameRoot { get; set; } = "client";
      public string EntityTypeNameGender { get; set; } = "e";
      public string EntityTypeNamePlural { get; set; } = "s";
      public string GestprojectEntityDatabaseName { get; set; } = "PARTICIPANTE";
      public string GestprojectEntityTypeDatabaseName { get; set; } = "PAR_TPA";
      public List<int?> CustomersIds { get; set; }
      public string CustomersIdsOnGestprojectConditionString { get; set; } = "";
      public List<GestprojectCustomerModel> GestprojectEntities { get; set; }
      public SynchronizableCustomerModel SynchronizableEntity { get; set; }
      public List<SynchronizableCustomerModel> SynchronizableEntities { get; set; }
      public DataTable DataTable { get; set; } = new DataTable();
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; } = null;
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

      public DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchemaProvider;
            SynchronizableEntities = new List<SynchronizableCustomerModel>();
            CustomersIds = new List<int?>();

            ManageSynchronizationTableStatus(TableSchema);

            if(GestprojectHasCustomers())
            {
               CustomersIds = GetGestprojectIdsFromPAR_TPAtable();

               CustomersIdsOnGestprojectConditionString = GenerateCustomersIdsQueryConditionString();

               GestprojectEntities = GetGestprojectEntities();

               foreach(GestprojectCustomerModel gestprojectEntity in GestprojectEntities)
               {
                  if(SynchronizationTableHasRegistries())
                  {
                     if(EntityIsAlreadyRegistered(gestprojectEntity))
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromEntity(gestprojectEntity);
                     }
                     else
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromEntity(gestprojectEntity);

                        SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                        RegisterEntityOnSynchronizationTable(SynchronizableEntity);

                     }
                  }
                  else
                  {
                     SynchronizableEntity = GenerateSynchronizableEntityFromEntity(gestprojectEntity);

                     SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                     RegisterEntityOnSynchronizationTable(SynchronizableEntity);
                  };

                  AppendSynchronizationTableDataToEntity(SynchronizableEntity);

                  SynchronizableEntities.Add(SynchronizableEntity);
               }
            }
            else
            {
               MessageBox.Show($"No encontramos {EntityTypeNameRoot + EntityTypeNameGender + EntityTypeNamePlural} en Sage.");
            }                                                     

            CreateAndDefineDataSource();
            PaintEntitiesOnDataSource();
            return DataTable;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public void ManageSynchronizationTableStatus
      (
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         ISynchronizationDatabaseTableManager providersSyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = providersSyncronizationTableStatusManager.TableExists(
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider.TableName
            );

         if(tableExists == false)
         {
            providersSyncronizationTableStatusManager.CreateTable
            (
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider
            );
         };
      }


      public bool GestprojectHasCustomers()
      {
         try
         {
            Connection.Open();

            List<GestprojectCustomerModel> entities = new List<GestprojectCustomerModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {GestprojectEntityDatabaseName})
            BEGIN
               SELECT
                  PAR_ID 
               FROM 
                  {GestprojectEntityDatabaseName}
            END
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  }
               }
            }
            return false;
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
         finally
         {
            Connection.Close();
         }
      }


      public List<int?> GetGestprojectIdsFromPAR_TPAtable()
      {
         try
         {
            Connection.Open();

            List<int?> ids = new List<int?>();

            string sqlString = $@"
            SELECT 
               PAR_ID
            FROM 
               {GestprojectEntityTypeDatabaseName}
            WHERE
               TPA_ID=@TPA_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@TPA_ID", 1);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int? id = reader["PAR_ID"] as int?;

                     ids.Add(id);
                  }
               }
            }

            return ids;
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
         finally
         {
            Connection.Close();
         }
      }


      public string GenerateCustomersIdsQueryConditionString()
      {
         try
         {
            string selectedIds = "";

            foreach(int id in CustomersIds)
            {
               selectedIds += $"'{id}',";
            }

            selectedIds = selectedIds.TrimEnd(',');

            return selectedIds;
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


      public List<GestprojectCustomerModel> GetGestprojectEntities()
      {
         try
         {
            Connection.Open();

            List<GestprojectCustomerModel> entities = new List<GestprojectCustomerModel>();

            string sqlString = $@"
            SELECT 
               PAR_ID
               ,PAR_SUBCTA_CONTABLE
               ,PAR_NOMBRE
               ,PAR_APELLIDO_1
               ,PAR_APELLIDO_2
               ,PAR_NOMBRE_COMERCIAL
               ,PAR_CIF_NIF
               ,PAR_DIRECCION_1
               ,PAR_CP_1
               ,PAR_LOCALIDAD_1
               ,PAR_PROVINCIA_1
               ,PAR_PAIS_1 
            FROM 
               {GestprojectEntityDatabaseName}
            WHERE
               PAR_ID
            IN
               ({CustomersIdsOnGestprojectConditionString})
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectCustomerModel entity = new GestprojectCustomerModel();

                     entity.PAR_ID = reader["PAR_ID"] as int?;
                     entity.PAR_SUBCTA_CONTABLE = (reader["PAR_SUBCTA_CONTABLE"] as string) ?? "";
                     entity.PAR_NOMBRE = (reader["PAR_NOMBRE"] as string) ?? "";
                     entity.PAR_APELLIDO_1 = (reader["PAR_APELLIDO_1"] as string) ?? "";
                     entity.PAR_APELLIDO_2 = (reader["PAR_APELLIDO_2"] as string) ?? "";
                     entity.PAR_NOMBRE_COMERCIAL = (reader["PAR_NOMBRE_COMERCIAL"] as string) ?? "";
                     entity.PAR_CIF_NIF = (reader["PAR_CIF_NIF"] as string) ?? "";
                     entity.PAR_DIRECCION_1 = (reader["PAR_DIRECCION_1"] as string) ?? "";
                     entity.PAR_CP_1 = (reader["PAR_CP_1"] as string) ?? "";
                     entity.PAR_LOCALIDAD_1 = (reader["PAR_LOCALIDAD_1"] as string) ?? "";
                     entity.PAR_PROVINCIA_1 = (reader["PAR_PROVINCIA_1"] as string) ?? "";
                     entity.PAR_PAIS_1 = (reader["PAR_PAIS_1"] as string) ?? "";

                     entities.Add(entity);
                  }
               }
            }

            return entities;
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
         finally
         {
            Connection.Close();
         }
      }


      public bool SynchronizationTableHasRegistries()
      {
         try
         {
            Connection.Open();

            List<GestprojectCustomerModel> entities = new List<GestprojectCustomerModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT
                  ID 
               FROM 
                  {TableSchema.TableName}
            END
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  };
               };
            };

            return false;
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
         finally
         {
            Connection.Close();
         }
      }


      public SynchronizableCustomerModel GenerateSynchronizableEntityFromEntity
      (
         GestprojectCustomerModel gestprojectEntity
      )
      {
         SynchronizableCustomerModel synchronizableEntity = new SynchronizableCustomerModel();

         synchronizableEntity.PAR_ID = gestprojectEntity.PAR_ID;
         synchronizableEntity.PAR_SUBCTA_CONTABLE = gestprojectEntity.PAR_SUBCTA_CONTABLE;
         synchronizableEntity.PAR_NOMBRE = gestprojectEntity.PAR_NOMBRE;
         synchronizableEntity.PAR_CIF_NIF = gestprojectEntity.PAR_CIF_NIF;
         synchronizableEntity.PAR_DIRECCION_1 = gestprojectEntity.PAR_DIRECCION_1;
         synchronizableEntity.PAR_CP_1 = gestprojectEntity.PAR_CP_1;
         synchronizableEntity.PAR_LOCALIDAD_1 = gestprojectEntity.PAR_LOCALIDAD_1;
         synchronizableEntity.PAR_PROVINCIA_1 = gestprojectEntity.PAR_PROVINCIA_1;
         synchronizableEntity.PAR_PAIS_1 = gestprojectEntity.PAR_PAIS_1;

         return synchronizableEntity;
      }


      public bool EntityIsAlreadyRegistered
      (
         GestprojectCustomerModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT
                  ID
               FROM
                  {TableSchema.TableName}
               WHERE
                  PAR_ID=@PAR_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  };
               };
            };

            return false;
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
         finally
         {
            Connection.Close();
         }
      }


      public void SetEntitySynchronizationStatusToUntransferred
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;
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


      public bool RegisterEntityOnSynchronizationTable
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  {TableSchema.TableName}
               (
                  SYNC_STATUS
                  ,PAR_ID
                  ,PAR_SUBCTA_CONTABLE
                  ,PAR_NOMBRE
                  ,PAR_APELLIDO_1
                  ,PAR_APELLIDO_2
                  ,PAR_NOMBRE_COMERCIAL
                  ,PAR_CIF_NIF
                  ,PAR_DIRECCION_1
                  ,PAR_CP_1
                  ,PAR_LOCALIDAD_1
                  ,PAR_PROVINCIA_1
                  ,PAR_PAIS_1
                  ,S50_CODE
                  ,S50_GUID_ID
               )
               VALUES
               (
                  @SYNC_STATUS
                  ,@PAR_ID
                  ,@PAR_SUBCTA_CONTABLE
                  ,@PAR_NOMBRE
                  ,@PAR_APELLIDO_1
                  ,@PAR_APELLIDO_2
                  ,@PAR_NOMBRE_COMERCIAL
                  ,@PAR_CIF_NIF
                  ,@PAR_DIRECCION_1
                  ,@PAR_CP_1
                  ,@PAR_LOCALIDAD_1
                  ,@PAR_PROVINCIA_1
                  ,@PAR_PAIS_1
                  ,@S50_CODE
                  ,@S50_GUID_ID
               )
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE", entity.PAR_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@PAR_NOMBRE", entity.PAR_NOMBRE);
               command.Parameters.AddWithValue("@PAR_APELLIDO_1", entity.PAR_APELLIDO_1);
               command.Parameters.AddWithValue("@PAR_APELLIDO_2", entity.PAR_APELLIDO_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE_COMERCIAL", entity.PAR_NOMBRE_COMERCIAL);
               command.Parameters.AddWithValue("@PAR_CIF_NIF", entity.PAR_CIF_NIF);
               command.Parameters.AddWithValue("@PAR_DIRECCION_1", entity.PAR_DIRECCION_1);
               command.Parameters.AddWithValue("@PAR_CP_1", entity.PAR_CP_1);
               command.Parameters.AddWithValue("@PAR_LOCALIDAD_1", entity.PAR_LOCALIDAD_1);
               command.Parameters.AddWithValue("@PAR_PROVINCIA_1", entity.PAR_PROVINCIA_1);
               command.Parameters.AddWithValue("@PAR_PAIS_1", entity.PAR_PAIS_1);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               command.ExecuteNonQuery();
            };

            return false;
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
         finally
         {
            Connection.Close();
         };
      }


      public void AppendSynchronizationTableDataToEntity
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT
                  ID
                  ,SYNC_STATUS
                  ,PAR_ID
                  ,S50_CODE
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE
                  ,GP_USU_ID
                  ,COMMENTS
               FROM
                  {TableSchema.TableName}
               WHERE
                  PAR_ID=@PAR_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.PAR_ID = reader["PAR_ID"] as int?;
                     entity.S50_CODE = reader["S50_CODE"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;

                     entity.PAR_SUBCTA_CONTABLE = entity.S50_CODE;
                  }
               }
            }
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
         finally
         {
            Connection.Close();
         }
      }









      public void CreateAndDefineDataSource()
      {
         try
         {
            IDataTableGenerator providersDataTableGenerator = new SyncrhonizationDataTableGenerator();
            DataTable = providersDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
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

      public void PaintEntitiesOnDataSource()
      {
         try
         {
            ISynchronizableEntityPainter<SynchronizableCustomerModel> entityPainter = new EntityPainter<SynchronizableCustomerModel>();

            entityPainter.PaintEntityListOnDataTable(
               SynchronizableEntities,
               DataTable,
               TableSchema.ColumnsTuplesList
            );
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
