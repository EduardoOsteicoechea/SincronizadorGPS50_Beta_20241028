//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   internal class TaxSynchronizationWorkflow2
//   {
//      private string _EntityTypeNameRoot { get; set; } = "";
//      private string _EntityTypeNameGender { get; set; } = "";
//      private string _EntityTypePlural { get; set; } = "";
//      private IGestprojectConnectionManager _GestprojectConnectionManager { get; set; } = null;
//      private CompanyGroup _Sage50CompanyGroupData { get; set; } = null;
//      private ISynchronizationTableSchemaProvider _TableSchema { get; set; } = null;
//      private List<GestprojectTaxModel> _SynchronizationTableEntities { get; set; } = null;
//      private List<GestprojectTaxModel> _DesynchronizedEntities { get; set; } = null;
//      private List<Sage50TaxModel> _Sage50Entities { get; set; } = null;
//      private DialogResult _DialogResult { get; set; } = DialogResult.Cancel;
//      private List<GestprojectTaxModel> _EntitiesToBeCreated { get; set; } = null;
//      private List<Sage50TaxModel> _SageEntitiesToBeSynchronized { get; set; } = null;
//      public void DetermineEntityGenderAndPluralFormat()
//      {
//         this._EntityTypeNameRoot = "impuest";
//         this._EntityTypeNameGender = "o";
//         this._EntityTypePlural = "(s)";
//      }
//      public void DisplaySynchronizationAutorizationDialog()
//      {
//         if(this._DesynchronizedEntities.Count > 0)
//         {
//            this._DialogResult = MessageBox.Show($"Partiendo de la selección encontramos {this._DesynchronizedEntities.Count} {this._EntityTypeNameRoot + this._EntityTypeNameGender + this._EntityTypePlural} desactualizad{this._EntityTypeNameGender + this._EntityTypePlural}.\n\n¿Desea sincronizarl{this._EntityTypeNameGender + this._EntityTypePlural}?", "Confirmación de actualización", MessageBoxButtons.OKCancel);
//         }
//         else
//         {
//            MessageBox.Show($"Los {this._SynchronizationTableEntities.Count - (this._SynchronizationTableEntities.Count - _Sage50Entities.Count)} {this._EntityTypeNameRoot + this._EntityTypeNameGender + this._EntityTypePlural} están sincronizad{this._EntityTypeNameGender + this._EntityTypePlural}.");
//         };
//      }

//      public void PopulateEntitiesToBeCreatedAndUpdatedLists()
//      {
//         List<int?> unsynchronizedEntityListIds = this._DesynchronizedEntities.Select(entity => entity.IMP_ID).ToList();

//         foreach(Sage50TaxModel sageEntity in this._Sage50Entities)
//         {

//            List<GestprojectTaxModel> gestprojectTaxTableTaxes = new GetGestprojectTaxTableTaxes(
//                  this._GestprojectConnectionManager.GestprojectSqlConnection,
//                  "IMPUESTO_CONFIG"
//               ).Entities;

//            List<int?> gestprojectTaxTableTaxesIds = gestprojectTaxTableTaxes.Select(x => x.IMP_ID).ToList();
//            List<string> gestprojectTaxTableTaxesDescriptions = gestprojectTaxTableTaxes.Select(x => x.IMP_DESCRIPCION).ToList();

//            GestprojectTaxModel correspondingUnsynchronizedSynchronizationEntity = null;
//            if(sageEntity.NOMBRE.Contains("IVA"))
//            {
//               correspondingUnsynchronizedSynchronizationEntity =
//                  this._DesynchronizedEntities
//                  .FirstOrDefault(
//                     entity => entity.IMP_SUBCTA_CONTABLE == sageEntity.CTA_IV_REP
//                     &&
//                     gestprojectTaxTableTaxesDescriptions.Contains(sageEntity.NOMBRE)
//                     //gestprojectTaxTableTaxesIds.Contains(entity.IMP_ID)
//                     &&
//                     entity.S50_COMPANY_GROUP_GUID_ID != ""
//                  );

