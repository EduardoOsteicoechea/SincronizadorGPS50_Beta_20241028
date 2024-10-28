//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Reflection;

//namespace SincronizadorGPS50
//{
//   public class GestprojectProvidersProcessor : ISynchronizableEntityProcessor<GestprojectProviderModel, Sage50ProviderModel>
//   {
//      public List<GestprojectProviderModel> ProcessedEntities { get; set; }
//      public bool MustBeRegistered { get; set; } = false;
//      public bool MustBeSkipped { get; set; } = false;
//      public bool MustBeUpdated { get; set; } = false;
//      public bool MustBeDeleted { get; set; } = false;

//      public List<GestprojectProviderModel> ProcessEntityList
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchema,
//         List<GestprojectProviderModel> gestprojectEntites,
//         List<Sage50ProviderModel> sage50Entities
//      )
//      {
//         try
//         {
//            ProcessedEntities = new List<GestprojectProviderModel>();

//            for(int i = 0; i < gestprojectEntites.Count; i++)
//            {
//               GestprojectProviderModel entity = gestprojectEntites[i];

//               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);

//               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

//               if(MustBeSkipped)
//               {
//                  continue;
//               }
//               else if(MustBeRegistered)
//               {
//                  RegisterEntity(connection, tableSchema, entity);
//               }
//               else if(MustBeUpdated)
//               {
//                  UpdateEntity(connection, tableSchema, entity);
//               };

//               ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);

//               if(MustBeDeleted)
//               {
//                  DeleteEntity(connection, tableSchema, gestprojectEntites, entity);
//                  RegisterEntity(connection, tableSchema, entity);
//               };

//               UpdateEntity(connection, tableSchema, entity);

//               ProcessedEntities.Add(entity);
//            };

//            return ProcessedEntities;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }


//      public void AppendSynchronizationTableDataToEntity
//      (
//         SqlConnection connection, 
//         ISynchronizationTableSchemaProvider tableSchema, 
//         GestprojectProviderModel entity
//      )
//      {
//         try
//         {
//            new EntitySynchronizationTable<GestprojectProviderModel>().AppendTableDataToEntity
//            (
//               connection,
//               tableSchema.TableName,
//               new List<(string, System.Type)>()
//               {
//                  (tableSchema.Id.ColumnDatabaseName, tableSchema.Id.ColumnValueType),
//                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, tableSchema.SynchronizationStatus.ColumnValueType),
//                  (tableSchema.Sage50Code.ColumnDatabaseName, tableSchema.Sage50Code.ColumnValueType),
//                  (tableSchema.Sage50GuidId.ColumnDatabaseName, tableSchema.Sage50GuidId.ColumnValueType),
//                  (tableSchema.CompanyGroupName.ColumnDatabaseName, tableSchema.CompanyGroupName.ColumnValueType),
//                  (tableSchema.CompanyGroupCode.ColumnDatabaseName, tableSchema.CompanyGroupCode.ColumnValueType),
//                  (tableSchema.CompanyGroupMainCode.ColumnDatabaseName, tableSchema.CompanyGroupMainCode.ColumnValueType),
//                  (tableSchema.CompanyGroupGuidId.ColumnDatabaseName, tableSchema.CompanyGroupGuidId.ColumnValueType),
//                  (tableSchema.LastUpdate.ColumnDatabaseName, tableSchema.LastUpdate.ColumnValueType),
//                  (tableSchema.ParentUserId.ColumnDatabaseName, tableSchema.ParentUserId.ColumnValueType),
//                  (tableSchema.Comments.ColumnDatabaseName, tableSchema.Comments.ColumnValueType),
//               },
//               (tableSchema.GestprojectId.ColumnDatabaseName,entity.PAR_ID),
//               entity
//            );
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, GestprojectProviderModel entity)
//      {
//         try
//         {
//            MustBeRegistered = !new WasEntityRegistered(
//               connection,
//               tableSchema.TableName,
//               tableSchema.GestprojectId.ColumnDatabaseName,
//               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID)
//            ).ItIs;

//            bool registeredInDifferentCompanyGroup =
//            entity.S50_COMPANY_GROUP_GUID_ID != ""
//            &&
//            sage50ConnectionManager.CompanyGroupData.CompanyGuidId != entity.S50_COMPANY_GROUP_GUID_ID;

//            MustBeSkipped = registeredInDifferentCompanyGroup;

//            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";

//            bool synchronizedInThePast =
//            entity.S50_COMPANY_GROUP_GUID_ID != ""
//            &&
//            sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

