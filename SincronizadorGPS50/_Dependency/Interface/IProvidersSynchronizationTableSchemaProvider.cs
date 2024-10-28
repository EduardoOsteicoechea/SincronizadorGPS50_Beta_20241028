using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface IProvidersSynchronizationTableSchemaProvider
   {
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) SynchronizationTableProviderIdColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) SynchronizationStatusColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderIdColumn { get; set; }

      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderAccountableSubaccountColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderNameColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderCommercialNameColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderCIFNIFColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderAddressColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderPostalCodeColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderLocalityColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderProvinceColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderCountryColumn { get; set; }

      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderCodeColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderGuidIdColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderCompanyGroupNameColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderCompanyGroupCodeColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderCompanyGroupMainCodeColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) Sage50ProviderCompanyGroupGuidIdColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) CommentsColumn { get; set; }
      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) ProviderLastUpdateTerminalColumn { get; set; }

      (string ColumnDatabaseName, string ColumnUserFriendlyNane, Type ColumnValueType) GestprojectProviderParentUserIdColumn { get; set; }
   }
}
