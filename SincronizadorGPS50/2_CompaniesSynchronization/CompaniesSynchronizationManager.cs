using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class CompaniesSynchronizationManager : IEntitySynchronizationManager
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

            UIFactory<SynchronizableCompanyModel, SageCompanyModel>.GenerateTabPage
            (
               // Application Constructor
               new SynchronizationTabGenerator<SynchronizableCompanyModel, SageCompanyModel>(),

               // UI comon
               hostTab.TabPage.Controls,
               new TabPageMainPanelTableLayoutPanelGenerator(),
               new TabPageLayoutPanelRowGenerator(),
               new MiddleRowControlsGenerator<SynchronizableCompanyModel, SageCompanyModel>(),
               new TopRowControlsGenerator<SynchronizableCompanyModel, SageCompanyModel>(false),
               new BottomRowControlsGenerator<SynchronizableCompanyModel, SageCompanyModel>(),

               // Connectors
               gestprojectConnectionManager,
               sage50ConnectionManager,

               // Data Managers
               new CompaniesSynchronizationTableSchemaProvider(),
               new CompaniesDataTableManager(),
               new CompaniesSynchronizer()
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