//            MustBeUpdated = neverSynchronized || synchronizedInThePast;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//      public void RegisterEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectProviderModel entity)
//      {
//         try
//         {
//            new RegisterEntity
//            (
//               connection,
//               tableSchema.TableName,
//               new List<(string, dynamic)>(){
//                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
//                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID),
//                  (tableSchema.GestprojectName.ColumnDatabaseName, entity.PAR_NOMBRE),
//                  (tableSchema.GestprojectLastName1.ColumnDatabaseName, entity.PAR_APELLIDO_1),
//                  (tableSchema.GestprojectLastName2.ColumnDatabaseName, entity.PAR_APELLIDO_2),
//                  (tableSchema.FullName.ColumnDatabaseName, entity.NOMBRE_COMPLETO),
//                  (tableSchema.Cif.ColumnDatabaseName, entity.PAR_CIF_NIF),
//                  (tableSchema.Address.ColumnDatabaseName, entity.PAR_DIRECCION_1),
//                  (tableSchema.PostalCode.ColumnDatabaseName, entity.PAR_CP_1),
//                  (tableSchema.Locality.ColumnDatabaseName, entity.PAR_LOCALIDAD_1),
//                  (tableSchema.Province.ColumnDatabaseName, entity.PAR_PROVINCIA_1),
//                  (tableSchema.Country.ColumnDatabaseName, entity.PAR_PAIS_1)
//               }
//            );

//            AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//      public void UpdateEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectProviderModel entity)
//      {
//         try
//         {
//            new UpdateEntity
//            (
//               connection,
//               tableSchema.TableName,
//               new List<(string, dynamic)>(){
//                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, entity.SYNC_STATUS),
//                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID),
//                  (tableSchema.GestprojectName.ColumnDatabaseName, entity.PAR_NOMBRE),
//                  (tableSchema.GestprojectLastName1.ColumnDatabaseName, entity.PAR_APELLIDO_1),
//                  (tableSchema.GestprojectLastName2.ColumnDatabaseName, entity.PAR_APELLIDO_2),
//                  (tableSchema.FullName.ColumnDatabaseName, entity.NOMBRE_COMPLETO),
//                  (tableSchema.Cif.ColumnDatabaseName, entity.PAR_CIF_NIF),
//                  (tableSchema.Address.ColumnDatabaseName, entity.PAR_DIRECCION_1),
//                  (tableSchema.PostalCode.ColumnDatabaseName, entity.PAR_CP_1),
//                  (tableSchema.Locality.ColumnDatabaseName, entity.PAR_LOCALIDAD_1),
//                  (tableSchema.Province.ColumnDatabaseName, entity.PAR_PROVINCIA_1),
//                  (tableSchema.Country.ColumnDatabaseName, entity.PAR_PAIS_1)
//               },
//               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID)
//            );
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50ProviderModel> sage50Entities, GestprojectProviderModel entity)
//      {
//         try
//         {
//            ValidateProviderSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateProviderSyncronizationStatus(
//               entity,
//               sage50Entities,
//               tableSchema.FullName.ColumnDatabaseName,
//               tableSchema.Cif.ColumnDatabaseName,
//               tableSchema.Address.ColumnDatabaseName,
//               tableSchema.PostalCode.ColumnDatabaseName,
//               tableSchema.Province.ColumnDatabaseName
//            );

//            MustBeDeleted = ProviderSyncronizationStatusValidator.MustBeDeleted;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//      public void DeleteEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<GestprojectProviderModel> gestprojectEntites, GestprojectProviderModel entity)
//      {
//         try
//         {
//            new DeleteEntityFromSynchronizationTable(
//               connection,
//               tableSchema.TableName,
//               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID),
//               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
//            );

//            new ClearEntityDataInGestproject(
//               connection,
//               "PARTICIPANTE",
//               new List<string>(){
//                  tableSchema.AccountableSubaccount.ColumnDatabaseName
//               },
//               (tableSchema.GestprojectId.ColumnDatabaseName, entity.PAR_ID)
//            );

//            ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }

//      public void ClearEntitySynchronizationData(GestprojectProviderModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
//      {
//         try
//         {
//            for(global::System.Int32 i = 0; i < entityPropertiesValuesTupleList.Count; i++)
//            {
//               typeof(GestprojectProviderModel).GetProperty(entityPropertiesValuesTupleList[i].propertyName).SetValue(entity, entityPropertiesValuesTupleList[i].defaultValue);
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//   }
//}