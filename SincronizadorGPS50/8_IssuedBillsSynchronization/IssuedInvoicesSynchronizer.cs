using Infragistics.Designers.SqlEditor;
using sage.ew.docventatpv;
using sage.ew.ewbase;
using sage.ew.global.Diccionarios;
using sage.ew.global;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static sage.ew.docsven.FirmaElectronica;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using sage.ew.cliente;
using sage.ew.db;
using sage.ew.lote.Clases;
using System.Threading.Tasks;

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
      public List<SynchronizableIssuedInvoiceModel> Invoices { get; set; }
      public List<SynchronizableIssuedInvoiceDetailModel> InvoicesDetails { get; set; }

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

                  //ValidateTaxName();

                  //GetSelectedInvoiceSageProjectCode(entity);
                  if(CreateInvoceOnSage(entity))
                  {
                     RegisterEntitySynchronizationData(entity);
                  }
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

            foreach(string id in SelectedIds.Select(id => id.ToString()).ToList())
            {
               selectedIds += $"'{id}',";
            }

            selectedIds = selectedIds.TrimEnd(',');

            //string sqlString = $@"
            //   SELECT * FROM 
            //      {TableSchema.TableName} 
            //   WHERE 
            //      ID 
            //   IN ({selectedIds})
            //;";

            string sqlString = $@"
               SELECT * FROM 
                  {TableSchema.TableName} 
               WHERE 
                  FCE_ID 
               IN ({selectedIds})
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
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
                     entitiy.SYNC_STATUS = (reader["SYNC_STATUS"] as string) ?? "";
                     entitiy.DFE_ID = (reader["DFE_ID"] as int?) ?? -1;
                     entitiy.DFE_CONCEPTO = (reader["DFE_CONCEPTO"] as string) ?? "";
                     entitiy.DFE_PRECIO_UNIDAD = (reader["DFE_PRECIO_UNIDAD"] as decimal?) ?? 0;
                     entitiy.DFE_UNIDADES = (reader["DFE_UNIDADES"] as decimal?) ?? 0;
                     entitiy.DFE_SUBTOTAL = (reader["DFE_SUBTOTAL"] as decimal?) ?? 0;
                     entitiy.PRY_ID = (reader["PRY_ID"] as int?) ?? -1;
                     entitiy.FCE_ID = (reader["FCE_ID"] as int?) ?? -1;
                     entitiy.DFE_SUBTOTAL_BASE = (reader["DFE_SUBTOTAL_BASE"] as decimal?) ?? 0;
                     entitiy.S50_GUID_ID = (reader["S50_GUID_ID"] as string) ?? "";
                     entitiy.S50_COMPANY_GROUP_NAME = (reader["S50_COMPANY_GROUP_NAME"] as string) ?? "";
                     entitiy.S50_COMPANY_GROUP_CODE = (reader["S50_COMPANY_GROUP_CODE"] as string) ?? "";
                     entitiy.S50_COMPANY_GROUP_MAIN_CODE = (reader["S50_COMPANY_GROUP_MAIN_CODE"] as string) ?? "";
                     entitiy.S50_COMPANY_GROUP_GUID_ID = (reader["S50_COMPANY_GROUP_GUID_ID"] as string) ?? "";
                     entitiy.LAST_UPDATE = (reader["LAST_UPDATE"] as DateTime?) ?? DateTime.Now;
                     entitiy.GP_USU_ID = (reader["GP_USU_ID"] as int?) ?? -1;
                     entitiy.COMMENTS = (reader["COMMENTS"] as string) ?? "";

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
         }
      }

      public void GetSelectedInvoiceSageProjectCode
      (
         SynchronizableIssuedInvoiceModel invoice
      )
      {
         try
         {
            SynchronizableIssuedInvoiceDetailModel firstEntityDetail =
            InvoicesDetails.FirstOrDefault(
               detail => detail.FCE_ID == invoice.FCE_ID && detail.PRY_ID != -1
            );

            invoice.SageProjectCode = GetSageProjectCodeById(firstEntityDetail.PRY_ID);
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


      public string GetSageProjectCodeById
      (
         int? projectId
      )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
            SELECT 
               S50_CODE
            FROM 
               INT_SAGE_SINC_PROYECTOS
            WHERE 
               PRY_ID=@PRY_ID
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PRY_ID", projectId);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     return reader["S50_CODE"] as string;
                  };
               };
            };

            return "";
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

      public bool CreateInvoceOnSage
      (
         SynchronizableIssuedInvoiceModel invoice
      )
      {
         try
         {
            List<SynchronizableIssuedInvoiceDetailModel> invoiceDetails = InvoicesDetails.Where(detail => detail.FCE_ID == invoice.FCE_ID).ToList();

            IssuedInvoiceProxy issuedInvoiceProxy = new IssuedInvoiceProxy();

            issuedInvoiceProxy._CreateAlbaran(invoice, invoiceDetails);
            if(issuedInvoiceProxy._oEntidad.Cabecera.guid != "")
            {
               invoice.S50_GUID_ID = issuedInvoiceProxy._oEntidad.Cabecera.guid;
               return true;
            }
            else
            {
               MessageBox.Show($"Error en la creación de la factura con el id \"{invoice.FCE_ID}\" en Gestproject");
               return false;
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

      public class IssuedInvoiceProxy
      {
         SalesAlbaranModel _oAlbaranVenta = new SalesAlbaranModel();
         public dynamic _oEntidad;
         private clsAlbavenLineas _LinAlbaran = new clsAlbavenLineas();

         private int _nDigitos = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_digitos));
         private int _nArticulo = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_articulo));

         private bool _llFactSer = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Factser"));       // Trabajar con series de docummentos (albaranes)
         private bool _llSerFact = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Serfact"));       // Trabajar con series de docummentos (facturas)

         private string _Letra = string.Empty;
         private string _Numero = string.Empty;
         private string _Almacen = EW_GLOBAL._GetVariable("wc_almacen").ToString();

         public IssuedInvoiceProxy()
         {
            _CrearEntidad();
         }

         public void _CreateAlbaran
         (
            SynchronizableIssuedInvoiceModel invoice, 
            List<SynchronizableIssuedInvoiceDetailModel> invoiceDetails
         )
         {
            bool llOk = false;

            _CrearEntidad();
            _oEntidad.Cabecera.cliente = invoice.PAR_SUBCTA_CONTABLE.Trim();
            //_oEntidad.Cabecera.obra = invoice.SageProjectCode.Trim();

            foreach(SynchronizableIssuedInvoiceDetailModel detail in invoiceDetails)
            {
               _CrearObjetoLinea();
               _LinAlbaran.cuentaContable = invoice.FCE_SUBCTA_CONTABLE;
               _LinAlbaran.tipoiva = invoice.TaxCode;
               _LinAlbaran.definicion = detail.DFE_CONCEPTO;
               _LinAlbaran.unidades = detail.DFE_UNIDADES ?? 0;
               _LinAlbaran.precio = detail.DFE_PRECIO_UNIDAD ?? 0;
               _AddObjetoLinea();
            };

            llOk = _oAlbaranVenta._Create(_oEntidad);

            if(_oAlbaranVenta._Error_Message != "")
            {
               MessageBox.Show(_oAlbaranVenta._Error_Message);
            }
         }

         private void _CrearEntidad()
         {
            _oEntidad = new Sage50Connector.clsAlbaven();
            _oEntidad.Cabecera.letra = "";
            _oEntidad.Cabecera.almacen = _Almacen;
            //_oEntidad.Cabecera.formapago = "00";
            _oEntidad.Cabecera.formapago = "01";

            if(_llFactSer)
               _oEntidad.Cabecera.letra = "SF";
         }

         private void _CrearObjetoLinea()
         {
            _LinAlbaran = new clsAlbavenLineas();
         }

         private void _AddObjetoLinea()
         {
            int lnLinea = 0;

            if(_oEntidad.Lineas != null && _oEntidad.Lineas.Count != 0)
               lnLinea = ((List<clsAlbavenLineas>)((clsAlbaven)_oEntidad).Lineas).Max(x => x.linea);

            lnLinea++;

            _LinAlbaran.linea = lnLinea;
            _oEntidad.Lineas.Add(_LinAlbaran);
         }
      }

      public class SalesAlbaranModel : BaseDocument
      {
         ewDocVentaTPV _oDocVenta;
         ewDocVentaLinTPV _oLinia;
         LinVenDet<LinVenDetLotes> _oLinVenDetLotes;

         public Boolean _Create(clsAlbaven toAlbaven)
         {
            _Error_Message = string.Empty;
            bool llOk = false, llContinue = false;

            if(toAlbaven.Cabecera != null && toAlbaven.Lineas != null && toAlbaven.Lineas.Count != 0)
            {
               Cliente loCliente;

               _oDocVenta = new ewDocVentaTPV();
               _oLinia = new ewDocVentaLinTPV();

               toAlbaven.Cabecera.empresa = _Empresa;
               toAlbaven.Cabecera.ejercicio = _Ejercicio;
               toAlbaven.Cabecera.letra = toAlbaven.Cabecera.letra.Trim().PadLeft(2, ' ');
               toAlbaven.Cabecera.numero = toAlbaven.Cabecera.numero.Trim().PadLeft(10, ' ');
               toAlbaven.Cabecera.obra = toAlbaven.Cabecera.obra;

               // Validamos que existe código de cliente
               if(toAlbaven.Direccion != null)
               {
                  // Usamos el CIF, para ubicar el primer registro que coincida en la tabla de cliente
                  if(string.IsNullOrEmpty(toAlbaven.Cabecera.cliente) && !string.IsNullOrEmpty(toAlbaven.Direccion.cif))
                     toAlbaven.Cabecera.cliente = DB.SQLValor("CLIENTES", "CIF", toAlbaven.Direccion.cif, "CODIGO").ToString();
               }

               // Aplicamos el codigo de clientes varios si es necesario
               toAlbaven.Cabecera.cliente = (string.IsNullOrEmpty(toAlbaven.Cabecera.cliente)) ? _CliVarios : toAlbaven.Cabecera.cliente;

               // Abrimos el objeto de cliente
               loCliente = new Cliente();
               loCliente._Codigo = toAlbaven.Cabecera.cliente;

               // comprobamos que exista el cliente para poder crear el pedido
               if(loCliente._Existe_Registro())
               {
                  // comprobamos si el pedido ya existe
                  if(_oDocVenta._Existe(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.numero, toAlbaven.Cabecera.letra))
                  {
                     // si ya existe, lo cargamos
                     _oDocVenta._Load(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.numero, toAlbaven.Cabecera.letra);
                     if(_oDocVenta._Cabecera._Cliente != toAlbaven.Cabecera.cliente)
                        _Error_Message += "El Código del cliente del pedido es " + _oDocVenta._Cabecera._Cliente + ", y se esta informando un cliente es diferente " + toAlbaven.Cabecera.cliente + "\r\n";
                     else
                        llContinue = true;
                  }
                  else
                  {
                     // no existe, lo creamos
                     _oDocVenta._New(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.letra, toAlbaven.Cabecera.numero);
                     _oDocVenta._Cabecera._Cliente = toAlbaven.Cabecera.cliente;
                     llContinue = true;
                  }

                  if(llContinue)
                  {
                     // validamos la forma de pago
                     string lsFPago = DB.SQLValor("FPAG", "CODIGO", toAlbaven.Cabecera.formapago, "CODIGO").ToString();
                     if(!string.IsNullOrEmpty(lsFPago))
                        _oDocVenta._Cabecera._FormaPago = lsFPago;

                     if(!string.IsNullOrEmpty(toAlbaven.Cabecera.observaciones))
                        _oDocVenta._Cabecera._Observacio = toAlbaven.Cabecera.observaciones;

                     // Dirección de envío para el pedido de venta
                     if(toAlbaven.Direccion != null)
                     {
                        if(loCliente._ClienteContado)
                        {
                           _oDocVenta._Cabecera._DatosContado._Nombre = toAlbaven.Direccion.nombre;
                           _oDocVenta._Cabecera._DatosContado._Cif = toAlbaven.Direccion.cif;
                           _oDocVenta._Cabecera._DatosContado._Direccion = toAlbaven.Direccion.direccion;
                           _oDocVenta._Cabecera._DatosContado._CodPost = toAlbaven.Direccion.codpos;
                           _oDocVenta._Cabecera._DatosContado._Poblacion = toAlbaven.Direccion.poblacion;
                           _oDocVenta._Cabecera._DatosContado._Provincia = toAlbaven.Direccion.provincia;
                           _oDocVenta._Cabecera._DatosContado._Pais = toAlbaven.Direccion.pais;
                           _oDocVenta._Cabecera._DatosContado._Email = toAlbaven.Direccion.email;
                           _oDocVenta._Cabecera._DatosContado._Telefono = toAlbaven.Direccion.telefono;
                           _oDocVenta._Cabecera._DatosContado._Save();
                        }
                        else
                        {
                           // Obtenemos un datatable con las direcciones de la ficha del cliente
                           DataTable loDirecciones = loCliente._Direcciones();

                           // Buscamos la dirección dentro del datatable
                           DataRow[] loRow = loDirecciones.Select(String.Format("direccion = '{0}' AND codpos = '{1}' AND poblacion = '{2}' AND provincia = '{3}'", toAlbaven.Direccion.direccion, toAlbaven.Direccion.codpos, toAlbaven.Direccion.poblacion, toAlbaven.Direccion.provincia));

                           if(loRow.Length > 0)
                           {
                              // Si la dirección existe, le aplicamos el número de la linea relacionado al pedido de venta
                              _oDocVenta._Cabecera._Env_cli = Convert.ToInt16(loRow[0]["linea"]);
                           }
                           else
                           {
                              // damos de alta la nueva dirección de envío en la ficha del cliente
                              Cliente.Direcciones.Direccion loDireccion = loCliente._TRelDirecciones._NewItem();
                              loDireccion._Nombre = toAlbaven.Direccion.nombre;
                              loDireccion._Direccion = toAlbaven.Direccion.direccion;
                              loDireccion._CodPos = toAlbaven.Direccion.codpos;
                              loDireccion._Poblacion = toAlbaven.Direccion.poblacion;
                              loDireccion._Provincia = toAlbaven.Direccion.provincia;
                              loDireccion._Pais = toAlbaven.Direccion.pais;
                              loDireccion._Telefono = toAlbaven.Direccion.telefono;
                              loDireccion._Tipo = (int)Cliente.Direcciones.TiposDirecciones.Envios;   // indicamos que la dirección es de envios.
                                                                                                      // grabamos el registro
                              loCliente._TRelDirecciones._SaveItem(loDireccion);
                              //

                              _oDocVenta._Cabecera._Env_cli = loDireccion._Linia;
                           }

                           loDirecciones.Dispose();
                        }
                     }

                     if(toAlbaven.Lineas != null && toAlbaven.Lineas.Count != 0)
                     {
                        string lsCodigo = string.Empty;

                        foreach(var LineaAlbaran in toAlbaven.Lineas)
                        {
                           _oLinia = _oDocVenta._AddLinea();
                           
                           _oLinia._Coste = 0;
                           _oLinia._Cuenta = LineaAlbaran.cuentaContable;
                           _oLinia._TipoIva = LineaAlbaran.tipoiva;

                           _oLinia._Definicion = LineaAlbaran.definicion;
                           _oLinia._Unidades = LineaAlbaran.unidades;
                           _oLinia._Precio = LineaAlbaran.precio;

                           toAlbaven.Cabecera.precios = true;

                           if(toAlbaven.Cabecera.precios)
                           {
                              _oLinia._Precio = LineaAlbaran.precio;
                              _oLinia._Dto1 = LineaAlbaran.dto1;
                              _oLinia._Dto2 = LineaAlbaran.dto2;
                              _oLinia._Recalcular_Importe();
                           };

                           //if(_oLinia._Save())
                           //{
                           //   if(_oLinVenDetLotes != null)
                           //      _oLinVenDetLotes._Save();
                           //}

                           //_oLinVenDetLotes = null;

                           _oLinia._Save();

                           Task.Delay(100);
                        }
                     }

                     _oDocVenta._Totalizar();
                     llOk = _oDocVenta._Save();

                     if(llOk)
                     {
                        toAlbaven.Cabecera.numero = _oDocVenta._Numero;
                        toAlbaven.Cabecera.factura = _oDocVenta._Cabecera._Factura;
                        toAlbaven.Cabecera.guid = _oDocVenta._Cabecera._Guid_Id;
                     }
                     else
                     {
                        string lsNumero = string.IsNullOrEmpty(toAlbaven.Cabecera.letra) ? "" : toAlbaven.Cabecera.letra;
                        lsNumero += toAlbaven.Cabecera.numero;
                        _Error_Message += "No se a podido guardar el albarán de venta :" + lsNumero + "\r\n";
                     }
                  }
               }
               else
               {
                  _Error_Message += "El Código de cliente " + toAlbaven.Cabecera.cliente + ", no existe\r\n";
               }
            }
            else
            {
               if(toAlbaven.Cabecera == null)
                  _Error_Message += "Los datos de la cabecera albarán son obligatorios\r\n";
               else
                  _Error_Message += "Es obligatorio insertar un artículo para poder generar el albarán\r\n";
            }

            return llOk;
         }
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

               command.Parameters.AddWithValue("@ID", entity.ID);

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
                     string entityGuid = (reader["S50_GUID_ID"] as string) ?? "";
                     bool entityGuidIsEmpty = entityGuid == "";
                     bool entityGuidIsDBNull = reader["S50_GUID_ID"].GetType() == typeof(System.DBNull);

                     if(entityGuidIsEmpty || entityGuidIsDBNull)
                     {
                        return false;
                     }
                     else
                     {
                        return true;
                     }
                  }
               }
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
         }
         finally
         {
            Connection.Close();
         }
      }
   }
}
