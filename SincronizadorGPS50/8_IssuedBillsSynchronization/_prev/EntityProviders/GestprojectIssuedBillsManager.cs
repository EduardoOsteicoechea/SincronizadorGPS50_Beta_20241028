//using GestprojectDataManager;
//using Infragistics.Designers.SqlEditor;
//using Infragistics.Documents.Excel.ConditionalFormatting;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class GestprojectIssuedBillsManager
//   {
//      public List<GestprojectIssuedBillModel> GestprojectEntityList { get; set; } = new List<GestprojectIssuedBillModel>();
//      public List<GestprojectIssuedBillModel> GetEntities
//      (
//         System.Data.SqlClient.SqlConnection connection, 
//         string tableName,
//         List<(string columnName, Type columnType)> columnsAndTypesToQuery
//      )
//      {
//         try
//         {
//            connection.Open();

//            StringBuilder columnsAndValuesStringBuilder = new StringBuilder();
//            for(global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
//            {
//               if(columnsAndTypesToQuery[i].columnName != "PROYECTO" && columnsAndTypesToQuery[i].columnName != "TIPO")
//               {
//                  columnsAndValuesStringBuilder.Append($"{columnsAndTypesToQuery[i].columnName},");
//               };
//            };

//            string sqlString = $@"
//            SELECT 
//               {columnsAndValuesStringBuilder.ToString().TrimEnd(',')}
//            FROM 
//               {tableName} 
//            ;";

//            //MessageBox.Show(sqlString);

//            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
//            {
//               using(SqlDataReader reader = sqlCommand.ExecuteReader())
//               {
//                  while(reader.Read())
//                  {
//                     GestprojectIssuedBillModel entity = new GestprojectIssuedBillModel();

//                     PropertyInfo[] properties = entity.GetType().GetProperties();

//                     for(global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
//                     {
//                        TypeRevisor<GestprojectIssuedBillModel>.Check(
//                           columnsAndTypesToQuery[i].columnType,
//                           columnsAndTypesToQuery[i].columnName,
//                           entity,
//                           reader,
//                           i,
//                           properties
//                        );
//                     };

//                     GestprojectEntityList.Add(entity);
//                  };
//               };
//            };

//            return GestprojectEntityList;
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         }
//         finally
//         {
//            connection.Close();
//         };
//      }
//   }
//}
