using Infragistics.Win.Misc;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class CreateBottomRowUI
   {
      internal CreateBottomRowUI()
      {
         try
         {
            ClientsUIHolder.BottomRowTableLayoutPanel = new TableLayoutPanel();
            ClientsUIHolder.BottomRowTableLayoutPanel.ColumnCount = 4;
            ClientsUIHolder.BottomRowTableLayoutPanel.RowCount = 1;
            ClientsUIHolder.BottomRowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 80f));
            ClientsUIHolder.BottomRowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
            ClientsUIHolder.BottomRowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
            ClientsUIHolder.BottomRowTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 110));
            ClientsUIHolder.BottomRowTableLayoutPanel.Dock = DockStyle.Fill;

            ClientsUIHolder.BottomRowCloseButton = new UltraButton();
            ClientsUIHolder.BottomRowCloseButton.Text = "Salir";
            ClientsUIHolder.BottomRowCloseButton.Dock = DockStyle.Fill;
            ClientsUIHolder.BottomRowCloseButton.Click += (object sender, System.EventArgs e) =>
            {
               DialogResult result = MessageBox.Show(
               "¿Desea cerrar la aplicación?", "Confirmación",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
            );

               if(result == DialogResult.Yes)
                  MainWindowActions.CloseCompletellyAndAbruptly();
            };

            ClientsUIHolder.BottomRow.ClientArea.Controls.Add(ClientsUIHolder.BottomRowTableLayoutPanel);
            ClientsUIHolder.BottomRowTableLayoutPanel.Controls.Add(ClientsUIHolder.BottomRowCloseButton, 3, 0);
         }
         catch(Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
