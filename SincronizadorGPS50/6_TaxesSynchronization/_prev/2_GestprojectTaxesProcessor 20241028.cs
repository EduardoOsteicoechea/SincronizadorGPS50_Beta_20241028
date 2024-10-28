using SincronizadorGPS50.Workflows.Sage50Connection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectTaxesProcessor : ISynchronizableEntityProcessor<GestprojectTaxModel, Sage50TaxModel>
   {
      public List<GestprojectTaxModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;
      public bool NevesWasSynchronized { get; set; } = false;

      public List<GestprojectTaxModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectTaxModel> gestprojectEntites,
         List<Sage50TaxModel> sage50Entities
      )
      {
         try
         {
            //MessageBox.Show(
            //   "At: GestprojectTaxesProcessor\n.ProcessEntityList" + "\n\n" +
            //   "gestprojectEntites.Count: " + gestprojectEntites.Count + "\n" +
            //   "sage50Entities.Count: " + sage50Entities.Count
            //);

            //////////////////////////////////////
            // Here get's changed
            //////////////////////////////////////

            ProcessedEntities = new List<GestprojectTaxModel>();

            for(int i = 0; i < gestprojectEntites.Count; i++)
            {
               GestprojectTaxModel entity = gestprojectEntites[i];

               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);

               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  continue;
               }
               else if(MustBeRegistered)
               {
                  RegisterEntity(connection, tableSchema, entity);
                  AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               }
               else if(MustBeUpdated)
               {
                  UpdateEntity(connection, tableSchema, entity);
               };

               ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);

               if(MustBeDeleted)
               {
                  DeleteEntity(connection, tableSchema, gestprojectEntites, entity);
                  RegisterEntity(connection, tableSchema, entity);
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


      public void AppendSynchronizationTableDataToEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectTaxModel entity)
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               * 
            FROM 
               {tableSchema.TableName} 
            WHERE
               IMP_DESCRIPCION=@IMP_DESCRIPCION
   
            ";
               //      AND
               //S50_GUID_ID=@S50_GUID_ID

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@IMP_DESCRIPCION", entity.IMP_DESCRIPCION);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while (reader.Read()) 
                  {
                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.S50_CODE = reader["S50_CODE"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as System.DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;
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
      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, GestprojectTaxModel entity)
      {
         try
         {
            //new VisualizePropertiesAndValues<GestprojectTaxModel>(entity.IMP_NOMBRE, entity);

            MustBeRegistered = !new WasTaxRegistered(
               connection,
               tableSchema.TableName,
               tableSchema.GestprojectId.ColumnDatabaseName,
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID)
            //(tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            ).ItWas;
            //).ItWas && entity.S50_GUID_ID == "";

            bool registeredInDifferentCompanyGroup =
            entity.S50_COMPANY_GROUP_GUID_ID.Trim() != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId.Trim() != entity.S50_COMPANY_GROUP_GUID_ID.Trim();

            //MessageBox.Show(
            //"entity.S50_COMPANY_GROUP_GUID_ID: " + entity.S50_COMPANY_GROUP_GUID_ID + "\n\n" +
            //"sage50ConnectionManager.CompanyGroupData.CompanyGuidId: " + sage50ConnectionManager.CompanyGroupData.CompanyGuidId + "\n\n" +
            //"registeredInDifferentCompanyGroup: " + registeredInDifferentCompanyGroup
            //);

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";
            NevesWasSynchronized = neverSynchronized;

            bool synchronizedInThePast =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast || entity.IMP_SUBCTA_CONTABLE != "";

            //MessageBox.Show(
            //   "MustBeRegistered: " + MustBeRegistered + "\n" +
            //   "MustBeSkipped: " + MustBeSkipped + "\n" +
            //   "MustBeUpdated: " + MustBeUpdated + "\n"
            //);
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
      public void RegisterEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectTaxModel entity)
      {
         try
         {
            connection.Open();

            string sqlString2 = $@"
            INSERT INTO 
               {tableSchema.TableName} 
               (
                  IMP_ID
                  ,IMP_TIPO
                  ,IMP_NOMBRE
                  ,IMP_DESCRIPCION
                  ,IMP_VALOR
                  ,IMP_SUBCTA_CONTABLE
                  ,IMP_SUBCTA_CONTABLE_2
                  ,S50_CODE
                  ,S50_GUID_ID
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
                  ,@S50_CODE
                  ,@S50_GUID_ID
               )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString2, connection))
            {
               command.Parameters.AddWithValue("@IMP_ID", entity.IMP_ID);
               command.Parameters.AddWithValue("@IMP_TIPO", entity.IMP_TIPO);
               command.Parameters.AddWithValue("@IMP_NOMBRE", entity.IMP_NOMBRE);
               command.Parameters.AddWithValue("@IMP_DESCRIPCION", entity.IMP_DESCRIPCION);
               command.Parameters.AddWithValue("@IMP_VALOR", entity.IMP_VALOR);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE", entity.IMP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE_2", entity.IMP_SUBCTA_CONTABLE_2);
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


      public void UpdateEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectTaxModel entity)
      {
         try
         {
            new UpdateEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>()
               {
                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, entity.SYNC_STATUS),
                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID),
                  (tableSchema.GestprojectType.ColumnDatabaseName, entity.IMP_TIPO),
                  (tableSchema.GestprojectName.ColumnDatabaseName, entity.IMP_NOMBRE),
                  (tableSchema.GestprojectValue.ColumnDatabaseName, entity.IMP_VALOR),
                  (tableSchema.AccountableSubaccount.ColumnDatabaseName, entity.IMP_SUBCTA_CONTABLE),
                  //(tableSchema.Sage50Code.ColumnDatabaseName, entity.S50_CODE),
                  //(tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
                  //(tableSchema.CompanyGroupName.ColumnDatabaseName, entity.S50_COMPANY_GROUP_NAME),
                  //(tableSchema.CompanyGroupCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_CODE),
                  //(tableSchema.CompanyGroupMainCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_MAIN_CODE),
                  //(tableSchema.CompanyGroupGuidId.ColumnDatabaseName, entity.S50_COMPANY_GROUP_GUID_ID),
                  //(tableSchema.ParentUserId.ColumnDatabaseName, entity.GP_USU_ID),
               },
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID)
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
         };
      }
      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50TaxModel> sage50Entities, GestprojectTaxModel entity)
      {
         try
         {
            ValidateTaxSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateTaxSyncronizationStatus(
               entity,
               sage50Entities,
               tableSchema.GestprojectDescription.ColumnDatabaseName,
               tableSchema.GestprojectValue.ColumnDatabaseName,
               tableSchema.AccountableSubaccount.ColumnDatabaseName,
               tableSchema.AccountableSubaccount2.ColumnDatabaseName,
               NevesWasSynchronized
            );

            MustBeDeleted = ProviderSyncronizationStatusValidator.MustBeDeleted;
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
      public void DeleteEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<GestprojectTaxModel> gestprojectEntites, GestprojectTaxModel entity)
      {
         try
         {
            //new DeleteEntityFromSynchronizationTable(
            //   connection,
            //   tableSchema.TableName,
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID),
            //   (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            //);

            //new ClearEntityDataInGestproject(
            //      connection,
            //      "IMPUESTO_CONFIG",
            //      new List<string>(){
            //      tableSchema.AccountableSubaccount.ColumnDatabaseName
            //   },
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.IMP_ID),
            //   (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            //);

            //ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
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

      public void ClearEntitySynchronizationData(GestprojectTaxModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
      {
         try
         {
            for(global::System.Int32 i = 0; i < entityPropertiesValuesTupleList.Count; i++)
            {
               typeof(GestprojectTaxModel).GetProperty(entityPropertiesValuesTupleList[i].propertyName).SetValue(entity, entityPropertiesValuesTupleList[i].defaultValue);
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
   }
}