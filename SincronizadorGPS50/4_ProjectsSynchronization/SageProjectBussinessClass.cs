using sage.ew.cliente;
using sage.ew.global.Diccionarios;
using sage.ew.global;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class SageProjectBussinessClass : BaseMaster
   {
      private LinkFuncSage50 _oLinkFuncs = new LinkFuncSage50();
      public Obra _oObra = null;
      private int _nDigitos = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_obra));


      public string _ValidateCode(string tsCode)
      {
         string lsCode = string.Empty;

         if(!string.IsNullOrEmpty(tsCode.Trim()))
         {
            lsCode = tsCode.Trim();
            if(lsCode.Length != _nDigitos)
               lsCode = lsCode.PadLeft(_nDigitos, '0');
         }

         return lsCode;
      }

      public override bool _Create(dynamic toObra)
      {
         bool llOk = false;
         string lsGuid = string.Empty;
         this._Error_Message = string.Empty;

         if(toObra != null)
         {
            if(
               string.IsNullOrWhiteSpace(toObra.codigo) == false
               && 
               string.IsNullOrWhiteSpace(toObra.nombre) == false
               && 
               toObra.codigo.Trim().Length == _nDigitos
            )
            {
               _oObra = new Obra();
               _oObra._Codigo = toObra.codigo;

               //if(_oObra._Existe_Registro() == false)
               //{
                  _oObra._New(toObra.codigo);
                  _oObra._Nombre = toObra.nombre;
                  _oObra._Direccion = toObra.direccion;
                  _oObra._Codpost = toObra.codpos;
                  _oObra._Poblacion = toObra.poblacion;
                  _oObra._Provincia = toObra.provincia;
                  _oObra._Cliente = toObra.cliente;
                  //_oObra._Cif = toObra.cif;

                  llOk = _oObra._Save();
                  if(llOk)
                  {
                     lsGuid = _oObra._Guid_Id;
                  }
               //}
               //else
               //{ 
               //   this._Error_Message += $"El código de Obra \"{toObra.codigo}\" ya existe";
               //}
            }
            else
            { 
               this._Error_Message += $"Encontramos un error entre el código de obra \"{toObra.codigo}\" y el nombre \"{toObra.nombre}\""; 
            }

            //_oObra = null;
         }

         return llOk;
      }
   }
}
