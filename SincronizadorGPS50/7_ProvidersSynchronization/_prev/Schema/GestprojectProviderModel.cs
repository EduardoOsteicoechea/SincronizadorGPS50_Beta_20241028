using System;

namespace SincronizadorGPS50
{
   public class GestprojectProviderModel
   {
      public int? ID { get; set; } = null;
      public string SYNC_STATUS { get; set; } = "";

      public int? PAR_ID { get; set; } = null;
      public string PAR_SUBCTA_CONTABLE_2 { get; set; } = "";
      public string PAR_NOMBRE { get; set; } = "";
      public string PAR_APELLIDO_1 { get; set; } = "";
      public string PAR_APELLIDO_2 { get; set; } = "";
      public string NOMBRE_COMPLETO
      {
         get
         {
            string apellido1 = PAR_APELLIDO_1 != "" ? " " + PAR_APELLIDO_1 : "";
            string apellido2 = PAR_APELLIDO_2 != "" ? " " + PAR_APELLIDO_2 : "";
            return $"{PAR_NOMBRE}{apellido1}{apellido2}";
         }
         set { }
      }
      //public string NOMBRE_COMPLETO { get; set; } = "";
      public string PAR_NOMBRE_COMERCIAL { get; set; } = "";
      public string PAR_CIF_NIF { get; set; } = "";
      public string PAR_DIRECCION_1 { get; set; } = "";
      public string PAR_CP_1 { get; set; } = "";
      public string PAR_LOCALIDAD_1 { get; set; } = "";
      public string PAR_PROVINCIA_1 { get; set; } = "";
      public string PAR_PAIS_1 { get; set; } = "";

      public string S50_CODE { get; set; } = "";
      public string S50_GUID_ID { get; set; } = "";
      public string S50_COMPANY_GROUP_NAME { get; set; } = "";
      public string S50_COMPANY_GROUP_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_GUID_ID { get; set; } = "";

      public DateTime? LAST_UPDATE { get; set; } = null;

      public int? GP_USU_ID { get; set; } = null;
      public string COMMENTS { get; set; } = "";
   }
}
