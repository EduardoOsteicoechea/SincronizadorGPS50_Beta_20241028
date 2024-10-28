using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using sage._50;
using sage.ew.db;
using sage.ew.global;
using sage.ew.usuario;
using sage.ew.interficies;
using sage.ew.empresa;
using Sage.ES.S50.Addons;
using sage.ew.ewbase;

namespace SincronizadorGPS50.Sage50Connector
{
   public class LinkSage50
   {
      //public clsDatos _oDatos = new clsDatos();
      public bool _Loaded_Ok = false;
      public bool _connected = false;
      public bool _EsMultiEmpresa = false;
      public string _Empresa = "";
      public string _Ejercicio = "";
      public string _Comunes = "";
      public string _ComunesPrincipal = "";
      public string _Error_Message = "";
      public string _Terminal = "";
      private string _DllTerminal = "";

      public LinkSage50() { }

      public LinkSage50(String tsTerminal)
      {
         if(!string.IsNullOrEmpty(tsTerminal))
         {
            this._Terminal = tsTerminal;
            this._DllTerminal = Path.Combine(this._Terminal, Sage50LibrariesFolder.Get(this._Terminal));
            this._AssemblyResolveBegin();
            this._Loaded_Ok = true;
         }
      }

      public Boolean _Connect(string tsUser, string tsPass, string tsEmpresa = "")
      {
         _connected = main_s50.Connect(this._Terminal, tsUser, tsPass, tsEmpresa);
         if(_connected == true)
         {
            _EsMultiEmpresa = this._HaveMultiCompany();
         }
         return _connected;
      }
      public void _Disconnect()
      {
         if(_connected == true)
         {
            DB.Conexion = "";
            _connected = false;
         }
         this._AssemblyResolveEnd();
      }
      public Boolean _LoadGlobalVariables(string tsEmpresa = "")
      {
         Boolean llOk = false;
         string lsEmpresa = this._Empresa;

         if(!string.IsNullOrEmpty(tsEmpresa.Trim()))
            lsEmpresa = tsEmpresa;

         llOk = main_s50._Cargar_Globales(lsEmpresa);

         if(llOk)
         {
            _Empresa = EW_GLOBAL._GetVariable("wc_empresa").ToString();
            _Ejercicio = EW_GLOBAL._GetVariable("wc_any").ToString();
            _Comunes = DB.DbComunes.ToString();
            _LoadEnvironmentCompany();
         }
         return llOk;
      }
      public void _LoadEnvironmentCompany(string tsEmpresa = "")
      {
         string lsEmpresa = this._Empresa;

         if(!string.IsNullOrEmpty(tsEmpresa.Trim()))
            lsEmpresa = tsEmpresa;

         Usuario._This._Cambiar_Empresa(lsEmpresa);
         _Empresa = EW_GLOBAL._GetVariable("wc_empresa").ToString();
         _Ejercicio = EW_GLOBAL._GetVariable("wc_any").ToString();
         _Comunes = DB.DbComunes.ToString();
      }
      public void _ExerciseChange(string tscEjercicio)
      {
         sage.ew.functions.Clases.CambioEjercicio loEjer = new sage.ew.functions.Clases.CambioEjercicio();
         loEjer._Cambiar(tscEjercicio);
         _LoadEnvironmentCompany();
      }




