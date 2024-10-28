//using Infragistics.Designers.SqlEditor;
//using Microsoft.Win32;
//using sage.ew.docventatpv;
//using SincronizadorGPS50.Workflows.Sage50Connection;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class IssuedBillsSynchronizer : IEntitySynchronizer<SynchronizableIssuedInvoiceModel, Sage50IssuedBillModel>
//   {
//      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
//      public string SynchornizableEntityDetailsTable { get; set; } = "INT_SAGE_SINC_FACTURA_EMITIDA_DETALLES";
//      public SqlConnection Connection { get; set; }
//      public ISage50ConnectionManager SageConnectionManager { get; set; }
//      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
//      public ewDocVentaTPV Document {get;set;}
//      public ewDocVentaLinTPV DetailManager {get;set;}
//      public SynchronizableIssuedInvoiceModel CurrentInvoice {get;set;}
//      public List<SynchronizableIssuedInvoiceDetailModel> CurrentInvoiceDetails {get;set;}

//      async public void Synchronize
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         ISage50ConnectionManager sage50ConnectionManager,
//         ISynchronizationTableSchemaProvider tableSchema,
//         List<int> selectedIdList
//      )
//      {
//         try
//         {
//            GestprojectConnectionManager = gestprojectConnectionManager;
//            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
//            SageConnectionManager = sage50ConnectionManager;
//            TableSchema = tableSchema;
//            CurrentInvoiceDetails = new List<SynchronizableIssuedInvoiceDetailModel> ();

//            //if(InforceSingleInvoiceSelection(selectedIdList))
//            //{
//               if(GetSelectedInvoiceFromSynchronizationTable(selectedIdList))
//               {
//                  GetSelectedInvoiceDetailsFromSynchronizationTable();
//                  bool creationWasSuccessful = await CreateInvoceOnSage();

//                  if(creationWasSuccessful)
//                  {
//                     RegisterEntityGuid();
//                  };
//               };
//            //};
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }


//      public bool InforceSingleInvoiceSelection
//      (
//         List<int> selectedIdList
//      )
//      {      
//         try
//         {
//            if(selectedIdList.Count > 1)
//            {
//               MessageBox.Show("Por limitaciones de Sage50, hemos restringido este proceso a una factura por operación. Por favor seleccione una única factura a la vez y presione el botón de sincronizar");
               
//               return false;
//            }
//            return true;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }


//      public bool GetSelectedInvoiceFromSynchronizationTable
//      (
//         List<int> selectedIdList
//      )
//      {      
//         try
//         {
//            Connection.Open();

//            string sqlString = $@"
//               SELECT 
//                  ID
//                  ,SYNC_STATUS
//                  ,FCE_ID
//                  ,PAR_DAO_ID
//                  ,FCE_REFERENCIA
//                  ,FCE_FECHA
//                  ,PAR_CLI_ID
//                  ,FCE_BASE_IMPONIBLE
//                  ,FCE_VALOR_IVA
//                  ,FCE_IVA
//                  ,FCE_VALOR_IRPF
//                  ,FCE_IRPF
//                  ,FCE_TOTAL_SUPLIDO
//                  ,FCE_TOTAL_FACTURA
//                  ,FCE_OBSERVACIONES
//                  ,FCE_IVA_IGIC
//                  ,PAR_SUBCTA_CONTABLE
//                  ,SageCompanyNumber
//                  ,TaxCode
//                  ,FCE_SUBCTA_CONTABLE
//                  ,S50_GUID_ID
//                  ,S50_COMPANY_GROUP_NAME
//                  ,S50_COMPANY_GROUP_CODE
//                  ,S50_COMPANY_GROUP_MAIN_CODE
//                  ,S50_COMPANY_GROUP_GUID_ID
//                  ,LAST_UPDATE
//                  ,GP_USU_ID
//                  ,COMMENTS
//               FROM 
//                  {TableSchema.TableName} 
//               WHERE 
//                  ID 
//               IN ('{selectedIdList.FirstOrDefault()}')
//            ;";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     string guidValue = reader["S50_GUID_ID"] as string;
//                     bool hasGuidValue = guidValue != "" && guidValue != null;

//                     if(hasGuidValue == false)
//                     {
//                        SynchronizableIssuedInvoiceModel entity = new SynchronizableIssuedInvoiceModel();
                     
