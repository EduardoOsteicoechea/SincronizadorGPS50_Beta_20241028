using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class ProviderSynchronizationManager : IEntitySynchronizationManager
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

            UIFactory<GestprojectProviderModel, Sage50ProviderModel>.GenerateTabPage
            (
               // Application Constructor
               new SynchronizationTabGenerator<GestprojectProviderModel, Sage50ProviderModel>(),

               // UI comon
               hostTab.TabPage.Controls,
               new TabPageMainPanelTableLayoutPanelGenerator(),
               new TabPageLayoutPanelRowGenerator(),
               new MiddleRowControlsGenerator<GestprojectProviderModel, Sage50ProviderModel>(),
               new TopRowControlsGenerator<GestprojectProviderModel, Sage50ProviderModel>(true, IdsToBeSelected.Synchronization),
               new BottomRowControlsGenerator<GestprojectProviderModel, Sage50ProviderModel>(),

               // Connectors
               gestprojectConnectionManager,
               sage50ConnectionManager,

               // Data Managers
               new ProvidersSynchronizationTableSchemaProvider(),
               new ProvidersDataTableManager(),
               new ProvidersSynchronizer()
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
