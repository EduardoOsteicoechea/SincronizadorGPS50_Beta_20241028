using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class TabPageMainPanelTableLayoutPanelGenerator : ITabPageMainPanelTableLayoutPanelGenerator
   {
      public TableLayoutPanel GenerateGlobalTableLayoutPanel()
      {
         TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
         tableLayoutPanel.ColumnCount = 1;
         tableLayoutPanel.RowCount = 3;
         tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
         tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 87.50f));
         tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
         tableLayoutPanel.Dock = DockStyle.Fill;
         return tableLayoutPanel;
      }
   }
}
