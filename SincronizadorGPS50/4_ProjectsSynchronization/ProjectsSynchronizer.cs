using Infragistics.Designers.SqlEditor;
using sage.ew.cliente;
using sage.ew.db;
using Sage.ES.S50.Modelos;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using SincronizadorGPS50.Workflows.Sage50Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static Sage.ES.S50.S50Update.Classes.S50UpdateLog;

namespace SincronizadorGPS50
{
   public class ProjectsSynchronizer : IEntitySynchronizer<SynchronizableProjectModel, Sage50ProjectModel>
   {
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public System.Data.SqlClient.SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<int> SelectedIds { get; set; }
      public List<SynchronizableProjectModel> SynchronizableEntities { get; set; }

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
            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchema;
            SelectedIds = selectedIdList;
            SynchronizableEntities = new List<SynchronizableProjectModel>();

            GetSelectedEntities();

            foreach(SynchronizableProjectModel entity in SynchronizableEntities)
            {
               if(ValidateIfEntityWasTransferred(entity) == false)
               {
                  string newEntityCode = GetNextAvailableSageEntityCode();

                  TransferEntity(entity, newEntityCode);
                  RegisterTransferredEntitySageData(entity);
               }
               else
               {
                  MessageBox.Show($"El proyecto \"{entity.PRY_NOMBRE}\" ya se había transferido a Sage50");
               };
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
         };
      }

