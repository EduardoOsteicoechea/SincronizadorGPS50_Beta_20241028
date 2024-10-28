using Infragistics.Win.Misc;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class GenerateCenterRowUI
   {
      internal GenerateCenterRowUI()
      {
         try
         {
            ///////////////////////////
            //LeftPanel
            ///////////////////////////

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowLeftPanel = new UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowLeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRowLeftPanel, 0, 0);

            ///////////////////////////
            // RightPanel
            ///////////////////////////

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowRightPanel = new UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRowRightPanel, 2, 0);

            ///////////////////////////
            // CenterPanel
            ///////////////////////////

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanel = new UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel = new TableLayoutPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.ColumnCount = 1;
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowCount = 8;
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.Dock = DockStyle.Fill;

            double componentHeight = 580;
            int keyButtonHeight = 40;

            // 0.Separator
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 12));
            // 1.ConnectionStatus
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(componentHeight * .10)));
            // 2.Terminal Connection Data
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(componentHeight * .18)));
            // 3.Validate terminal
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, keyButtonHeight));
            // 4.Select enterpryse group
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(componentHeight * .10)));
            // 5.Remember Data
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(keyButtonHeight * .7)));
            // 6.Connect Button - Manage Connection Button
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, keyButtonHeight ));
            // 7.Separator
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));

            //Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanel.ClientArea.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanelTableLayoutPanel);
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRowCenterPanel, 1, 0);
         }
         catch(System.Exception exception) { throw exception; };
      }
   }
}
