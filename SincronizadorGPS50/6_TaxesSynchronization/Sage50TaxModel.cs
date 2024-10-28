namespace SincronizadorGPS50
{
   public class Sage50TaxModel
   {
      // Sage50 fields
      public string CODIGO { get; set; }
      public string GUID_ID { get; set; }
      public string NOMBRE { get; set; }

      public string IVA { get; set; }
      public string CTA_IV_REP { get; set; }
      public string CTA_IV_SOP { get; set; }
      public decimal RETENCION { get; set; }
      public string CTA_RE_REP { get; set; }
      public string CTA_RE_SOP { get; set; }


      // Additional reference fields
      public string IMP_TIPO { get; set; }
   }
}
