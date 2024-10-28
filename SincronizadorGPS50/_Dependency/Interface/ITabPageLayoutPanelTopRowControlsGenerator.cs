using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   //internal interface ITabPageLayoutPanelTopRowControlsGenerator<T1,T2>
   internal interface ITabPageLayoutPanelTopRowControlsGenerator<T1, T2>
   {
      UltraGrid MiddleRowGrid { get; set; }
      System.Windows.Forms.TableLayoutPanel RowTableLayout { get; set; }
      Infragistics.Win.Misc.UltraButton RefreshButton { get; set; }
      Infragistics.Win.Misc.UltraButton SelectAllButton { get; set; }
      Infragistics.Win.Misc.UltraButton SynchronizeButton { get; set; }
      IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      ISage50ConnectionManager Sage50ConnectionManager { get; set; }
      ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
      IEntitySynchronizer<T1, T2> EntitySynchronizer { get; set; }

      void GenerateControls
      (
         Infragistics.Win.Misc.UltraPanel rowPanel,
         Infragistics.Win.UltraWinGrid.UltraGrid middleRowGrid,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> dataSourceGenerator,
         IEntitySynchronizer<T1, T2> entitySynchronizer
      );

      void GenerateRowTableLayoutPanel();

      void GenerateRefreshButtonControl();
      void GenerateSelectAllButtonControl();
      void GenerateSynchronizeButtonControl();

      void SetRefreshButtonClickEventHandler();
      void SetSelectAllButtonClickEventHandler();
      void SetSynchronizeButtonClickEventHandler();

      void RefreshButtonClickEventHandler(object sender, System.EventArgs e);
      void SelectAllButtonClickEventHandler(object sender, System.EventArgs e);
      void SynchronizeButtonClickEventHandler(object sender, System.EventArgs e);

      void AddButtonsToRowTableLayoutPanel();
      void AddRowTableLayoutPanelToRowPanel(UltraPanel rowPanel);
   }
}
