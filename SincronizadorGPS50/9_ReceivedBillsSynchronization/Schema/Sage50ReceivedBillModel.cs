using System;

namespace SincronizadorGPS50
{
   public class Sage50ReceivedBillModel
   {
      public string GUID_ID { get; set; } = "";
      public string EMPRESA { get; set; } = "";
      public string NUMERO { get; set; } = "";
      public DateTime? CREATED { get; set; } = null;
      public string PROVEEDOR { get; set; } = "";
      public decimal? IMPORTE { get; set; } = null;
      public decimal? TOTALDOC { get; set; } = null;
   }
}
