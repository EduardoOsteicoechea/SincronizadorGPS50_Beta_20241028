using System;
using System.Collections.Generic;
using System.Data;
using sage.ew.db;
using sage.ew.global;
using sage.ew.global.Diccionarios;

namespace SincronizadorGPS50.Sage50Connector
{
   public class Sage50Work : BaseMaster
   {
      private LinkFuncSage50 _oLinkFuncs = new LinkFuncSage50();
      private sage.ew.cliente.Obra _oEntity = null;
      private int _nDigitos = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_digitos));
      public Sage50Work()
      {
         psDb = "gestion";
         psTable = "proveed";
      }

      public override bool _Load(ref dynamic toeEntity)
      {
         bool llOk = false;

         if(toeEntity != null)
         {
            if(!string.IsNullOrWhiteSpace(toeEntity.codigo) && toeEntity.codigo.Trim().Length == _nDigitos)
            {
               _oEntity = new sage.ew.cliente.Obra();
               _oEntity._Codigo = toeEntity.codigo;
               toeEntity.nombre = _oEntity._Nombre;
               toeEntity.direccion = _oEntity._Direccion;
               toeEntity.poblacion = _oEntity._Poblacion;
               toeEntity.provincia = _oEntity._Provincia;
               toeEntity.telefono = _oEntity._Telefono;
               toeEntity.codpos = _oEntity._Codpost;
               toeEntity.tipo_iva = _oEntity._TipoIVA;
               toeEntity.existeregistro = _oEntity._Existe_Registro();

               llOk = true;
            }
            else
            { 
               this._Error_Message += "No se a indicado el códgo del cliente o la longitud del codigo es diferente a " + _nDigitos + " digitos \r\n"; 
            }
         }
         return llOk;
      }

      public override bool _Create(dynamic toeEntity)
      {
         bool llOk = false;
         string lsPais = string.Empty, lsCodPos = string.Empty, lsTipoIva = string.Empty, lsTipoRet = string.Empty;
         this._Error_Message = string.Empty;

         if(toeEntity != null)
         {
            if(!string.IsNullOrWhiteSpace(toeEntity.codigo) && !string.IsNullOrWhiteSpace(toeEntity.nombre) && toeEntity.codigo.Trim().Length == _nDigitos)
            {
               _oEntity = new sage.ew.cliente.Obra();
               _oEntity._Codigo = toeEntity.codigo;

               if(!_oEntity._Existe_Registro())
               {
                  _oEntity._New(toeEntity.codigo);
                  _oEntity._Nombre = toeEntity.nombre;               
                  _oEntity._Direccion = toeEntity.direccion;
                  _oEntity._Poblacion = toeEntity.poblacion;
                  _oEntity._Provincia = toeEntity.provincia;
                  _oEntity._Telefono = toeEntity.telefono;

                  _ConvertData("codpos", toeEntity, ref _oEntity);
                  _ConvertData("tipo_iva", toeEntity, ref _oEntity);
                  _ConvertData("tipo_ret", toeEntity, ref _oEntity);
                  

                  llOk = _oEntity._Save();
                  if(!llOk)
                  {
                     this._Error_Message += _oEntity._Error_Message + "\r\n";
                  };
               }
            }
            _oEntity = null;
         }
         return llOk;
      }

      public override bool _Update(dynamic toeEntity)
      {
         bool llOk = false;
         this._Error_Message = string.Empty;
         if(toeEntity != null)
         {
            _oEntity = new sage.ew.cliente.Obra();
            _oEntity._Codigo = toeEntity.codigo;
            _oEntity._Nombre = toeEntity.nombre;
            _oEntity._Direccion = toeEntity.direccion;
            _oEntity._Poblacion = toeEntity.poblacion;
            _oEntity._Provincia = toeEntity.provincia;
            _oEntity._Telefono = toeEntity.telefono;

            _ConvertData("codpos", toeEntity, ref _oEntity);
            _ConvertData("tipo_iva", toeEntity, ref _oEntity);
            _ConvertData("tipo_ret", toeEntity, ref _oEntity);

            llOk = _oEntity._Save();
            if(!llOk)
            {
               this._Error_Message += _oEntity._Error_Message + "\r\n";
            };
         }
         return llOk;
      }

      public override bool _Delete(dynamic toeEntity)
      {
         bool llOk = false;
         this._Error_Message = string.Empty;

         if(toeEntity != null)
         {
            _oEntity = new sage.ew.cliente.Obra(toeEntity.codigo);
            llOk = _oEntity._Delete();
            if(!llOk)
               this._Error_Message += _oEntity._Error_Message + "\r\n";
         };
         return llOk;
      }

      public bool _MandateCustomer(clsEntityMandate toMandate)
      {
         bool llOk = false;
         string lsSQL = string.Empty;
         DataTable loDatosCliente = new DataTable();

         Dictionary<string, object> _Filtro = new Dictionary<string, object>();
         _Filtro.Add("mandato", toMandate.numero);

         _Vista loVista = new _Vista("COMUNES", "MANDATOS");

         loVista._Requery(_Filtro);

         if(loVista._Reccount == 0)
         {
            lsSQL = "Select a.nombre, a.cif, a.direccion, a.poblacion, a.pais, a.codpost as codpos, b.codigo as banc_cli, b.iban, b.cuentaiban, b.swift " +
                    "From {0} a Left Join {1} b On a.codigo = b.cliente and orden = 1 " +
                    "Where a.codigo = {2} ";
            lsSQL = string.Format(lsSQL, DB.SQLDatabase("CLIENTES"), DB.SQLDatabase("BANC_CLI"), toMandate.cliente);

            DB.SQLExec(lsSQL, ref loDatosCliente);

            loVista._AppendBlank();
            loVista._CurrentRow["cliente"] = toMandate.cliente;
            loVista._CurrentRow["linia"] = 1;
            loVista._CurrentRow["mandato"] = toMandate.numero;
            loVista._CurrentRow["fecha_fin"] = toMandate.fecha_fin;
            loVista._CurrentRow["fecha_fir"] = toMandate.fecha_fir;
            loVista._CurrentRow["defecto"] = toMandate.defecto;
            loVista._CurrentRow["tipo"] = (toMandate.tipo == 0 ? 1 : toMandate.tipo);
            loVista._CurrentRow["tipo_pago"] = (toMandate.tipo_pago == 0 ? 1 : toMandate.tipo_pago);

            loVista._CurrentRow["cli_nomb"] = loDatosCliente.Rows[0]["nombre"].ToString();
            loVista._CurrentRow["cli_direc"] = loDatosCliente.Rows[0]["direccion"].ToString();
            loVista._CurrentRow["cli_nif"] = loDatosCliente.Rows[0]["cif"].ToString();

            loVista._CurrentRow["cli_pais"] = loDatosCliente.Rows[0]["pais"].ToString();
            loVista._CurrentRow["cli_codpos"] = loDatosCliente.Rows[0]["codpos"].ToString();
            loVista._CurrentRow["cli_iban"] = loDatosCliente.Rows[0]["iban"].ToString().Trim() + loDatosCliente.Rows[0]["cuentaiban"].ToString().Trim();
            loVista._CurrentRow["cli_bic"] = loDatosCliente.Rows[0]["swift"];

            loVista._CurrentRow["mandcont"] = 1;
            loVista._CurrentRow["numefe"] = toMandate.numefe;
            loVista._CurrentRow["numefpro"] = toMandate.numefpro;
            loVista._CurrentRow["estpro"] = false;
            loVista._CurrentRow["banc_cli"] = loDatosCliente.Rows[0]["banc_cli"].ToString();

            loVista._CurrentRow["guid_id"] = Guid.NewGuid().ToString().ToUpper();

            loDatosCliente.Dispose();
            loDatosCliente = null;

         }
         else
         {
            loVista._CurrentRow["fecha_fir"] = toMandate.fecha_fir;
            loVista._CurrentRow["fecha_fin"] = toMandate.fecha_fin;
            loVista._CurrentRow["numefe"] = toMandate.numefe;
            loVista._CurrentRow["numefpro"] = toMandate.numefpro;
            loVista._CurrentRow["tipo"] = (toMandate.tipo == 0 ? 1 : toMandate.tipo);
            loVista._CurrentRow["tipo_pago"] = (toMandate.tipo_pago == 0 ? 1 : toMandate.tipo_pago);
         }

         llOk = loVista._TableUpdate();
         loVista = null;
         return llOk;
      }


      private void _ConvertData(string tsTipo, dynamic toeEntity, ref sage.ew.cliente.Obra toCliente)
      {
         switch(tsTipo.ToLower())
         {
            case "codpos":
               if(!string.IsNullOrWhiteSpace(toeEntity.codpos))
               {
                  string lsCodPos = toeEntity.codpos;
                  if(!string.IsNullOrWhiteSpace(lsCodPos))
                     toCliente._Codpost = lsCodPos;
                  else
                     this._Error_Message += string.Format("¡El código postal {0} no existe!", toeEntity.codpos) + "\r\n";
               }
               break;
            case "tipo_iva":
               if(!string.IsNullOrWhiteSpace(toeEntity.tipo_iva))
               {
                  string lsTipoIva = toeEntity.tipo_iva;
                  if(!string.IsNullOrWhiteSpace(lsTipoIva))
                     toCliente._TipoIVA = lsTipoIva;
                  else
                     this._Error_Message += string.Format("¡El tipo de iva {0} no existe!", toeEntity.tipo_iva) + "\r\n";
               }
               break;
            default:
               break;
         }
      }
   }
}