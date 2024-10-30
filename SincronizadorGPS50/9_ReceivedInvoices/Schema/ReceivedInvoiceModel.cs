using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class SageReceivedInvoiceModel 
   {
      public string CompanyNumber {get;set;} = "";
      public string Number {get;set;} = "";
      public string ProviderCode {get;set;} = "";
      public string GuidId {get;set;} = "";
      public string IvaObject {get;set;} = "";
      public List<SincronizadorGPS50ReceivedInvoiceDetailModel> Details {get;set;} = null;
   }
}
