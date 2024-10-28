using SincronizadorGPS50.Workflows.Sage50Connection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectReceivedBillsProcessor : ISynchronizableEntityProcessor<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
   {
      public List<GestprojectReceivedBillModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;
      public bool NevesWasSynchronized { get; set; } = false;

      public List<GestprojectReceivedBillModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectReceivedBillModel> gestprojectEntites,
         List<Sage50ReceivedBillModel> sage50Entities
      )
      {
         try
         {
            ProcessedEntities = new List<GestprojectReceivedBillModel>();

            for(int i = 0; i < gestprojectEntites.Count; i++)
            {
               GestprojectReceivedBillModel entity = gestprojectEntites[i];

               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  //MessageBox.Show(entity.FCP_ID + " MustBeSkipped");
                  continue;
               }
               else if(MustBeRegistered)
               {
                  //MessageBox.Show(entity.FCP_ID + " MustBeRegistered");
                  RegisterEntity(connection, tableSchema, entity);
               }
               else if(MustBeUpdated)
               {
                  //MessageBox.Show(entity.FCP_ID + " MustBeUpdated");
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


      public void AppendSynchronizationTableDataToEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectReceivedBillModel entity)
      {
         try
         {
            new EntitySynchronizationTable<GestprojectReceivedBillModel>().AppendTableDataToEntity
            (
               connection,
               tableSchema.TableName,
               tableSchema.SynchronizationFieldsTupleList,
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
               entity,               
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCP_ID)
            );
            
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());
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
      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, GestprojectReceivedBillModel entity)
      {
         try
         {
            MustBeRegistered = !new WasReceivedBillRegistered(
               connection,
               tableSchema.TableName,
               tableSchema.GestprojectId.ColumnDatabaseName,
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCP_ID),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
            ).ItWas;

            bool registeredInDifferentCompanyGroup =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId != entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";
            NevesWasSynchronized = neverSynchronized;

            bool synchronizedInThePast =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast;

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
      public void RegisterEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectReceivedBillModel entity)
      {
         try
         {
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());

            new RegisterEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>()
               {
                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCP_ID),
                  (tableSchema.GestprojectDaoId.ColumnDatabaseName, entity.PAR_DAO_ID),
                  (tableSchema.GestprojectBillNumber.ColumnDatabaseName, entity.FCP_NUM_FACTURA),
                  (tableSchema.GestprojectDate.ColumnDatabaseName, entity.FCP_FECHA),
                  (tableSchema.GestprojectProId.ColumnDatabaseName, entity.PAR_PRO_ID),
                  (tableSchema.GestprojectTaxableBase.ColumnDatabaseName, entity.FCP_BASE_IMPONIBLE),
                  (tableSchema.GestprojectIvaValue.ColumnDatabaseName, entity.FCP_VALOR_IVA),
                  (tableSchema.GestprojectIvaValueInEuros.ColumnDatabaseName, entity.FCP_IVA),
                  (tableSchema.GestprojectIrpfValue.ColumnDatabaseName, entity.FCP_VALOR_IRPF),
                  (tableSchema.GestprojectIrpfValueInEuros.ColumnDatabaseName, entity.FCP_IRPF),
                  (tableSchema.GestprojectBillTotal.ColumnDatabaseName, entity.FCP_TOTAL_FACTURA)
               }
            );

            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //};
            //MessageBox.Show(stringBuilder.ToString());

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
      public void UpdateEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, GestprojectReceivedBillModel entity)
      {
         try
         {
            //StringBuilder stringBuilder = new StringBuilder();
            //foreach(var item in entity.GetType().GetProperties())
            //{
            //   stringBuilder.Append($"{item.Name}: {item.GetValue(entity)}\n");
            //}
            //MessageBox.Show(stringBuilder.ToString());

            new UpdateEntity
            (
               connection,
               tableSchema.TableName,
               new List<(string, dynamic)>()
               {
                  (tableSchema.SynchronizationStatus.ColumnDatabaseName, entity.SYNC_STATUS),
                  (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCP_ID),
                  (tableSchema.GestprojectDaoId.ColumnDatabaseName, entity.PAR_DAO_ID),
                  (tableSchema.GestprojectDate.ColumnDatabaseName, entity.FCP_FECHA),
                  (tableSchema.GestprojectProId.ColumnDatabaseName, entity.PAR_PRO_ID),
                  (tableSchema.GestprojectTaxableBase.ColumnDatabaseName, entity.FCP_BASE_IMPONIBLE),
                  (tableSchema.GestprojectIvaValue.ColumnDatabaseName, entity.FCP_VALOR_IVA),
                  (tableSchema.GestprojectIvaValueInEuros.ColumnDatabaseName, entity.FCP_IVA),
                  (tableSchema.GestprojectIrpfValue.ColumnDatabaseName, entity.FCP_VALOR_IRPF),
                  (tableSchema.GestprojectIrpfValueInEuros.ColumnDatabaseName, entity.FCP_IRPF),
                  (tableSchema.GestprojectBillTotal.ColumnDatabaseName, entity.FCP_TOTAL_FACTURA)
               },
               (tableSchema.GestprojectId.ColumnDatabaseName, entity.FCP_ID),
               (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID)
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
      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50ReceivedBillModel> sage50Entities, GestprojectReceivedBillModel entity)
      {
         try
         {
            ValidateReceivedBillSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateReceivedBillSyncronizationStatus(
               entity,
               sage50Entities,
               (tableSchema.GestprojectDaoId.ColumnDatabaseName),
               (tableSchema.GestprojectBillNumber.ColumnDatabaseName),
               (tableSchema.GestprojectDate.ColumnDatabaseName),
               (tableSchema.GestprojectProId.ColumnDatabaseName),
               (tableSchema.GestprojectTaxableBase.ColumnDatabaseName),
               (tableSchema.GestprojectBillTotal.ColumnDatabaseName),
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
      public void DeleteEntity(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<GestprojectReceivedBillModel> gestprojectEntites, GestprojectReceivedBillModel entity)
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

      public void ClearEntitySynchronizationData(GestprojectReceivedBillModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
      {
         try
         {
            for(global::System.Int32 i = 0; i < entityPropertiesValuesTupleList.Count; i++)
            {
               typeof(GestprojectReceivedBillModel).GetProperty(entityPropertiesValuesTupleList[i].propertyName).SetValue(entity, entityPropertiesValuesTupleList[i].defaultValue);
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