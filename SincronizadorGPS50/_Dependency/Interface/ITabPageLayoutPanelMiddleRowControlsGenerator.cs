using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;

namespace SincronizadorGPS50
{
   internal interface ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2>
   {
      UltraGrid Grid { get; set; }
      void CreateGrid();
      void SetGridFilters();
      void PreventGridUpdates();
      void StyleGrid();
      void SetGridDataSource
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      );
      void AddGridToRow(UltraPanel row);
      void SetClickCellEventHandler();
      void SetAfterRowFilterChangedEventHandler();
      void AfterRowFilterChangedEventHandler(object sender, AfterRowFilterChangedEventArgs e);

      void GenerateControls
      (
         UltraPanel rowPanel,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      );
   }
}
