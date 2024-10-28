//using SincronizadorGPS50.Sage50Connector;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   internal class ExsistingTaxListWorkflow
//   {
//      public ExsistingTaxListWorkflow
//      (
//         System.Data.SqlClient.SqlConnection connection,
//         CompanyGroup Sage50ConnectionManager, 
//         List<GestprojectTaxModel> entityList,
//         List<GestprojectTaxModel> unsynchronizedEntityList,
//         ISynchronizationTableSchemaProvider tableSchema
//      )
//      {
//         try
//         {
//            string entityTypeName = "impuesto(s)";
//            if(unsynchronizedEntityList.Count > 0)
//            {
//               DialogResult result = MessageBox.Show($"Partiendo de la selección encontramos {unsynchronizedEntityList.Count} {entityTypeName} desactualizados.\n\n¿Desea sincronizarlo(s)?", "Confirmación de actualización", MessageBoxButtons.OKCancel);

//               if(result == DialogResult.OK)
//               {
//                  string tableName = "IMPUESTO_CONFIG";

//                  List<GestprojectTaxModel> gestprojectDatabaseEntities = new GestprojectTaxesManager().GetEntities(
//                     connection,
//                     tableName,
//                     tableSchema.GestprojectFieldsTupleList
//                  );

//                  List<string> gestprojectEntitiesSubaccountableAccountList = gestprojectDatabaseEntities.Select(x => x.IMP_SUBCTA_CONTABLE).ToList();

//                  List<int?> gestprojectEntitiesIdList = gestprojectDatabaseEntities.Select(x => x.IMP_ID).ToList();

//                  new DataVisualizer<string>("gestprojectDatabaseEntities", gestprojectDatabaseEntities.Select(x=>x.IMP_NOMBRE));
//                  new DataVisualizer<string>("gestprojectEntitiesSubaccountableAccountList", gestprojectEntitiesSubaccountableAccountList);
//                  new DataVisualizer<int?>("gestprojectEntitiesIdList", gestprojectEntitiesIdList);

//                  for(global::System.Int32 i = 0; i < entityList.Count; i++)
//                  {
//                     if(unsynchronizedEntityList.Contains(entityList[i]))
//                     {
//                        GestprojectTaxModel entity = entityList[i];

//                        PropertyInfo[] propertyInfoArray = entity.GetType().GetProperties();

//                        // Get the Gestproject database entity corresponding tax
//                        if(gestprojectEntitiesSubaccountableAccountList.Contains(entity.IMP_SUBCTA_CONTABLE))
//                        {
//                           GestprojectTaxModel gestprojectDatabaseCorrespondingEntity = gestprojectDatabaseEntities.FirstOrDefault(x => x.IMP_SUBCTA_CONTABLE == entity.IMP_SUBCTA_CONTABLE);

//                           if(gestprojectDatabaseCorrespondingEntity.IMP_ID == entity.IMP_ID)
//                           {
//                              // Create the custom Gestproject Taxes table sql string
//                              StringBuilder GestprojectTaxesColumnsAndValuesStringBuilder = new StringBuilder();
//                              for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//                              {
//                                 string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                                 dynamic value = entity.GetType().GetProperty(currentColumnName).GetValue(entity, null);

//                                 if(currentColumnName != "IMP_ID")
//                                 {
//                                    GestprojectTaxesColumnsAndValuesStringBuilder.Append($"{currentColumnName}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
//                                 };
//                              };

//                              // Create the custom Synchronization table sql string
//                              StringBuilder SynchronizationTableColumnsAndValuesStringBuilder = new StringBuilder();
//                              for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//                              {
//                                 string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                                 dynamic value = entity.GetType().GetProperty(currentColumnName).GetValue(entity, null);

//                                 if(currentColumnName != "IMP_ID")
//                                 {
//                                    SynchronizationTableColumnsAndValuesStringBuilder.Append($"{currentColumnName}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
//                                 };
//                              };

//                              new UpdateTax( 
//                                 connection, 
//                                 GestprojectTaxesColumnsAndValuesStringBuilder.ToString().TrimEnd(','), 
//                                 GestprojectTaxesCondition1StringBuilder.ToString().TrimEnd(','),
//                                 SynchronizationTableColumnsAndValuesStringBuilder.ToString().TrimEnd(','), 
//                                 SynchronizationTableCondition1StringBuilder.ToString().TrimEnd(','),
//                                 entity, 
//                                 tableSchema, 
//                                 Sage50ConnectionManager 
//                              );                              
//                           };
//                        }
//                        else
//                        {                           
//                           // Create the custom sql string. For example, for new taxes, you want to skip the Gestproject Id Co
//                           StringBuilder columnsNamesStringBuilder = new StringBuilder();
//                           StringBuilder columnsValuesStringBuilder = new StringBuilder();
//                           for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//                           {
//                              string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                              dynamic value = entity.GetType().GetProperty(currentColumnName).GetValue(entity, null);

//                              if(currentColumnName != "IMP_ID")
//                              {
//                                 columnsNamesStringBuilder.Append($"{currentColumnName},");
//                                 columnsValuesStringBuilder.Append($"{DynamicValuesFormatters.Formatters[value.GetType()](value)},");
//                              };
//                           };     
                              
//                           MessageBox.Show("Tax doesn't existis on Gestproject");

//                           //new InsertTax
//                           //(
//                           //   connection, 
//                           //   gestprojectIdToAssingToEntity,
//                           //   columnsNamesStringBuilder.ToString().TrimEnd(','), 
//                           //   columnsValuesStringBuilder.ToString().TrimEnd(','), 
//                           //   entity,
//                           //   tableSchema,
//                           //   Sage50ConnectionManager
//                           //);
//                        }; 
//                     };
//                  };
//               };
//            }
//            else
//            {
//               MessageBox.Show($"Los {entityList.Count} {entityTypeName} están sincronizados.");
//            };
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//   }
//}