//                        entity.ID = reader["ID"] as int?;
//                        entity.SYNC_STATUS = reader["SYNC_STATUS"] as string;
//                        entity.FCE_ID = reader["FCE_ID"] as int?;
//                        entity.PAR_DAO_ID = reader["PAR_DAO_ID"] as int?;
//                        entity.FCE_REFERENCIA = reader["FCE_REFERENCIA"] as string;
//                        entity.FCE_FECHA = reader["FCE_FECHA"] as DateTime?;
//                        entity.PAR_CLI_ID = reader["PAR_CLI_ID"] as int?;
//                        entity.FCE_BASE_IMPONIBLE = reader["FCE_BASE_IMPONIBLE"] as decimal?;
//                        entity.FCE_VALOR_IVA = reader["FCE_VALOR_IVA"] as decimal?;
//                        entity.FCE_IVA = reader["FCE_IVA"] as decimal?;
//                        entity.FCE_VALOR_IRPF = reader["FCE_VALOR_IRPF"] as decimal?;
//                        entity.FCE_IRPF = reader["FCE_IRPF"] as decimal?;
//                        entity.FCE_TOTAL_SUPLIDO = reader["FCE_TOTAL_SUPLIDO"] as decimal?;
//                        entity.FCE_TOTAL_FACTURA = reader["FCE_TOTAL_FACTURA"] as decimal?;
//                        entity.FCE_OBSERVACIONES = reader["FCE_OBSERVACIONES"] as string;
//                        entity.FCE_IVA_IGIC = reader["FCE_IVA_IGIC"] as string;
//                        entity.PAR_SUBCTA_CONTABLE = reader["PAR_SUBCTA_CONTABLE"] as string;
//                        entity.SageCompanyNumber = reader["SageCompanyNumber"] as string;
//                        entity.TaxCode = reader["TaxCode"] as string;
//                        entity.FCE_SUBCTA_CONTABLE = reader["FCE_SUBCTA_CONTABLE"] as string;
//                        entity.S50_GUID_ID = reader["S50_GUID_ID"] as string;
//                        entity.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
//                        entity.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
//                        entity.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
//                        entity.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
//                        entity.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
//                        entity.GP_USU_ID = reader["GP_USU_ID"] as int?;
//                        entity.COMMENTS = reader["COMMENTS"] as string;

//                        CurrentInvoice = entity;

//                        return true;
//                     }
//                     else
//                     {
//                        MessageBox.Show("La factura ya había sido creada en Sage50. Detuvimos la operación para evitar duplicación de datos.");
//                        return false;
//                     };
//                  };
//               };
//            };            
//            return false;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            Connection.Close();
//         };
//      }


//      public void GetSelectedInvoiceDetailsFromSynchronizationTable()
//      {      
//         try
//         {
//            Connection.Open();

//            string sqlString = $@"
//               SELECT 
//                  ID
//                  ,SYNC_STATUS
//                  ,DFE_ID
//                  ,DFE_CONCEPTO
//                  ,DFE_PRECIO_UNIDAD
//                  ,DFE_UNIDADES
//                  ,DFE_SUBTOTAL
//                  ,PRY_ID
//                  ,FCE_ID
//                  ,DFE_SUBTOTAL_BASE
//                  ,S50_GUID_ID
//                  ,S50_COMPANY_GROUP_NAME
//                  ,S50_COMPANY_GROUP_CODE
//                  ,S50_COMPANY_GROUP_MAIN_CODE
//                  ,S50_COMPANY_GROUP_GUID_ID
//                  ,LAST_UPDATE
//                  ,GP_USU_ID
//                  ,COMMENTS
//               FROM 
//                  {SynchornizableEntityDetailsTable} 
//               WHERE 
//                  FCE_ID=@FCE_ID
//            ;";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@FCE_ID",CurrentInvoice.FCE_ID);

//               using(SqlDataReader reader = command.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     SynchronizableIssuedInvoiceDetailModel entitiy = new SynchronizableIssuedInvoiceDetailModel();
                     
//                     entitiy.ID = reader["ID"] as int?;
//                     entitiy.SYNC_STATUS = reader["SYNC_STATUS"] as string;
//                     entitiy.DFE_ID = reader["DFE_ID"] as int?;
//                     entitiy.DFE_CONCEPTO = reader["DFE_CONCEPTO"] as string;
//                     entitiy.DFE_PRECIO_UNIDAD = reader["DFE_PRECIO_UNIDAD"] as decimal?;
//                     entitiy.DFE_UNIDADES = reader["DFE_UNIDADES"] as decimal?;
//                     entitiy.DFE_SUBTOTAL = reader["DFE_SUBTOTAL"] as decimal?;
//                     entitiy.PRY_ID = reader["PRY_ID"] as int?;
//                     entitiy.FCE_ID = reader["FCE_ID"] as int?;
//                     entitiy.DFE_SUBTOTAL_BASE = reader["DFE_SUBTOTAL_BASE"] as decimal?;
//                     entitiy.S50_GUID_ID = reader["S50_GUID_ID"] as string;
//                     entitiy.S50_COMPANY_GROUP_NAME = reader["S50_COMPANY_GROUP_NAME"] as string;
//                     entitiy.S50_COMPANY_GROUP_CODE = reader["S50_COMPANY_GROUP_CODE"] as string;
//                     entitiy.S50_COMPANY_GROUP_MAIN_CODE = reader["S50_COMPANY_GROUP_MAIN_CODE"] as string;
//                     entitiy.S50_COMPANY_GROUP_GUID_ID = reader["S50_COMPANY_GROUP_GUID_ID"] as string;
//                     entitiy.LAST_UPDATE = reader["LAST_UPDATE"] as DateTime?;
//                     entitiy.GP_USU_ID = reader["GP_USU_ID"] as int?;
//                     entitiy.COMMENTS = reader["COMMENTS"] as string;

