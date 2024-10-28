using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Data;
using System.Reflection;

namespace SincronizadorGPS50
{
   internal class CreateTableControl
   {
      internal DataTable Table { get; set; }
      public CreateTableControl(GestprojectDataManager.CustomerSyncronizationTableSchema tableSchema)
      {
         try
         {
            Table = new DataTable();

            // 0. SynchronizationTableClientIdColumn
            Table.Columns.Add(
               tableSchema.SynchronizationTableClientIdColumn.ColumnUserFriendlyNane,
               typeof(string)
            );
            // 1. SynchronizationStatusColumn
            Table.Columns.Add(
               tableSchema.SynchronizationStatusColumn.ColumnUserFriendlyNane,
               tableSchema.SynchronizationStatusColumn.ColumnValueType
            );
            // 2. GestprojectClientIdColumn
            Table.Columns.Add(
               tableSchema.GestprojectClientIdColumn.ColumnUserFriendlyNane,
               typeof(string)
            );

            // 3. GestprojectClientAccountableSubaccountColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnValueType
            );
            // 4. GestprojectClientNameColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientNameColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientNameColumn.ColumnValueType
            );
            // 5. GestprojectClientCommercialNameColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientCommercialNameColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientCommercialNameColumn.ColumnValueType
            );
            // 6. GestprojectClientCIFNIFColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientCIFNIFColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientCIFNIFColumn.ColumnValueType
            );
            // 7. GestprojectClientAddressColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientAddressColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientAddressColumn.ColumnValueType
            );
            // 8. GestprojectClientPostalCodeColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientPostalCodeColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientPostalCodeColumn.ColumnValueType
            );
            // 9. GestprojectClientLocalityColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientLocalityColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientLocalityColumn.ColumnValueType
            );
            // 10. GestprojectClientProvinceColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientProvinceColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientProvinceColumn.ColumnValueType
            );
            // 11. GestprojectClientCountryColumn
            Table.Columns.Add(
                tableSchema.GestprojectClientCountryColumn.ColumnUserFriendlyNane,
                tableSchema.GestprojectClientCountryColumn.ColumnValueType
            );

            // 12. Sage50ClientCodeColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientCodeColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientCodeColumn.ColumnValueType
            );
            // 13. Sage50ClientGuidIdColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientGuidIdColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientGuidIdColumn.ColumnValueType
            );
            // 14. Sage50ClientCompanyGroupNameColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnValueType
            );
            // 15. Sage50ClientCompanyGroupCodeColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnValueType
            );
            // 16. Sage50ClientCompanyGroupMainCodeColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnValueType
            );
            // 17. Sage50ClientCompanyGroupGuidIdColumn
            Table.Columns.Add(
               tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnUserFriendlyNane,
               tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnValueType
            );
            // 18. ClientLastUpdateTerminalColumn
            Table.Columns.Add(
               tableSchema.ClientLastUpdateTerminalColumn.ColumnUserFriendlyNane,
               typeof(string)
            );
            // 19. ParentUserIdColumn
            Table.Columns.Add(
               tableSchema.GestprojectClientParentUserIdColumn.ColumnUserFriendlyNane,
               typeof(string)
            );
            // 20. CommentsColumn
            Table.Columns.Add(
               tableSchema.CommentsColumn.ColumnUserFriendlyNane,
               tableSchema.CommentsColumn.ColumnValueType
            );

            for(int i = 0; i < Table.Columns.Count; i++)
            {
               if(i >= Table.Columns.Count - 1)
               {
                  Table.Columns[i].MaxLength = 300;
               };
            };
         }
         catch (System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
