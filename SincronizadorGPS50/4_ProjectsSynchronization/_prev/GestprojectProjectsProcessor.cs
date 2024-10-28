using SincronizadorGPS50.Workflows.Sage50Connection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectProjectsProcessor : ISynchronizableEntityProcessor<GestprojectProjectModel, Sage50ProjectModel>
   {
      public List<GestprojectProjectModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;

      public List<GestprojectProjectModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectProjectModel> gestprojectEntites,
         List<Sage50ProjectModel> sage50Entities
      )
      {
         try
         {
            ProcessedEntities = new List<GestprojectProjectModel>();

            for(int i = 0; i < gestprojectEntites.Count; i++)
            {
               GestprojectProjectModel entity = gestprojectEntites[i];

               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  continue;
               }
               else if(MustBeRegistered)
               {
                  RegisterEntity(connection, tableSchema, entity);
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


      public void AppendSynchronizationTableDataToEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectProjectModel entity)
      {
         try
         {
            new EntitySynchronizationTable<GestprojectProjectModel>().AppendTableDataToEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, System.Type)>()
               {
               (tableSchema.Id.ColumnDatabaseName, tableSchema.Id.ColumnValueType),
               (tableSchema.SynchronizationStatus.ColumnDatabaseName, tableSchema.SynchronizationStatus.ColumnValueType),
               (tableSchema.Sage50Code.ColumnDatabaseName, tableSchema.Sage50Code.ColumnValueType),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, tableSchema.Sage50GuidId.ColumnValueType),
               (tableSchema.CompanyGroupName.ColumnDatabaseName, tableSchema.CompanyGroupName.ColumnValueType),
               (tableSchema.CompanyGroupCode.ColumnDatabaseName, tableSchema.CompanyGroupCode.ColumnValueType),
               (tableSchema.CompanyGroupMainCode.ColumnDatabaseName, tableSchema.CompanyGroupMainCode.ColumnValueType),
               (tableSchema.CompanyGroupGuidId.ColumnDatabaseName, tableSchema.CompanyGroupGuidId.ColumnValueType),
               (tableSchema.LastUpdate.ColumnDatabaseName, tableSchema.LastUpdate.ColumnValueType),
               (tableSchema.ParentUserId.ColumnDatabaseName, tableSchema.ParentUserId.ColumnValueType),
               (tableSchema.Comments.ColumnDatabaseName, tableSchema.Comments.ColumnValueType),
               },
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID),
               entity
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
      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, GestprojectProjectModel entity)
      {
         try
         {
            MustBeRegistered = !new WasEntityRegistered(
               connection,
               tableSchema.TableName,
               tableSchema.GestprojectId.ColumnDatabaseName,
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID)
            ).ItIs;

            bool registeredInDifferentCompanyGroup =
         entity.S50_COMPANY_GROUP_GUID_ID != ""
         &&
         sage50ConnectionManager.CompanyGroupData.CompanyGuidId != entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";

            bool synchronizedInThePast =
         entity.S50_COMPANY_GROUP_GUID_ID != ""
         &&
         sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast;
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
      public void RegisterEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectProjectModel entity)
      {
         try
         {
            new RegisterEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>(){
               (tableSchema.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID),
               (tableSchema.Name.ColumnDatabaseName, entity.PRY_NOMBRE),
               (tableSchema.Address.ColumnDatabaseName, entity.PRY_DIRECCION),
               (tableSchema.PostalCode.ColumnDatabaseName, entity.PRY_CP),
               (tableSchema.Locality.ColumnDatabaseName, entity.PRY_LOCALIDAD),
               (tableSchema.Province.ColumnDatabaseName, entity.PRY_PROVINCIA)
               }
            );

            AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
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
      public void UpdateEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectProjectModel entity)
      {
         try
         {
            new UpdateEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>(){
               (tableSchema.SynchronizationStatus.ColumnDatabaseName, entity.SYNC_STATUS),
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID),
               (tableSchema.Name.ColumnDatabaseName, entity.PRY_NOMBRE),
               (tableSchema.Address.ColumnDatabaseName, entity.PRY_DIRECCION),
               (tableSchema.PostalCode.ColumnDatabaseName, entity.PRY_CP),
               (tableSchema.Locality.ColumnDatabaseName, entity.PRY_LOCALIDAD),
               (tableSchema.Province.ColumnDatabaseName, entity.PRY_PROVINCIA)
               },
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID)
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
      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50ProjectModel> sage50Entities, GestprojectProjectModel entity)
      {
         try
         {
            ValidateProjectSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateProjectSyncronizationStatus(
            entity,
            sage50Entities,
            tableSchema.Name.ColumnDatabaseName,
            tableSchema.PostalCode.ColumnDatabaseName,
            tableSchema.Address.ColumnDatabaseName,
            tableSchema.Locality.ColumnDatabaseName,
            tableSchema.Province.ColumnDatabaseName
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
      public void DeleteEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<GestprojectProjectModel> gestprojectEntites, GestprojectProjectModel entity)
      {
         try
         {
            new DeleteEntityFromSynchronizationTable(
               connection,
               tableSchema.TableName,
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            );

            new ClearEntityDataInGestproject(
               connection,
               "PROYECTO",
               new List<string>(){
               tableSchema.AccountableSubaccount.ColumnDatabaseName
               },
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID)
            );

            ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
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

      public void ClearEntitySynchronizationData(GestprojectProjectModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
      {
         try
         {
            for(global::System.Int32 i = 0; i < entityPropertiesValuesTupleList.Count; i++)
            {
               typeof(GestprojectProjectModel).GetProperty(entityPropertiesValuesTupleList[i].propertyName).SetValue(entity, entityPropertiesValuesTupleList[i].defaultValue);
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