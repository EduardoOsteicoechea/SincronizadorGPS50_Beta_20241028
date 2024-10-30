using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class ReceivedInvoiceDetailModel
   {
      public string User { get; set; } = "";
      public string CompanyNumber { get; set; } = "";
      public string InvoiceNumber { get; set; } = ""; // DETALLE_FACTURA_PROVEEDOR -> FCP_ID
      public string Article { get; set; } = "";
      public string Definition { get; set; } = ""; // DETALLE_FACTURA_PROVEEDOR -> DFP_CONCEPTO
      public decimal Units { get; set; } = 0; // DETALLE_FACTURA_PROVEEDOR -> DFP_UNIDADES
      public decimal Price { get; set; } = 0; // DETALLE_FACTURA_PROVEEDOR -> DFP_PRECIO_UNIDAD
      public decimal Discount1 { get; set; } = 0;
      public decimal Discount2 { get; set; } = 0;
      public decimal Import { get; set; } = 0; // DETALLE_FACTURA_PROVEEDOR -> DFP_SUBTOTAL
      public string IvaType { get; set; } = ""; // FACTURA_PROVEEDOR -> FCP_IVA (obtener el código de iva, obtener la entitdad de iva, obtener el valor de iva)
      public decimal Cost { get; set; } = 0;
      public string Account { get; set; } = ""; // FACTURA_PROVEEDOR -> FCP_SUBCTA_CONTABLE

      public DateTime? Date { get; set; } = null;
      public int? LineNumber { get; set; } = null;
      public string ProviderCode { get; set; } = "";
      public decimal CurrencyPrice { get; set; } = 0;
      public decimal CurrencyImport { get; set; } = 0;
      public string GuidId { get; set; } = ""; // DETALLE_FACTURA_PROVEEDOR -> DFP_ID
   }
}                     