//               if(correspondingUnsynchronizedSynchronizationEntity == null)
//               {
//                  this._EntitiesToBeCreated.Add(correspondingUnsynchronizedSynchronizationEntity);
//               }
//               else
//               {
//                  this._SageEntitiesToBeSynchronized.Add(sageEntity);
//               };
//            }
//            else
//            {
//               correspondingUnsynchronizedSynchronizationEntity =
//                  this._DesynchronizedEntities
//                  .FirstOrDefault(
//                     entity => entity.IMP_SUBCTA_CONTABLE == sageEntity.CTA_RE_REP
//                     &&
//                     gestprojectTaxTableTaxesIds.Contains(entity.IMP_ID)
//                     &&
//                     entity.S50_COMPANY_GROUP_GUID_ID != ""
//                  );

//               if(correspondingUnsynchronizedSynchronizationEntity == null)
//               {
//                  this._EntitiesToBeCreated.Add(correspondingUnsynchronizedSynchronizationEntity);
//               }
//               else
//               {
//                  this._SageEntitiesToBeSynchronized.Add(sageEntity);
//               };
//            };
//         };
//      }

//      public void CreateEntities()
//      {
//         if(this._EntitiesToBeCreated.Count > 0)
//         {
//            foreach(GestprojectTaxModel entity in this._EntitiesToBeCreated)
//            {
//               GestprojectTaxModel entity = new GestprojectTaxModel();

//               entity.IMP_TIPO = Sage50Entity.IMP_TIPO;
//               if(Sage50Entity.IMP_TIPO == "IVA")
//               {
//                  entity.IMP_NOMBRE = $"{Sage50Entity.IMP_TIPO} {Sage50Entity.IVA.ToString().Split(',')[0]}";
//                  entity.IMP_VALOR = Convert.ToDecimal(Sage50Entity.IVA);
//                  entity.IMP_SUBCTA_CONTABLE = Sage50Entity.CTA_IV_REP;
//                  entity.IMP_SUBCTA_CONTABLE_2 = Sage50Entity.CTA_IV_SOP;
//               }
//               else
//               {
//                  entity.IMP_NOMBRE = $"{Sage50Entity.IMP_TIPO} {Sage50Entity.RETENCION.ToString().Split(',')[0]}";
//                  entity.IMP_VALOR = Sage50Entity.RETENCION;
//                  entity.IMP_SUBCTA_CONTABLE = Sage50Entity.CTA_RE_REP;
//                  entity.IMP_SUBCTA_CONTABLE_2 = Sage50Entity.CTA_RE_SOP;
//               };
//               entity.S50_CODE = Sage50Entity.CODIGO;
//               entity.S50_GUID_ID = Sage50Entity.GUID_ID;
//               entity.IMP_DESCRIPCION = Sage50Entity.NOMBRE;

//               StringBuilder GestprojectTaxesColumnsStringBuilder = new StringBuilder();
//               StringBuilder GestprojectTaxesValuesStringBuilder = new StringBuilder();
//               for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//               {
//                  string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                  Type currentColumnType = tableSchema.GestprojectFieldsTupleList[j].columnType;
//                  dynamic value =
//                     entity
//                     .GetType()
//                     .GetProperty(currentColumnName)
//                     .GetValue(entity, null);

//                  if(currentColumnName != "IMP_ID")
//                  {
//                     GestprojectTaxesColumnsStringBuilder.Append($"{currentColumnName},");
//                     GestprojectTaxesValuesStringBuilder.Append($"{DynamicValuesFormatters.Formatters[currentColumnType](value)},");
//                  };
//               };



//               new InsertSageEntityIntoGestprojectTaxTable(
//                  gestprojectConnectionManager.GestprojectSqlConnection,
//                  "IMPUESTO_CONFIG",
//                  GestprojectTaxesColumnsStringBuilder.ToString().TrimEnd(','),
//                  GestprojectTaxesValuesStringBuilder.ToString().TrimEnd(','),
//                  entity
//               );

//               new AppendGestprojectTaxTableIdToSynchronizationTable(
//                  gestprojectConnectionManager.GestprojectSqlConnection,
//                  "IMPUESTO_CONFIG",
//                  entity,
//                  tableSchema
//               );

