//using System;
//using System.Collections.Generic;

//namespace SincronizadorGPS50
//{
//   internal interface IEntitySynchronizer<T1, T2>
//   {

//      IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      ISage50ConnectionManager Sage50ConnectionManager { get; set; }
//      ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
//      void Synchronize
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
//         List<int> selectedIdList
//      );

//      List<T1> GestprojectEntityList { get; set; }
//      void StoreGestprojectEntityList
//      (
//         IGestprojectConnectionManager GestprojectConnectionManager,
//         List<int> selectedIdList,
//         string tableName,
//         List<(string, System.Type)> fieldsToBeRetrieved,
//         (string condition1ColumnName, string condition1Value) condition1Data
//      );

//      List<T2> Sage50EntityList { get; set; }
//      void StoreSage50EntityList
//      (
//         string dispatcherMechanismRoute,
//         string tableName,
//         List<(string, System.Type)> fieldsToBeRetrieved
//      );

//      List<T1> UnexistingGestprojectEntityList { get; set; }
//      List<T1> ExistingGestprojectEntityList { get; set; }
//      List<T1> UnsynchronizedGestprojectEntityList { get; set; }
//      void StoreBreakDownGestprojectEntityListByStatus
//      (
//         List<T1> GestprojectEntityList, 
//         List<T2> Sage50EntityList
//      );

//      bool SomeEntitiesExistsInSage50 { get; set; }
//      bool AllEntitiesExistsInSage50 { get; set; }
//      bool NoEntitiesExistsInSage50 { get; set; }
//      bool UnsynchronizedEntityExists { get; set; }
//      void DetermineEntitySincronizationWorkflow
//      (
//         List<T1> UnexistingGestprojectEntityList,
//         List<T1> ExistingGestprojectEntityList,
//         List<T1> UnsynchronizedGestprojectEntityList,
//         List<T1> GestprojectEntityList
//      );

//      void ExecuteSyncronizationWorkflow
//      (
//         bool SomeEntitiesExistsInSage50,
//         bool AllEntitiesExistsInSage50,
//         bool NoEntitiesExistsInSage50,
//         bool UnsynchronizedEntityExists,
//         IGestprojectConnectionManager GestprojectConnectionManager,
//         ISage50ConnectionManager Sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider,
//         List<T1> UnexistingGestprojectEntityList,
//         List<T1> ExistingGestprojectEntityList,
//         List<T1> UnsynchronizedGestprojectEntityList,
//         List<T1> GestprojectEntityList
//      );
//   }
//}