      /// This one
      public Boolean _GroupCompanyChange(string tscGrupoEmpresa)
      {
         bool llOk = false;

         string lcActGrupoEmp = string.Empty, lcNewGrupoEmp = string.Empty;
         string lcActEmpresa = string.Empty, lcNewEmpresa = string.Empty;
         string lcActEjercicio = string.Empty, lcNewEjercicio = string.Empty;
         bool tlRefrescar = false;

         lcActGrupoEmp = DB.DbComunes.Trim().Substring(4, 4);
         lcActEmpresa = Convert.ToString(EW_GLOBAL._GetVariable("wc_empresa"));
         lcActEjercicio = Convert.ToString(EW_GLOBAL._GetVariable("wc_any"));

         lcNewGrupoEmp = tscGrupoEmpresa;
         lcNewEmpresa = lcActEmpresa;
         lcNewEjercicio = lcActEjercicio;

         GrupoEmpresaSel _oGruposEmp = new GrupoEmpresaSel();

         if(_oGruposEmp.ExisteUsuario(lcNewGrupoEmp))
         {
            if(!string.IsNullOrWhiteSpace(lcNewEmpresa) && _oGruposEmp.VerificarEmpresaAcceso(lcNewGrupoEmp, lcNewEmpresa, ref lcNewEmpresa))
            {
               if(_oGruposEmp.AccesoUsuarioEmpresa(lcNewGrupoEmp, lcNewEmpresa))
               {
                  Usuario._This._GrupoCambiadoEnFrmLogin = "";

                  if(!_oGruposEmp._ComprobarPassword(lcNewGrupoEmp) || _oGruposEmp.EmpresaOcultaEnGrupo(lcNewGrupoEmp, lcNewEmpresa))
                  {
                     llOk = _oGruposEmp._CambiarGrupo(lcNewGrupoEmp, lcNewEmpresa, false);

                     if(!Usuario._This._ShowLoginCambioGrupoEmpresas(ref lcNewEmpresa, ref tlRefrescar))
                     {
                        _oGruposEmp._CambiarGrupo(lcActGrupoEmp, lcActEmpresa, false);
                        llOk = false;
                     }
                     else
                     { llOk = true; }
                  }

                  if(!string.IsNullOrWhiteSpace(Usuario._This._GrupoCambiadoEnFrmLogin))
                     lcNewGrupoEmp = Usuario._This._GrupoCambiadoEnFrmLogin;

                  AddonsController.Instance.Methods._CambioEmpresa(TipoExecute.Before, lcActEmpresa, lcNewEmpresa);

                  llOk = _oGruposEmp._CambiarGrupo(lcNewGrupoEmp, lcNewEmpresa, tlRefrescar);

                  if(llOk)
                  {
                     AddonsController.Instance.Methods._CambioEmpresa(TipoExecute.After, lcActEmpresa, lcNewEmpresa);

                     if(EW_GLOBAL._Empresa == null)
                        EW_GLOBAL._Empresa = new Empresa(Convert.ToString(EW_GLOBAL._GetVariable("wc_empresa")));

                     return llOk;
                  }
                  else
                     this._Error_Message = "Se ha producido un error intentando cambiar al grupo de empresa " + lcNewGrupoEmp + ".";
               }
               else
                  this._Error_Message = "El usuario actual no tiene acceso a la empresa del grupo seleccionado, seleccione otra empresa que tenga acceso.";
            }
            else
               this._Error_Message = "El usuario actual no tiene acceso a ninguna de las empresas del grupo seleccionado, no se puede cambiar al grupo de empresa seleccionado.";
         }
         else
         {
            this._Error_Message = "El usuario actual no existe en el grupo seleccionado, no se puede cambiar al grupo de empresa seleccionado.";
         }
         return llOk;
      }


      public Boolean CambiarGrupoEmpresa(string tscGrupoEmpresa)
      {
         bool llOk = false;
         string lcEmpresa = "";
         string lcGrupoActual = GrupoEmpresa._CodigoGrupoActual();

         string lcGrupoEjer;
         if(!string.IsNullOrWhiteSpace(tscGrupoEmpresa))
            lcGrupoEjer = tscGrupoEmpresa;
         else
            lcGrupoEjer = lcGrupoActual;

         if(lcGrupoActual == lcGrupoEjer)
            return llOk;

         GrupoEmpresa _oGrupoEmpresa = new GrupoEmpresa(lcGrupoEjer);

         if(_oGrupoEmpresa._dtEmpresasGrupo != null && _oGrupoEmpresa._dtEmpresasGrupo.Rows.Count > 0)
            lcEmpresa = Convert.ToString(_oGrupoEmpresa._dtEmpresasGrupo.Rows[0]["codigo"]);

         GrupoEmpresaSel loGrupo = new GrupoEmpresaSel();
         llOk = loGrupo._CambiarGrupo(lcGrupoEjer, lcEmpresa);

         return llOk;
      }

      public Boolean _HaveMultiCompany()
      {
         Boolean llOk = false;
         DataTable ldtGrupos = new DataTable();
         string lcComunesPripal = "";
         string lcCodPrin = "";

         lcCodPrin = GrupoempTools._Obtener_CodGrupoPripal(DB.DbComunes.Trim().Substring(4, 4));
         if(!String.IsNullOrEmpty(lcCodPrin.Trim()))
         {
            lcComunesPripal = "COMU" + lcCodPrin;
            llOk = true;
         }
         else
         {
            lcComunesPripal = DB.DbComunes.Trim();
            llOk = false;
         }

         if(llOk == true)
         {
            //llOk = _oDatos.Grupos_Multiempresa(lcCodPrin, ref ldtGrupos);
            ldtGrupos.Dispose();
         }

         _ComunesPrincipal = lcComunesPripal;

         return llOk;
      }

      public void _AssemblyResolveBegin()
      {
         if(!string.IsNullOrEmpty(_DllTerminal))
         {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(this.__CurrentDomain_AssemblyResolve);
         }
      }

      public void _AssemblyResolveEnd()
      {
         AppDomain currentDomain = AppDomain.CurrentDomain;
         currentDomain.AssemblyResolve -= new ResolveEventHandler(this.__CurrentDomain_AssemblyResolve);
      }

