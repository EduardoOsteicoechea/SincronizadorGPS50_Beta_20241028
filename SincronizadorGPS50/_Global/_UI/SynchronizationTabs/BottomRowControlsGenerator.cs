using Infragistics.Win.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class BottomRowControlsGenerator<T1, T2> : ITabPageLayoutPanelBottomRowControlsGenerator<T1, T2>
   {
      public TableLayoutPanel RowTableLayout { get; set; }
      public UltraButton ExitButton { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public ISage50ConnectionManager Sage50ConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider SynchronizationTableSchemaProvider { get; set; }
      public T1 TypeStore1 { get; set; }
      public T2 TypeStore2 { get; set; }

      public void GenerateControls
      (
         UltraPanel rowPanel,
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider
      )
      {
         GestprojectConnectionManager = gestprojectConnectionManager;
         Sage50ConnectionManager = sage50ConnectionManager;
         SynchronizationTableSchemaProvider = synchronizationTableSchemaProvider;
         GenerateRowTableLayoutPanel();
         GenerateExitButtonControl();
         SetExitButtonClickEventHandler();
         AddButtonsToRowTableLayoutPanel();
         AddRowTableLayoutPanelToRowPanel(rowPanel);
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
      public void GenerateExitButtonControl()
      {
         ExitButton = new UltraButton();
         ExitButton.Text = "Salir";
         ExitButton.Dock = DockStyle.Fill;
      }
      public void SetExitButtonClickEventHandler()
      {
         ExitButton.Click += ExitButtonClickEventHandler;
      }
      public void ExitButtonClickEventHandler(object sender, EventArgs e)
      {
         DialogResult result = MessageBox.Show(
               "¿Desea cerrar la aplicación?", "Confirmación",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
            );

         if(result == DialogResult.Yes)
            MainWindowActions.CloseCompletellyAndAbruptly();
      }
      public void AddButtonsToRowTableLayoutPanel()
      {
         RowTableLayout.Controls.Add(ExitButton, 3, 0);
      }
      public void AddRowTableLayoutPanelToRowPanel(UltraPanel rowPanel)
      {
         rowPanel.ClientArea.Controls.Add(RowTableLayout);
      }
   }
}
