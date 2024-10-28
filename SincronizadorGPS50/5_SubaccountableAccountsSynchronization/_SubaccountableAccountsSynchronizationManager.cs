using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class SubaccountableAccountsSynchronizationManager : IEntitySynchronizationManager
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

            UIFactory<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>.GenerateTabPage
            (
               // Application Constructor
               new SynchronizationTabGenerator<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>(),

               // UI comon
               hostTab.TabPage.Controls,
               new TabPageMainPanelTableLayoutPanelGenerator(),
               new TabPageLayoutPanelRowGenerator(),
               new MiddleRowControlsGenerator<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>(),
               new TopRowControlsGenerator<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>(),
               new BottomRowControlsGenerator<GestprojectSubaccountableAccountModel, Sage50SubaccountableAccountModel>(),

               // Connectors
               gestprojectConnectionManager,
               sage50ConnectionManager,

               // Data Managers
               new SubaccountableAccountsSynchronizationTableSchemaProvider(),
               new SubaccountableAccountsDataTableManager(),
               new SubaccountableAccountsSynchronizer()
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