      private Assembly __CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
      {
         string lcFile = args.Name.Replace(".exe", "");

         string lcDllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : lcFile;

         lcDllName = lcDllName.Replace(".", "_");
         if(lcDllName.EndsWith("_resources"))
         {
            return default(Assembly);
         }

         Assembly loAssembly = this.__GetAssemblyByName(args.Name);
         if(loAssembly != null)
         {
            return loAssembly;
         }

         try
         {
            return this.__LoadAssemblyFromPath(args.Name, this._DllTerminal);
         }
         catch(Exception)
         {
            return default(Assembly);
         }
      }

      private Assembly __GetAssemblyByName(string name)
      {
         return AppDomain.CurrentDomain.GetAssemblies().
                SingleOrDefault(assembly => assembly.GetName().Name == name);
      }

      private Assembly __LoadAssemblyFromPath(string assemblyName, string directoryPath)
      {
         string lsExtFile = "";

         foreach(string file in Directory.GetFiles(directoryPath))
         {
            lsExtFile = Path.GetExtension(file).ToLower();

            if(lsExtFile == ".dll" || lsExtFile == ".exe")
            {
               Assembly assembly;
               if(this.__TryLoadAssemblyFromFile(file, assemblyName, out assembly))
               {
                  return assembly;
               }
            }
         }

         return null;
      }

      private bool __TryLoadAssemblyFromFile(string file, string assemblyName, out Assembly assembly)
      {
         try
         {
            assemblyName += ",";
            string[] lcArgs0 = assemblyName.Split(',');

            file = new FileInfo(file).FullName;

            string assemblyFile = AssemblyName.GetAssemblyName(file).FullName.ToString();
            assemblyFile += ",";
            string[] lcArgs1 = assemblyFile.Split(',');

            if(lcArgs0[0] == lcArgs1[0])
            {
               assembly = Assembly.LoadFile(file);
               return true;
            }
         }
         catch
         {
         }

         assembly = null;
         return false;
      }
   }

   public class LinkFuncSage50
   {
      public string _Error_Message = "";
      public LinkFuncSage50() { }
      public string _CountryCompany()
      {
         string lcPais;

         lcPais = DB.SQLValor("CODIGOS", "EMPRESA", EW_GLOBAL._GetVariable("wc_empresa").ToString(), "PAIS").ToString();
         lcPais = (String.IsNullOrWhiteSpace(lcPais) ? "034" : lcPais);

         return lcPais;
      }
      public string _VerificateCountry(string pcPais)
      {
         string lcPais = "";

         if(String.IsNullOrWhiteSpace(pcPais))
            lcPais = this._CountryCompany();
         else
            lcPais = DB.SQLValor("PAISES", "CODIGO", pcPais, "CODIGO", "COMUNES").ToString();

         return lcPais.Trim();
      }
      public string _VerificatePostalCode(string pcCodpos)
      {
         string lcCodpos = "";

         if(!String.IsNullOrWhiteSpace(pcCodpos))
            lcCodpos = DB.SQLValor("CODPOS", "CODIGO", pcCodpos, "CODIGO").ToString();

         return lcCodpos.Trim();
      }
      public string _VerificateTaxType(string pcTipo_iva)
      {
         string lcTipo_iva = "";

         if(!String.IsNullOrWhiteSpace(pcTipo_iva))
            lcTipo_iva = DB.SQLValor("TIPO_IVA", "CODIGO", pcTipo_iva, "CODIGO").ToString();

         return lcTipo_iva.Trim();
      }
      public string _VerificateRetentionType(string pcTipo_ret)
      {
         string lcTipo_ret = "";

         if(!String.IsNullOrWhiteSpace(pcTipo_ret))
            lcTipo_ret = DB.SQLValor("TIPO_RET", "CODIGO", pcTipo_ret, "CODIGO").ToString();

         return lcTipo_ret.Trim();
      }
      public string _VerificatePaymentMethod(string pcFormapago)
      {
         string lcFormapago = "";

         if(!String.IsNullOrWhiteSpace(pcFormapago))
            lcFormapago = DB.SQLValor("FPAG", "CODIGO", pcFormapago, "CODIGO").ToString();

         return lcFormapago.Trim();
      }
      public Dictionary<string, object> _VerificateNIFCustomer(string pcNif)
      {
         Dictionary<string, object> loResultado = new Dictionary<string, object>();

         if(!String.IsNullOrWhiteSpace(pcNif))
            loResultado = DB.SQLREGValor("CLIENTES", "cif", pcNif);

         return loResultado;
      }
      public Dictionary<string, object> _VerificateNIFProvider(string pcNif)
      {
         Dictionary<string, object> loResultado = new Dictionary<string, object>();

         if(!String.IsNullOrWhiteSpace(pcNif))
            loResultado = DB.SQLREGValor("PROVEED", "cif", pcNif);

         return loResultado;
      }
   }
}