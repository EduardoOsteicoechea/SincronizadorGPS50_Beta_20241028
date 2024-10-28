using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface ISynchronizableEntityProcessor<T1, T2>
   {
      List<T1> ProcessedEntities { get; set; }
      List<T1> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<T1> gestprojectEntites,
         List<T2> sage50Entities
      );
      void AppendSynchronizationTableDataToEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         T1 entity
      );

      bool MustBeRegistered { get; set; }
      bool MustBeSkipped { get; set; }
      bool MustBeUpdated { get; set; }
      void DetermineEntityWorkflow
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         T1 entity
      );

      void RegisterEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         T1 entity
      );

      void UpdateEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         T1 entity
      );

      bool MustBeDeleted { get; set; }
      void ValidateEntitySynchronizationStatus
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         List<T2> sage50Entities,
         T1 entity
      );

      void DeleteEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         List<T1> gestprojectEntites,
         T1 entity
      );
      void ClearEntitySynchronizationData
      (
         T1 entity, 
         List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList
      );
   }
}
