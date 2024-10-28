using System;

namespace SincronizadorGPS50.Sage50Connector
{
   public class ProviderComparer
   {
      public bool Exists { get; set; } = false;
      public string Sage50Guid { get; set; } = "";
      public string Sage50Code{ get; set; } = "";
      public ProviderComparer
      (
         string name,
         string cif
      )
      {
         try
         {
            //foreach(Sage50ProviderModel Provider in new GetSage50Providers().ProviderList)
            //{
            //   bool nameFullMatch = name == Provider.NOMBRE;
            //   bool cifFullMatch = cif == Provider.CIF;

            //   if(nameFullMatch && cifFullMatch)
            //   {
            //      Sage50Guid = Provider.GUID_ID;
            //      Sage50Code = Provider.CODIGO;
            //      Exists = true;
            //   }
            //}
         }
         catch(Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
