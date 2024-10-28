using Infragistics.Designers.SqlEditor;
using sage.ew.db;
using Sage.ES.S50.Addons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class CompaniesDataTableManager : IGridDataSourceGenerator<SynchronizableCompanyModel, SageCompanyModel>
   {
      public List<SynchronizableCompanyModel> GestprojectEntities { get; set; }
      public List<SageCompanyModel> SageEntities { get; set; }
      public List<SynchronizableCompanyModel> SynchronizableEntities { get; set; }
      public IGestprojectConnectionManager GestprojectConnectionManager { get; set; }
      public SqlConnection Connection { get; set; }
      public ISage50ConnectionManager SageConnectionManager { get; set; }
      public ISynchronizationTableSchemaProvider TableSchema { get; set; }
      public DataTable DataTable { get; set; }

      public System.Data.DataTable GenerateDataTable
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         try
         {
            GestprojectConnectionManager = gestprojectConnectionManager;
            Connection = GestprojectConnectionManager.GestprojectSqlConnection;
            SageConnectionManager = sage50ConnectionManager;
            TableSchema = tableSchemaProvider;

            ManageSynchronizationTableStatus();
            LinkEndpointsModels();

            foreach (SynchronizableCompanyModel entity in SynchronizableEntities)
            {
               if (ValidateIfEntityWasSynchronized(entity) == false)
               {
                  RegisterEntityOnSynchronizationTable(entity);
                  //PrepareAllGestprojectEntitiesForRegistry(entity);
               }
               else
               {
                  try
                  {
                     Connection.Open();
                     AppendSynchronizationDataToEntity(entity);
                  }
                  catch(System.Exception exception)
                  {
                     throw ApplicationLogger.ReportError(
                        MethodBase.GetCurrentMethod().DeclaringType.Namespace,
                        MethodBase.GetCurrentMethod().DeclaringType.Name,
                        MethodBase.GetCurrentMethod().Name,
                        exception
                     );
                  }
                  finally
                  {
                     Connection.Close();
                  };
               }
            }

            CreateAndDefineDataSource();
            PaintEntitiesOnDataSource();

            return DataTable;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public void ManageSynchronizationTableStatus()
      {
         ISynchronizationDatabaseTableManager entitySyncronizationTableStatusManager = new EntitySyncronizationTableStatusManager();

         bool tableExists = entitySyncronizationTableStatusManager.TableExists(
               GestprojectConnectionManager.GestprojectSqlConnection,
               TableSchema.TableName
            );

         if(tableExists == false)
         {
            entitySyncronizationTableStatusManager.CreateTable
            (
               GestprojectConnectionManager.GestprojectSqlConnection,
               TableSchema
            );
         };
      }


      public void LinkEndpointsModels()
      {
         try
         {
            GestprojectEntities = GetGestprojectCompanies();
            SageEntities = GetSageCompanies();
            SynchronizableEntities = MatchMatchingCompanies();
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public List<SynchronizableCompanyModel> GetGestprojectCompanies()
      {
         try
         {
            Connection.Open();

            List<SynchronizableCompanyModel> sincronizadorGP50CompanyModelList = new List<SynchronizableCompanyModel>();

            string sqlString = $@"SELECT PAR_ID FROM [GESTPROJECT2020].[dbo].[PAR_TPA] WHERE TPA_ID=16;";

            string companiesIds = "";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     companiesIds += "'" + Convert.ToInt32(reader.GetValue(0)) + "',";
                  };
               };
            };

            companiesIds = companiesIds.TrimEnd(',');

            string sqlString2 = $@"SELECT PAR_ID,PAR_NOMBRE,PAR_CIF_NIF FROM [GESTPROJECT2020].[dbo].[PARTICIPANTE] WHERE PAR_ID IN ({companiesIds});";

            using(SqlCommand command = new SqlCommand(sqlString2, Connection))
            {
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     SynchronizableCompanyModel sincronizadorGP50CompanyModel = new SynchronizableCompanyModel();

                     sincronizadorGP50CompanyModel.PAR_ID = Convert.ToInt32(reader.GetValue(0));
                     sincronizadorGP50CompanyModel.PAR_NOMBRE = Convert.ToString(reader.GetValue(1));
                     sincronizadorGP50CompanyModel.PAR_CIF_NIF = Convert.ToString(reader.GetValue(2));

                     sincronizadorGP50CompanyModelList.Add(sincronizadorGP50CompanyModel);
                  };
               };
            };

            return sincronizadorGP50CompanyModelList;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      public List<SageCompanyModel> GetSageCompanies()
      {
         try
         {
            List<SageCompanyModel> sageCompanyModelList = new List<SageCompanyModel>();

            DataTable companiesTable = new DataTable();
            string sqlString = $@"
            SELECT
               CODIGO
               ,NOMBRE
               ,CIF
               ,GUID_ID
            FROM
               {DB.SQLDatabase("GESTION", "empresa")}
            ";

            DB.SQLExec(sqlString, ref companiesTable);

            for(int i = 0; i < companiesTable.Rows.Count; i++)
            {
               SageCompanyModel sageCompanyModel = new SageCompanyModel();

               sageCompanyModel.SageCompanyNumber = Convert.ToString(companiesTable.Rows[i].ItemArray[0]);
               sageCompanyModel.SageName = Convert.ToString(companiesTable.Rows[i].ItemArray[1]);
               sageCompanyModel.SageCifNif = Convert.ToString(companiesTable.Rows[i].ItemArray[2]);
               sageCompanyModel.SageGuidId = Convert.ToString(companiesTable.Rows[i].ItemArray[3]);

               sageCompanyModelList.Add(sageCompanyModel);
            };

            return sageCompanyModelList;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public List<SynchronizableCompanyModel> MatchMatchingCompanies()
      {
         try
         {
            List<SynchronizableCompanyModel> matchingCompaniesList = new List<SynchronizableCompanyModel>();

            foreach(SynchronizableCompanyModel gestprojectCompany in GestprojectEntities)
            {
               SageCompanyModel sageMatchingCompany = SageEntities.FirstOrDefault(
                  sageCompany => sageCompany.SageCifNif.Trim() == gestprojectCompany.PAR_CIF_NIF.Trim()
               );

               if(sageMatchingCompany != null)
               {
                  gestprojectCompany.SageCompanyNumber = sageMatchingCompany.SageCompanyNumber;
                  gestprojectCompany.S50_GUID_ID = sageMatchingCompany.SageGuidId;
                  gestprojectCompany.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
                  matchingCompaniesList.Add(gestprojectCompany);
               };
            };

            return matchingCompaniesList;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public bool ValidateIfEntityWasSynchronized(SynchronizableCompanyModel entity)
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               IF EXISTS (SELECT 1 FROM {TableSchema.TableName})
               BEGIN
                  SELECT
                     S50_GUID_ID
                  FROM
                     {TableSchema.TableName}
                  WHERE
                     PAR_CIF_NIF=@PAR_CIF_NIF
               END
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@PAR_CIF_NIF", entity.PAR_CIF_NIF);
                  
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     string entitySageGuid = reader["S50_GUID_ID"] as string;
                     if(entitySageGuid != "")
                     {
                        return true;
                     }
                     else
                     {
                        return false;
                     }
                  }
               }
            }

            return false;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      public void PrepareAllGestprojectEntitiesForRegistry(SynchronizableCompanyModel entity)
      {
         try
         {
            if(GestprojectEntities.Count > SynchronizableEntities.Count)
            {
               foreach(SynchronizableCompanyModel gestprojectEntity in GestprojectEntities)
               {
                  if(SynchronizableEntities.Any(
                     synchronizableEntity => synchronizableEntity.PAR_CIF_NIF == gestprojectEntity.PAR_CIF_NIF
                  ) == false)
                  {
                     gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
                     SynchronizableEntities.Add(gestprojectEntity);
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }


      public void RegisterEntityOnSynchronizationTable(SynchronizableCompanyModel entity )
      {
         try
         {
            Connection.Open();

            string sqlString = $@"
               INSERT INTO
                  {TableSchema.TableName}
               (
                  SYNC_STATUS
                  ,PAR_ID
                  ,PAR_NOMBRE
                  ,PAR_CIF_NIF
                  ,SageCompanyNumber
                  ,S50_GUID_ID
                  ,S50_COMPANY_GROUP_NAME
                  ,S50_COMPANY_GROUP_CODE
                  ,S50_COMPANY_GROUP_MAIN_CODE
                  ,S50_COMPANY_GROUP_GUID_ID
                  ,GP_USU_ID
                  ,COMMENTS
               )
               Values
               (
                  @SYNC_STATUS
                  ,@PAR_ID
                  ,@PAR_NOMBRE
                  ,@PAR_CIF_NIF
                  ,@SageCompanyNumber
                  ,@S50_GUID_ID
                  ,@S50_COMPANY_GROUP_NAME
                  ,@S50_COMPANY_GROUP_CODE
                  ,@S50_COMPANY_GROUP_MAIN_CODE
                  ,@S50_COMPANY_GROUP_GUID_ID
                  ,@GP_USU_ID
                  ,@COMMENTS               
               )
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, Connection))
            {
               command.Parameters.AddWithValue("@SYNC_STATUS", entity.SYNC_STATUS);
               command.Parameters.AddWithValue("@PAR_ID", entity.PAR_ID);
               command.Parameters.AddWithValue("@PAR_NOMBRE", entity.PAR_NOMBRE);
               command.Parameters.AddWithValue("@PAR_CIF_NIF", entity.PAR_CIF_NIF);
               command.Parameters.AddWithValue("@SageCompanyNumber", entity.SageCompanyNumber);
               command.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_NAME", SageConnectionManager.CompanyGroupData.CompanyName);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_CODE", SageConnectionManager.CompanyGroupData.CompanyCode);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_MAIN_CODE", SageConnectionManager.CompanyGroupData.CompanyMainCode);
               command.Parameters.AddWithValue("@S50_COMPANY_GROUP_GUID_ID", SageConnectionManager.CompanyGroupData.CompanyGuidId);
               command.Parameters.AddWithValue("@GP_USU_ID", GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID);
               command.Parameters.AddWithValue("@COMMENTS", entity.COMMENTS);

               if(command.ExecuteNonQuery() > 0)
               {
                  AppendSynchronizationDataToEntity(entity);
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            Connection.Close();
         };
      }


      public void AppendSynchronizationDataToEntity(SynchronizableCompanyModel entity)
      {
         try
         {
            string sqlString2 = $@"
            SELECT 
               ID,
               LAST_UPDATE,
               COMMENTS 
            FROM 
               {TableSchema.TableName} 
            WHERE 
               S50_GUID_ID=@S50_GUID_ID
            ;";

            using(SqlCommand command2 = new SqlCommand(sqlString2, Connection))
            {
               command2.Parameters.AddWithValue("@S50_GUID_ID", entity.S50_GUID_ID);

               using(SqlDataReader reader = command2.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int? id = reader["ID"] as int?;
                     DateTime? lastUpdate = reader["LAST_UPDATE"] as DateTime? ?? DateTime.Now;
                     string comments = reader["COMMENTS"] as string ?? "";

                     if(id.HasValue)
                     {
                        entity.ID = id.Value;
                        entity.S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
                        entity.S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
                        entity.S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
                        entity.S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
                        entity.GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;
                        entity.LAST_UPDATE = lastUpdate.Value;
                        entity.COMMENTS = comments;
                     }
                     else
                     {
                        throw new NullReferenceException();
                     };
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }





      public void CreateAndDefineDataSource()
      {
         IDataTableGenerator entityDataTableGenerator = new SyncrhonizationDataTableGenerator();
         DataTable = entityDataTableGenerator.CreateDataTable(TableSchema.ColumnsTuplesList);
      }


      public void PaintEntitiesOnDataSource()
      {
         ISynchronizableEntityPainter<SynchronizableCompanyModel> entityPainter = new EntityPainter<SynchronizableCompanyModel>();
         entityPainter.PaintEntityListOnDataTable(
            SynchronizableEntities,
            DataTable,
            TableSchema.ColumnsTuplesList
         );
      }
   }
}