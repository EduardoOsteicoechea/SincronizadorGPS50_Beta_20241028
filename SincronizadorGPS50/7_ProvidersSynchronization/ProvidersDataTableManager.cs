using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ProvidersDataTableManager : IGridDataSourceGenerator<GestprojectProviderModel, Sage50ProviderModel>
   {
      public List<Sage50ProviderModel> SageEntities { get; set; }
      public SynchronizableProviderModel SynchronizableEntity { get; set; }
      public List<SynchronizableProviderModel> SynchronizableEntities { get; set; }
      public DataTable DataTable { get; set; } = new DataTable();
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; } = null;
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

      public System.Data.DataTable GenerateDataTable
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
            SynchronizableEntities = new List<SynchronizableProviderModel>();

            ManageSynchronizationTableStatus(TableSchema);

            if(SageHasProviders())
            {
               SageEntities = GetSageEntities();

               foreach(Sage50ProviderModel sageEntity in SageEntities)
               {
                  if(SynchronizationTableHasRegistries())
                  {
                     if(EntityIsAlreadyRegistered(sageEntity))
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);
                     }
                     else
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);

                        SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                        RegisterEntityOnSynchronizationTable(SynchronizableEntity);

                     }
                  }
                  else
                  {
                     SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);

                     SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                     RegisterEntityOnSynchronizationTable(SynchronizableEntity);
                  };

                  AppendSynchronizationTableDataToEntity(SynchronizableEntity);

                  SynchronizableEntities.Add(SynchronizableEntity);
               }
            }
            else
            {
               MessageBox.Show("No encontramos proveedores en Sage.");
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


      public bool SageHasProviders()
      {

         try
         {
            List<Sage50ProviderModel> entities = new List<Sage50ProviderModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {DB.SQLDatabase("gestion","proveed")})
            BEGIN
               SELECT
                  guid_id 
               FROM 
                  {DB.SQLDatabase("gestion","proveed")}
            END
            ;";

            DataTable enentiesDataTable = new DataTable();

            DB.SQLExec(sqlString, ref enentiesDataTable);

            if(enentiesDataTable.Rows.Count > 0)
            {
               return true;
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
         };
      }


      public List<Sage50ProviderModel> GetSageEntities()
      {
         try
         {
            List<Sage50ProviderModel> entities = new List<Sage50ProviderModel>();

            string sqlString = $@"
            SELECT 
               codigo
               ,cif
               ,nombre
               ,direccion
               ,codpost
               ,poblacion
               ,provincia
               ,pais
               ,guid_id 
            FROM 
               {DB.SQLDatabase("gestion","proveed")}
            ;";

            DataTable enentiesDataTable = new DataTable();

            DB.SQLExec(sqlString, ref enentiesDataTable);

            if(enentiesDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < enentiesDataTable.Rows.Count; i++)
               {
                  Sage50ProviderModel entity = new Sage50ProviderModel();

                  entity.CODIGO = enentiesDataTable.Rows[i].ItemArray[0].ToString().Trim();
                  entity.CIF = enentiesDataTable.Rows[i].ItemArray[1].ToString().Trim();
                  entity.NOMBRE = enentiesDataTable.Rows[i].ItemArray[2].ToString().Trim();
                  entity.DIRECCION = enentiesDataTable.Rows[i].ItemArray[3].ToString().Trim();
                  entity.CODPOST = enentiesDataTable.Rows[i].ItemArray[4].ToString().Trim();
                  entity.POBLACION = enentiesDataTable.Rows[i].ItemArray[5].ToString().Trim();
                  entity.PROVINCIA = enentiesDataTable.Rows[i].ItemArray[6].ToString().Trim();
                  entity.PAIS = enentiesDataTable.Rows[i].ItemArray[7].ToString().Trim();
                  entity.GUID_ID = enentiesDataTable.Rows[i].ItemArray[8].ToString().Trim();

                  entities.Add(entity);
               };
            };
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
         };
      }


      public bool SynchronizationTableHasRegistries()
      {
         try
         {
            Connection.Open();

            List<Sage50ProviderModel> entities = new List<Sage50ProviderModel>();

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


      public SynchronizableProviderModel GenerateSynchronizableEntityFromSageEntity
      (
         Sage50ProviderModel sageEntity
      )
      {
         SynchronizableProviderModel synchronizableEntity = new SynchronizableProviderModel();

         synchronizableEntity.PAR_ID = -1;
         synchronizableEntity.PAR_SUBCTA_CONTABLE_2 = sageEntity.CODIGO;
         synchronizableEntity.PAR_NOMBRE = sageEntity.NOMBRE;
         synchronizableEntity.PAR_CIF_NIF = sageEntity.CIF;
         synchronizableEntity.PAR_DIRECCION_1 = sageEntity.DIRECCION;
         synchronizableEntity.PAR_CP_1 = sageEntity.CODPOST;
         synchronizableEntity.PAR_LOCALIDAD_1 = sageEntity.POBLACION;
         synchronizableEntity.PAR_PROVINCIA_1 = sageEntity.PROVINCIA;
         synchronizableEntity.PAR_PAIS_1 = sageEntity.PAIS;

         synchronizableEntity.S50_CODE = sageEntity.CODIGO;
         synchronizableEntity.S50_GUID_ID = sageEntity.GUID_ID;

         return synchronizableEntity;
      }


      public bool EntityIsAlreadyRegistered
      (
         Sage50ProviderModel entity
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
                  S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.GUID_ID);

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
         SynchronizableProviderModel entity
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
         SynchronizableProviderModel entity
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
                  ,PAR_SUBCTA_CONTABLE_2
                  ,PAR_NOMBRE
                  ,PAR_APELLIDO_1
                  ,PAR_APELLIDO_2
                  ,NOMBRE_COMPLETO
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
                  ,@PAR_SUBCTA_CONTABLE_2
                  ,@PAR_NOMBRE
                  ,@PAR_APELLIDO_1
                  ,@PAR_APELLIDO_2
                  ,@NOMBRE_COMPLETO
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
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2", entity.PAR_SUBCTA_CONTABLE_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE", entity.PAR_NOMBRE);
               command.Parameters.AddWithValue("@PAR_APELLIDO_1", entity.PAR_APELLIDO_1);
               command.Parameters.AddWithValue("@PAR_APELLIDO_2", entity.PAR_APELLIDO_2);
               command.Parameters.AddWithValue("@NOMBRE_COMPLETO", entity.NOMBRE_COMPLETO);
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
         SynchronizableProviderModel entity
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
                  S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

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
         IDataTableGenerator providersDataTableGenerator = new SyncrhonizationDataTableGenerator();
         DataTable = providersDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
      }

      public void PaintEntitiesOnDataSource()
      {
         ISynchronizableEntityPainter<SynchronizableProviderModel> entityPainter = new EntityPainter<SynchronizableProviderModel>();
         entityPainter.PaintEntityListOnDataTable(
            SynchronizableEntities,
            DataTable,
            TableSchema.ColumnsTuplesList
         );
      }
   }
}
