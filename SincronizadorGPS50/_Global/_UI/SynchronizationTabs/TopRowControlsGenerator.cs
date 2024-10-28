using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinLiveTileView;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   //internal class TopRowControlsGenerator<GestprojectProviderModel, Sage50ProviderModel> : ITabPageLayoutPanelTopRowControlsGenerator<GestprojectProviderModel, Sage50ProviderModel>
   internal class TopRowControlsGenerator<T1, T2> : ITabPageLayoutPanelTopRowControlsGenerator<T1, T2>
   {
      public UltraGrid MiddleRowGrid { get; set; }
      public TableLayoutPanel RowTableLayout { get; set; }
      public UltraButton RefreshButton { get; set; }
      public UltraButton SelectAllButton { get; set; }
      public UltraButton SynchronizeButton { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
      public IGridDataSourceGenerator<T1, T2> DataSourceGenerator { get; set; }
      public IEntitySynchronizer<T1, T2> EntitySynchronizer { get; set; }
      public bool SincronizationButtonDisabled { get; set; } = true;
      public string IdToBeSelected { get; set; } = "";

      public TopRowControlsGenerator(bool sincronizationButtonDisabled = true, string idToBeSelected = "")
      {
         SincronizationButtonDisabled = sincronizationButtonDisabled;
         IdToBeSelected = idToBeSelected;
      }

      public void GenerateControls
      (
         UltraPanel rowPanel,
         UltraGrid middleRowGrid,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> dataSourceGenerator,
         IEntitySynchronizer<T1, T2> entitySynchronizer
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Sage50ConnectionManager = sage50ConnectionManager;
            SynchronizationTableSchemaProvider = synchronizationTableSchemaProvider;
            MiddleRowGrid = middleRowGrid;
            DataSourceGenerator = dataSourceGenerator;
            EntitySynchronizer = entitySynchronizer;

            GenerateRowTableLayoutPanel();
            GenerateRefreshButtonControl();
            GenerateSelectAllButtonControl();
            GenerateSynchronizeButtonControl();
            SetRefreshButtonClickEventHandler();
            SetSelectAllButtonClickEventHandler();
            SetSynchronizeButtonClickEventHandler();
            AddButtonsToRowTableLayoutPanel();
            AddRowTableLayoutPanelToRowPanel(rowPanel);
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

      public void GenerateRowTableLayoutPanel()
      {
         RowTableLayout = new TableLayoutPanel();
         RowTableLayout.ColumnCount = 4;
         RowTableLayout.RowCount = 1;
         RowTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 84f));
         RowTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
         RowTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
         RowTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
         RowTableLayout.Dock = DockStyle.Fill;
      }
      public void GenerateRefreshButtonControl()
      {
         RefreshButton = new UltraButton();
         RefreshButton.Text = "Refrescar";
         RefreshButton.Dock = DockStyle.Fill;
      }
      public void GenerateSelectAllButtonControl()
      {
         SelectAllButton = new UltraButton();
         SelectAllButton.Text = "Seleccionar todo";
         SelectAllButton.Dock = DockStyle.Fill;
      }
      public void GenerateSynchronizeButtonControl()
      {
         SynchronizeButton = new UltraButton();
         SynchronizeButton.Text = "Sincronizar";
         SynchronizeButton.Dock = DockStyle.Fill;
         SynchronizeButton.Enabled = SincronizationButtonDisabled;
      }
      public void SetRefreshButtonClickEventHandler()
      {
         RefreshButton.Click += RefreshButtonClickEventHandler;
      }
      public void SetSelectAllButtonClickEventHandler()
      {
         SelectAllButton.Click += SelectAllButtonClickEventHandler;
      }
      public void SetSynchronizeButtonClickEventHandler()
      {
         SynchronizeButton.Click += SynchronizeButtonClickEventHandler;
      }

      public void RefreshButtonClickEventHandler(object sender, System.EventArgs e)
      {
         System.Data.DataTable dataTable = DataSourceGenerator.GenerateDataTable(
            GestprojectConnectionManager,
            Sage50ConnectionManager,
            SynchronizationTableSchemaProvider
         );

         ManageUserInteractionWithUI.RefreshTable(MiddleRowGrid, dataTable);
      }
      public void SelectAllButtonClickEventHandler(object sender, EventArgs e)
      {
         ManageUserInteractionWithUI.SelectNonfiltered(MiddleRowGrid);
      }
      public void SynchronizeButtonClickEventHandler(object sender, EventArgs e)
      {
         try
         {
            List<int> selectedIdList = ManageUserInteractionWithUI.GetSelectedIfAnyOrAll(MiddleRowGrid, IdToBeSelected);

            EntitySynchronizer.Synchronize
            (
               GestprojectConnectionManager,
               Sage50ConnectionManager,
               SynchronizationTableSchemaProvider,
               selectedIdList
            );

            System.Data.DataTable dataTable = DataSourceGenerator.GenerateDataTable(
               GestprojectConnectionManager,
               Sage50ConnectionManager,
               SynchronizationTableSchemaProvider
            );

            ManageUserInteractionWithUI.RefreshTable(MiddleRowGrid, dataTable);
         }
         catch (System.Exception exception)
         {
            new DeadEndException(exception);
         };
      }
      public void AddButtonsToRowTableLayoutPanel()
      {
         RowTableLayout.Controls.Add(RefreshButton, 1, 0);
         RowTableLayout.Controls.Add(SelectAllButton, 2, 0);
         RowTableLayout.Controls.Add(SynchronizeButton, 3, 0);
      }
      public void AddRowTableLayoutPanelToRowPanel(UltraPanel rowPanel)
      {
         rowPanel.ClientArea.Controls.Add(RowTableLayout);
      }
   }
}