//               StringBuilder TaxSynchronizationDataAppendingString = new StringBuilder();

//               int? currentSessionUserId = gestprojectConnectionManager.GestprojectUserRememberableData.GP_CNX_ID;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.ParentUserId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[currentSessionUserId.GetType()](currentSessionUserId)},");

//               string taxCode = Sage50Entity.CODIGO;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50Code.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxCode.GetType()](taxCode)},");

//               string taxGuidId = Sage50Entity.GUID_ID;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50GuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxGuidId.GetType()](taxGuidId)},");

//               string companyGroupName = Sage50CompanyGroupData.CompanyName;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupName.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupName)},");

//               string companyGroupCode = Sage50CompanyGroupData.CompanyCode;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupCode)},");

//               string companyGroupMainCode = Sage50CompanyGroupData.CompanyMainCode;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupMainCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupMainCode)},");

//               string companyGroupGuidId = Sage50CompanyGroupData.CompanyGuidId;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupGuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupGuidId)},");

//               ////////////////////////////////////////
//               /// This last step is required to relate the current worked entity to it's synchronization table registry
//               ////////////////////////////////////////

//               correspondingUnsynchronizedSynchronizationEntity.S50_GUID_ID = Sage50Entity.GUID_ID;

//               new AppendSynchronizationDataToEntityRegistry(
//                  gestprojectConnectionManager.GestprojectSqlConnection,
//                  tableSchema,
//                  correspondingUnsynchronizedSynchronizationEntity,
//                  TaxSynchronizationDataAppendingString.ToString().TrimEnd(',')
//               );
//            };
//         };
//      }

//      public void UpdateEntities()
//      {
//         if(this._SageEntitiesToBeSynchronized.Count > 0)
//         {
//            foreach(Sage50TaxModel entity in this._SageEntitiesToBeSynchronized)
//            {
//               StringBuilder GestprojectTaxesColumnsAndValuesStringBuilder = new StringBuilder();

//               for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//               {
//                  string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                  dynamic value =
//                     correspondingUnsynchronizedSynchronizationEntity
//                     .GetType()
//                     .GetProperty(currentColumnName)
//                     .GetValue(correspondingUnsynchronizedSynchronizationEntity, null);

//                  if(currentColumnName != "IMP_ID")
//                  {
//                     GestprojectTaxesColumnsAndValuesStringBuilder.Append($"{currentColumnName}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
//                  };
//               };

//               new UpdateGestprojectTaxDataWithSageTaxData(
//                  gestprojectConnectionManager.GestprojectSqlConnection,
//                  "IMPUESTO_CONFIG",
//                  GestprojectTaxesColumnsAndValuesStringBuilder.ToString().TrimEnd(','),
//                  correspondingUnsynchronizedSynchronizationEntity
//               );

//               ////////////////////////////////////////
//               /// Complete Entity Synchonization Data appending process
//               ////////////////////////////////////////

//               StringBuilder TaxSynchronizationDataAppendingString = new StringBuilder();

//               int? currentSessionUserId = gestprojectConnectionManager.GestprojectUserRememberableData.GP_CNX_ID;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.ParentUserId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[currentSessionUserId.GetType()](currentSessionUserId)},");

//               string taxCode = Sage50Entity.CODIGO;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50Code.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxCode.GetType()](taxCode)},");

//               string taxGuidId = Sage50Entity.GUID_ID;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50GuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxGuidId.GetType()](taxGuidId)},");

//               string companyGroupName = Sage50CompanyGroupData.CompanyName;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupName.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupName)},");

//               string companyGroupCode = Sage50CompanyGroupData.CompanyCode;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupCode)},");

//               string companyGroupMainCode = Sage50CompanyGroupData.CompanyMainCode;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupMainCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupMainCode)},");

//               string companyGroupGuidId = Sage50CompanyGroupData.CompanyGuidId;
//               TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupGuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupGuidId)},");

//               ////////////////////////////////////////
//               /// This last step is required to relate the current worked entity to it's synchronization table registry
//               ////////////////////////////////////////

//               correspondingUnsynchronizedSynchronizationEntity.S50_GUID_ID = Sage50Entity.GUID_ID;

