using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public delegate System.Data.DataTable DataTableGeneratorDelegate
   (
      IGestprojectConnectionManager gestprojectConnectionManager,
      ISage50ConnectionManager sage50ConnectionManager,
      ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider
   );
}
