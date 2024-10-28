using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class TaxesSynchronizer : IEntitySynchronizer<GestprojectTaxModel, Sage50TaxModel>
   {
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<int> SelectedIds { get; set; }
      public string SelectIdsQueryConditionString { get; set; }
      public List<SynchronizableTaxModel> SynchronizableEntities { get; set; }

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

            SelectIdsQueryConditionString = GenerateSelectedIdsQueryConditionString();

            SynchronizableEntities = GetSelectedEntities();

            foreach(SynchronizableTaxModel entity in SynchronizableEntities)
            {
               if(EntityWasAlreadyTransferred(entity) == false)
               {
                  AppendNextAvailableIdOnIMPUESTO_CONFIGtableToEntity(entity);
                  RegisterEntityDataOnGestprojectIMPUESTO_CONFIGtable(entity);
                  RegisterEntityPARTICIPANTEtableIdOnSynchronizationTable(entity);
                  ComplementEntitySynchronizationTableData(entity);
               }
               else
               {
                  MessageBox.Show($"El impuesto \"{entity.IMP_NOMBRE}\" ya fue importado a Gestproject.");
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


      public List<SynchronizableTaxModel> GetSelectedEntities()
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

            List<SynchronizableTaxModel> entities = new List<SynchronizableTaxModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  
                  while(reader.Read())
                  {
                     SynchronizableTaxModel entity = new SynchronizableTaxModel();

                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;

                     entity.IMP_ID = reader["IMP_ID"] as int?;
                     entity.IMP_TIPO = reader["IMP_TIPO"] as string;
                     entity.IMP_NOMBRE = reader["IMP_NOMBRE"] as string;
                     entity.IMP_DESCRIPCION = reader["IMP_DESCRIPCION"] as string;
                     entity.IMP_VALOR = reader["IMP_VALOR"] as decimal?;
                     entity.IMP_SUBCTA_CONTABLE = reader["IMP_SUBCTA_CONTABLE"] as string;
                     entity.IMP_SUBCTA_CONTABLE_2 = reader["IMP_SUBCTA_CONTABLE_2"] as string;

                     entity.S50_CODE = reader["S50_CODE"] as string;
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
         SynchronizableTaxModel entity
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

            List<SynchronizableTaxModel> entities = new List<SynchronizableTaxModel>();

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


      public void AppendNextAvailableIdOnIMPUESTO_CONFIGtableToEntity
      (
         SynchronizableTaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  MAX(IMP_ID)
               FROM 
                  IMPUESTO_CONFIG
            ;";

            List<SynchronizableTaxModel> entities = new List<SynchronizableTaxModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     bool valueIsDBNull = reader.GetValue(0).GetType() == typeof(DBNull);
                     int? currentMaximumIdValue = reader.GetValue(0) as int?;

                     if(valueIsDBNull)
                     {
                        entity.IMP_ID = 1;
                     }
                     else
                     {
                        entity.IMP_ID = currentMaximumIdValue + 1;
                     }

                     return;
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


      public void RegisterEntityDataOnGestprojectIMPUESTO_CONFIGtable
      (
         SynchronizableTaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  IMPUESTO_CONFIG
               (
                  IMP_ID
                  ,IMP_TIPO
                  ,IMP_NOMBRE
                  ,IMP_DESCRIPCION
                  ,IMP_VALOR
                  ,IMP_SUBCTA_CONTABLE
                  ,IMP_SUBCTA_CONTABLE_2
               )
               VALUES
               (
                  @IMP_ID
                  ,@IMP_TIPO
                  ,@IMP_NOMBRE
                  ,@IMP_DESCRIPCION
                  ,@IMP_VALOR
                  ,@IMP_SUBCTA_CONTABLE
                  ,@IMP_SUBCTA_CONTABLE_2
               )
            ;";

            List<SynchronizableTaxModel> entities = new List<SynchronizableTaxModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@IMP_ID", entity.IMP_ID);
               command.Parameters.AddWithValue("@IMP_TIPO", entity.IMP_TIPO);
               command.Parameters.AddWithValue("@IMP_NOMBRE", entity.IMP_NOMBRE);
               command.Parameters.AddWithValue("@IMP_DESCRIPCION", entity.IMP_DESCRIPCION);
               command.Parameters.AddWithValue("@IMP_VALOR", entity.IMP_VALOR);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE", entity.IMP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE_2", entity.IMP_SUBCTA_CONTABLE_2);

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
         SynchronizableTaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               UPDATE
                  {TableSchema.TableName}
               SET
                  IMP_ID=@IMP_ID
               WHERE
                  ID=@ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@IMP_ID", entity.IMP_ID);
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
         SynchronizableTaxModel entity
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
