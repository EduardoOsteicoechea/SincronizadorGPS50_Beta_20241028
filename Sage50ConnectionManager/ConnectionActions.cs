using System.Windows.Forms;

namespace SincronizadorGPS50.Sage50Connector
{
   public static class ConnectionActions
   {
      public static LinkSage50 Sage50ConnectionManager { get; set; } = null;
      public static bool Connect(string Sage50LocalTerminalPath, string Sage50Username, string Sage50Password)
      {
         try
         {
            Sage50ConnectionManager = new LinkSage50(Sage50LocalTerminalPath);

            return Sage50ConnectionManager._Connect(Sage50Username, Sage50Password);
         }
         catch(System.Exception ex)
         {
            MessageBox.Show(ex.Message);
            return false;
         };
      }

      public static void Disconnect()
      {
         Sage50ConnectionManager._Disconnect();
         Sage50ConnectionManager = null;
      }
   }
}
