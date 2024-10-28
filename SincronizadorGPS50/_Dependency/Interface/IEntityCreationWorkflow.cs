using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50._Dependency.Interface
{
   internal interface IEntityCreationWorkflow<T1>
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
