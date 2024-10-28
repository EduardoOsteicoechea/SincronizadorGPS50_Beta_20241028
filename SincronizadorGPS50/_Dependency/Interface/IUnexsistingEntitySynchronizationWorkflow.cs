using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface IUnexsistingEntitySynchronizationWorkflow<T1>
   {
      List<T1> ExistingEntities { get; set; }
      List<T1> UnexistingEntities { get; set; }
      void Execute
      (
         IGestprojectConnectionManager GestprojectConnectionManager,
         ISage50ConnectionManager Sage50ConnectionManager,
         List<T1> entityList,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      );
   }
}
