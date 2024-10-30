using Infragistics.Designers.SqlEditor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ProjectsDataTableManager : IGridDataSourceGenerator<SynchronizableProjectModel, Sage50ProjectModel>
   {
      public string EntityTypeNameSingularArticle { get; set; } = "el";
      public string EntityTypeNamePluralArticle { get; set; } = "los";
      public string EntityTypeNameRoot { get; set; } = "proyect";
      public string EntityTypeNameGender { get; set; } = "o";
      public string EntityTypeNamePlural { get; set; } = "s";
      public List<SynchronizableProjectModel> SynchronizableEntities { get; set; }
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
            SynchronizableEntities = new List<SynchronizableProjectModel>();

            ManageSynchronizationTableStatus(TableSchema);

            if(GestprojectEntityTableHasEntities())
            {
               GetGestprojectEntities();
               foreach(SynchronizableProjectModel entity in SynchronizableEntities)
               {
                  if(SynchronizationTableHasRegistries())
                  {
                     if(ValidateIfEntityExistsOnSynchronizationTable(entity) == false)
                        ExecuteUnregisteredEntityWorkflow(entity);
                  }
                  else
                     ExecuteUnregisteredEntityWorkflow(entity);

                  ExecuteRegisteredEntityWorkflow(entity);
               }
            }
            else
               MessageBox.Show($"No encontramos {EntityTypeNameRoot + EntityTypeNameGender + EntityTypeNamePlural} en Gestproject.");

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
         try
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

      public bool GestprojectEntityTableHasEntities()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM PROYECTO)
            BEGIN
               SELECT
                  PRY_ID
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

      public bool SynchronizationTableHasRegistries()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT
                  ID
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

      public void GetGestprojectEntities()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
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
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableProjectModel entity = new SynchronizableProjectModel();

                     entity.PRY_ID = (reader["PRY_ID"] as int?) ?? -1;
                     entity.PRY_CODIGO = (reader["PRY_CODIGO"] as string) ?? "";
                     entity.PRY_NOMBRE = (reader["PRY_NOMBRE"] as string) ?? "";
                     entity.PRY_DIRECCION = (reader["PRY_DIRECCION"] as string) ?? "";
                     entity.PRY_LOCALIDAD = (reader["PRY_LOCALIDAD"] as string) ?? "";
                     entity.PRY_PROVINCIA = (reader["PRY_PROVINCIA"] as string) ?? "";
                     entity.PRY_CP = (reader["PRY_CP"] as string) ?? "";

                     SynchronizableEntities.Add(entity);
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

      public bool ValidateIfEntityExistsOnSynchronizationTable
      (
         SynchronizableProjectModel entity
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
               PRY_ID=@PRY_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);

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

      public void ExecuteUnregisteredEntityWorkflow
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            AppendGestprojectClientIdOnGestprojectToEntity(entity);

            if(entity.PAR_ID == -1)
               entity.ProjectClientSageCode = "";
            else
               AppendClientSageCodeToEntity(entity);

            RegisterEntityOnSynchronizationTable(entity);
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

      public void ExecuteRegisteredEntityWorkflow
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            AppendSynchronizationTableData(entity);
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

      public void AppendGestprojectClientIdOnGestprojectToEntity
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               PAR_ID
            FROM
               PRY_PAR_CLI
            WHERE
               PRY_ID=@PRY_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int? clientId = (reader["PAR_ID"] as int?) ?? -1;

                     bool clientIdIsDBNull = reader["PAR_ID"].GetType() == typeof(System.DBNull);

                     if(clientIdIsDBNull)
                        throw new NullReferenceException("El id del cliente en la tabla \"PRY_PAR_CLI\" era nulo.");

                     entity.PAR_ID = clientId;
                     break;
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

      public void AppendClientSageCodeToEntity
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               S50_CODE
            FROM
               INT_SAGE_SINC_CLIENTES
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
                     string sageGuid = (reader["S50_CODE"] as string) ?? "";

                     bool sageGuidIsEmpty = sageGuid == "";
                     bool sageGuidIsDBNull = reader["S50_CODE"].GetType() == typeof(System.DBNull);

                     if(sageGuidIsEmpty)
                        throw new Exception("El guid del cliente en la tabla de sincronización de clientes era una cadena de texto vacía.");

                     if(sageGuidIsDBNull)
                        throw new NullReferenceException("El guid del cliente en la tabla de sincronización de clientes era nulo.");

                     entity.ProjectClientSageCode = sageGuid;
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

      public void RegisterEntityOnSynchronizationTable
      (
         SynchronizableProjectModel entity
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
                  ,PRY_ID
                  ,PRY_CODIGO
                  ,PRY_NOMBRE
                  ,PRY_DIRECCION
                  ,PRY_LOCALIDAD
                  ,PRY_PROVINCIA
                  ,PRY_CP
                  ,PAR_ID
                  ,ProjectClientSageCode
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
                  ,@PAR_ID
                  ,@ProjectClientSageCode
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
               ;";

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
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@ProjectClientSageCode", entity.ProjectClientSageCode);
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

      public void AppendSynchronizationTableData
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ID
               ,SYNC_STATUS
               ,SYNC_STATUS
               ,PAR_ID
               ,ProjectClientSageCode
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
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.PAR_ID = reader["PAR_ID"] as int?;
                     entity.ProjectClientSageCode = reader["ProjectClientSageCode"] as string;
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
            ISynchronizableEntityPainter<SynchronizableProjectModel> entityPainter = new EntityPainter<SynchronizableProjectModel>();
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
