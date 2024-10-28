using System.Data.SqlClient;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class CreateClientSynchronizationTable
   {
      public CreateClientSynchronizationTable
      (
         System.Data.SqlClient.SqlConnection connection,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            CREATE TABLE {tableSchema.TableName} 
               (
                  {tableSchema.SynchronizationTableClientIdColumn.ColumnDatabaseName} INT PRIMARY KEY IDENTITY(1,1), 
                  {tableSchema.SynchronizationStatusColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.GestprojectClientIdColumn.ColumnDatabaseName} INT, 
                  {tableSchema.GestprojectClientAccountableSubaccountColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientNameColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientCommercialNameColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientCIFNIFColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientAddressColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientPostalCodeColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientLocalityColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientProvinceColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientCountryColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.Sage50ClientCodeColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.Sage50ClientGuidIdColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.Sage50ClientCompanyGroupNameColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.Sage50ClientCompanyGroupCodeColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.Sage50ClientCompanyGroupMainCodeColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.Sage50ClientCompanyGroupGuidIdColumn.ColumnDatabaseName} VARCHAR(MAX), 
                  {tableSchema.CommentsColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.GestprojectClientParentUserIdColumn.ColumnDatabaseName} VARCHAR(MAX),
                  {tableSchema.ClientLastUpdateTerminalColumn.ColumnDatabaseName} DATETIME DEFAULT GETDATE() NOT NULL
               )
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         }
         catch(SqlException exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.CreateClientSynchronizationTable:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
