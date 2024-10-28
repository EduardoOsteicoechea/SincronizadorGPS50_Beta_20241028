

//using System;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class GestprojectProviderModel
//   {
//      public GestprojectProviderModel()
//      {

//      }
//      public int? synchronization_table_id { get; set; } = null;
//      public int? PAR_ID { get; set; } = null;
//      public string PAR_SUBCTA_CONTABLE_2 { get; set; } = "";
//      public string PAR_NOMBRE { get; set; } = "";
//      public string PAR_APELLIDO_1 { get; set; } = "";
//      public string PAR_APELLIDO_2 { get; set; } = "";
//      public string fullName
//      {
//         get {
//            string apellido1 = PAR_APELLIDO_1 != "" ? " " + PAR_APELLIDO_1 : "";
//            string apellido2 = PAR_APELLIDO_2 != "" ? " " + PAR_APELLIDO_2 : "";
//            return $@"{PAR_NOMBRE}{apellido1}{apellido2}";
//         }
//         set { }
//      }
//      public string PAR_NOMBRE_COMERCIAL { get; set; } = "";
//      public string PAR_CIF_NIF { get; set; } = "";
//      public string PAR_DIRECCION_1 { get; set; } = "";
//      public string PAR_CP_1 { get; set; } = "";
//      public string PAR_LOCALIDAD_1 { get; set; } = "";
//      public string PAR_PROVINCIA_1 { get; set; } = "";
//      public string PAR_PAIS_1 { get; set; } = "";

//      public string synchronization_status { get; set; } = "";
//      public string sage50_code { get; set; } = "";
//      public string sage50_guid_id { get; set; } = "";
//      public string sage50_company_group_name { get; set; } = "";
//      public string sage50_company_group_code { get; set; } = "";
//      public string sage50_company_group_main_code { get; set; } = "";
//      public string sage50_company_group_guid_id { get; set; } = "";

//      public string comments { get; set; } = "";
//      public DateTime? last_record { get; set; } = null;

//      public int? parent_gesproject_user_id { get; set; } = null;
//   }
//}
