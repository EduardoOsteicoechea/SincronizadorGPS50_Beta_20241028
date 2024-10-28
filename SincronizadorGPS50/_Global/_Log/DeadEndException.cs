using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal class DeadEndException
   {
      public DeadEndException(System.Exception exception) 
      {
         try
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         catch
         {
            System.Windows.Forms.MessageBox.Show($"Procederemos a cerrar la aplicación.");
            MainWindowActions.CloseCompletellyAndAbruptly();
         };
      }
   }
}
