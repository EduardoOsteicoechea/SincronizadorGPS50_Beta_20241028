using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface IDataTableGenerator
   {
      System.Data.DataTable CreateDataTable(List<(string columnName, string friendlyName, Type columnType, string columnDefinition, dynamic defaultValue)> tableFieldsTupleList);      
   }
}