//                     CurrentInvoiceDetails.Add(entitiy);
//                  };
//               };
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            Connection.Close();
//         };
//      }


//      async public Task<bool> CreateInvoceOnSage()
//      {
//         try
//         {
//            Document = new ewDocVentaTPV();
//            ewDocVentaLinTPV detailManager = new ewDocVentaLinTPV();
//            Document._Cabecera._Cliente = CurrentInvoice.PAR_SUBCTA_CONTABLE.Trim();
//            Document._Cabecera._FormaPago = "01";

//            //await Task.Delay(100);

//            Document._New(CurrentInvoice.SageCompanyNumber.Trim(), "", "");

//            foreach(SynchronizableIssuedInvoiceDetailModel entityDetail in CurrentInvoiceDetails)
//            {            
//               detailManager = Document._AddLinea();
//               detailManager._Cuenta = CurrentInvoice.FCE_SUBCTA_CONTABLE;
//               detailManager._TipoIva = CurrentInvoice.TaxCode;
//               detailManager._Definicion = entityDetail.DFE_CONCEPTO;
//               detailManager._Unidades = entityDetail.DFE_UNIDADES ?? 0;
//               detailManager._Precio = entityDetail.DFE_PRECIO_UNIDAD ?? 0;
//               detailManager._Recalcular_Importe();
            
//               if(detailManager._Save() == false) 
//               {
//                  throw new Exception("Error: We couldn't register the detail " + detailManager._Definicion);
//               };
//            };
            
//            Document._Totalizar();
            
//            if(Document._Save() == false) 
//            {
//               throw new Exception("Error: We couldn't register the document");
//            }
//            else
//            {
//               return true;
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }


//      public void RegisterEntityGuid()
//      {      
//         try
//         {
//            Connection.Open();

//            string sqlString = $@"
//               UPDATE 
//                  {TableSchema.TableName}
//               SET
//                  S50_GUID_ID=@S50_GUID_ID,
//                  SYNC_STATUS=@SYNC_STATUS
//               WHERE
//                  ID=@ID
//            ;";

//            using(SqlCommand command = new SqlCommand(sqlString, Connection))
//            {
//               command.Parameters.AddWithValue("@S50_GUID_ID", Document._Cabecera._Guid_Id);
//               command.Parameters.AddWithValue("@SYNC_STATUS", SynchronizationStatusOptions.Transferido);
//               command.Parameters.AddWithValue("@ID",CurrentInvoice.ID);

//               command.ExecuteNonQuery();
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            Connection.Close();
//         };
//      }





//      //public void CreateDocument()
//      //{
//      //   try
//      //   {
//      //      Document = new ewDocVentaTPV();
//      //      Document._Cabecera._Cliente = "43000002"; // PAR_CLI_ID ->INT_SAGE_SINC_CLIENTES: PAR_ID -> PAR_SUBCTA_CONTABLE
//      //      Document._Cabecera._FormaPago = "01";
//      //      Document._New("01", "", ""); // 01 = PAR_DAO_ID ->INT_SAGE_SINC_EMPRESAS: PAR_ID -> SageCompanyNumber

//      //      DetailManager = Document._AddLinea();
//      //      DetailManager._Cuenta = "70000001";
//      //      DetailManager._TipoIva = "03"; // FCE_IVA_IGIC ->INT_SAGE_SINC_IMPUESTOS: IMP_NOMBRE -> S50_CODE
//      //      DetailManager._Definicion = "linea1ssssss";
//      //      DetailManager._Unidades = 2;
//      //      DetailManager._Precio = 4589;
//      //      DetailManager._Recalcular_Importe();

//      //      if(DetailManager._Save() == false) throw new Exception("Error: We couldn't register the detail " + DetailManager._Definicion);

//      //      DetailManager = Document._AddLinea();
//      //      DetailManager._Cuenta = "64000000";
//      //      DetailManager._TipoIva = "11";
//      //      DetailManager._Definicion = "linea2aaaaaaa";
//      //      DetailManager._Unidades = 2;
//      //      DetailManager._Precio = 214;
//      //      DetailManager._Recalcular_Importe();

//      //      if(DetailManager._Save() == false) throw new Exception("Error: We couldn't register the detail " + DetailManager._Definicion);

//      //      Document._Totalizar();
            
//      //      if(Document._Save() == false) throw new Exception("Error: We couldn't register the document");
//      //   }
//      //   catch(System.Exception exception)
//      //   {
//      //      throw ApplicationLogger.ReportError(
//      //         MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//      //         MethodBase.GetCurrentMethod().DeclaringType.Name,
//      //         MethodBase.GetCurrentMethod().Name,
//      //         exception
//      //      );
//      //   };
//      //}


//   }
//}
