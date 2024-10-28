
namespace SincronizadorGPS50.GestprojectDataManager
{
   public class SynchronizerUserRememberableDataModel
   {
      public int? GP_CNX_ID { get; set; } = null;
      public string GP_CNX_PERSONAL { get; set; } = "";
      public string GP_CNX_USUARIO { get; set; } = "";
      public string GP_CNX_PERFIL { get; set; } = "";
      public string GP_CNX_EQUIPO { get; set; } = "";
      public int? GP_USU_ID { get; set; } = null;
      public string SAGE_50_LOCAL_TERMINAL_PATH { get; set; } = "";
      public string SAGE_50_USER_NAME { get; set; } = "";
      public string SAGE_50_PASSWORD { get; set; } = "";
      public string SAGE_50_COMPANY_GROUP_NAME { get; set; } = "";
      public string SAGE_50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string SAGE_50_COMPANY_GROUP_CODE { get; set; } = "";
      public string SAGE_50_COMPANY_GROUP_GUID_ID { get; set; } = "";
      public byte REMEMBER { get; set; } = 0;
   }
}
