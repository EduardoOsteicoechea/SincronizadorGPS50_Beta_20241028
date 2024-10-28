using Infragistics.Designers.SqlEditor;
using sage.ew.db;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;

namespace SincronizadorGPS50
{
   public class ProvidersSynchronizer : IEntitySynchronizer<GestprojectProviderModel, Sage50ProviderModel>
   {
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<int> SelectedIds { get; set; }
      public string SelectIdsQueryConditionString { get; set; }
      public List<SynchronizableProviderModel> SynchronizableEntities { get; set; }

      public void Synchronize
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<int> selectedIdList
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchema;
            SelectedIds = selectedIdList;

            #region synchronizationActions

               SelectIdsQueryConditionString = GenerateSelectedIdsQueryConditionString();

               SynchronizableEntities = GetSelectedEntities();

               foreach(SynchronizableProviderModel entity in SynchronizableEntities)
               {
                  if(EntityWasAlreadyTransferred(entity) == false)
                  {
                     AppendNextAvailableIdOnPARTICIPANTEtableToEntity(entity);
                     RegisterEntityDataOnGestprojectPARTICIPANTEtable(entity);
                     RegisterEntityGestprojectIdOnPAR_TPAtable(entity);
                     RegisterEntityPARTICIPANTEtableIdOnSynchronizationTable(entity);
                     ComplementEntitySynchronizationTableData(entity);
                  }
                  else
                  {
                     MessageBox.Show($"El proveedor \"{entity.NOMBRE_COMPLETO}\" ya fue importado a Gestproject.");
                  }
               }

            #endregion synchronizationActions
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


      public string GenerateSelectedIdsQueryConditionString()
      {
         try
         {
            string selectedIds = "";

            foreach(string id in SelectedIds.Select(id => id.ToString()).ToList())
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


      public List<SynchronizableProviderModel> GetSelectedEntities()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  *
               FROM 
                  {TableSchema.TableName} 
               WHERE 
                  ID 
               IN ({SelectIdsQueryConditionString})
            ;";

            List<SynchronizableProviderModel> entities = new List<SynchronizableProviderModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  
                  while(reader.Read())
                  {
                     SynchronizableProviderModel entity = new SynchronizableProviderModel();

                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;

                     entity.PAR_SUBCTA_CONTABLE_2 = reader["PAR_SUBCTA_CONTABLE_2"] as string;
                     entity.PAR_NOMBRE = reader["PAR_NOMBRE"] as string;
                     entity.PAR_APELLIDO_1 = reader["PAR_APELLIDO_1"] as string;
                     entity.PAR_APELLIDO_2 = reader["PAR_APELLIDO_2"] as string;
                     entity.NOMBRE_COMPLETO = reader["NOMBRE_COMPLETO"] as string;
                     entity.PAR_NOMBRE_COMERCIAL = reader["PAR_NOMBRE_COMERCIAL"] as string;
                     entity.PAR_CIF_NIF = reader["PAR_CIF_NIF"] as string;
                     entity.PAR_DIRECCION_1 = reader["PAR_DIRECCION_1"] as string;
                     entity.PAR_CP_1 = reader["PAR_CP_1"] as string;
                     entity.PAR_LOCALIDAD_1 = reader["PAR_LOCALIDAD_1"] as string;
                     entity.PAR_PROVINCIA_1 = reader["PAR_PROVINCIA_1"] as string;
                     entity.PAR_PAIS_1 = reader["PAR_PAIS_1"] as string;
                     entity.S50_CODE = reader["S50_CODE"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;

                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;

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


      public bool EntityWasAlreadyTransferred
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  S50_COMPANY_GROUP_GUID_ID
               FROM 
                  {TableSchema.TableName} 
               WHERE 
                  ID=@ID
            ;";

            List<SynchronizableProviderModel> entities = new List<SynchronizableProviderModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@ID", entity.ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     string entityCompanyGroupGuid = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     bool entityCompanyGroupGuidIsEmpty = entityCompanyGroupGuid == "";
                     bool entityCompanyGroupGuidIsDBNull = reader.GetValue(0).GetType() == typeof(System.DBNull);

                     if(entityCompanyGroupGuidIsEmpty || entityCompanyGroupGuidIsDBNull)
                     {
                        return false;
                     }
                     else
                     {
                        return true;
                     }
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


      public void AppendNextAvailableIdOnPARTICIPANTEtableToEntity
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  MAX(PAR_ID)
               FROM 
                  PARTICIPANTE
            ;";

            List<SynchronizableProviderModel> entities = new List<SynchronizableProviderModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int? currentMaximumIdValue = reader.GetValue(0) as int?;
                     entity.PAR_ID = currentMaximumIdValue + 1;
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


      public void RegisterEntityDataOnGestprojectPARTICIPANTEtable
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  PARTICIPANTE
               (
                  PAR_ID
                  ,PAR_SUBCTA_CONTABLE_2
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
               )
               VALUES
               (
                  @PAR_ID
                  ,@PAR_SUBCTA_CONTABLE_2
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
               )
            ;";

            List<SynchronizableProviderModel> entities = new List<SynchronizableProviderModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2", entity.PAR_SUBCTA_CONTABLE_2);
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


      public void RegisterEntityGestprojectIdOnPAR_TPAtable
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  PAR_TPA
               (
                  TPA_ID
                  ,PAR_ID
               )
               VALUES
               (
                  @TPA_ID
                  ,@PAR_ID
               )
            ;";

            List<SynchronizableProviderModel> entities = new List<SynchronizableProviderModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@TPA_ID", 12);
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);

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


      public void RegisterEntityPARTICIPANTEtableIdOnSynchronizationTable
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               UPDATE
                  {TableSchema.TableName}
               SET
                  PAR_ID=@PAR_ID
               WHERE
                  ID=@ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@ID", entity.ID);

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


      public void ComplementEntitySynchronizationTableData
      (
         SynchronizableProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               UPDATE
                  {TableSchema.TableName}
               SET
                  SYNC_STATUS=@SYNC_STATUS
                  ,S50_CODE=@S50_CODE
                  ,S50_GUID_ID=@S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME=@S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE=@S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE=@S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID=@S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE=@LAST_UPDATE
                  ,GP_USU_ID=@GP_USU_ID
               WHERE
                  ID=@ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Transferido;

               entity.S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
               entity.S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
               entity.S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
               entity.S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
               entity.GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;

               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entity.S50_COMPANY_GROUP_NAME);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entity.S50_COMPANY_GROUP_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entity.S50_COMPANY_GROUP_MAIN_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entity.S50_COMPANY_GROUP_GUID_ID);
               command.Parameters.AddWithValue("@LAST_UPDATE", entity.LAST_UPDATE);
               command.Parameters.AddWithValue("@GP_USU_ID", entity.GP_USU_ID);

               command.Parameters.AddWithValue("@ID", entity.ID);

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


























   }
}
