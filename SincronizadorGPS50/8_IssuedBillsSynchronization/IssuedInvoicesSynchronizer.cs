using Infragistics.Designers.SqlEditor;
using sage.ew.docventatpv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;

namespace SincronizadorGPS50
{
   public class IssuedInvoicesSynchronizer : IEntitySynchronizer<SynchronizableIssuedInvoiceModel, Sage50IssuedBillModel>
   {
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public string SynchornizableEntityDetailsTable { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA_DETALLES";
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public List<int> SelectedIds { get; set; }
      public List<SynchronizableIssuedInvoiceModel> Invoices {get;set;}
      public List<SynchronizableIssuedInvoiceDetailModel> InvoicesDetails {get;set;}

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
            SelectedIds = selectedIdList;
            Invoices = new List<SynchronizableIssuedInvoiceModel>();
            InvoicesDetails = new List<SynchronizableIssuedInvoiceDetailModel>();

            GetSelectedInvoiceFromSynchronizationTable();

            foreach(SynchronizableIssuedInvoiceModel entity in Invoices)
            {
               if(ValidateIfEntityWasTransferred(entity) == false)
               {
                  GetSelectedInvoiceDetailsFromSynchronizationTable(entity);
                  CreateInvoceOnSage(entity);
                  RegisterEntitySynchronizationData(entity);
               }
               else
               {
                  MessageBox.Show(
                     $"La factura con el Id \"{entity.FCE_ID}\" en Gestproject ya fue transferida. Pasaremos por alto este documento."
                  );
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


      public void GetSelectedInvoiceFromSynchronizationTable()
      {      
         try
         {
            Connection.Open();

            string selectedIds = "";

            foreach (string id in SelectedIds.Select(id => id.ToString()).ToList())
            {
               selectedIds += $"'{id}',";
            }

            selectedIds = selectedIds.TrimEnd(',');

            string sqlString = $@"
               SELECT * FROM 
                  {TableSchema.TableName} 
               WHERE 
                  ID 
               IN ({selectedIds})
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     //string guidValue = reader["S50_GUID_ID"] as string;
                     //bool hasGuidValue = guidValue != "" && guidValue != null && reader["S50_GUID_ID"].GetType() != typeof(System.DBNull);

                     //MessageBox.Show($@"
                     //   guidValue:  {guidValue}
                     //   hasGuidValue:  {hasGuidValue}
                     //");

                     //if(hasGuidValue == false)
                     //{
                        SynchronizableIssuedInvoiceModel entity = new SynchronizableIssuedInvoiceModel();

                        entity.ID = reader["ID"] as int?;
                        entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                        entity.FCE_ID = reader["FCE_ID"] as int?;
                        entity.PAR_DAO_ID = reader["PAR_DAO_ID"] as int?;
                        entity.FCE_REFERENCIA = reader["FCE_REFERENCIA"] as string;
                        entity.FCE_FECHA = reader["FCE_FECHA"] as DateTime?;
                        entity.PAR_CLI_ID = reader["PAR_CLI_ID"] as int?;
                        entity.FCE_BASE_IMPONIBLE = reader["FCE_BASE_IMPONIBLE"] as decimal?;
                        entity.FCE_VALOR_IVA = reader["FCE_VALOR_IVA"] as decimal?;
                        entity.FCE_IVA = reader["FCE_IVA"] as decimal?;
                        entity.FCE_VALOR_IRPF = reader["FCE_VALOR_IRPF"] as decimal?;
                        entity.FCE_IRPF = reader["FCE_IRPF"] as decimal?;
                        entity.FCE_TOTAL_SUPLIDO = reader["FCE_TOTAL_SUPLIDO"] as decimal?;
                        entity.FCE_TOTAL_FACTURA = reader["FCE_TOTAL_FACTURA"] as decimal?;
                        entity.FCE_OBSERVACIONES = reader["FCE_OBSERVACIONES"] as string;
                        entity.FCE_IVA_IGIC = reader["FCE_IVA_IGIC"] as string;
                        entity.PAR_SUBCTA_CONTABLE = reader["PAR_SUBCTA_CONTABLE"] as string;
                        entity.SageCompanyNumber = reader["SageCompanyNumber"] as string;
                        entity.TaxCode = reader["TaxCode"] as string;
                        entity.FCE_SUBCTA_CONTABLE = reader["FCE_SUBCTA_CONTABLE"] as string;
                        entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                        entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                        entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                        entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                        entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                        entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                        entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
                        entity.COMMENTS = reader["COMMENTS"] as string;

                        Invoices.Add(entity);
                     //}
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
         };
      }


      public void GetSelectedInvoiceDetailsFromSynchronizationTable
      (
         SynchronizableIssuedInvoiceModel invoice
      )
      {      
         try
         {
            Connection.Open();
           
            string sqlString = $@"
               SELECT 
                  ID
                  ,SYNC_STATUS
                  ,DFE_ID
                  ,DFE_CONCEPTO
                  ,DFE_PRECIO_UNIDAD
                  ,DFE_UNIDADES
                  ,DFE_SUBTOTAL
                  ,PRY_ID
                  ,FCE_ID
                  ,DFE_SUBTOTAL_BASE
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,LAST_UPDATE
                  ,GP_USU_ID
                  ,COMMENTS
               FROM 
                  {SynchornizableEntityDetailsTable} 
               WHERE 
                  FCE_ID=@FCE_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCE_ID", invoice.FCE_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableIssuedInvoiceDetailModel entitiy = new SynchronizableIssuedInvoiceDetailModel();
                     
                     entitiy.ID = reader["ID"] as int?;
                     entitiy.SYNC_STATUS = reader["SYNC_STATUS"] as string;
                     entitiy.DFE_ID = reader["DFE_ID"] as int?;
                     entitiy.DFE_CONCEPTO = reader["DFE_CONCEPTO"] as string;
                     entitiy.DFE_PRECIO_UNIDAD = reader["DFE_PRECIO_UNIDAD"] as decimal?;
                     entitiy.DFE_UNIDADES = reader["DFE_UNIDADES"] as decimal?;
                     entitiy.DFE_SUBTOTAL = reader["DFE_SUBTOTAL"] as decimal?;
                     entitiy.PRY_ID = reader["PRY_ID"] as int?;
                     entitiy.FCE_ID = reader["FCE_ID"] as int?;
                     entitiy.DFE_SUBTOTAL_BASE = reader["DFE_SUBTOTAL_BASE"] as decimal?;
                     entitiy.S50_GUID_ID = reader["S50_GUID_ID"] as string;
                     entitiy.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
                     entitiy.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
                     entitiy.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
                     entitiy.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
                     entitiy.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
                     entitiy.GP_USU_ID = reader["GP_USU_ID"] as int?;
                     entitiy.COMMENTS = reader["COMMENTS"] as string;

                     InvoicesDetails.Add(entitiy);
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


      public void CreateInvoceOnSage
      (
         SynchronizableIssuedInvoiceModel invoice
      )
      {
         try
         {
            List<SynchronizableIssuedInvoiceDetailModel> invoiceDetails = InvoicesDetails.Where(detail => detail.FCE_ID == invoice.FCE_ID).ToList();

            IssuedInvoiceProxy issuedInvoiceProxy = new IssuedInvoiceProxy();
            issuedInvoiceProxy._CreateAlbaran(invoice, invoiceDetails);
            invoice.S50_GUID_ID = issuedInvoiceProxy._oEntidad.Cabecera.guid;
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


      public void RegisterEntitySynchronizationData
      (
         SynchronizableIssuedInvoiceModel entity
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
                  ,S50_COMPANY_GROUP_NAME=@S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE=@S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE=@S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID=@S50_COMPANY_GROUP_GUID_ID
                  ,GP_USU_ID=@GP_USU_ID
                  ,COMMENTS=@COMMENTS
               WHERE
                  ID=@ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               entity.SYNC_STATUS = SynchronizationStatusOptions.Transferido;
               entity.S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
               entity.S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
               entity.S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
               entity.S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
               entity.GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;

               command.Parameters.AddWithValue("@ID",entity.ID);
               
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@S50_CODE", entity.S50_CODE);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", entity.S50_COMPANY_GROUP_NAME);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", entity.S50_COMPANY_GROUP_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", entity.S50_COMPANY_GROUP_MAIN_CODE);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", entity.S50_COMPANY_GROUP_GUID_ID);
               command.Parameters.AddWithValue("@GP_USU_ID", entity.GP_USU_ID);
               command.Parameters.AddWithValue("@COMMENTS", entity.COMMENTS ?? "");

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


      public bool ValidateIfEntityWasTransferred
      (
         SynchronizableIssuedInvoiceModel entity
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT 
               S50_GUID_ID
            FROM
               {TableSchema.TableName}
            WHERE
               FCE_ID=@FCE_ID
            ";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@FCE_ID", entity.FCE_ID);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     string entityGuid = reader["S50_GUID_ID"] as string;

                     if(entityGuid != "" && reader["S50_GUID_ID"].GetType() != typeof(System.DBNull))
                     {
                        return true;
                     }
                     else
                     {
                        return false;
                     }
                  }
               }
            }

            //return true;
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
   }
}
