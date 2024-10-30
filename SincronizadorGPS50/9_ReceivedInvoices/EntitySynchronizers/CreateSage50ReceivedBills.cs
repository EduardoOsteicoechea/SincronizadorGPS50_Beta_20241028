using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using sage.ew.global;
using sage.ew.global.Diccionarios;
using SincronizadorGPS50.Sage50Connector;
using SincronizadorGPS50.Workflows.Sage50Connection;


namespace SincronizadorGPS50
{
	public class CreateSage50ReceivedBills
	{
		AlbaranVenta _oAlbaranVenta = new AlbaranVenta();
		public dynamic _oEntidad;
		private clsAlbavenLineas _LinAlbaran = new clsAlbavenLineas();

		private int _nDigitos = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_digitos));
		private int _nArticulo = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_articulo));

		private bool _llFactSer = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Factser"));       // Trabajar con series de docummentos (albaranes)
		private bool _llSerFact = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Serfact"));       // Trabajar con series de docummentos (facturas)

		private string _Letra = string.Empty;
		private string _Numero = string.Empty;
		private string _Almacen = EW_GLOBAL._GetVariable("wc_almacen").ToString();

		public CreateSage50ReceivedBills()
		{
			try
			{
				_CrearEntidad();
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

		public void _CrearEjemploAlbaran()
		{
			try
			{
				_oEntidad = new clsAlbaven();
				_oEntidad.Cabecera.letra = "SF";
				_oEntidad.Cabecera.almacen = _Almacen;
				_oEntidad.Cabecera.formapago = "00";
				_oEntidad.Cabecera.numero = "AAAAAA123456";
				_oEntidad.Cabecera.cliente = "43000002";

				_CrearObjetoLinea();
				_LinAlbaran.articulo = "1";
				_LinAlbaran.definicion = "ARTICULO 1";
				_LinAlbaran.unidades = 2;
				_LinAlbaran.precio = 20;
				_LinAlbaran.dto1 = 0;
				_LinAlbaran.dto2 = 0;
				_LinAlbaran.tipoiva = "03";
				_AddObjetoLinea();

				this._oAlbaranVenta._Create(this._oEntidad);
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




		//   public void _CrearEjemploAlbaran()
		//   {
		//      try
		//      {
		//         bool llOk = false;
		//         _CrearEntidad();

		//         _oEntidad.Cabecera.cliente = "43000002";

		//         // Linea1
		//         _CrearObjetoLinea();
		//         _LinAlbaran.articulo = "1";
		//         _LinAlbaran.definicion = "ARTICULO 1";
		//         _LinAlbaran.unidades = 2;
		//         _LinAlbaran.precio = 20;
		//         _LinAlbaran.dto1 = 0;
		//         _LinAlbaran.dto2 = 0;
		//         _LinAlbaran.tipoiva = "03";
		//         _AddObjetoLinea();

		//         //_CrearObjetoLineaLote();
		//         //clsAlbavenLineasLotes loItemLote = new clsAlbavenLineasLotes();
		//         //loItemLote.lote = "000091";
		//         //loItemLote.unidades = 1;
		//         //_LinAlbaran.lotes.Add(loItemLote);

		//         //loItemLote = new clsAlbavenLineasLotes();
		//         //loItemLote.lote = "000092";
		//         //loItemLote.unidades = 1;
		//         //_LinAlbaran.lotes.Add(loItemLote);

		//         //_AddObjetoLinea();

		//MessageBox.Show($"Cantidad de líneas: {this._oEntidad.Lineas.Count}");

		//         llOk = this._oAlbaranVenta._Create(this._oEntidad);
		//      }
		//      catch(System.Exception exception)
		//      {
		//         throw ApplicationLogger.ReportError(
		//            MethodBase.GetCurrentMethod().DeclaringType.Namespace,
		//            MethodBase.GetCurrentMethod().DeclaringType.Name,
		//            MethodBase.GetCurrentMethod().Name,
		//            exception
		//         );
		//      };
		//   }
		private void _CrearEntidad()
		{
			try
			{
				_oEntidad = new clsAlbaven(); // Albarán de Venta Sage 50 = Facturas emitidas de Gest project
				_oEntidad.Cabecera.letra = "";
				_oEntidad.Cabecera.almacen = _Almacen;
				_oEntidad.Cabecera.formapago = "00";
				_oEntidad.Cabecera.numero = "ooooooooo";
				if(_llFactSer)
				{
					_oEntidad.Cabecera.letra = "SF";
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
		private void _CrearObjetoLinea()
		{
			try
			{
				_LinAlbaran = new clsAlbavenLineas();
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
		private void _AddObjetoLinea()
		{
			try
			{
				int lnLinea = 0;
				if(_oEntidad.Lineas != null && _oEntidad.Lineas.Count != 0)
				{
					lnLinea = ((List<clsAlbavenLineas>)((clsAlbaven)_oEntidad).Lineas).Max(x => x.linea);
				};
				lnLinea++;
				_LinAlbaran.linea = lnLinea;
				_oEntidad.Lineas.Add(_LinAlbaran);
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
		private void _CrearObjetoLineaLote()
		{
			try
			{
				_LinAlbaran.lotes = new List<clsAlbavenLineasLotes>();
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
