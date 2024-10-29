using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ReceivedBillsSynchronizer : IEntitySynchronizer<GestprojectReceivedBillModel, Sage50ReceivedBillModel>
   {
      public string EntityTypeNameSingularArticle { get; set; } = "la";
      public string EntityTypeNamePluralArticle { get; set; } = "las";
      public string EntityTypeNameRoot { get; set; } = "factur";
      public string EntityTypeNameGender { get; set; } = "a";
      public string EntityTypeNamePlural { get; set; } = "s";
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public string SynchronizableEntityDetailsTable { get; set; } = "INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES";
      public List<SincronizadorGP50ReceivedInvoiceModel> SynchronizadorGPS50ReceivedInvoices { get; set; }
      public List<SincronizadorGPS50ReceivedInvoiceDetailModel> SynchronizadorGPS50ReceivedInvoicesDetails { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }

      public void Synchronize
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<int> selectedIdList
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchema;
            SynchronizadorGPS50ReceivedInvoices = new List<SincronizadorGP50ReceivedInvoiceModel>();
            SynchronizadorGPS50ReceivedInvoicesDetails = new List<SincronizadorGPS50ReceivedInvoiceDetailModel>();

            GetEntitiesFromSyncronizationTable();
            GetEntitiesDetailsFromSincronizationTable();

            foreach(SincronizadorGP50ReceivedInvoiceModel entity in SynchronizadorGPS50ReceivedInvoices)
            {
               if(ValidateIfSynchronizableEntityExistOnGestproject(entity) == false)
               {
                  TransferSynchronizationData(entity);
                  SetEntityAsTransfered(entity);
               }
               else
               {
                  MessageBox.Show($"{EntityTypeNameSingularArticle.Replace(EntityTypeNameSingularArticle[0],EntityTypeNameSingularArticle[0].ToString().ToUpper()[0])} {EntityTypeNameRoot + EntityTypeNameGender} \"{entity.FCP_NUM_FACTURA.Trim()}\" de Sage50 ya fue transferida.");
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
      }


      public void GetEntitiesFromSyncronizationTable()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT * FROM {TableSchema.TableName}
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SincronizadorGP50ReceivedInvoiceModel entity = new SincronizadorGP50ReceivedInvoiceModel();

                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.FCP_ID = reader["FCP_ID"] as int?;
                     entity.PAR_DAO_ID = reader["PAR_DAO_ID"] as int?;
                     entity.FCP_NUM_FACTURA = reader["FCP_NUM_FACTURA"] as string;
                     entity.FCP_FECHA = reader["FCP_FECHA"] as DateTime?;
                     entity.PAR_PRO_ID = reader["PAR_PRO_ID"] as int?;
                     entity.FCP_SUBCTA_CONTABLE = reader["FCP_SUBCTA_CONTABLE"] as string;
                     entity.FCP_BASE_IMPONIBLE = reader["FCP_BASE_IMPONIBLE"] as decimal?;
                     entity.FCP_VALOR_IVA = reader["FCP_VALOR_IVA"] as decimal?;
                     entity.FCP_IVA = reader["FCP_IVA"] as decimal?;
                     entity.FCP_VALOR_IRPF = reader["FCP_VALOR_IRPF"] as decimal?;
                     entity.FCP_IRPF = reader["FCP_IRPF"] as decimal?;
                     entity.FCP_TOTAL_FACTURA = reader["FCP_TOTAL_FACTURA"] as decimal?;
                     entity.FCP_OBSERVACIONES = reader["FCP_OBSERVACIONES"] as string;
                     entity.PRY_ID = reader["PRY_ID"] as int?;
                     entity.FCP_EJERCICIO = reader["FCP_EJERCICIO"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;

                     SynchronizadorGPS50ReceivedInvoices.Add(entity);
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


      public void GetEntitiesDetailsFromSincronizationTable()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               SELECT * FROM {SynchronizableEntityDetailsTable}
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SincronizadorGPS50ReceivedInvoiceDetailModel entity = new SincronizadorGPS50ReceivedInvoiceDetailModel();

                     entity.ID = reader["ID"] as int?;
                     entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entity.DFP_ID = reader["DFP_ID"] as int?;
                     entity.DFP_CONCEPTO = reader["DFP_CONCEPTO"] as string;
                     entity.DFP_PRECIO_UNIDAD = reader["DFP_PRECIO_UNIDAD"] as decimal?;
                     entity.DFP_UNIDADES = reader["DFP_UNIDADES"] as decimal?;
                     entity.DFP_SUBTOTAL = reader["DFP_SUBTOTAL"] as decimal?;
                     entity.PRY_ID = reader["PRY_ID"] as int?;
                     entity.FCP_ID = reader["FCP_ID"] as int?;
                     entity.DFP_ESTRUCTURAL = reader["DFP_ESTRUCTURAL"] as string;
                     entity.INVOICE_GUID_ID = reader["INVOICE_GUID_ID"] as string;
                     entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entity.COMMENTS = reader["COMMENTS"] as string;

                     SynchronizadorGPS50ReceivedInvoicesDetails.Add(entity);
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


      public void TransferSynchronizationData
      (
         SincronizadorGP50ReceivedInvoiceModel entity
      )
      {
         try
         {
            if(ValidateIfSynchronizableEntityExistOnGestproject(entity) == false)
            {
               RecordSynchronizableEntityOnGestproject(entity, GetNextAvailableEntityId());
               GetRecordedSynchronizableEntityGestprojectId(entity);
               RegisterSynchronizableEntityGestprojectId(entity);

               foreach(
                  SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail in SynchronizadorGPS50ReceivedInvoicesDetails
                  .Where(entityDetail => entityDetail.INVOICE_GUID_ID == entity.S50_GUID_ID)
               )
               {
                  AppendSynchronizableEntityGestprojectIdToDetails(entityDetail, entity);
                  RecordSynchronizableEntityDetailsOnGestproject(entityDetail, GetNextAvailableEntityDetailId());
                  GetRecordedSynchronizableEntityDetailsGestprojectId(entityDetail);
                  RegisterSynchronizableEntityDetailsGestprojectId(entityDetail);
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
         };
      }

      public int? GetNextAvailableEntityId()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ISNULL(MAX(FCP_ID),-1)
            FROM
               FACTURA_PROVEEDOR
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return Convert.ToInt32(reader.GetValue(0)) == -1 ? 1 : Convert.ToInt32(reader.GetValue(0)) + 1;
                  };
               };
            };

            return 1;
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

      public int? GetNextAvailableEntityDetailId()
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               ISNULL(MAX(DFP_ID),-1)
            FROM
               DETALLE_FACTURA_PROVEEDOR
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return Convert.ToInt32(reader.GetValue(0)) == -1 ? 1 : Convert.ToInt32(reader.GetValue(0)) + 1;
                  };
               };
            };

            return 1;
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

      public bool ValidateIfSynchronizableEntityExistOnGestproject
      (
         SincronizadorGP50ReceivedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               *
            FROM
               FACTURA_PROVEEDOR
            WHERE
               FCP_ID=@FCP_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);

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
         };
      }

      public void RecordSynchronizableEntityOnGestproject
      (
         SincronizadorGP50ReceivedInvoiceModel entity,
         int? fcpId
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            INSERT INTO
               FACTURA_PROVEEDOR
            (
               FCP_ID
               ,FCP_EJERCICIO
               ,PAR_DAO_ID
               ,FCP_NUM_FACTURA
               ,FCP_FECHA
               ,PAR_PRO_ID
               ,FCP_SUBCTA_CONTABLE
               ,FCP_BASE_IMPONIBLE
               ,FCP_VALOR_IVA
               ,FCP_IVA
               ,FCP_VALOR_IRPF
               ,FCP_IRPF
               ,FCP_TOTAL_FACTURA
               ,FCP_OBSERVACIONES
            )
            VALUES
            (
               @FCP_ID
               ,@FCP_EJERCICIO
               ,@PAR_DAO_ID
               ,@FCP_NUM_FACTURA
               ,@FCP_FECHA
               ,@PAR_PRO_ID
               ,@FCP_SUBCTA_CONTABLE
               ,@FCP_BASE_IMPONIBLE
               ,@FCP_VALOR_IVA
               ,@FCP_IVA
               ,@FCP_VALOR_IRPF
               ,@FCP_IRPF
               ,@FCP_TOTAL_FACTURA
               ,@FCP_OBSERVACIONES
            )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCP_ID", fcpId);
               command.Parameters.AddWithValue("@FCP_EJERCICIO", entity.FCP_EJERCICIO);
               command.Parameters.AddWithValue("@PAR_DAO_ID", entity.PAR_DAO_ID);
               command.Parameters.AddWithValue("@FCP_NUM_FACTURA", entity.FCP_NUM_FACTURA);
               command.Parameters.AddWithValue("@FCP_FECHA", entity.FCP_FECHA);
               command.Parameters.AddWithValue("@PAR_PRO_ID", entity.PAR_PRO_ID);
               command.Parameters.AddWithValue("@FCP_SUBCTA_CONTABLE", entity.FCP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@FCP_BASE_IMPONIBLE", entity.FCP_BASE_IMPONIBLE);
               command.Parameters.AddWithValue("@FCP_VALOR_IVA", entity.FCP_VALOR_IVA);
               command.Parameters.AddWithValue("@FCP_IVA", entity.FCP_IVA);
               command.Parameters.AddWithValue("@FCP_VALOR_IRPF", entity.FCP_VALOR_IRPF);
               command.Parameters.AddWithValue("@FCP_IRPF", entity.FCP_IRPF);
               command.Parameters.AddWithValue("@FCP_TOTAL_FACTURA", entity.FCP_TOTAL_FACTURA);
               command.Parameters.AddWithValue("@FCP_OBSERVACIONES", entity.FCP_OBSERVACIONES);

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

      public void GetRecordedSynchronizableEntityGestprojectId
      (
         SincronizadorGP50ReceivedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT
               FCP_ID
            FROM
               FACTURA_PROVEEDOR
            WHERE
               PAR_DAO_ID=@PAR_DAO_ID
            AND
               FCP_NUM_FACTURA=@FCP_NUM_FACTURA
            AND
               FCP_FECHA=@FCP_FECHA
            AND
               PAR_PRO_ID=@PAR_PRO_ID
            AND
               FCP_SUBCTA_CONTABLE=@FCP_SUBCTA_CONTABLE
            AND
               FCP_BASE_IMPONIBLE=@FCP_BASE_IMPONIBLE
            AND
               FCP_VALOR_IVA=@FCP_VALOR_IVA
            AND
               FCP_IVA=@FCP_IVA
            AND
               FCP_TOTAL_FACTURA=@FCP_TOTAL_FACTURA
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_DAO_ID", entity.PAR_DAO_ID);
               command.Parameters.AddWithValue("@FCP_NUM_FACTURA", entity.FCP_NUM_FACTURA);
               command.Parameters.AddWithValue("@FCP_FECHA", entity.FCP_FECHA);
               command.Parameters.AddWithValue("@PAR_PRO_ID", entity.PAR_PRO_ID);
               command.Parameters.AddWithValue("@FCP_SUBCTA_CONTABLE", entity.FCP_SUBCTA_CONTABLE);
               command.Parameters.AddWithValue("@FCP_BASE_IMPONIBLE", entity.FCP_BASE_IMPONIBLE);
               command.Parameters.AddWithValue("@FCP_VALOR_IVA", entity.FCP_VALOR_IVA);
               command.Parameters.AddWithValue("@FCP_IVA", entity.FCP_IVA);
               command.Parameters.AddWithValue("@FCP_TOTAL_FACTURA", entity.FCP_TOTAL_FACTURA);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.FCP_ID = reader["FCP_ID"] as int?;
                     break;
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

      public void RegisterSynchronizableEntityGestprojectId
      (
         SincronizadorGP50ReceivedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            UPDATE
               {TableSchema.TableName}
            SET
               FCP_ID=@FCP_ID
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

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

      public void AppendSynchronizableEntityGestprojectIdToDetails
      (
         SincronizadorGPS50ReceivedInvoiceDetailModel detailEntity,
         SincronizadorGP50ReceivedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            UPDATE
               {SynchronizableEntityDetailsTable}
            SET
               FCP_ID=@FCP_ID
            WHERE
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               detailEntity.FCP_ID = entity.FCP_ID;
               command.Parameters.AddWithValue("@FCP_ID", detailEntity.FCP_ID);
               command.Parameters.AddWithValue("@S50_GUID_ID", detailEntity.S50_GUID_ID);

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

      public void RecordSynchronizableEntityDetailsOnGestproject
      (
         SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail,
         int? dfpId
      )
      {
         try
         {
            Connection.Open();

            string condidtionalProjectIdColumn = entityDetail.PRY_ID != -1 ? ",PRY_ID" : "";
            string condidtionalProjectIdValue = entityDetail.PRY_ID != -1 ? ",@PRY_ID" : "";

            string sqlString = $@"
            INSERT INTO
               DETALLE_FACTURA_PROVEEDOR
            (
               DFP_ID
               ,DFP_CONCEPTO
               ,DFP_PRECIO_UNIDAD
               ,DFP_UNIDADES
               ,DFP_SUBTOTAL
               {condidtionalProjectIdColumn}
               ,FCP_ID
               ,DFP_ESTRUCTURAL
            )
            VALUES
            (
               @DFP_ID
               ,@DFP_CONCEPTO
               ,@DFP_PRECIO_UNIDAD
               ,@DFP_UNIDADES
               ,@DFP_SUBTOTAL
               {condidtionalProjectIdValue}
               ,@FCP_ID
               ,@DFP_ESTRUCTURAL
            )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@DFP_ID", dfpId);
               command.Parameters.AddWithValue("@DFP_CONCEPTO", entityDetail.DFP_CONCEPTO);
               command.Parameters.AddWithValue("@DFP_PRECIO_UNIDAD", entityDetail.DFP_PRECIO_UNIDAD);
               command.Parameters.AddWithValue("@DFP_UNIDADES", entityDetail.DFP_UNIDADES);
               command.Parameters.AddWithValue("@DFP_SUBTOTAL", entityDetail.DFP_SUBTOTAL);
               // When uncommenting add the field in the statement above (both in the first parenthesys as in the second)
               command.Parameters.AddWithValue("@PRY_ID", entityDetail.PRY_ID);
               command.Parameters.AddWithValue("@FCP_ID", entityDetail.FCP_ID);
               command.Parameters.AddWithValue("@DFP_ESTRUCTURAL", entityDetail.DFP_ESTRUCTURAL);

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

      public void GetRecordedSynchronizableEntityDetailsGestprojectId
      (
         SincronizadorGPS50ReceivedInvoiceDetailModel entity
      )
      {
         try
         {
            Connection.Open();

            string condidtionalProjectId = entity.PRY_ID != -1 ? "AND PRY_ID=@PRY_ID" : "";

            string sqlString = $@"
            SELECT
               DFP_ID
            FROM
               DETALLE_FACTURA_PROVEEDOR
            WHERE
               DFP_CONCEPTO=@DFP_CONCEPTO
            AND
               DFP_PRECIO_UNIDAD=@DFP_PRECIO_UNIDAD
            AND
               DFP_UNIDADES=@DFP_UNIDADES
            AND
               DFP_SUBTOTAL=@DFP_SUBTOTAL
            {
               condidtionalProjectId
            }
            AND
               FCP_ID=@FCP_ID
            AND
               DFP_ESTRUCTURAL=@DFP_ESTRUCTURAL
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@DFP_CONCEPTO", entity.DFP_CONCEPTO);
               command.Parameters.AddWithValue("@DFP_PRECIO_UNIDAD", entity.DFP_PRECIO_UNIDAD);
               command.Parameters.AddWithValue("@DFP_UNIDADES", entity.DFP_UNIDADES);
               command.Parameters.AddWithValue("@DFP_SUBTOTAL", entity.DFP_SUBTOTAL);
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);
               command.Parameters.AddWithValue("@DFP_ESTRUCTURAL", entity.DFP_ESTRUCTURAL);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.DFP_ID = reader["DFP_ID"] as int?;
                     break;
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

      public void RegisterSynchronizableEntityDetailsGestprojectId
      (
         SincronizadorGPS50ReceivedInvoiceDetailModel entity
      )
      {
         try
         {
            Connection.Open();
            string condidtionalProjectId = entity.PRY_ID != -1 ? "AND PRY_ID=@PRY_ID" : "";


            string sqlString = $@"
            UPDATE
               INT_SAGE_SINC_FACTURA_RECIBIDA_DETALLES
            SET
               DFP_ID=@DFP_ID
            WHERE
               DFP_CONCEPTO=@DFP_CONCEPTO
            AND
               DFP_PRECIO_UNIDAD=@DFP_PRECIO_UNIDAD
            AND
               DFP_UNIDADES=@DFP_UNIDADES
            AND
               DFP_SUBTOTAL=@DFP_SUBTOTAL
            {
               condidtionalProjectId
            }
            AND
               FCP_ID=@FCP_ID
            AND
               DFP_ESTRUCTURAL=@DFP_ESTRUCTURAL
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@DFP_ID", entity.DFP_ID);
               command.Parameters.AddWithValue("@DFP_CONCEPTO", entity.DFP_CONCEPTO);
               command.Parameters.AddWithValue("@DFP_PRECIO_UNIDAD", entity.DFP_PRECIO_UNIDAD);
               command.Parameters.AddWithValue("@DFP_UNIDADES", entity.DFP_UNIDADES);
               command.Parameters.AddWithValue("@DFP_SUBTOTAL", entity.DFP_SUBTOTAL);
               command.Parameters.AddWithValue("@PRY_ID", entity.PRY_ID);
               command.Parameters.AddWithValue("@FCP_ID", entity.FCP_ID);
               command.Parameters.AddWithValue("@DFP_ESTRUCTURAL", entity.DFP_ESTRUCTURAL);

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



      public void SetEntityAsTransfered
      (
         SincronizadorGP50ReceivedInvoiceModel entity
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
               WHERE
                  S50_GUID_ID=@S50_GUID_ID
               ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", SynchronizationStatusOptions.Transferido);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               command.ExecuteNonQuery();
            };

            foreach(SincronizadorGPS50ReceivedInvoiceDetailModel entityDetail in SynchronizadorGPS50ReceivedInvoicesDetails)
            {
               string sqlString2 = $@"
                  UPDATE
                     {SynchronizableEntityDetailsTable}
                  SET
                     SYNC_STATUS=@SYNC_STATUS
                  WHERE
                     S50_GUID_ID=@S50_GUID_ID
                  ;";

               using(SqlCommand command = new SqlCommand(sqlString2, Connection))
               {
                  command.Parameters.AddWithValue("@SYNC_STATUS", SynchronizationStatusOptions.Transferido);
                  command.Parameters.AddWithValue("@S50_GUID_ID", entityDetail.S50_GUID_ID);

                  command.ExecuteNonQuery();
               };
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
   }
}
