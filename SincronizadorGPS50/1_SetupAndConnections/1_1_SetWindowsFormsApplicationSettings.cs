

namespace SincronizadorGPS50
{
   internal class WindowsFormsApplicationSettings
   {
      internal bool IsSuccessful { get; set; } = false;
      internal void SetInitialSettings()
      {
         try
         {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            IsSuccessful = true;
         }
         catch(System.Exception exception)
         {
            throw exception;
         }
      }
   }
}