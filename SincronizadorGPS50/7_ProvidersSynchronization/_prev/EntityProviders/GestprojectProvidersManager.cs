using GestprojectDataManager;
using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class GestprojectProvidersManager
   {
      public List<GestprojectProviderModel> GestprojectProvidersList { get; set; } = new List<GestprojectProviderModel>();
      public List<GestprojectProviderModel> GetAll(System.Data.SqlClient.SqlConnection connection, List<int> IdList = null)
      {
         try
         {
            List<int> gestProjectProviderIdList = new GestprojectPAR_TPA().Get(connection, "12");

               //new VisualizePropertiesAndValues<int>(
               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
               //   "gestProjectProviderIdList",
               //   gestProjectProviderIdList
               //);

            if(gestProjectProviderIdList.Count > 0)
            {
               List<GestprojectParticipantModel> gestprojectProviderParticipantList = new GestprojectParticipants().Get(connection, gestProjectProviderIdList);
               
               //new VisualizePropertiesAndValues<GestprojectParticipantModel>(
               //   MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
               //   "gestprojectProviderParticipantList",
               //   gestprojectProviderParticipantList
               //);

               for(global::System.Int32 i = 0; i < gestprojectProviderParticipantList.Count; i++)
               {
                  GestprojectParticipantModel gestprojectProviderParticipant = gestprojectProviderParticipantList[i];
                  GestprojectProviderModel gestprojectProvider = new GestprojectProviderModel();

                  gestprojectProvider.PAR_ID = gestprojectProviderParticipant.PAR_ID;
                  gestprojectProvider.PAR_SUBCTA_CONTABLE_2 = gestprojectProviderParticipant.PAR_SUBCTA_CONTABLE_2;
                  gestprojectProvider.PAR_NOMBRE = gestprojectProviderParticipant.PAR_NOMBRE;
                  gestprojectProvider.PAR_NOMBRE_COMERCIAL = gestprojectProviderParticipant.PAR_NOMBRE_COMERCIAL;
                  gestprojectProvider.PAR_CIF_NIF = gestprojectProviderParticipant.PAR_CIF_NIF;
                  gestprojectProvider.PAR_DIRECCION_1 = gestprojectProviderParticipant.PAR_DIRECCION_1;
                  gestprojectProvider.PAR_CP_1 = gestprojectProviderParticipant.PAR_CP_1;
                  gestprojectProvider.PAR_LOCALIDAD_1 = gestprojectProviderParticipant.PAR_LOCALIDAD_1;
                  gestprojectProvider.PAR_PROVINCIA_1 = gestprojectProviderParticipant.PAR_PROVINCIA_1;
                  gestprojectProvider.PAR_PAIS_1 = gestprojectProviderParticipant.PAR_PAIS_1;
                  gestprojectProvider.PAR_APELLIDO_1 = gestprojectProviderParticipant.PAR_APELLIDO_1;
                  gestprojectProvider.PAR_APELLIDO_2 = gestprojectProviderParticipant.PAR_APELLIDO_2;

                  GestprojectProvidersList.Add(gestprojectProvider);
               };
            };

            return GestprojectProvidersList;
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
