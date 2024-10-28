namespace SincronizadorGPS50
{
   internal static class Program
   {
      [System.STAThreadAttribute]
      internal static void Main()
      {
         try
         {
            System.Windows.Forms.Application.Run(new SetupAndConnections());
         }
         catch (System.Exception exception)
         {
            new DeadEndException(exception);
         };
      }
   }
}