//               new AppendSynchronizationDataToEntityRegistry(
//                  gestprojectConnectionManager.GestprojectSqlConnection,
//                  tableSchema,
//                  correspondingUnsynchronizedSynchronizationEntity,
//                  TaxSynchronizationDataAppendingString.ToString().TrimEnd(',')
//               );
//            };
//         };
//      }

//      public TaxSynchronizationWorkflow
//      (
//         IGestprojectConnectionManager gestprojectConnectionManager,
//         CompanyGroup sage50CompanyGroupData,
//         ISynchronizationTableSchemaProvider tableSchema,
//         List<GestprojectTaxModel> synchronizationTableEntities,
//         List<GestprojectTaxModel> unsynchronizedEntityList,
//         List<Sage50TaxModel> sage50Entities
//      )
//      {
//         try
//         {
//            this._GestprojectConnectionManager = gestprojectConnectionManager;
//            this._Sage50CompanyGroupData = sage50CompanyGroupData;
//            this._TableSchema = tableSchema;
//            this._SynchronizationTableEntities = synchronizationTableEntities;
//            this._DesynchronizedEntities = unsynchronizedEntityList;
//            this._Sage50Entities = sage50Entities;

//            this.DetermineEntityGenderAndPluralFormat();
//            this.DisplaySynchronizationAutorizationDialog();
//            this.PopulateEntitiesToBeCreatedAndUpdatedLists();
//            this.CreateEntities();
//            this.UpdateEntities();


//            string entityTypeName = "impuesto(s)";

//            //MessageBox.Show("unsynchronizedEntityList.Count: " + unsynchronizedEntityList.Count);
//            //MessageBox.Show("Sage50Entities.Count: " + Sage50Entities.Count);

//            if(unsynchronizedEntityList.Count > 0)
//            {
//               DialogResult result = MessageBox.Show($"Partiendo de la selección encontramos {unsynchronizedEntityList.Count} {entityTypeName} desactualizados.\n\n¿Desea sincronizarlo(s)?", "Confirmación de actualización", MessageBoxButtons.OKCancel);

//               if(result == DialogResult.OK)
//               {
//                  ////////////////////////////////////////
//                  // For this entity, we'll always override the Gestproject data
//                  // This will keep the Gestproject tax data updated in relation to Sage Tax data
//                  ////////////////////////////////////////

//                  List<int?> unsynchronizedEntityListIds = unsynchronizedEntityList.Select(entity => entity.IMP_ID).ToList();

//                  //new DataVisualizer<int?>("gestprojectTaxTableTaxesIds", unsynchronizedEntityListIds);

//                  foreach(Sage50TaxModel Sage50Entity in this._Sage50Entities)
//                  {
//                     ////////////////////////////////////////
//                     // Validate if Sage50 entity has been synchronized
//                     ////////////////////////////////////////

//                     List<GestprojectTaxModel> gestprojectTaxTableTaxes = new GetGestprojectTaxTableTaxes(
//                           gestprojectConnectionManager.GestprojectSqlConnection,
//                           "IMPUESTO_CONFIG"
//                        ).Entities;

//                     ////////////////////////////////////
//                     /// Here could be the error. I'm looking for an Id that hasn't been added yet.
//                     /// try changing this by the description
//                     /// BE CAREFULL THIS MAY DAMAGE THE SYNCHRONIZATION PROCESS
//                     ////////////////////////////////////

//                     List<int?> gestprojectTaxTableTaxesIds = gestprojectTaxTableTaxes.Select(x => x.IMP_ID).ToList();
//                     List<string> gestprojectTaxTableTaxesDescriptions = gestprojectTaxTableTaxes.Select(x => x.IMP_DESCRIPCION).ToList();

//                     //MessageBox.Show("gestprojectTaxTableTaxes: " + gestprojectTaxTableTaxes.Count);

