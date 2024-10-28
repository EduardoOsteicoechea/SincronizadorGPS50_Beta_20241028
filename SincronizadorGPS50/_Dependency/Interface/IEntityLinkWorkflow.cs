using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal interface IEntityLinkWorkflow<T1>
   {
      void Execute
      (
         IGestprojectConnectionManager GestprojectConnectionManager,
         ISage50ConnectionManager Sage50ConnectionManager,
         T1 entity,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      );
   }
}
