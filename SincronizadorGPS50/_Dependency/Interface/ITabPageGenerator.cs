

using Infragistics.Win.Misc;
using System.Data;

namespace SincronizadorGPS50
{
   internal interface ITabPageGenerator<T1, T2>
   {
      IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      ISage50ConnectionManager Sage50ConnectionManager { get; set; }
      ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
      Infragistics.Win.Misc.UltraPanel MainPanel { get; set; }
      System.Windows.Forms.TableLayoutPanel TabPageTableLayoutPanel { get; set; }
      Infragistics.Win.Misc.UltraPanel TopRow { get; set; }
      Infragistics.Win.Misc.UltraPanel MiddleRow { get; set; }
      Infragistics.Win.Misc.UltraPanel BottomRow { get; set; }
      IEntitySynchronizer<T1, T2> EntitySynchronizer { get; set; }
      // Must follow this order
      void Build(
         Infragistics.Win.UltraWinTabControl.UltraTabPageControl.ControlCollection MainWindowUITabControlCollection,
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
      );

      Infragistics.Win.Misc.UltraPanel CreateMainPanel();
      System.Windows.Forms.TableLayoutPanel GenerateMainPanelTableLayoutPanel(ITabPageMainPanelTableLayoutPanelGenerator tabPageMainPanelTableLayoutGenerator);
      void AddTabPageTableLayoutToMainPanel(System.Windows.Forms.TableLayoutPanel tabPageTableLayoutPanel, Infragistics.Win.Misc.UltraPanel mainPanel);
      void AddMainPanelToTab
      (
         Infragistics.Win.Misc.UltraPanel mainPanel, 
         Infragistics.Win.UltraWinTabControl.UltraTabPageControl.ControlCollection tabControlCollection
      );

      Infragistics.Win.Misc.UltraPanel CreateTopRow(ITabPageLayoutPanelRowGenerator rowGenerator,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator);
      void CreateAndAddTopRowControls
      (
         Infragistics.Win.Misc.UltraPanel topRow,
         Infragistics.Win.UltraWinGrid.UltraGrid middleRowGrid,
         ITabPageLayoutPanelTopRowControlsGenerator<T1, T2> topRowControlsGenerator,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      );


      Infragistics.Win.Misc.UltraPanel CreateMiddleRow(ITabPageLayoutPanelRowGenerator rowGenerator);
      void CreateAndAddMiddleRowControls
      (
         Infragistics.Win.Misc.UltraPanel middleRow, 
         ITabPageLayoutPanelMiddleRowControlsGenerator<T1, T2> middleRowControlsGenerator,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         IGridDataSourceGenerator<T1, T2> gridDataSourceGenerator
      );


      Infragistics.Win.Misc.UltraPanel CreateBottomRow(ITabPageLayoutPanelRowGenerator rowGenerator);
      void CreateAndAddBottomRowControls
      (
         Infragistics.Win.Misc.UltraPanel bottomRow, 
         ITabPageLayoutPanelBottomRowControlsGenerator<T1, T2> bottomRowControlsGenerator
      );


      void AddRowsPanelsToTabPageTableLayout
      (
         Infragistics.Win.Misc.UltraPanel topRow,
         Infragistics.Win.Misc.UltraPanel middleRow,
         Infragistics.Win.Misc.UltraPanel bottomRow,
         System.Windows.Forms.TableLayoutPanel tabPageTableLayoutPanel
      );
   }
}
