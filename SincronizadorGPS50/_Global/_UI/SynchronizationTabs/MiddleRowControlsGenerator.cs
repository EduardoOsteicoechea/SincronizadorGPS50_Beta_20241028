using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   //internal class MiddleRowControlsGenerator : ITabPageLayoutPanelMiddleRowControlsGenerator<T12, Sage50ProviderMode2l>
   internal class MiddleRowControlsGenerator<T1, T2> : ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2>
   {
      public Infragistics.Win.UltraWinGrid.UltraGrid Grid { get; set; }
      public void GenerateControls
      (
         UltraPanel rowPanel,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      )
      {
         CreateGrid();
         SetGridFilters();
         PreventGridUpdates();
         StyleGrid();
         SetGridDataSource(
            gestprojectConnectionManager, 
            sage50ConnectionManager, 
            synchronizationTableSchemaProvider, 
            gridDataSourceGenerator
         );
         AddGridToRow(rowPanel);
         SetClickCellEventHandler();
         SetAfterRowFilterChangedEventHandler();
      }
      public void CreateGrid()
      {
         Grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
      }
      public void SetGridFilters()
      {
         Grid.DisplayLayout.Override.FilterUIProvider = new ColumnsFilter();
         Grid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
      }
      public void PreventGridUpdates()
      {
         Grid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
         Grid.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
      }
      public void StyleGrid()
      {
         Grid.DisplayLayout.Override.AllowColSizing = AllowColSizing.Free;
         Grid.DisplayLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
         Grid.DisplayLayout.Override.DefaultColWidth = 80;
         Grid.Dock = System.Windows.Forms.DockStyle.Fill;
      }
      public void SetGridDataSource
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      )
      {
         System.Data.DataTable dataSource = gridDataSourceGenerator.GenerateDataTable(
            gestprojectConnectionManager, 
            sage50ConnectionManager, 
            synchronizationTableSchemaProvider
         );

         Grid.DataSource = dataSource;
      }
      public void AddGridToRow(UltraPanel row)
      {
         row.ClientArea.Controls.Add(Grid);
      }
      public void SetClickCellEventHandler()
      {
         Grid.ClickCell += ManageUserInteractionWithUI.ConfigureTable;
      }
      public void SetAfterRowFilterChangedEventHandler()
      {
         Grid.AfterRowFilterChanged += AfterRowFilterChangedEventHandler;
      }
      public void AfterRowFilterChangedEventHandler(object sender, AfterRowFilterChangedEventArgs e)
      {
         ManageUserInteractionWithUI.DeselectRows(this.Grid);
      }
   }
}
