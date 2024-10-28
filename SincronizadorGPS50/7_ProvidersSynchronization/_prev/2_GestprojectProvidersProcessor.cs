using Infragistics.Designers.SqlEditor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectProvidersProcessor : ISynchronizableEntityProcessor<GestprojectProviderModel, Sage50ProviderModel>
   {
      public List<GestprojectProviderModel> ProcessedEntities { get; set; }
      public bool MustBeRegistered { get; set; } = false;
      public bool MustBeSkipped { get; set; } = false;
      public bool MustBeUpdated { get; set; } = false;
      public bool MustBeDeleted { get; set; } = false;
      public SqlConnection Connection { get; set; } = null;
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<GestprojectProviderModel> GestprojectEntites { get; set; }
      public List<Sage50ProviderModel> SageEntities { get; set; }

      public List<GestprojectProviderModel> ProcessEntityList
      (
         System.Data.SqlClient.SqlConnection connection,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectProviderModel> gestprojectEntites,
         List<Sage50ProviderModel> sage50Entities
      )
      {
         try
         {
            ProcessedEntities = new List<GestprojectProviderModel>();
            GestprojectEntites = new List<GestprojectProviderModel>();
            SageEntities = new List<Sage50ProviderModel>();
            
            Connection = connection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchema;
            GestprojectEntites = gestprojectEntites;
            SageEntities = sage50Entities;

            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //   "gestprojectEntites",
            //   gestprojectEntites
            //);

            for(int i = 0; i < GestprojectEntites.Count; i++)
            {
               GestprojectProviderModel entity = GestprojectEntites[i];
               
               AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
               
               DetermineEntityWorkflow(connection, sage50ConnectionManager, tableSchema, entity);

               if(MustBeSkipped)
               {
                  continue;
               }
               else if(MustBeRegistered)
               {
                  RegisterEntity(connection, tableSchema, entity);
                  AppendSynchronizationTableDataToEntity(connection, tableSchema, entity);
                  //MessageBox.Show("Registering: " + entity.NOMBRE_COMPLETO);
               }
               else if(MustBeUpdated)
               {
                  UpdateEntity(connection, tableSchema, entity);
                  //MessageBox.Show("Updating: " + entity.NOMBRE_COMPLETO);
               };

               ValidateEntitySynchronizationStatus(connection, tableSchema, sage50Entities, entity);

               if(
                  entity.SYNC_STATUS == SynchronizationStatusOptions.EliminadoDeSage 
                     ||
                  entity.SYNC_STATUS == SynchronizationStatusOptions.EliminadoDeGestproject
               )
               {
                  DeleteEntity(connection, tableSchema, gestprojectEntites, entity);
                  ClearEntitySynchronizationData(entity, tableSchema.SynchronizationFieldsDefaultValuesTupleList);
                  RegisterEntity(connection, tableSchema, entity);
               };

               UpdateEntity(connection, tableSchema, entity);

               ProcessedEntities.Add(entity);
            };

            //new VisualizePropertiesAndValues<GestprojectProviderModel>(
            //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //   "gestprojectEntites",
            //   gestprojectEntites
            //);

            return ProcessedEntities;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public void AppendSynchronizationTableDataToEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT
                  ID
                  ,SYNC_STATUS
                  ,S50_CODE
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE
                  ,GP_USU_ID
                  ,COMMENTS
               FROM
                  {TableSchema.TableName}
               WHERE
                  S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while (reader.Read()) 
                  {
                     entity.ID = FilterSelectQueryResult(typeof(int), reader.GetValue(0),"ID",-1);
                     entity.SYNC_STATUS = FilterSelectQueryResult(typeof(string), reader.GetValue(1),"SYNC_STATUS","");
                     entity.S50_CODE = FilterSelectQueryResult(typeof(string), reader.GetValue(2),"S50_CODE","");
                     entity.S50_GUID_ID = FilterSelectQueryResult(typeof(string), reader.GetValue(3),"S50_GUID_ID","");
                     entity.S50_COMPANY_GROUP_NAME = FilterSelectQueryResult(typeof(string), reader.GetValue(4),"S50_COMPANY_GROUP_NAME","");
                     entity.S50_COMPANY_GROUP_CODE = FilterSelectQueryResult(typeof(string), reader.GetValue(5),"S50_COMPANY_GROUP_CODE","");
                     entity.S50_COMPANY_GROUP_MAIN_CODE = FilterSelectQueryResult(typeof(string), reader.GetValue(6),"S50_COMPANY_GROUP_MAIN_CODE","");
                     entity.S50_COMPANY_GROUP_GUID_ID = FilterSelectQueryResult(typeof(string), reader.GetValue(7),"S50_COMPANY_GROUP_GUID_ID","");
                     entity.LAST_UPDATE = FilterSelectQueryResult(typeof(DateTime), reader.GetValue(8),"LAST_UPDATE",DateTime.Now);
                     entity.GP_USU_ID = FilterSelectQueryResult(typeof(int), reader.GetValue(9),"GP_USU_ID",-1);
                     entity.COMMENTS = FilterSelectQueryResult(typeof(string), reader.GetValue(10),"COMMENTS","");
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      public dynamic FilterSelectQueryResult(Type valueType, dynamic value, string columnName, dynamic defaultValue = default)
      {
         try
         {
            //MessageBox.Show($"The value was: {value}. It's type was: " + value.GetType());
            //MessageBox.Show($"The valueType was: " + valueType);
            if(valueType == typeof(string))
            {
               //MessageBox.Show("The value was a string: " + value.GetType());
               return Convert.ToString(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(int))
            {
               //MessageBox.Show("The value was an integer: " + value.GetType());
               return Convert.ToInt32(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(decimal))
            {
               //MessageBox.Show("The value was a decimal: " + value.GetType());
               return Convert.ToDecimal(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(DateTime))
            {
               //MessageBox.Show("The value was a DateTime: " + value.GetType());
               return Convert.ToDateTime(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }

            throw new ArgumentException("You supplied an invalid type at: " + columnName + ". It's value was: " + value);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }


      public void DetermineEntityWorkflow(SqlConnection connection, ISage50ConnectionManager sage50ConnectionManager, ISynchronizationTableSchemaProvider tableSchema, GestprojectProviderModel entity)
      {
         try
         {
            MustBeRegistered = WasEntityRegistered(entity) == false;

            bool registeredInDifferentCompanyGroup =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            SageConnectionManager.CompanyGroupData.CompanyGuidId != entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeSkipped = registeredInDifferentCompanyGroup;

            bool neverSynchronized = entity.S50_COMPANY_GROUP_GUID_ID == "";

            bool synchronizedInThePast =
            entity.S50_COMPANY_GROUP_GUID_ID != ""
            &&
            sage50ConnectionManager.CompanyGroupData.CompanyGuidId == entity.S50_COMPANY_GROUP_GUID_ID;

            MustBeUpdated = neverSynchronized || synchronizedInThePast;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public bool WasEntityRegistered(GestprojectProviderModel entity)
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  PAR_ID
               FROM 
                  {TableSchema.TableName}
               WHERE 
                  ID=@ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@ID", entity.ID ?? -1);
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     if(reader.GetValue(0).GetType().Name != "DBNull" || System.Convert.ToString(reader.GetValue(0)) != "")
                     {
                        return true;

                     }
                  };
               };
            };
            return false;
         }
         catch(SqlException exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      public void RegisterEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectProviderModel entity
      )
      {
         try
         {
            // Located before "Connection.Open()" because this method manages it's own connection
            //entity.PAR_ID = GetLastGestprojectEntityTableId();

            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  {TableSchema.TableName}
               (
                  PAR_ID
                  ,PAR_SUBCTA_CONTABLE_2
                  ,PAR_NOMBRE
                  ,PAR_APELLIDO_1
                  ,PAR_APELLIDO_2
                  ,PAR_NOMBRE_COMERCIAL
                  ,PAR_CIF_NIF
                  ,PAR_DIRECCION_1
                  ,PAR_CP_1
                  ,PAR_LOCALIDAD_1
                  ,PAR_PROVINCIA_1
                  ,PAR_PAIS_1
                  ,S50_CODE
                  ,S50_GUID_ID
               )
               Values
               (
                  @PAR_ID
                  ,@PAR_SUBCTA_CONTABLE_2
                  ,@PAR_NOMBRE
                  ,@PAR_APELLIDO_1
                  ,@PAR_APELLIDO_2
                  ,@PAR_NOMBRE_COMERCIAL
                  ,@PAR_CIF_NIF
                  ,@PAR_DIRECCION_1
                  ,@PAR_CP_1
                  ,@PAR_LOCALIDAD_1
                  ,@PAR_PROVINCIA_1
                  ,@PAR_PAIS_1
                  ,@S50_CODE
                  ,@S50_GUID_ID
               )
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            { 
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@PAR_SUBCTA_CONTABLE_2", entity.PAR_SUBCTA_CONTABLE_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE", entity.PAR_NOMBRE);
               command.Parameters.AddWithValue("@PAR_APELLIDO_1", entity.PAR_APELLIDO_1);
               command.Parameters.AddWithValue("@PAR_APELLIDO_2", entity.PAR_APELLIDO_2);
               command.Parameters.AddWithValue("@PAR_NOMBRE_COMERCIAL", entity.PAR_NOMBRE_COMERCIAL);
               command.Parameters.AddWithValue("@PAR_CIF_NIF", entity.PAR_CIF_NIF);
               command.Parameters.AddWithValue("@PAR_DIRECCION_1", entity.PAR_DIRECCION_1);
               command.Parameters.AddWithValue("@PAR_CP_1", entity.PAR_CP_1);
               command.Parameters.AddWithValue("@PAR_LOCALIDAD_1", entity.PAR_LOCALIDAD_1);
               command.Parameters.AddWithValue("@PAR_PROVINCIA_1", entity.PAR_PROVINCIA_1);
               command.Parameters.AddWithValue("@PAR_PAIS_1", entity.PAR_PAIS_1);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               command.ExecuteNonQuery();
            };
         }
         catch(SqlException exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

       public int GetLastGestprojectEntityTableId()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT 
                  MAX(PAR_ID)
               FROM
                  {TableSchema.GestprojectEntityTableName}
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while (reader.Read()) 
                  {
                     return Convert.ToInt32(reader.GetValue(0)) + 1;
                  }
               };
            };

            throw new Exception();
         }
         catch(SqlException exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }



      public void UpdateEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectProviderModel entity
      )
      {
         try
         {        
            Connection.Open();

            string sqlString = $@"
            UPDATE 
               {TableSchema.TableName}
            SET
               SYNC_STATUS=@SYNC_STATUS
               ,S50_CODE=@S50_CODE
               ,S50_GUID_ID=@S50_GUID_ID
            WHERE
               ID=@ID
            ;";   

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@ID", entity.ID);
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS ?? "");
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE ?? "");
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID ?? "");

               command.ExecuteNonQuery();
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      //public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50ProviderModel> sage50Entities, GestprojectProviderModel entity)
      //{
      //   try
      //   {
      //      ValidateProviderSyncronizationStatus ProviderSyncronizationStatusValidator = new ValidateProviderSyncronizationStatus(
      //         entity,
      //         sage50Entities,
      //         tableSchema.FullName.ColumnDatabaseName,
      //         tableSchema.Cif.ColumnDatabaseName,
      //         tableSchema.Address.ColumnDatabaseName,
      //         tableSchema.PostalCode.ColumnDatabaseName,
      //         tableSchema.Province.ColumnDatabaseName
      //      );

      //      MustBeDeleted = ProviderSyncronizationStatusValidator.MustBeDeleted;
      //   }
      //   catch(System.Exception exception)
      //   {
      //      throw ApplicationLogger.ReportError(
      //         MethodBase.GetCurrentMethod().DeclaringType.Namespace,
      //         MethodBase.GetCurrentMethod().DeclaringType.Name,
      //         MethodBase.GetCurrentMethod().Name,
      //         exception
      //      );
      //   };
      //}

      public void ValidateEntitySynchronizationStatus(SqlConnection connection, ISynchronizationTableSchemaProvider tableSchema, List<Sage50ProviderModel> sage50Entities, GestprojectProviderModel entity)
      {
         try
         {
            string statusTracker = SynchronizationStatusOptions.Sincronizado;

            if(entity.PAR_ID == -1 || entity.S50_COMPANY_GROUP_GUID_ID == "")
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
               statusTracker = SynchronizationStatusOptions.Desincronizado;
            }

            if(IsSynchronizedWithASageEntity(entity, sage50Entities) == false)
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
               statusTracker = SynchronizationStatusOptions.Desincronizado;
            }

            if(WasRegisteredButDeletedFromSage(entity, sage50Entities) == true)
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.EliminadoDeSage;
               statusTracker = SynchronizationStatusOptions.EliminadoDeSage;
            }

            if(WasRegisteredButDeletedFromGestproject(entity) == true)
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.EliminadoDeGestproject;
               statusTracker = SynchronizationStatusOptions.EliminadoDeGestproject;
            }

            if(statusTracker == SynchronizationStatusOptions.Sincronizado)
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
            }
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public bool IsSynchronizedWithASageEntity
      (
         GestprojectProviderModel entity, 
         List<Sage50ProviderModel> sage50Entities
      )
      {
         try
         {
            Sage50ProviderModel sageEntity = sage50Entities.FirstOrDefault(
               currentSageEntity 
                  => 
               currentSageEntity.CODIGO == entity.PAR_SUBCTA_CONTABLE_2
                  &&
               currentSageEntity.CIF == entity.PAR_CIF_NIF
                  &&
               currentSageEntity.NOMBRE == entity.NOMBRE_COMPLETO
                  &&
               currentSageEntity.DIRECCION == entity.PAR_DIRECCION_1
                  &&
               currentSageEntity.CODPOST == entity.PAR_CP_1
                  &&
               currentSageEntity.POBLACION == entity.PAR_LOCALIDAD_1
                  &&
               currentSageEntity.PROVINCIA == entity.PAR_PROVINCIA_1
               //   &&
               //currentSageEntity.GUID_ID == entity.S50_GUID_ID
            );

            return sageEntity != null;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public bool WasRegisteredButDeletedFromSage
      (
         GestprojectProviderModel entity, 
         List<Sage50ProviderModel> sage50Entities
      )
      {
         try
         {
            if(entity.S50_GUID_ID != "")
            {
               Sage50ProviderModel sageEntity = sage50Entities.FirstOrDefault(
                  currentSageEntity => currentSageEntity.GUID_ID == entity.S50_GUID_ID
               );

               return sageEntity == null;
            }
            return false;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public bool WasRegisteredButDeletedFromGestproject
      (
         GestprojectProviderModel entity
      )
      {
         try
         {
            if(entity.S50_COMPANY_GROUP_GUID_ID != "")
            {
               return entity.PAR_ID == -1;
            }
            return false;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public void DeleteEntity
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         List<GestprojectProviderModel> gestprojectEntites, 
         GestprojectProviderModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            DELETE FROM
               PARTICIPANTE
            WHERE
               ID=@ID
            ;";
   
            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@ID", entity.PAR_ID);
               command.ExecuteNonQuery();
            }
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }

      public void ClearEntitySynchronizationData(GestprojectProviderModel entity, List<(string propertyName, dynamic defaultValue)> entityPropertiesValuesTupleList)
      {
         try
         {
            entity.ID = null;
            entity.SYNC_STATUS = "";
            entity.S50_CODE = "";
            entity.S50_GUID_ID = "";
            entity.S50_COMPANY_GROUP_CODE = "";
            entity.S50_COMPANY_GROUP_CODE = "";
            entity.S50_COMPANY_GROUP_MAIN_CODE = "";
            entity.S50_GUID_ID = "";
            entity.COMMENTS = "";
            entity.GP_USU_ID = null;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}