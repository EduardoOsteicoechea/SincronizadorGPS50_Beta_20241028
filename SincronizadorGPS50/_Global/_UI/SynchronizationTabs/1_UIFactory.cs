using Infragistics.Win.UltraWinTabControl;
using System.Reflection;

namespace SincronizadorGPS50
{
   internal static class UIFactory<T1, T2>
   {
      internal static void GenerateTabPage
      (
         ITabPageGenerator<T1, T2> tabPageGenerator,

         UltraTabPageControl.ControlCollection MainWindowUITabControlCollection, // just call the .Add() method and pass the "MainPanel" UltraPanel that contains the "TabPageTableLayoutPanel" TableLayoutPanel, which itself will contain the UltraPanel corresponding to the UI rows along with their respective TableLayoutPanels, controls and event handlers.

         ITabPageMainPanelTableLayoutPanelGenerator tabPageMainPanelTableLayoutGenerator, // Generates tab page main panel table layout.
         ITabPageLayoutPanelRowGenerator tabPageUIRowGenerator, // Generates the Row UltraPanel.
         ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2> tabPageUImiddleRowControlsGenerator, // Generates the Row's UltraPanel TableLayout controls.
         ITabPageLayoutPanelTopRowControlsGenerator<T1, T2> tabPageUItopRowControlsGenerator, // Generates the Row's UltraPanel TableLayout controls.
         ITabPageLayoutPanelBottomRowControlsGenerator<T1, T2> tabPageUIbottomRowControlsGenerator, // Generates the Row's UltraPanel TableLayout controls.

         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
               
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator,
         IEntitySynchronizer<T1, T2> entitySynchronizer
      )
      {
         try
         {
            tabPageGenerator.Build(
               MainWindowUITabControlCollection,
               tabPageMainPanelTableLayoutGenerator,
               tabPageUIRowGenerator,
               tabPageUImiddleRowControlsGenerator,
               tabPageUItopRowControlsGenerator,
               tabPageUIbottomRowControlsGenerator,
               gestprojectConnectionManager,
               sage50ConnectionManager,
               synchronizationTableSchemaProvider,
               gridDataSourceGenerator,
               entitySynchronizer
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