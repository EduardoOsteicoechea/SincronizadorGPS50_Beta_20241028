using sage.ew.cliente;
using sage.ew.db;
using sage.ew.docventatpv;
using sage.ew.lote.Clases;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Data;

namespace SincronizadorGPS50
{
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

                        if(_oLinia._Save())
                        {
                           if(_oLinVenDetLotes != null)
                              _oLinVenDetLotes._Save();
                        }

                        _oLinVenDetLotes = null;
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
}
