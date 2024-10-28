

namespace SincronizadorGPS50
{
   internal interface IGridDataSourceGenerator<T1,T2>
   {
      System.Data.DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      );
   }
}
