//using sage.ew.global.Diccionarios;
//using sage.ew.global;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class IssuedInvoiceProxy
//   {
//        SalesAlbaranModel _oAlbaranVenta = new SalesAlbaranModel();
//        public dynamic _oEntidad;
//        private clsAlbavenLineas _LinAlbaran = new clsAlbavenLineas();

//        private int _nDigitos = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_digitos));
//        private int _nArticulo = Convert.ToInt32(EW_GLOBAL._GetLenCampo(KeyDiccionarioLenCampos.wn_articulo));

//        private bool _llFactSer = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Factser"));       // Trabajar con series de docummentos (albaranes)
//        private bool _llSerFact = Convert.ToBoolean(EW_GLOBAL._GetVariable("wl_Serfact"));       // Trabajar con series de docummentos (facturas)

//        private string _Letra = string.Empty;
//        private string _Numero = string.Empty;
//        private string _Almacen = EW_GLOBAL._GetVariable("wc_almacen").ToString();

//        public IssuedInvoiceProxy()
//        {
//            _CrearEntidad();
//        }

//        public void _CreateAlbaran(SynchronizableIssuedInvoiceModel invoice, List<SynchronizableIssuedInvoiceDetailModel> invoiceDetails)
//        {
//            bool llOk = false;

//            _CrearEntidad();
//            _oEntidad.Cabecera.cliente = invoice.PAR_SUBCTA_CONTABLE.Trim();
//            _oEntidad.Cabecera.obra = invoice.SageProjectCode.Trim();

//            foreach(SynchronizableIssuedInvoiceDetailModel detail in invoiceDetails)
//            {
//               _CrearObjetoLinea();
//               _LinAlbaran.cuentaContable = invoice.FCE_SUBCTA_CONTABLE;
//               _LinAlbaran.tipoiva = invoice.TaxCode;
//               _LinAlbaran.definicion = detail.DFE_CONCEPTO;
//               _LinAlbaran.unidades = detail.DFE_UNIDADES ?? 0;
//               _LinAlbaran.precio = detail.DFE_PRECIO_UNIDAD ?? 0;
//               _AddObjetoLinea();
//            };

//            llOk = _oAlbaranVenta._Create(_oEntidad);

//            if(_oAlbaranVenta._Error_Message != "")
//            {
//               MessageBox.Show(_oAlbaranVenta._Error_Message);
//            }
//        }

//        private void _CrearEntidad()
//        {
//            _oEntidad = new Sage50Connector.clsAlbaven();
//            _oEntidad.Cabecera.letra = "";
//            _oEntidad.Cabecera.almacen = _Almacen;
//            //_oEntidad.Cabecera.formapago = "00";
//            _oEntidad.Cabecera.formapago = "01";

//            if (_llFactSer)
//                _oEntidad.Cabecera.letra = "SF";
//        }

//        private void _CrearObjetoLinea()
//        {
//            _LinAlbaran = new clsAlbavenLineas();
//        }

//        private void _AddObjetoLinea()
//        {
//            int lnLinea = 0;

//            if (_oEntidad.Lineas != null && _oEntidad.Lineas.Count != 0)
//                lnLinea = ((List<clsAlbavenLineas>)((clsAlbaven)_oEntidad).Lineas).Max(x => x.linea);

//            lnLinea++;

//            _LinAlbaran.linea = lnLinea;
//            _oEntidad.Lineas.Add(_LinAlbaran);
//        }
//    }
//}
