using Infragistics.Designers.SqlEditor;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class InsertSubaccountableAccount
   {
      public InsertSubaccountableAccount
      (
         System.Data.SqlClient.SqlConnection connection, 
         int? gestprojectIdToAssingToEntity,
         string columnsNamesStringBuilder, 
         string columnsValuesStringBuilder, 
         GestprojectSubaccountableAccountModel entity ,
         ISynchronizationTableSchemaProvider tableSchema,
         CompanyGroup Sage50ConnectionManager
      )
      {
         string sqlString = $@"
         INSERT INTO {"CONTAPLUS_SUBCUENTA"}
            ({columnsNamesStringBuilder.ToString().TrimEnd(',')}) 
         VALUES 
            ({columnsValuesStringBuilder.ToString().TrimEnd(',')})
         WHERE
            COS_GRUPO='{entity.COS_GRUPO}'
         AND 
            COS_ID={gestprojectIdToAssingToEntity}
         ;";

         MessageBox.Show(sqlString);

         using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
         {
            sqlCommand.ExecuteNonQuery();
         };

         string updateEntitySyncrhonizationStatusSql = $@"
            UPDATE 
            {tableSchema.TableName} 
            SET 
               {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}', 
               COS_ID={gestprojectIdToAssingToEntity},
               [S50_COMPANY_GROUP_NAME]={gestprojectIdToAssingToEntity},
               [S50_COMPANY_GROUP_CODE]={gestprojectIdToAssingToEntity},
               [S50_COMPANY_GROUP_MAIN_CODE]={gestprojectIdToAssingToEntity},
               [S50_COMPANY_GROUP_GUID_ID]={gestprojectIdToAssingToEntity},
               [GP_USU_ID]={gestprojectIdToAssingToEntity},
            WHERE
               S50_GUID_ID='{entity.S50_GUID_ID}'
         ";

         MessageBox.Show(sqlString);

         using(SqlCommand sqlCommand = new SqlCommand(updateEntitySyncrhonizationStatusSql, connection))
         {
            sqlCommand.ExecuteNonQuery();
         };
      }
   }
}
