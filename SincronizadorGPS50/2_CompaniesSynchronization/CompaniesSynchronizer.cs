using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class CompaniesSynchronizer : IEntitySynchronizer<SynchronizableCompanyModel, SageCompanyModel>
   {
      public IGestprojectConnectionManager GestprojectConnectionManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public ISage50ConnectionManager Sage50ConnectionManager { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public List<SynchronizableCompanyModel> GestprojectEntityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public List<SageCompanyModel> Sage50EntityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public List<SynchronizableCompanyModel> UnexistingGestprojectEntityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public List<SynchronizableCompanyModel> ExistingGestprojectEntityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public List<SynchronizableCompanyModel> UnsynchronizedGestprojectEntityList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public bool SomeEntitiesExistsInSage50 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public bool AllEntitiesExistsInSage50 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public bool NoEntitiesExistsInSage50 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      public bool UnsynchronizedEntityExists { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public void DetermineEntitySincronizationWorkflow(List<SynchronizableCompanyModel> UnexistingGestprojectEntityList, List<SynchronizableCompanyModel> ExistingGestprojectEntityList, List<SynchronizableCompanyModel> UnsynchronizedGestprojectEntityList, List<SynchronizableCompanyModel> GestprojectEntityList) => throw new NotImplementedException();
      public void ExecuteSyncronizationWorkflow(bool SomeEntitiesExistsInSage50, bool AllEntitiesExistsInSage50, bool NoEntitiesExistsInSage50, bool UnsynchronizedEntityExists, IGestprojectConnectionManager GestprojectConnectionManager, ISage50ConnectionManager Sage50ConnectionManager, ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider, List<SynchronizableCompanyModel> UnexistingGestprojectEntityList, List<SynchronizableCompanyModel> ExistingGestprojectEntityList, List<SynchronizableCompanyModel> UnsynchronizedGestprojectEntityList, List<SynchronizableCompanyModel> GestprojectEntityList) => throw new NotImplementedException();
      public void StoreBreakDownGestprojectEntityListByStatus(List<SynchronizableCompanyModel> GestprojectEntityList, List<SageCompanyModel> Sage50EntityList) => throw new NotImplementedException();
      public void StoreGestprojectEntityList(IGestprojectConnectionManager GestprojectConnectionManager, List<int> selectedIdList, string tableName, List<(string, Type)> fieldsToBeRetrieved, (string condition1ColumnName, string condition1Value) condition1Data) => throw new NotImplementedException();
      public void StoreSage50EntityList(string dispatcherMechanismRoute, string tableName, List<(string, Type)> fieldsToBeRetrieved) => throw new NotImplementedException();
      public void Synchronize(IGestprojectConnectionManager gestprojectConnectionManager, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider, List<int> selectedIdList) => throw new NotImplementedException();
   }
}
