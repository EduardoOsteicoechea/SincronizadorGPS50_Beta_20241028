using sage.ew.articulo;
using sage.ew.cliente;
using sage.ew.db;
using sage.ew.docsven;
using sage.ew.docventatpv;
using sage.ew.ewbase;
using sage.ew.lote.Clases;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class SalesDocument { }
   public class AlbaranVenta : BaseDocument
   {
      ewDocVentaTPV _oDocVenta;
      ewDocVentaLinTPV _oLinia;
      LinVenDet<LinVenDetLotes> _oLinVenDetLotes;

      public Boolean _Create(clsAlbaven toAlbaven)
      {
         this._Error_Message = string.Empty;
         bool llOk = false, llContinue = false;

         if(toAlbaven.Cabecera != null && toAlbaven.Lineas != null && toAlbaven.Lineas.Count != 0)
         {
            Cliente loCliente;

            _oDocVenta = new ewDocVentaTPV();
            _oLinia = new ewDocVentaLinTPV();

            toAlbaven.Cabecera.empresa = this._Empresa;
            toAlbaven.Cabecera.ejercicio = this._Ejercicio;
            toAlbaven.Cabecera.letra = toAlbaven.Cabecera.letra.Trim().PadLeft(2, ' ');
            toAlbaven.Cabecera.numero = toAlbaven.Cabecera.numero.Trim().PadLeft(10, ' ');

            try
            {
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

            try
            {
               // Validamos que existe código de cliente
               if(toAlbaven.Direccion != null)
               {
                  // Usamos el CIF, para ubicar el primer registro que coincida en la tabla de cliente
                  if(string.IsNullOrEmpty(toAlbaven.Cabecera.cliente) && !string.IsNullOrEmpty(toAlbaven.Direccion.cif))
                     toAlbaven.Cabecera.cliente = DB.SQLValor("CLIENTES", "CIF", toAlbaven.Direccion.cif, "CODIGO").ToString();
               }

               // Aplicamos el codigo de clientes varios si es necesario
               toAlbaven.Cabecera.cliente = (string.IsNullOrEmpty(toAlbaven.Cabecera.cliente)) ? this._CliVarios : toAlbaven.Cabecera.cliente;

               // Abrimos el objeto de cliente
               loCliente = new Cliente();
               loCliente._Codigo = toAlbaven.Cabecera.cliente;
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

            // comprobamos que exista el cliente para poder crear el pedido
            if(loCliente._Existe_Registro())
            {
               try
               {
                  // comprobamos si el pedido ya existe
                  if(_oDocVenta._Existe(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.numero, toAlbaven.Cabecera.letra))
                  {
                     // si ya existe, lo cargamos
                     _oDocVenta._Load(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.numero, toAlbaven.Cabecera.letra);
                     if(_oDocVenta._Cabecera._Cliente != toAlbaven.Cabecera.cliente)
                        this._Error_Message += "El Código del cliente del pedido es " + _oDocVenta._Cabecera._Cliente + ", y se esta informando un cliente es diferente " + toAlbaven.Cabecera.cliente + "\r\n";
                     else
                        llContinue = true;
                  }
                  else
                  {
                     MessageBox.Show("before\n\n _oDocVenta._New(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.letra, toAlbaven.Cabecera.numero);");

					       MessageBox.Show(
					       "toAlbaven.Cabecera.empresa: " + toAlbaven.Cabecera.empresa + "\n" + 
					       "toAlbaven.Cabecera.letra: " + toAlbaven.Cabecera.letra  + "\n" +  
					       "toAlbaven.Cabecera.numero: " + toAlbaven.Cabecera.numero
					       );

                     _oDocVenta._New(toAlbaven.Cabecera.empresa, toAlbaven.Cabecera.letra, toAlbaven.Cabecera.numero);
                     _oDocVenta._Cabecera._Cliente = toAlbaven.Cabecera.cliente;
                     llContinue = true;
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

               
				MessageBox.Show("Line 120");


               try
               {
                  if(llContinue)
                  {
                     // validamos la forma de pago
                     string lsFPago = DB.SQLValor("FPAG", "CODIGO", toAlbaven.Cabecera.formapago, "CODIGO").ToString();
                     if(!string.IsNullOrEmpty(lsFPago))
                        _oDocVenta._Cabecera._FormaPago = lsFPago;

                     if(!string.IsNullOrEmpty(toAlbaven.Cabecera.observaciones))
                        _oDocVenta._Cabecera._Observacio = toAlbaven.Cabecera.observaciones;

                     try
                     {
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

                     
				MessageBox.Show("Line 200");

                     try
                     {

                        if(toAlbaven.Lineas != null && toAlbaven.Lineas.Count != 0)
                        {
                           string lsCodigo = string.Empty;

                           foreach(var LineaAlbaran in toAlbaven.Lineas)
                           {
                              _oLinia = _oDocVenta._AddLinea();

                              lsCodigo = DB.SQLValor("ARTICULO", "CODIGO", LineaAlbaran.articulo, "CODIGO").ToString();
                              if(!string.IsNullOrEmpty(lsCodigo))
                              {
                                 _oLinia._Articulo = lsCodigo;
                                 //_oLinia._Talla = LineaAlbaran.talla;
                                 //_oLinia._Color = LineaAlbaran.color;
                                 ////_oLinia._Peso = LineaPedido.peso;
                                 ////_oLinia._Cajas = LineaPedido.cajas;

                                 if(LineaAlbaran.lotes == null)
                                    _oLinia._Unidades = LineaAlbaran.unidades;

                                 if(LineaAlbaran.lotes != null)
                                 {
                                    _oLinVenDetLotes = new LinVenDet<LinVenDetLotes>();
                                    _oLinVenDetLotes._Lineas = _oLinia;
                                    _oLinVenDetLotes._Automatico = true;

                                    Dictionary<String, Object> ldicValores = new Dictionary<string, object>();
                                    ewCampo loCampoUnidades = new ewCampo();
                                    loCampoUnidades._OldVal = loCampoUnidades._NewVal = LineaAlbaran.unidades;
                                    ldicValores.Add("_nUnidades", loCampoUnidades);
                                    _oLinia._UpdateSilent(ldicValores);

                                    foreach(clsAlbavenLineasLotes loItem in LineaAlbaran.lotes)
                                    {
                                       DataRow loRow = null;

                                       //Lote loLote = new Lote(loItem.lote, _oLinia);
                                       Lote loLote = new Lote(loItem.lote, _oLinia);

                                       // Obteniendo el DataTable de la asigancion de los lotes en venta
                                       DataTable ldStockLote = _Stocklote_AsignacionVentas(loLote, _oLinia, loItem.lote);

                                       if(ldStockLote.Rows.Count > 0)
                                       {
                                          loRow = ldStockLote.Rows[0];
                                          loRow["uniasig"] = loItem.unidades;
                                          loRow["pesasig"] = loItem.peso;
                                          //loRow["ubica"] = loItem.ubicacion;
                                          loRow["asi"] = "";
                                          loRow["sel"] = true;

                                          _oLinVenDetLotes._lisCodigos.Add(new LinVenDetLotes(_oLinia, loRow, false));
                                          int lnPos = (_oLinVenDetLotes._lisCodigos.Count - 1);

                                          _oLinVenDetLotes._lisCodigos[lnPos]._Posicion = (lnPos + 1);
                                       }
                                       loRow = null;
                                       ldStockLote.Dispose();
                                    }
                                 }
                                 toAlbaven.Cabecera.precios = true;

                                 if(toAlbaven.Cabecera.precios)
                                 {
                                    _oLinia._Precio = LineaAlbaran.precio;
                                    _oLinia._Dto1 = LineaAlbaran.dto1;
                                    _oLinia._Dto2 = LineaAlbaran.dto2;
                                    _oLinia._Recalcular_Importe();
                                 }

                              }
                              else
                              {
                                 // Es una linea de comentario
                                 if(!string.IsNullOrEmpty(LineaAlbaran.definicion) && !string.IsNullOrEmpty(LineaAlbaran.cuentaContable) && !string.IsNullOrEmpty(LineaAlbaran.tipoiva))
                                 {
                                    _oLinia._Definicion = LineaAlbaran.definicion;
                                    _oLinia._TipoIva = LineaAlbaran.tipoiva;
                                    _oLinia._Cuenta = LineaAlbaran.cuentaContable;
                                    _oLinia._Unidades = LineaAlbaran.unidades;

                                    if(toAlbaven.Cabecera.precios)
                                    {
                                       _oLinia._Precio = LineaAlbaran.precio;
                                       _oLinia._Dto1 = LineaAlbaran.dto1;
                                       _oLinia._Dto2 = LineaAlbaran.dto2;
                                       _oLinia._Recalcular_Importe();
                                    };
                                 }
                                 else if(!string.IsNullOrEmpty(LineaAlbaran.definicion))
                                 {
                                    _oLinia._Definicion = LineaAlbaran.definicion;
                                    _oLinia._TipoIva = "";
                                    _oLinia._Cuenta = "";
                                 }
                                 else
                                 {
                                    this._Error_Message += "El código de articulo " + LineaAlbaran.articulo + ", no existe\r\n";
                                 }
                              }


                              if(_oLinia._Save())
                              {
                                 if(_oLinVenDetLotes != null)
								          {
                                             _oLinVenDetLotes._Save();
									         MessageBox.Show("Lines saved");
								          };
                              }
                              _oLinVenDetLotes = null;
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
                     };

                     _oDocVenta._Totalizar();
                     llOk = _oDocVenta._Save();

                     if(llOk)
                     {
                        toAlbaven.Cabecera.numero = _oDocVenta._Numero;
                        toAlbaven.Cabecera.factura = _oDocVenta._Cabecera._Factura;
                     }
                     else
                     {
                        string lsNumero = string.IsNullOrEmpty(toAlbaven.Cabecera.letra) ? "" : toAlbaven.Cabecera.letra;
                        lsNumero += toAlbaven.Cabecera.numero;
                        this._Error_Message += "No se a podido guardar el albarán de venta :" + lsNumero + "\r\n";
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
               };
            }
            else
            {
				this._Error_Message += "El Código de cliente " + toAlbaven.Cabecera.cliente + ", no existe\r\n";
				MessageBox.Show(_Error_Message);
            }
         }
         else
         {
            if(toAlbaven.Cabecera == null)
               this._Error_Message += "Los datos de la cabecera albarán son obligatorios\r\n";
            else
               this._Error_Message += "Es obligatorio insertar un artículo para poder generar el albarán\r\n";

			MessageBox.Show(_Error_Message);
         }
         return llOk;
      }
   }
}