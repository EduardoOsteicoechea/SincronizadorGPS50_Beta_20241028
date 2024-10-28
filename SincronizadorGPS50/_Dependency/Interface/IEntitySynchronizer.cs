using System;
using System.Collections.Generic;

namespace SincronizadorGPS50
{
   internal interface IEntitySynchronizer<T1, T2>
   {
      void Synchronize
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider,
         List<int> selectedIdList
      );
   }
}
