using Infragistics.Win.UltraWinTabControl;

namespace SincronizadorGPS50
{
   public interface IEntitySynchronizationManager
   {
      void Launch
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         UltraTab hostTab
      );
   }
}