//                     GestprojectTaxModel correspondingUnsynchronizedSynchronizationEntity = null;
//                     if(Sage50Entity.NOMBRE.Contains("IVA"))
//                     {
//                        correspondingUnsynchronizedSynchronizationEntity =
//                           unsynchronizedEntityList
//                           .FirstOrDefault(
//                              entity => entity.IMP_SUBCTA_CONTABLE == Sage50Entity.CTA_IV_REP
//                              &&
//                              gestprojectTaxTableTaxesDescriptions.Contains(Sage50Entity.NOMBRE)
//                              //gestprojectTaxTableTaxesIds.Contains(entity.IMP_ID)
//                              &&
//                              entity.S50_COMPANY_GROUP_GUID_ID != ""
//                           );

//                        //MessageBox.Show("Analizing IVA");
//                     }
//                     else
//                     {
//                        //MessageBox.Show("Sincronizing");

//                        correspondingUnsynchronizedSynchronizationEntity =
//                           unsynchronizedEntityList
//                           .FirstOrDefault(
//                              entity => entity.IMP_SUBCTA_CONTABLE == Sage50Entity.CTA_RE_REP
//                              &&
//                              gestprojectTaxTableTaxesIds.Contains(entity.IMP_ID)
//                              &&
//                              entity.S50_COMPANY_GROUP_GUID_ID != ""
//                           );

//                        //MessageBox.Show("Analizing IRPF");
//                     };

//                     //new VisualizePropertiesAndValues<GestprojectTaxModel>(correspondingUnsynchronizedSynchronizationEntity.IMP_DESCRIPCION, correspondingUnsynchronizedSynchronizationEntity);

//                     ////////////////////////////////////////
//                     /// If never synchronized                     
//                     ////////////////////////////////////////

//                     if(correspondingUnsynchronizedSynchronizationEntity == null)
//                     //if(correspondingUnsynchronizedSynchronizationEntity != null)
//                     {
//                        //MessageBox.Show("Sincronizing");

//                        GestprojectTaxModel entity = new GestprojectTaxModel();
//                        entity.IMP_TIPO = Sage50Entity.IMP_TIPO;
//                        if(Sage50Entity.IMP_TIPO == "IVA")
//                        {
//                           entity.IMP_NOMBRE = $"{Sage50Entity.IMP_TIPO} {Sage50Entity.IVA.ToString().Split(',')[0]}";
//                           entity.IMP_VALOR = Convert.ToDecimal(Sage50Entity.IVA);
//                           entity.IMP_SUBCTA_CONTABLE = Sage50Entity.CTA_IV_REP;
//                           entity.IMP_SUBCTA_CONTABLE_2 = Sage50Entity.CTA_IV_SOP;
//                        }
//                        else
//                        {
//                           entity.IMP_NOMBRE = $"{Sage50Entity.IMP_TIPO} {Sage50Entity.RETENCION.ToString().Split(',')[0]}";
//                           entity.IMP_VALOR = Sage50Entity.RETENCION;
//                           entity.IMP_SUBCTA_CONTABLE = Sage50Entity.CTA_RE_REP;
//                           entity.IMP_SUBCTA_CONTABLE_2 = Sage50Entity.CTA_RE_SOP;
//                        };
//                        entity.S50_CODE = Sage50Entity.CODIGO;
//                        entity.S50_GUID_ID = Sage50Entity.GUID_ID;
//                        entity.IMP_DESCRIPCION = Sage50Entity.NOMBRE;

//                        StringBuilder GestprojectTaxesColumnsStringBuilder = new StringBuilder();
//                        StringBuilder GestprojectTaxesValuesStringBuilder = new StringBuilder();
//                        for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//                        {
//                           string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                           Type currentColumnType = tableSchema.GestprojectFieldsTupleList[j].columnType;
//                           dynamic value =
//                              entity
//                              .GetType()
//                              .GetProperty(currentColumnName)
//                              .GetValue(entity, null);

//                           if(currentColumnName != "IMP_ID")
//                           {
//                              GestprojectTaxesColumnsStringBuilder.Append($"{currentColumnName},");
//                              GestprojectTaxesValuesStringBuilder.Append($"{DynamicValuesFormatters.Formatters[currentColumnType](value)},");
//                           };
//                        };



//                        new InsertSageEntityIntoGestprojectTaxTable(
//                           gestprojectConnectionManager.GestprojectSqlConnection,
//                           "IMPUESTO_CONFIG",
//                           GestprojectTaxesColumnsStringBuilder.ToString().TrimEnd(','),
//                           GestprojectTaxesValuesStringBuilder.ToString().TrimEnd(','),
//                           entity
//                        );

