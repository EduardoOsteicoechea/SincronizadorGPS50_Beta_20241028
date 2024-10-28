//using Infragistics.Win.Misc;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class GenerateSage50ConnectionTabPageUI
   {
      internal GenerateSage50ConnectionTabPageUI()
      {
         try
         {
            MainWindowUIHolder.MainTabControlMainPanel = new Infragistics.Win.Misc.UltraPanel();
            MainWindowUIHolder.MainTabControlMainPanel.Dock = DockStyle.Fill;

            MainWindowUIHolder.MainWindowTableLayoutPanel = new TableLayoutPanel();
            MainWindowUIHolder.MainWindowTableLayoutPanel.ColumnCount = 1;
            MainWindowUIHolder.MainWindowTableLayoutPanel.RowCount = 3;
            MainWindowUIHolder.MainWindowTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            //MainWindowUIHolder.MainWindowTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 87.50f));
            MainWindowUIHolder.MainWindowTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 580)); // Mínimal height in 14 inch 768px screen
            MainWindowUIHolder.MainWindowTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            MainWindowUIHolder.MainWindowTableLayoutPanel.Dock = DockStyle.Fill;

            MainWindowUIHolder.MainTabControlMainPanel.ClientArea.Controls.Add(MainWindowUIHolder.MainWindowTableLayoutPanel);
            MainWindowUIHolder.Sage50ConnectionTab.TabPage.Controls.Add(MainWindowUIHolder.MainTabControlMainPanel);

            // TopRow;
            // TopRow;
            // TopRow;
            // TopRow;
            // TopRow;

            Sage50ConnectionUIHolder.Sage50ConnectionTopRow = new Infragistics.Win.Misc.UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionTopRow.Dock = System.Windows.Forms.DockStyle.Fill;
            //Sage50ConnectionUIHolder.Sage50ConnectionTopRow.Appearance.BackColor = StyleHolder.c_transparent;

            MainWindowUIHolder.MainWindowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionTopRow, 0, 0);

            // CenterRow
            // CenterRow
            // CenterRow
            // CenterRow
            // CenterRow

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel = new TableLayoutPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.ColumnCount = 3;
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.RowCount = 1;
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Convert.ToInt32(Math.Round(StyleHolder.ScreenWorkableWidth * .30))));
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel.Dock = DockStyle.Fill;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRow = new Infragistics.Win.Misc.UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRow.Height = StyleHolder.CenterRowHeight;
            Sage50ConnectionUIHolder.Sage50ConnectionCenterRow.Dock = System.Windows.Forms.DockStyle.Fill;
            //Sage50ConnectionUIHolder.Sage50ConnectionCenterRow.Appearance.BackColor = StyleHolder.c_transparent;

            Sage50ConnectionUIHolder.Sage50ConnectionCenterRow.ClientArea.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRowTableLayoutPanel);
            MainWindowUIHolder.MainWindowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionCenterRow, 0, 1);

            // BottomRow
            // BottomRow
            // BottomRow
            // BottomRow
            // BottomRow

            Sage50ConnectionUIHolder.Sage50ConnectionBottomRow = new Infragistics.Win.Misc.UltraPanel();
            Sage50ConnectionUIHolder.Sage50ConnectionBottomRow.Dock = System.Windows.Forms.DockStyle.Fill;
            //Sage50ConnectionUIHolder.Sage50ConnectionBottomRow.Appearance.BackColor = StyleHolder.c_transparent;

            MainWindowUIHolder.MainWindowTableLayoutPanel.Controls.Add(Sage50ConnectionUIHolder.Sage50ConnectionBottomRow, 0, 2);
         }
         catch(System.Exception exception)
         {
            throw exception;
         }
      }
   }
}