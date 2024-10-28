using Infragistics.Win.Misc;
namespace SincronizadorGPS50
{
   internal class TabPageLayoutPanelRowGenerator : ITabPageLayoutPanelRowGenerator
   {
      public UltraPanel RowPanel { get; set; }

      public UltraPanel GenerateRowPanel()
      {
         RowPanel = new UltraPanel();
         RowPanel.Height = StyleHolder.CenterRowHeight;
         RowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         return RowPanel;
      }
   }
}
