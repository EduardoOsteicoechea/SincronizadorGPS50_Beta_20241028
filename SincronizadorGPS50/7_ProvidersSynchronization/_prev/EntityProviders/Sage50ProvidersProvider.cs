using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal class Sage50ProvidersProvider : ISage50EntitiesProvider<Sage50ProviderModel>
   {

      public List<Sage50ProviderModel> GetProviders()
      {
         return new GetSage50Providers().Entities;
      }
   }
}
