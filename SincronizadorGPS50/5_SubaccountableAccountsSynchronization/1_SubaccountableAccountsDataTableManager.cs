using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class SubaccountableAccountsDataTableManager : IGridDataSourceGenerator<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>
   {
      public List<GestprojectSubaccountableAccountModel> GestprojectEntities { get; set; }
      public List<Sage50SubaccountableAccountModel> Sage50Entities { get; set; }
      public List<GestprojectSubaccountableAccountModel> ProcessedGestprojectEntities { get; set; }
      public DataTable DataTable { get; set; }

      public System.Data.DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         try
         {
            ManageSynchronizationTableStatus(gestprojectConnectionManager, tableSchema);
            
            /////////////////////////////////
            // In this entity in particular, the process of getting the entity list to be processed cosist of
            // 1. Getting all the Gestproject entities, then,
            // 2. getting all the sage entities, then,
            // 3. verifiying if the sage entities already exist in Gestproject.
            // 3.2. if the sage entity exists in Gestproject, we'll ignore it.
            // 3.3. if the sage entitiy doesn't exist in Gestproject,
            // we'll instantiate a new Gestproject entity to
            // store the sage's unexisting entity values, and then,
            // add it to the Gestproject entity list to be processed.
            /////////////////////////////////
            
            GetAndStoreGestprojectEntities(gestprojectConnectionManager, tableSchema);
            
            /////////////////////////////////
            // The following Sage entities list will be used
            // to verify the synchronization status of each
            // Gestproject entity that already exist in Sage
            // on the "ProccessAndStoreGestprojectEntities" method
            /////////////////////////////////
            
            GetAndStoreSage50Entities(tableSchema);

            ProccessAndStoreGestprojectEntities(
               gestprojectConnectionManager,
               sage50ConnectionManager,
               tableSchema,
               GestprojectEntities,
               Sage50Entities
            );

            CreateAndDefineDataSource(tableSchema);
            PaintEntitiesOnDataSource(tableSchema, ProcessedGestprojectEntities, DataTable);
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
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
               gestprojectConnectionManager.GestprojectSqlConnection,
               tableSchema.TableName
            );

         if(tableExists == false)
         {
            entitySyncronizationTableStatusManager.CreateTable
            (
               gestprojectConnectionManager.GestprojectSqlConnection,
               tableSchema
            );
         };
      }

      public void GetAndStoreGestprojectEntities
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         /////////////////////////////////
         // Declare a new list to avoid duplication on table refresh
         /////////////////////////////////
         
         GestprojectEntities = new List<GestprojectSubaccountableAccountModel> ();

         /////////////////////////////////
         // Get all Gestproject Database entities
         /////////////////////////////////
         
         GestprojectEntities = new GetGestprojectSubaccountableAccounts(
            gestprojectConnectionManager.GestprojectSqlConnection,
            tableSchema
         ).Entities;

         /////////////////////////////////
         /// Get all the Codes and Names of the Gestproject entities list 
         /// To check if the sage entities match with some of them
         /////////////////////////////////

         var subaccountableAccountCodesList = GestprojectEntities.Select(x=>x.COS_CODIGO);
         var subaccountableAccountNamesList = GestprojectEntities.Select(x=>x.COS_NOMBRE);
         
         /////////////////////////////////
         // Get all Sage entities
         // Which codes begin with "6", "7" and "553"
         /////////////////////////////////
         
         List<Sage50SubaccountableAccountModel> sage50Entities = new GetSage50SubaccountableAccounts(tableSchema).Entities;
         
         /////////////////////////////////
         // Check if the sage entites already exist in Gestproject
         /////////////////////////////////

         foreach(var sageEntity in sage50Entities)
         {
            /////////////////////////////////
            // Check if the sage entity name and code exists in Gestproject's names and codes list
            /////////////////////////////////
         
            bool theSageEntityCodeExistInGestproject = subaccountableAccountCodesList.Contains(sageEntity.CODIGO);
            bool theSageEntityNameExistInGestproject = subaccountableAccountNamesList.Contains(sageEntity.NOMBRE);
            bool theSageEntityCodeAndNameExistInGestproject = theSageEntityCodeExistInGestproject && theSageEntityNameExistInGestproject;

            /////////////////////////////////
            // Check if the Gestproject entity list contain a single entity that has both
            // the code and name of the currently analized sage entity
            // to validate an exact match
            /////////////////////////////////

            bool sageEntityExistInGestproject = false;
            if(theSageEntityCodeAndNameExistInGestproject)
            {
               GestprojectSubaccountableAccountModel existingEntity = GestprojectEntities.FirstOrDefault(
                  x => (x.COS_CODIGO == sageEntity.CODIGO && x.COS_NOMBRE == sageEntity.NOMBRE) 
               );

               sageEntityExistInGestproject = existingEntity != null;
            }; 

            /////////////////////////////////
            // If the entity exists, create a corresponding Gestproject model
            // and populate it's properties with the Sage entity properties 
            // corresponding values and add it to the Gestproject entities list.
            /////////////////////////////////
               
            if(sageEntityExistInGestproject == false)
            {
               GestprojectSubaccountableAccountModel correspondingSageEntity = new GestprojectSubaccountableAccountModel();

               correspondingSageEntity.COS_ID = -1;
               correspondingSageEntity.COS_CODIGO = sageEntity.CODIGO.Trim();
               correspondingSageEntity.COS_NOMBRE = sageEntity.NOMBRE.Trim();

               /////////////////////////////////
               // Obtain the COS_GRUPO value from the "CODIGO" beggining characters
               // and add it to the corresponding Gestproject entity.
               /////////////////////////////////

               if(sageEntity.CODIGO.StartsWith("6"))
               {
                  correspondingSageEntity.COS_GRUPO = "FR";
               };
               if(sageEntity.CODIGO.StartsWith("7"))
               {
                  correspondingSageEntity.COS_GRUPO = "FE";
               };
               if(sageEntity.CODIGO.StartsWith("553"))
               {
                  correspondingSageEntity.COS_GRUPO = "SU";
               };

               GestprojectEntities.Add(correspondingSageEntity);
            };
         };
         /////////////////////////////////
         // At this point we should have a GestprojectEntities List containing
         // All the Gestproject entites plus all the Sage entities that do not 
         // exist already in the Gestproject database
         /////////////////////////////////
      }

      public void GetAndStoreSage50Entities
      (
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         /////////////////////////////////
         // Declare a new list to avoid duplication on table refresh
         /////////////////////////////////
         
         Sage50Entities = new List<Sage50SubaccountableAccountModel>();
         
         /////////////////////////////////
         // Get all Sage entities
         // Which codes begin with "6", "7" and "553"
         /////////////////////////////////
         
         Sage50Entities = new GetSage50SubaccountableAccounts(tableSchema).Entities;
      }

      public void ProccessAndStoreGestprojectEntities
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectSubaccountableAccountModel> GestprojectEntities,
         List<Sage50SubaccountableAccountModel> Sage50Entities
      )
      {
         ISynchronizableEntityProcessor<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel> gestprojectProvidersProcessor = new GestprojectSubaccountableAccountsProcessor();

         ProcessedGestprojectEntities = gestprojectProvidersProcessor.ProcessEntityList(
            gestprojectConnectionManager.GestprojectSqlConnection,
            sage50ConnectionManager,
            tableSchema,
            GestprojectEntities,
            Sage50Entities
         );
      }

      public void CreateAndDefineDataSource
      (
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
         DataTable = entityDataTableGenerator.CreateDataTable(tableSchema.ColumnsTuplesList);
      }

      public void PaintEntitiesOnDataSource
      (
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectSubaccountableAccountModel> ProcessedGestprojectEntities,
         DataTable dataTable
      )
      {
         ISynchronizableEntityPainter<GestprojectSubaccountableAccountModel> entityPainter = new EntityPainter<GestprojectSubaccountableAccountModel>();
         entityPainter.PaintEntityListOnDataTable(
            ProcessedGestprojectEntities,
            dataTable,
            tableSchema.ColumnsTuplesList
         );
      }
   }
}