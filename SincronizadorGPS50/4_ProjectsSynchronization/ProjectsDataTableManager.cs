using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class ProjectsDataTableManager : IGridDataSourceGenerator<SynchronizableProjectModel, Sage50ProjectModel>
   {
      public List<SynchronizableProjectModel> SynchronizableProjects { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public System.Data.SqlClient.SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public DataTable DataTable { get; set; }

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
            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchemaProvider;
            SynchronizableProjects = new List<SynchronizableProjectModel>();

            ManageSynchronizationTableStatus(TableSchema);

            GetGestprojectEntities();

            if(ValidateIfEntityExistsOnSynchronizationTable() == true)
            {
               AppendSynchronizationTableData();
            }
            else
            {
               RegisterEntityOnSynchronizationTable();
               AppendSynchronizationTableData();
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

      public void GetGestprojectEntities()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM PROYECTO)
            BEGIN
               SELECT
                  PRY_ID
                  ,PRY_CODIGO
                  ,PRY_NOMBRE
                  ,PRY_DIRECCION
                  ,PRY_LOCALIDAD
                  ,PRY_PROVINCIA
                  ,PRY_CP
               FROM
                  PROYECTO
            END
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableProjectModel entity = new SynchronizableProjectModel();

                     entity.PRY_ID = reader["PRY_ID"] as int?;
                     entity.PRY_CODIGO = reader["PRY_CODIGO"] as string;
                     entity.PRY_NOMBRE = reader["PRY_NOMBRE"] as string;
                     entity.PRY_DIRECCION = reader["PRY_DIRECCION"] as string;
                     entity.PRY_LOCALIDAD = reader["PRY_LOCALIDAD"] as string;
                     entity.PRY_PROVINCIA = reader["PRY_PROVINCIA"] as string;
                     entity.PRY_CP = reader["PRY_CP"] as string;

                     SynchronizableProjects.Add(entity);
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


      public bool ValidateIfEntityExistsOnSynchronizationTable()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT
                  *
               FROM
                  {TableSchema.TableName}
            END
            ";

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


      public void AppendSynchronizationTableData()
      {
         try
         {
            Connection.Open();

            foreach(SynchronizableProjectModel entity in SynchronizableProjects)
            {
               string sqlString = $@"
               IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
               BEGIN
                  SELECT
                     ID
                     ,SYNC_STATUS
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
                     PRY_ID=@PRY_ID
               END
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.Parameters.AddWithValue("@PRY_ID",entity.PRY_ID);

                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {
                        entity.ID = reader["ID"] as int?;
                        entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
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


      public void RegisterEntityOnSynchronizationTable()
      {
         
         try
         {
            Connection.Open();

            foreach(SynchronizableProjectModel entity in SynchronizableProjects)
            {
               string sqlString = $@"
               INSERT INTO
                  {TableSchema.TableName}
               (
                  SYNC_STATUS
                  ,PRY_ID
                  ,PRY_CODIGO
                  ,PRY_NOMBRE
                  ,PRY_DIRECCION
                  ,PRY_LOCALIDAD
                  ,PRY_PROVINCIA
                  ,PRY_CP
                  ,S50_CODE
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE
                  ,GP_USU_ID
                  ,COMMENTS
               )
               VALUES
               (
                  @SYNC_STATUS
                  ,@PRY_ID
                  ,@PRY_CODIGO
                  ,@PRY_NOMBRE
                  ,@PRY_DIRECCION
                  ,@PRY_LOCALIDAD
                  ,@PRY_PROVINCIA
                  ,@PRY_CP
                  ,@S50_CODE
                  ,@S50_GUID_ID
                  ,@S50_COMPANY_GROUP_NAME
                  ,@S50_COMPANY_GROUP_CODE
                  ,@S50_COMPANY_GROUP_MAIN_CODE
                  ,@S50_COMPANY_GROUP_GUID_ID
                  ,@LAST_UPDATE
                  ,@GP_USU_ID
                  ,@COMMENTS               
               )
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;
                  
                  command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
                  command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);
                  command.Parameters.AddWithValue("@PRY_CODIGO", entity.PRY_CODIGO);
                  command.Parameters.AddWithValue("@PRY_NOMBRE", entity.PRY_NOMBRE);
                  command.Parameters.AddWithValue("@PRY_DIRECCION", entity.PRY_DIRECCION);
                  command.Parameters.AddWithValue("@PRY_LOCALIDAD", entity.PRY_LOCALIDAD);
                  command.Parameters.AddWithValue("@PRY_PROVINCIA", entity.PRY_PROVINCIA);
                  command.Parameters.AddWithValue("@PRY_CP", entity.PRY_CP);
                  command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
                  command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
                  command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entity.S50_COMPANY_GROUP_NAME);
                  command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entity.S50_COMPANY_GROUP_CODE);
                  command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entity.S50_COMPANY_GROUP_MAIN_CODE);
                  command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entity.S50_COMPANY_GROUP_GUID_ID);
                  command.Parameters.AddWithValue("@LAST_UPDATE", entity.LAST_UPDATE);
                  command.Parameters.AddWithValue("@GP_USU_ID", entity.GP_USU_ID);
                  command.Parameters.AddWithValue("@COMMENTS", entity.COMMENTS);

                  command.ExecuteNonQuery();
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
         ISynchronizableEntityPainter<SynchronizableProjectModel> entityPainter = new EntityPainter<SynchronizableProjectModel>();
         entityPainter.PaintEntityListOnDataTable(
            SynchronizableProjects,
            DataTable,
            TableSchema.ColumnsTuplesList
         );
      }
   }
}
