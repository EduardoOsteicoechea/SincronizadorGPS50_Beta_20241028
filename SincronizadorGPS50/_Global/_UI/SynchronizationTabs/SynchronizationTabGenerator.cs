using Infragistics.Win.Misc;
using SincronizadorGPS50.Workflows.Sage50Connection;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class SynchronizationTabGenerator<T1, T2> : ITabPageGenerator<T1, T2>
   {
      public UltraPanel MainPanel { get; set;}
      public TableLayoutPanel TabPageTableLayoutPanel { get; set; }
      public UltraPanel TopRow { get; set; }
      public UltraPanel MiddleRow { get; set; }
      public UltraPanel BottomRow { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
      public DataTableGeneratorDelegate DataTableGeneratorDelegate { get; set; }
      public IEntitySynchronizer<T1, T2> EntitySynchronizer { get; set; }

      public void Build
      (
         Control.ControlCollection MainWindowUITabControlCollection, 
         ITabPageMainPanelTableLayoutPanelGenerator tabPageMainPanelTableLayoutGenerator, 
         ITabPageLayoutPanelRowGenerator tabPageUIRowGenerator,
         ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2> tabPageUImiddleRowControlsGenerator,
         ITabPageLayoutPanelTopRowControlsGenerator<T1, T2> tabPageUItopRowControlsGenerator, 
         ITabPageLayoutPanelBottomRowControlsGenerator<T1, T2> tabPageUIbottomRowControlsGenerator, 
         IGestprojectConnectionManager gestprojectConnectionManager, 
         ISage50ConnectionManager sage50ConnectionManager, 
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator,

         IEntitySynchronizer<T1, T2> entitySynchronizer
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Sage50ConnectionManager = sage50ConnectionManager;
            SynchronizationTableSchemaProvider = synchronizationTableSchemaProvider;
            DataTableGeneratorDelegate = gridDataSourceGenerator.GenerateDataTable;
            EntitySynchronizer = entitySynchronizer;

            MainPanel = CreateMainPanel();
            TabPageTableLayoutPanel = GenerateMainPanelTableLayoutPanel(tabPageMainPanelTableLayoutGenerator);
            AddTabPageTableLayoutToMainPanel(TabPageTableLayoutPanel, MainPanel);
            AddMainPanelToTab(MainPanel, MainWindowUITabControlCollection);

            MiddleRow = CreateMiddleRow(tabPageUIRowGenerator);
            CreateAndAddMiddleRowControls(MiddleRow, tabPageUImiddleRowControlsGenerator, gestprojectConnectionManager, sage50ConnectionManager, synchronizationTableSchemaProvider, gridDataSourceGenerator);

            TopRow = CreateTopRow(tabPageUIRowGenerator);
            CreateAndAddTopRowControls(TopRow, tabPageUImiddleRowControlsGenerator.Grid, tabPageUItopRowControlsGenerator, gridDataSourceGenerator);

            BottomRow = CreateBottomRow(tabPageUIRowGenerator);
            CreateAndAddBottomRowControls(BottomRow, tabPageUIbottomRowControlsGenerator);

            AddRowsPanelsToTabPageTableLayout(TopRow, MiddleRow, BottomRow, TabPageTableLayoutPanel);
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

      public UltraPanel CreateMainPanel() 
      {
         UltraPanel panel = new UltraPanel();
         panel.Dock = DockStyle.Fill;
         return panel;
      }
      public TableLayoutPanel GenerateMainPanelTableLayoutPanel(ITabPageMainPanelTableLayoutPanelGenerator tabPageMainPanelTableLayoutGenerator)
      {
         return tabPageMainPanelTableLayoutGenerator.GenerateGlobalTableLayoutPanel();
      }
      public void AddTabPageTableLayoutToMainPanel(TableLayoutPanel tabPageTableLayoutPanel, UltraPanel mainPanel)
      {
         mainPanel.ClientArea.Controls.Add(tabPageTableLayoutPanel);
      }
      public void AddMainPanelToTab(UltraPanel mainPanel, Control.ControlCollection tabControlCollection)
      {
         tabControlCollection.Add(mainPanel);
      }


      public UltraPanel CreateTopRow(ITabPageLayoutPanelRowGenerator rowGenerator)
      {
         return rowGenerator.GenerateRowPanel();
      }

      public UltraPanel CreateTopRow(ITabPageLayoutPanelRowGenerator rowGenerator, IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator) => throw new NotImplementedException();

      public void CreateAndAddTopRowControls
      (
         UltraPanel topRow,
         Infragistics.Win.UltraWinGrid.UltraGrid middleRowGrid,
         ITabPageLayoutPanelTopRowControlsGenerator<T1, T2> tabPageUItopRowControlsGenerator,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      )
      {
         try
         {
            tabPageUItopRowControlsGenerator.GenerateControls
            (
               topRow,
               middleRowGrid,
               GestprojectConnectionManager,
               Sage50ConnectionManager,
               SynchronizationTableSchemaProvider,
               gridDataSourceGenerator,
               EntitySynchronizer
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

      public UltraPanel CreateMiddleRow(ITabPageLayoutPanelRowGenerator rowGenerator)
      {
         return rowGenerator.GenerateRowPanel();
      }
      public void CreateAndAddMiddleRowControls
      (
         UltraPanel middleRow, 
         ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2> middleRowControlsGenerator, 
         IGestprojectConnectionManager gestprojectConnectionManager, 
         ISage50ConnectionManager sage50ConnectionManager, 
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider, 
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      )
      {
         middleRowControlsGenerator.GenerateControls
         (
            middleRow,
            gestprojectConnectionManager,
            sage50ConnectionManager,
            synchronizationTableSchemaProvider,
            gridDataSourceGenerator
         );
      }

      public UltraPanel CreateBottomRow(ITabPageLayoutPanelRowGenerator rowGenerator)
      {
         return rowGenerator.GenerateRowPanel();
      }
      public void CreateAndAddBottomRowControls
      (
         UltraPanel bottomRow, 
         ITabPageLayoutPanelBottomRowControlsGenerator<T1, T2> tabPageUIbottomRowControlsGenerator
      )
      {
         tabPageUIbottomRowControlsGenerator.GenerateControls
         (
            bottomRow,
            GestprojectConnectionManager,
            Sage50ConnectionManager,
            SynchronizationTableSchemaProvider
         );
      }
      public void AddRowsPanelsToTabPageTableLayout(UltraPanel topRow, UltraPanel middleRow, UltraPanel bottomRow, TableLayoutPanel tabPageTableLayoutPanel)
      {
         tabPageTableLayoutPanel.Controls.Add(topRow, 0, 0);
         tabPageTableLayoutPanel.Controls.Add(middleRow, 0, 1);
         tabPageTableLayoutPanel.Controls.Add(bottomRow, 0, 2);
      }
   }
}
