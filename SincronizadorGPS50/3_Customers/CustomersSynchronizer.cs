using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class CustomersSynchronizer : IEntitySynchronizer<SynchronizableCustomerModel, SageCustomerModel>
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
      public SageCustomer SageEntity { get; set; }
      public List<GestprojectCustomerModel> GestprojectEntities { get; set; }
      public SynchronizableCustomerModel SynchronizableEntity { get; set; }
      public List<SynchronizableCustomerModel> SynchronizableEntities { get; set; }
      public DataTable DataTable { get; set; } = new DataTable();
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; } = null;
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<int> SelectedIds { get; set; }
      public string SelectIdsQueryConditionString { get; set; }


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

            foreach(SynchronizableCustomerModel entity in SynchronizableEntities)
            {
               if(EntityWasAlreadyTransferred(entity) == false)
               {
                  GetSageEntityNextAvailableCode(entity);
                  SageEntity = CreateSageEntity(entity);
                  GetSageEntityRelevantData(entity);
                  UpdateEntityRelevantDataOnSynchronizationTable(entity);
                  UpdateEntityRelevantDataOnGestproject(entity);
                  ComplementEntitySynchronizationTableData(entity);
               }
               else
               {
                  MessageBox.Show($"{EntityTypeNameSingularArticle} {EntityTypeNameRoot + EntityTypeNameGender} \"{entity.NOMBRE_COMPLETO}\" ya fue transferido a Sage50.");
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

      public List<SynchronizableCustomerModel> GetSelectedEntities()
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

            List<SynchronizableCustomerModel> entities = new List<SynchronizableCustomerModel>();

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  
                  while(reader.Read())
                  {
                     SynchronizableCustomerModel entity = new SynchronizableCustomerModel();

                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;

                     entity.PAR_SUBCTA_CONTABLE = reader["PAR_SUBCTA_CONTABLE"] as string;
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
         SynchronizableCustomerModel entity
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

            List<SynchronizableCustomerModel> entities = new List<SynchronizableCustomerModel>();

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

      public void GetSageEntityNextAvailableCode
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            string getNewEntityCode = $@"SELECT MAX(CODIGO) FROM {DB.SQLDatabase("gestion","clientes")} WHERE CODIGO LIKE '43%';";
            DataTable sageEntityDataTable = new DataTable();
            DB.SQLExec(getNewEntityCode, ref sageEntityDataTable);

            if(sageEntityDataTable.Rows.Count > 0)
            {
               entity.S50_CODE = (Convert.ToInt32(sageEntityDataTable.Rows[0].ItemArray[0]) + 1).ToString();
            }
            else
            {
               entity.S50_CODE = "1";
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

      public SageCustomer CreateSageEntity
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            SageCustomer sageEntity = new SageCustomer();
            clsEntityCustomerModel clsEntityCustomerInstance = new clsEntityCustomerModel();

            clsEntityCustomerInstance.codigo = entity.S50_CODE.Trim();
            clsEntityCustomerInstance.pais = entity.PAR_PAIS_1.Trim();
            clsEntityCustomerInstance.nombre = entity.NOMBRE_COMPLETO.Trim();
            clsEntityCustomerInstance.codpos = entity.PAR_CP_1.Trim();
            clsEntityCustomerInstance.cif = entity.PAR_CIF_NIF.Trim();
            clsEntityCustomerInstance.direccion = entity.PAR_DIRECCION_1.Trim();
            clsEntityCustomerInstance.provincia = entity.PAR_PROVINCIA_1.Trim();

            if(sageEntity._Create(clsEntityCustomerInstance))
            {
               return sageEntity;
            }

            throw new Exception($"Error en la creación del cliente {entity.NOMBRE_COMPLETO}");
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

      public void GetSageEntityRelevantData
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            entity.PAR_SUBCTA_CONTABLE = SageEntity._oCliente._Codigo;
            entity.S50_CODE = SageEntity._oCliente._Codigo;
            entity.S50_GUID_ID = SageEntity._oCliente._Guid_Id;
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

      public void UpdateEntityRelevantDataOnSynchronizationTable
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            UPDATE
               {TableSchema.TableName}
            SET
               PAR_SUBCTA_CONTABLE=@PAR_SUBCTA_CONTABLE
               ,S50_CODE=@S50_CODE
               ,S50_GUID_ID=@S50_GUID_ID
            WHERE
               ID=@ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE", entity.PAR_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
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

      public void UpdateEntityRelevantDataOnGestproject
      (
         SynchronizableCustomerModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            UPDATE
               {GestprojectEntityDatabaseName}
            SET
               PAR_SUBCTA_CONTABLE=@PAR_SUBCTA_CONTABLE
            WHERE
               PAR_ID=@PAR_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE", entity.PAR_SUBCTA_CONTABLE);
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

      public void ComplementEntitySynchronizationTableData
      (
         SynchronizableCustomerModel entity
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