//                        new AppendGestprojectTaxTableIdToSynchronizationTable(
//                           gestprojectConnectionManager.GestprojectSqlConnection,
//                           "IMPUESTO_CONFIG",
//                           entity,
//                           tableSchema
//                        );

//                        correspondingUnsynchronizedSynchronizationEntity = entity;
//                     }
//                     ////////////////////////////////////////
//                     /// If previously Synchronized
//                     ////////////////////////////////////////
//                     else
//                     {
//                        //MessageBox.Show("Updating");

//                        StringBuilder GestprojectTaxesColumnsAndValuesStringBuilder = new StringBuilder();
//                        for(global::System.Int32 j = 0; j < tableSchema.GestprojectFieldsTupleList.Count; j++)
//                        {
//                           string currentColumnName = tableSchema.GestprojectFieldsTupleList[j].columnName;
//                           dynamic value =
//                              correspondingUnsynchronizedSynchronizationEntity
//                              .GetType()
//                              .GetProperty(currentColumnName)
//                              .GetValue(correspondingUnsynchronizedSynchronizationEntity, null);

//                           if(currentColumnName != "IMP_ID")
//                           {
//                              GestprojectTaxesColumnsAndValuesStringBuilder.Append($"{currentColumnName}={DynamicValuesFormatters.Formatters[value.GetType()](value)},");
//                           };
//                        };

//                        new UpdateGestprojectTaxDataWithSageTaxData(
//                           gestprojectConnectionManager.GestprojectSqlConnection,
//                           "IMPUESTO_CONFIG",
//                           GestprojectTaxesColumnsAndValuesStringBuilder.ToString().TrimEnd(','),
//                           correspondingUnsynchronizedSynchronizationEntity
//                        );
//                     }

//                     ////////////////////////////////////////
//                     /// Complete Entity Synchonization Data appending process
//                     ////////////////////////////////////////

//                     StringBuilder TaxSynchronizationDataAppendingString = new StringBuilder();

//                     int? currentSessionUserId = gestprojectConnectionManager.GestprojectUserRememberableData.GP_CNX_ID;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.ParentUserId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[currentSessionUserId.GetType()](currentSessionUserId)},");

//                     string taxCode = Sage50Entity.CODIGO;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50Code.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxCode.GetType()](taxCode)},");

//                     string taxGuidId = Sage50Entity.GUID_ID;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.Sage50GuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[taxGuidId.GetType()](taxGuidId)},");

//                     string companyGroupName = Sage50CompanyGroupData.CompanyName;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupName.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupName)},");

//                     string companyGroupCode = Sage50CompanyGroupData.CompanyCode;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupCode)},");

//                     string companyGroupMainCode = Sage50CompanyGroupData.CompanyMainCode;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupMainCode.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupMainCode)},");

//                     string companyGroupGuidId = Sage50CompanyGroupData.CompanyGuidId;
//                     TaxSynchronizationDataAppendingString.Append($"{tableSchema.CompanyGroupGuidId.ColumnDatabaseName}={DynamicValuesFormatters.Formatters[companyGroupName.GetType()](companyGroupGuidId)},");

//                     ////////////////////////////////////////
//                     /// This last step is required to relate the current worked entity to it's synchronization table registry
//                     ////////////////////////////////////////

//                     correspondingUnsynchronizedSynchronizationEntity.S50_GUID_ID = Sage50Entity.GUID_ID;

//                     new AppendSynchronizationDataToEntityRegistry(
//                        gestprojectConnectionManager.GestprojectSqlConnection,
//                        tableSchema,
//                        correspondingUnsynchronizedSynchronizationEntity,
//                        TaxSynchronizationDataAppendingString.ToString().TrimEnd(',')
//                     );
//                  };
//               };
//            }
//            else
//            {
//               MessageBox.Show($"Los {SynchronizationTableEntities.Count - (SynchronizationTableEntities.Count - Sage50Entities.Count)} {entityTypeName} están sincronizados.");
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
