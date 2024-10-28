using sage.ew.docscompra;
using System.Collections.Generic;

namespace SincronizadorGPS50
{
   public class SageReceivedInvoiceEnrichedModel
   {
      public SageReceivedInvoiceModel BaseReceivedInvoiceModel {get;set;} = null;
      public List<SageReceivedInvoiceBaseModelTaxModel> SageReceivedInvoiceBaseModelTaxModelList {get;set;} = null;
      public ewDocCompraFACTURA SageReceivedInvoice {get;set;} = null;
      public SynchronizableCompanyModel SageReceivedInvoiceCompany {get;set;} = null;
      public GestprojectProviderModel SageReceivedInvoiceProvider {get;set;} = null;
      public GestprojectProjectModel SageReceivedInvoiceProject {get;set;} = null;
   }
}
