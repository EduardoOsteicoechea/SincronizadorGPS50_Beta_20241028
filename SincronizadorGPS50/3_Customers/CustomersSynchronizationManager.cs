using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class CustomersSynchronizationManager : IEntitySynchronizationManager
   {
      public void Launch
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         UltraTab hostTab
      )
      {
         try
         {
            hostTab.Enabled = true;
            MainWindowUIHolder.MainTabControl.SelectedTab = hostTab;

            UIFactory<SynchronizableCustomerModel, SageCustomerModel>.GenerateTabPage
            (
               // Application Constructor
               new SynchronizationTabGenerator<SynchronizableCustomerModel, SageCustomerModel>(),

               // UI common
               hostTab.TabPage.Controls,
               new TabPageMainPanelTableLayoutPanelGenerator(),
               new TabPageLayoutPanelRowGenerator(),
               new MiddleRowControlsGenerator<SynchronizableCustomerModel, SageCustomerModel>(),
               new TopRowControlsGenerator<SynchronizableCustomerModel, SageCustomerModel>(
                  true, 
                  IdsToBeSelected.Synchronization
               ),
               new BottomRowControlsGenerator<SynchronizableCustomerModel, SageCustomerModel>(),

               // Connectors
               gestprojectConnectionManager,
               sage50ConnectionManager,

               // Data Managers
               new CustomersSynchronizationTableSchemaProvider(),
               new CustomersDataTableManager(),
               new CustomersSynchronizer()
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
   }
}
