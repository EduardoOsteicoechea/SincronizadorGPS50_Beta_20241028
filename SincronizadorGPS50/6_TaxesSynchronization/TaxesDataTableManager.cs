using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class TaxesDataTableManager : IGridDataSourceGenerator<GestprojectTaxModel, Sage50TaxModel>
   {
      public List<Sage50TaxModel> SageEntities { get; set; }
      public SynchronizableTaxModel SynchronizableEntity { get; set; }
      public List<SynchronizableTaxModel> SynchronizableEntities { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; } = null;
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public DataTable DataTable { get; set; }

      public System.Data.DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = gestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchemaProvider;
            SynchronizableEntities = new List<SynchronizableTaxModel>();
            SageEntities = new List<Sage50TaxModel>();

            ManageSynchronizationTableStatus(tableSchemaProvider);            

            if(SageHasIvas() && SageHasIrpfs())
            {
               SageEntities.AddRange(GetSageIvas());
               SageEntities.AddRange(GetSageIrpfs());

               foreach(Sage50TaxModel sageEntity in SageEntities)
               {
                  if(SynchronizationTableHasRegistries())
                  {
                     if(EntityIsAlreadyRegistered(sageEntity))
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);
                     }
                     else
                     {
                        SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);

                        SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                        RegisterEntityOnSynchronizationTable(SynchronizableEntity);

                     }
                  }
                  else
                  {
                     SynchronizableEntity = GenerateSynchronizableEntityFromSageEntity(sageEntity);

                     SetEntitySynchronizationStatusToUntransferred(SynchronizableEntity);

                     RegisterEntityOnSynchronizationTable(SynchronizableEntity);
                  };

                  AppendSynchronizationTableDataToEntity(SynchronizableEntity);

                  SynchronizableEntities.Add(SynchronizableEntity);
               }
            }
            else
            {
               MessageBox.Show("No encontramos impuestos en Sage.");
            }

            CreateAndDefineDataSource();
            PaintEntitiesOnDataSource();
            return DataTable;
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


      public void ManageSynchronizationTableStatus
      (
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider.TableName
            );

         if(tableExists == false)
         {
            entitySyncronizationTableStatusManager.CreateTable
            (
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider
            );
         };
      }


      public bool SageHasIvas()
      {

         try
         {
            List<Sage50TaxModel> entities = new List<Sage50TaxModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {DB.SQLDatabase("gestion","tipo_iva")})
            BEGIN
               SELECT
                  guid_id 
               FROM 
                  {DB.SQLDatabase("gestion","tipo_iva")}
            END
            ;";

            DataTable enentiesDataTable = new DataTable();

            DB.SQLExec(sqlString, ref enentiesDataTable);

            if(enentiesDataTable.Rows.Count > 0)
            {
               return true;
            };
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


      public bool SageHasIrpfs()
      {

         try
         {
            List<Sage50TaxModel> entities = new List<Sage50TaxModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {DB.SQLDatabase("gestion","tipo_ret")})
            BEGIN
               SELECT
                  guid_id 
               FROM 
                  {DB.SQLDatabase("gestion","tipo_ret")}
            END
            ;";

            DataTable enentiesDataTable = new DataTable();

            DB.SQLExec(sqlString, ref enentiesDataTable);

            if(enentiesDataTable.Rows.Count > 0)
            {
               return true;
            };
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


      public List<Sage50TaxModel> GetSageIvas()
      {
         try
         {
            List<Sage50TaxModel> entities = new List<Sage50TaxModel>();
          
            string sqlString1 = $@"
            SELECT 
               GUID_ID,
               IVA,
               NOMBRE,
               CTA_IV_REP,
               CTA_IV_SOP,
               CODIGO
            FROM {DB.SQLDatabase("gestion","tipo_iva")}
            ;";

            DataTable table1 = new DataTable();

            DB.SQLExec(sqlString1, ref table1);

            if(table1.Rows.Count > 0)
            {
               for(int i = 0; i < table1.Rows.Count; i++)
               {
                  Sage50TaxModel sage50Entity = new Sage50TaxModel();

                  sage50Entity.GUID_ID = table1.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Entity.IVA = Convert.ToString(table1.Rows[i].ItemArray[1]);
                  sage50Entity.NOMBRE = table1.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Entity.CTA_IV_REP = table1.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Entity.CTA_IV_SOP = table1.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Entity.CODIGO = table1.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Entity.IMP_TIPO = "IVA";

                  entities.Add(sage50Entity);
               };
            };

            return entities;
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


      public List<Sage50TaxModel> GetSageIrpfs()
      {
         try
         {
            List<Sage50TaxModel> entities = new List<Sage50TaxModel>();

            string sqlString2 = $@"
            SELECT 
               guid_id,
               nombre,
               retencion,
               cta_re_rep,
               cta_re_sop,
               codigo
            FROM 
               {DB.SQLDatabase("gestion","tipo_ret")}
            ;";

            DataTable table2 = new DataTable();

            DB.SQLExec(sqlString2, ref table2);

            if(table2.Rows.Count > 0)
            {
               for(int i = 0; i < table2.Rows.Count; i++)
               {
                  Sage50TaxModel sage50Entity = new Sage50TaxModel();

                  sage50Entity.GUID_ID = table2.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Entity.NOMBRE = table2.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Entity.RETENCION = Convert.ToDecimal(table2.Rows[i].ItemArray[2]);
                  sage50Entity.CTA_RE_REP = table2.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Entity.CTA_RE_SOP = table2.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Entity.CODIGO = table2.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Entity.IMP_TIPO = "IRPF";

                  entities.Add(sage50Entity);
               };
            };

            return entities;
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


      public bool SynchronizationTableHasRegistries()
      {
         try
         {
            Connection.Open();

            List<Sage50TaxModel> entities = new List<Sage50TaxModel>();

            string sqlString = $@"
            IF EXISTS(SELECT 1 FROM {TableSchema.TableName})
            BEGIN
               SELECT
                  ID 
               FROM 
                  {TableSchema.TableName}
            END
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  };
               };
            };

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
         }
         finally
         {
            Connection.Close();
         }
      }


      public SynchronizableTaxModel GenerateSynchronizableEntityFromSageEntity
      (
         Sage50TaxModel sageEntity
      )
      {
         try
         {
            SynchronizableTaxModel synchronizableEntity = new SynchronizableTaxModel();

            synchronizableEntity.IMP_ID = -1;
            synchronizableEntity.IMP_TIPO = sageEntity.IMP_TIPO;
            synchronizableEntity.IMP_DESCRIPCION = sageEntity.NOMBRE.Trim();

            if(sageEntity.IMP_TIPO == "IVA")
            {
               synchronizableEntity.IMP_NOMBRE = $"{sageEntity.IMP_TIPO} {sageEntity.IVA.ToString().Split(',')[0]} {((synchronizableEntity.IMP_DESCRIPCION.Split(' ').Select(parte => parte[0]).Aggregate("", (accumulated, current) => accumulated + current)).ToUpper()).Substring(1)}";
               synchronizableEntity.IMP_VALOR = Convert.ToDecimal(sageEntity.IVA);
               synchronizableEntity.IMP_SUBCTA_CONTABLE = sageEntity.CTA_IV_REP;
               synchronizableEntity.IMP_SUBCTA_CONTABLE_2 = sageEntity.CTA_IV_SOP;
            }
            else
            {
               synchronizableEntity.IMP_NOMBRE = $"{sageEntity.IMP_TIPO} {sageEntity.RETENCION.ToString().Split(',')[0]}";
               synchronizableEntity.IMP_VALOR = sageEntity.RETENCION;
               synchronizableEntity.IMP_SUBCTA_CONTABLE = sageEntity.CTA_RE_REP;
               synchronizableEntity.IMP_SUBCTA_CONTABLE_2 = sageEntity.CTA_RE_SOP;
            };

            synchronizableEntity.S50_CODE = sageEntity.CODIGO;
            synchronizableEntity.S50_GUID_ID = sageEntity.GUID_ID;

            return synchronizableEntity;
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


      public bool EntityIsAlreadyRegistered
      (
         Sage50TaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT
                  ID
               FROM
                  {TableSchema.TableName}
               WHERE
                  S50_GUID_ID=@S50_GUID_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return true;
                  };
               };
            };

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
         }
         finally
         {
            Connection.Close();
         }
      }


      public void SetEntitySynchronizationStatusToUntransferred
      (
         SynchronizableTaxModel entity
      )
      {
         try
         {
            entity.SYNC_STATUS = SynchronizationStatusOptions.NoTransferido;
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


      public bool RegisterEntityOnSynchronizationTable
      (
         SynchronizableTaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  {TableSchema.TableName}
               (
                  SYNC_STATUS
                  ,IMP_ID
                  ,IMP_TIPO
                  ,IMP_NOMBRE
                  ,IMP_DESCRIPCION
                  ,IMP_VALOR
                  ,IMP_SUBCTA_CONTABLE
                  ,IMP_SUBCTA_CONTABLE_2
                  ,S50_CODE
                  ,S50_GUID_ID
               )
               VALUES
               (
                  @SYNC_STATUS
                  ,@IMP_ID
                  ,@IMP_TIPO
                  ,@IMP_NOMBRE
                  ,@IMP_DESCRIPCION
                  ,@IMP_VALOR
                  ,@IMP_SUBCTA_CONTABLE
                  ,@IMP_SUBCTA_CONTABLE_2
                  ,@S50_CODE
                  ,@S50_GUID_ID
               )
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@IMP_ID", entity.IMP_ID);
               command.Parameters.AddWithValue("@IMP_TIPO", entity.IMP_TIPO);
               command.Parameters.AddWithValue("@IMP_NOMBRE", entity.IMP_NOMBRE);
               command.Parameters.AddWithValue("@IMP_DESCRIPCION", entity.IMP_DESCRIPCION);
               command.Parameters.AddWithValue("@IMP_VALOR", entity.IMP_VALOR);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE", entity.IMP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@IMP_SUBCTA_CONTABLE_2", entity.IMP_SUBCTA_CONTABLE_2);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               command.ExecuteNonQuery();
            };

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
         }
         finally
         {
            Connection.Close();
         };
      }


      public void AppendSynchronizationTableDataToEntity
      (
         SynchronizableTaxModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT
                  ID
                  ,SYNC_STATUS
                  ,IMP_ID
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

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.IMP_ID = reader["IMP_ID"] as int?;
                     entity.S50_CODE = reader["S50_CODE"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;
                  }
               }
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
         }
      }


      public void CreateAndDefineDataSource()
      {
         try
         {
            IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
            DataTable = entityDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
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


      public void PaintEntitiesOnDataSource()
      {
         try
         {
            ISynchronizableEntityPainter<SynchronizableTaxModel> entityPainter = new EntityPainter<SynchronizableTaxModel>();
            entityPainter.PaintEntityListOnDataTable(
               SynchronizableEntities,
               DataTable,
               TableSchema.ColumnsTuplesList
            );
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