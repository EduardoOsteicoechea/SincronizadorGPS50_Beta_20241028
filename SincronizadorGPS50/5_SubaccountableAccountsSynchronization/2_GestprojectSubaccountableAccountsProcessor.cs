using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class GestprojectSubaccountableAccountsProcessor : ISynchronizableEntityProcessor<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>
   {
      public List<GestprojectSubaccountableAccountModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;
      public bool NevesWasSynchronized { get; set; } = false;

      public List<GestprojectSubaccountableAccountModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectSubaccountableAccountModel> gestprojectEntites,
         List<Sage50SubaccountableAccountModel> sage50Entities
      )
      {
         try
         {
            ProcessedEntities = new List<GestprojectSubaccountableAccountModel>();

            for(int i = 0; i < gestprojectEntites.Count; i++)
            {
               GestprojectSubaccountableAccountModel entity = gestprojectEntites[i];

               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);

               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  //MessageBox.Show(entity.COS_NOMBRE + " MustBeSkipped");
                  continue;
               }
               else if(MustBeRegistered)
               {
                  //MessageBox.Show(entity.COS_NOMBRE + " MustBeRegistered");
                  RegisterEntity(connection, tableSchema, entity);
                  AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               }
               else if(MustBeUpdated)
               {
                  //MessageBox.Show(entity.COS_NOMBRE + " MustBeUpdated");
                  UpdateEntity(connection, tableSchema, entity);
               };

               ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);

               if(MustBeDeleted)
               {
                  DeleteEntity(connection, tableSchema, gestprojectEntites, entity);
                  ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
                  RegisterEntity(connection, tableSchema, entity);
                  AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
                  ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);
               };

               UpdateEntity(connection, tableSchema, entity);

               ProcessedEntities.Add(entity);
            };

            return ProcessedEntities;
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


      public void AppendSynchronizationTableDataToEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            bool isASage50Entity = entity.COS_ID == -1;
            bool isAGestprojectEntity = !isASage50Entity;

            connection.Open();
            string sqlString = "";
            string sqlStringColumns = $@"
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
            ";

            if(isASage50Entity == true)
            {
               sqlString = $@"
                  SELECT
                     {sqlStringColumns}
                  FROM
                     {tableSchema.TableName}
                  WHERE
                     COS_NOMBRE=@COS_NOMBRE
                  AND
                     COS_CODIGO=@COS_CODIGO
               ;";               
            }
            else
            {
               sqlString = $@"
                  SELECT
                     {sqlStringColumns}
                  FROM
                     {tableSchema.TableName}
                  WHERE
                     COS_ID=@COS_ID
               ;";   
            };

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE);
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO);

               if(isAGestprojectEntity == true)
               {
                  command.Parameters.AddWithValue("@COS_ID", entity.COS_ID);
               };

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = Convert.ToInt32(
                        reader.GetValue(0).GetType() != typeof(DBNull) ? reader.GetValue(0) : 0
                     );
                     entity.SYNC_STATUS = Convert.ToString(
                        reader.GetValue(1).GetType() != typeof(DBNull) ? reader.GetValue(1) : ""
                     );
                     entity.S50_CODE = Convert.ToString(
                        reader.GetValue(2).GetType() != typeof(DBNull) ? reader.GetValue(2) : ""
                     );
                     entity.S50_GUID_ID = Convert.ToString(
                        reader.GetValue(3).GetType() != typeof(DBNull) ? reader.GetValue(3) : ""
                     );
                     entity.S50_COMPANY_GROUP_NAME = Convert.ToString(
                        reader.GetValue(4).GetType() != typeof(DBNull) ? reader.GetValue(4) : ""
                     );
                     entity.S50_COMPANY_GROUP_CODE = Convert.ToString(
                        reader.GetValue(5).GetType() != typeof(DBNull) ? reader.GetValue(5) : ""
                     );
                     entity.S50_COMPANY_GROUP_MAIN_CODE = Convert.ToString(
                        reader.GetValue(6).GetType() != typeof(DBNull) ? reader.GetValue(6) : ""
                     );
                     entity.S50_COMPANY_GROUP_GUID_ID = Convert.ToString(
                        reader.GetValue(7).GetType() != typeof(DBNull) ? reader.GetValue(7) : ""
                     );
                     entity.LAST_UPDATE = Convert.ToDateTime(
                        reader.GetValue(8).GetType() != typeof(DBNull) ? reader.GetValue(8) : DateTime.Now
                     );
                     entity.GP_USU_ID = Convert.ToInt32(
                        reader.GetValue(9).GetType() != typeof(DBNull) ? reader.GetValue(9) : 0
                     );
                     entity.COMMENTS = Convert.ToString(
                        reader.GetValue(10).GetType() != typeof(DBNull) ? reader.GetValue(10) : ""
                     );
                  };
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
         }
         finally
         {
            connection.Close();
         };
      }

      public void DetermineEntityWorkflow
      (
         SqlConnection connection, 
         ISage50ConnectionManager sage50ConnectionManager, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            bool wasEntityRegistered = this.WasSubaccountableAccountRegistered(
               connection,
               tableSchema,
               entity
            );

            MustBeRegistered = wasEntityRegistered == false;

            bool registeredInDifferentCompanyGroup =
               entity.S50_COMPANY_GROUP_GUID_ID.Trim() != ""
                  &&
               sage50ConnectionManager.CompanyGroupData.CompanyGuidId.Trim() != entity.S50_COMPANY_GROUP_GUID_ID.Trim();

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";
            NevesWasSynchronized = neverSynchronized;

            bool synchronizedInThePast =
               entity.S50_COMPANY_GROUP_GUID_ID != ""
                  &&
               sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast || entity.S50_GUID_ID != "";
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


      public bool WasSubaccountableAccountRegistered
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         { 
            connection.Open();
                 
            string sqlString = $@"
               SELECT 
                  ID
               FROM 
                  {tableSchema.TableName}
               WHERE 
                  COS_NOMBRE=@COS_NOMBRE
               AND 
                  COS_CODIGO=@COS_CODIGO
               AND 
                  COS_GRUPO=@COS_GRUPO
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE);
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO);
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO);
               
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
            connection.Close();
         };
      }


      public void RegisterEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         { 
            connection.Open();

            string sqlString2 = $@"
            INSERT INTO 
               {tableSchema.TableName} 
               (
                  COS_ID
                  ,COS_CODIGO
                  ,COS_NOMBRE
                  ,COS_GRUPO
                  ,S50_CODE
                  ,S50_GUID_ID
               ) 
            VALUES 
               (
                  @COS_ID
                  ,@COS_CODIGO
                  ,@COS_NOMBRE
                  ,@COS_GRUPO
                  ,@S50_CODE
                  ,@S50_GUID_ID
               )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString2, connection))
            {  
               command.Parameters.AddWithValue("@COS_ID", entity.COS_ID);
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO);
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE);
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE ?? "");
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID ?? "");

               command.ExecuteNonQuery();
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
         finally
         {
            connection.Close();
         };
      }


      public void UpdateEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {            
         bool isASage50Entity = entity.COS_ID == -1;
         bool isAGestprojectEntity = !isASage50Entity;

         try
         {        
            connection.Open();
            
            string sqlString = "";

            if(isASage50Entity == true)
            {    
               sqlString = $@"
               UPDATE 
                  {tableSchema.TableName}
               SET
                  SYNC_STATUS=@SYNC_STATUS
                  ,COS_GRUPO=@COS_GRUPO
               WHERE
                  COS_CODIGO=@COS_CODIGO
               AND
                  COS_NOMBRE=@COS_NOMBRE
               ;";      
            }
            else
            {
               sqlString = $@"
               UPDATE 
                  {tableSchema.TableName}
               SET
                  SYNC_STATUS=@SYNC_STATUS
                  ,COS_CODIGO=@COS_CODIGO
                  ,COS_NOMBRE=@COS_NOMBRE
                  ,COS_GRUPO=@COS_GRUPO
               WHERE
                  COS_ID=@COS_ID
               ;";   
            }

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS ?? "");
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO ?? "");
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE ?? "");
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO ?? "");
               command.Parameters.AddWithValue("@COS_ID", entity.COS_ID);

               command.ExecuteNonQuery();
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
         finally
         {
            connection.Close();
         };
      }


      public void ValidateEntitySynchronizationStatus
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         List<Sage50SubaccountableAccountModel> sage50Entities, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            MustBeDeleted = new ValidateSubaccountableAccountSyncronizationStatus(
               entity,
               sage50Entities,
               tableSchema.GestprojectCode.ColumnDatabaseName,
               tableSchema.GestprojectName.ColumnDatabaseName,
               tableSchema.GestprojectGroup.ColumnDatabaseName,
               NevesWasSynchronized
            ).MustBeDeleted;
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


      public void DeleteEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         List<GestprojectSubaccountableAccountModel> gestprojectEntites, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            DELETE FROM 
               {tableSchema.TableName} 
            WHERE 
               COS_NOMBRE=@COS_NOMBRE
            AND
               COS_CODIGO=@COS_CODIGO
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE);
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO);

               command.ExecuteNonQuery();
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
         finally
         {
            connection.Close();
         };
      }


      public void ClearEntitySynchronizationData
      (
         GestprojectSubaccountableAccountModel entity, 
         List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList
      )
      {
         try
         {
            entity.ID = null;
            entity.SYNC_STATUS = "";

            entity.S50_CODE = "";
            entity.S50_GUID_ID = "";
            entity.S50_COMPANY_GROUP_NAME = "";
            entity.S50_COMPANY_GROUP_CODE = "";
            entity.S50_COMPANY_GROUP_MAIN_CODE = "";
            entity.S50_COMPANY_GROUP_GUID_ID = "";

            entity.LAST_UPDATE = DateTime.Now;
            entity.GP_USU_ID = null;
            entity.COMMENTS = "";
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
   }
}