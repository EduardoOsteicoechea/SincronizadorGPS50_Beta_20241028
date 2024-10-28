

namespace SincronizadorGPS50
{
   internal class ApplicationGlobalStyles
   {
      internal void ApplyGestprojectCurrentStyle()
      {
         try
         {
            Infragistics.Win.AppStyling.StyleManager.Load(GestprojectDataHolder.GestprojectLocalDeviceUserData.StyleFilePath);
         } 
         catch (System.Exception exception) 
         {
            throw new System.Exception("ApplyGestprojectGlobalStyle. Ln.11: \n\n" + exception.Message);
         }
      }
   }
}
