using System;
using System.Activities.Presentation.Metadata;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WFHelper.XamlClr
{
    public class XamlClrRef
    {
        #region 常数和字段

        private const string FxKey = "b77a5c561934e089";

        private const string FxKey2 = "31bf3856ad364e35";

        private AssemblyName _assemblyName;

        private Version _version;

        #endregion 常数和字段

        #region 构造函数和析构函数

        public XamlClrRef(string clrNamespace)
        {
            this.ClrNamespace = clrNamespace;
            this.GetAsssemblyName();
        }

        #endregion 构造函数和析构函数

        #region 属性

        private Assembly _assembly;

        private Assembly Assembly
        {
            get
            {
                return this._assembly;
            }
            set
            {
                this._assembly = value;
                LoadDesignerAssembly();
            }
        }

        private void LoadDesignerAssembly()
        {
            if (this._assembly != null && !this.IsFrameworkAssembly())
            {
                var file = new Uri(_assembly.CodeBase).AbsolutePath;
                var designerName = file.Insert(file.LastIndexOf('.'), ".design");

                try
                {
                    this.DesignerAssembly = Assembly.LoadFile(designerName);

                    // TODO: This is a hack to see if copying the assemblies to the bin folder fixes the problem of the designer failing to load an assembly.
                    this.CopyAssemblies(Assembly);
                    this.CopyAssemblies(DesignerAssembly);
                    RegisterDesigner();
                }
                catch (FileLoadException)
                {
                }
                catch (FileNotFoundException)
                {
                }
            }
        }

        private void RegisterDesigner()
        {
            foreach (var metadataType in DesignerAssembly.GetTypes().Where(t => typeof(IRegisterMetadata).IsAssignableFrom(t)))
            {
                var metadata = Activator.CreateInstance(metadataType) as IRegisterMetadata;
                if (metadata != null)
                {
                    metadata.Register();
                }
            }
        }

        private void CopyAssemblies(Assembly copyAssembly)
        {
            var sourcePath = new Uri(copyAssembly.CodeBase).AbsolutePath;
            // ReSharper disable AssignNullToNotNullAttribute
            var destPath = Path.Combine(Path.GetDirectoryName(new Uri(this.GetType().Assembly.CodeBase).AbsolutePath), Path.GetFileName(sourcePath));
            // ReSharper restore AssignNullToNotNullAttribute
            try
            {
                File.Copy(sourcePath, destPath, true);
            }
            catch (Exception)
            {
                // TODO: Do nothing for now
                // throw;
            }
        }

        public string ClrNamespace { get; set; }

        public string CodeBase
        {
            get
            {
                return this._assemblyName.CodeBase ?? this._assemblyName.Name + ".dll";
            }
        }

        public bool Loaded
        {
            get
            {
                return this.Assembly != null;
            }
        }

        public string Name
        {
            get
            {
                return this._assemblyName.Name;
            }
        }

        protected string Culture
        {
            get
            {
                return this._assemblyName.CultureInfo == null ? "neutral" : this._assemblyName.CultureInfo.Name;
            }
        }

        private string Version
        {
            get
            {
                if (this._version == null)
                {
                    this._version = this.GetDefaultVersion();
                }
                return this._version.ToString();
            }
        }

        #endregion 属性

        #region 公共方法

        public bool Load()
        {
            return this.IsFrameworkAssembly() ? this.TryLoad(FxKey) || this.TryLoad(FxKey2) : this.TryLoad();
        }

        #endregion 公共方法

        #region 方法

        private void GetAsssemblyName()
        {
            var parts = this.ClrNamespace.Split(';');

            if (parts.Length == 2)
            {
                var namevalue = parts[1].Split('=');

                if (namevalue.Length == 2)
                {
                    var name = namevalue[1].Trim();
                    this._assemblyName = new AssemblyName(name);
                }
            }
        }

        private Version GetDefaultVersion()
        {
            if (this.IsFrameworkAssembly())
            {
                return new Version(Assembly.GetExecutingAssembly().ImageRuntimeVersion.Substring(1, 3) + ".0.0");
            }

            return null;
        }

        private string GetNameWithKey(string key)
        {
            return string.Format("{0},Version={1},Culture={2},PublicKeyToken={3}", this.Name, this.Version, this.Culture, key);
        }

        private bool IsFrameworkAssembly()
        {
            return this.Name.ToLowerInvariant() == "mscorlib" || this.Name.StartsWith("System") || this.Name.StartsWith("Microsoft.CSharp") || this.Name.StartsWith("Microsoft.VisualBasic");
        }

        private bool TryLoad(string fxKey)
        {
            try
            {
                this.Assembly = Assembly.Load(this.GetNameWithKey(fxKey));
                return true;
            }
            catch (FileLoadException)
            {
            }
            catch (FileNotFoundException)
            {
            }
            return false;
        }

        private bool TryLoad()
        {
            try
            {
                this.Assembly = Assembly.Load(this._assemblyName);
                return true;
            }
            catch (FileLoadException)
            {
            }
            catch (FileNotFoundException)
            {
            }

            return false;
        }

        #endregion 方法

        public Assembly DesignerAssembly { get; set; }

        public bool Load(string fileName)
        {
            try
            {
                this.Assembly = Assembly.LoadFile(fileName);
                return true;
            }
            catch (FileLoadException)
            {
            }
            catch (FileNotFoundException)
            {
            }

            return false;
        }
    }
}