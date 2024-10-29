

namespace SincronizadorGPS50
{
   public class GestprojectCustomerModel
   {
      public int? PAR_ID { get; set; } = -1;
      public string PAR_SUBCTA_CONTABLE { get; set; } = "";
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
      public string PAR_NOMBRE_COMERCIAL { get; set; } = "";
      public string PAR_CIF_NIF { get; set; } = "";
      public string PAR_DIRECCION_1 { get; set; } = "";
      public string PAR_CP_1 { get; set; } = "";
      public string PAR_LOCALIDAD_1 { get; set; } = "";
      public string PAR_PROVINCIA_1 { get; set; } = "";
      public string PAR_PAIS_1 { get; set; } = "";
   }
}