      public void GetSelectedEntities()
      {
         try
         {
            Connection.Open();

            foreach(int id in SelectedIds)
            {
               string sqlString = $@"
               SELECT 
                  *
               FROM
                  {TableSchema.TableName}
               WHERE
                  PRY_ID=@PRY_ID
               ";

               using(SqlCommand command = new SqlCommand(sqlString, Connection))
               {
                  command.Parameters.AddWithValue("@PRY_ID", id);

                  using(SqlDataReader reader = command.ExecuteReader())
                  {
                     while(reader.Read())
                     {
                        SynchronizableProjectModel entity = new SynchronizableProjectModel();

                        entity.ID = reader["ID"] as int?;
                        entity.SYNC_STATUS = (reader["SYNC_STATUS"] as string) ?? "";
                        entity.PRY_ID = reader["PRY_ID"] as int?;
                        entity.PRY_CODIGO = (reader["PRY_CODIGO"] as string) ?? "";
                        entity.PRY_NOMBRE = (reader["PRY_NOMBRE"] as string) ?? "";
                        entity.PRY_DIRECCION = (reader["PRY_DIRECCION"] as string) ?? "";
                        entity.PRY_LOCALIDAD = (reader["PRY_LOCALIDAD"] as string) ?? "";
                        entity.PRY_PROVINCIA = (reader["PRY_PROVINCIA"] as string) ?? "";
                        entity.PRY_CP = (reader["PRY_CP"] as string) ?? "";
                        entity.S50_CODE = (reader["S50_CODE"] as string) ?? "";
                        entity.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                        entity.S50_COMPANY_GROUP_NAME = (reader["S50_COMPANY_GROUP_NAME"] as string) ?? "";
                        entity.S50_COMPANY_GROUP_CODE = (reader["S50_COMPANY_GROUP_CODE"] as string) ?? "";
                        entity.S50_COMPANY_GROUP_MAIN_CODE = (reader["S50_COMPANY_GROUP_MAIN_CODE"] as string) ?? "";
                        entity.S50_COMPANY_GROUP_GUID_ID = (reader["S50_COMPANY_GROUP_GUID_ID"] as string) ?? "";
                        entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                        entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                        entity.COMMENTS = (reader["COMMENTS"] as string) ?? "";

                        SynchronizableEntities.Add(entity);
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

      public int? GetProjectCustomerId
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  PAR_ID_EMPRESA
               FROM
                  PROYECTO
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
                     int? participantId = (reader["PAR_ID_EMPRESA"] as int?) ?? -1;
                     return participantId;
                  }
               }
            }

            return -1;
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

      public string GetProjectCustomerCodeById
      (
         int? id
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
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_ID", id);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     string customerCode = (reader["S50_CODE"] as string) ?? "";
                     return customerCode;
                  }
               }
            }

            return "";
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


      public bool ValidateIfEntityWasTransferred
      (
         SynchronizableProjectModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT 
               S50_GUID_ID
            FROM
               {TableSchema.TableName}
            WHERE
               ID=@ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@ID", entity.ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     string entityGuid = reader["S50_GUID_ID"] as string;

                     if(entityGuid == "")
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

            return true;
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


      public string GetNextAvailableSageEntityCode()
      {
         try
         {
            string getNewEntityCode = $@" SELECT MAX(CODIGO) FROM {DB.SQLDatabase("comunes","obra")};";

            DataTable sageEntityDataTable = new DataTable();

            DB.SQLExec(getNewEntityCode, ref sageEntityDataTable);

            string newSageEntityCode = "";

            if(sageEntityDataTable.Rows.Count > 0)
            {
               newSageEntityCode = (Convert.ToInt32(sageEntityDataTable.Rows[0].ItemArray[0]) + 1).ToString();
            }
            else
            {
               newSageEntityCode = "1";
            };

            newSageEntityCode = newSageEntityCode.PadLeft(5, '0');

            return newSageEntityCode;
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

      public void TransferEntity
      (
         SynchronizableProjectModel entity, string newSageEntityCode
      )
      {
         try
         {
            //Obra sageEntity = new sage.ew.cliente.Obra();
            //sageEntity._Codigo = newSageEntityCode ?? "";
            //sageEntity._Nombre = entity.PRY_NOMBRE ?? "";
            //sageEntity._Direccion = entity.PRY_DIRECCION ?? "";
            //sageEntity._Codpost = entity.PRY_CP ?? "";
            //sageEntity._Poblacion = entity.PRY_LOCALIDAD ?? "";
            //sageEntity._Provincia = entity.PRY_PROVINCIA ?? "";

            ProjectEntityForSageCreation projectEntityForSageCreation = new ProjectEntityForSageCreation();
            projectEntityForSageCreation.codigo = newSageEntityCode ?? "";
            projectEntityForSageCreation.nombre = entity.PRY_NOMBRE ?? "";
            projectEntityForSageCreation.direccion = entity.PRY_DIRECCION ?? "";
            projectEntityForSageCreation.poblacion = entity.PRY_LOCALIDAD ?? "";
            projectEntityForSageCreation.provincia = entity.PRY_PROVINCIA ?? "";
            projectEntityForSageCreation.codpos = entity.PRY_CP ?? "";
            int? customerId = GetProjectCustomerId(entity);
            projectEntityForSageCreation.cliente = GetProjectCustomerCodeById(customerId);

            SageProjectBussinessClass sageProjectBussinessClass = new SageProjectBussinessClass();
            sageProjectBussinessClass._Create(projectEntityForSageCreation);

            //if(sageEntity._Save())
            if(sageProjectBussinessClass._Create(projectEntityForSageCreation))
            {
               //entity.S50_CODE = sageEntity._Codigo;
               //entity.S50_GUID_ID = sageEntity._Guid_Id;
               entity.S50_CODE = sageProjectBussinessClass._oObra._Codigo;
               entity.S50_GUID_ID = sageProjectBussinessClass._oObra._Guid_Id;
            }
            else
            {
               if(sageProjectBussinessClass._Error_Message != "")
               {
                  MessageBox.Show(sageProjectBussinessClass._Error_Message);
               }
               MessageBox.Show($"No pudimos transferir la obra \"{entity.PRY_NOMBRE}\"");
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


      public bool RegisterTransferredEntitySageData
      (
         SynchronizableProjectModel entity
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
               ,GP_USU_ID=@GP_USU_ID
               ,COMMENTS=@COMMENTS
            WHERE
               ID=@ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Transferido;
               entity.S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
               entity.S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
               entity.S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
               entity.S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
               entity.GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;

               command.Parameters.AddWithValue("@ID", entity.ID);
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entity.S50_COMPANY_GROUP_NAME);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entity.S50_COMPANY_GROUP_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entity.S50_COMPANY_GROUP_MAIN_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entity.S50_COMPANY_GROUP_GUID_ID);
               command.Parameters.AddWithValue("@GP_USU_ID", entity.GP_USU_ID);
               command.Parameters.AddWithValue("@COMMENTS", entity.COMMENTS);

               command.ExecuteNonQuery();
            }

            return true;
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
