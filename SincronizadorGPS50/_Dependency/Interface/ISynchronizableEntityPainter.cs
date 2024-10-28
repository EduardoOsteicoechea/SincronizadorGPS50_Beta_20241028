using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface ISynchronizableEntityPainter<T1>
   {
      void PaintEntityListOnDataTable(
         List<T1> proccessedGestprojectEntities,
         DataTable dataTable,
         List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> tableFieldsTupleList
      );
   }
}
