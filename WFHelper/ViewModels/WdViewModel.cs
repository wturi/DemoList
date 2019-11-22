using ActivityLibrary.Mail;
using Design;
using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Expressions;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.Validation;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Input;
using Microsoft.CSharp.Activities;
using WFHelper.Executions;
using WFHelper.Extensions;
using WFHelper.ExtensionService;
using WFHelper.XamlClr;
using Image = System.Drawing.Image;

namespace WFHelper.ViewModels
{
    public class WdViewModel : INotifyPropertyChanged
    {
        #region 常数和字段

        /// <summary>
        /// 可能存在的模板文件
        /// </summary>
        private const string TemplateXaml = "template.xaml";

        private const string UntitledXaml = "RPA.Xaml";

        private WorkflowDesigner _workflowDesigner;

        #endregion 常数和字段

        #region 属性

        #region 文件名称

        private string _fileName;

        // ReSharper disable once MemberCanBePrivate.Global
        public string FileName
        {
            get => this._fileName;

            private set
            {
                this._fileName = value;
                this.Title = $"{_title} - {this.FileName}";
            }
        }

        #endregion 文件名称

        #region 操作状态

        private string _status;

        // ReSharper disable once MemberCanBePrivate.Global
        public string Status
        {
            get => this._status;

            set
            {
                this._status = value;
                this.NotifyChanged("Status");
            }
        }

        #endregion 操作状态

        #region IDE名称

        private string _title = "工作流设计台";

        // ReSharper disable once MemberCanBePrivate.Global
        public string Title
        {
            get => this._title;

            private set
            {
                this._title = value;
                this.NotifyChanged("Title");
            }
        }

        #endregion IDE名称

        #region 工具栏面板

        // ReSharper disable once MemberCanBePrivate.Global
        public object ToolboxPanel { get; private set; }

        #endregion 工具栏面板

        #region wf实例

        // ReSharper disable once MemberCanBePrivate.Global
        public WorkflowDesigner WorkflowDesigner
        {
            get => this._workflowDesigner;

            private set
            {
                this._workflowDesigner = value;
                this.NotifyChanged("WorkflowDesignerPanel");
                this.NotifyChanged("WorkflowPropertyPanel");
            }
        }

        #endregion wf实例

        #region 错误信息

        // ReSharper disable once MemberCanBePrivate.Global
        public ObservableCollection<string> WorkflowErrors { get; } = new ObservableCollection<string>();

        #endregion 错误信息

        public object WorkflowDesignerPanel => this.WorkflowDesigner.View;

        public object WorkflowPropertyPanel => this.WorkflowDesigner.PropertyInspectorView;

        /// <summary>
        /// 工作流 XAML 代码
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public string XAML
        {
            get
            {
                //画布 to xaml
                if (this.WorkflowDesigner.Text == null) return null;
                this.WorkflowDesigner.Flush();
                return this.WorkflowDesigner.Text;
            }
            set
            {
                //xaml to 画布
                this.WorkflowDesigner = new WorkflowDesigner()
                {
                    Text = value
                };
                this.WorkflowDesigner.Load();
            }
        }

        #endregion 属性

        #region 构造函数和析构函数

        public WdViewModel()
        {
            (new DesignerMetadata()).Register();

            //初始化
            Metadata.RegisterAll();

            //工具栏
            LoadToolboxIconsForBuiltInActivities();
            this.ToolboxPanel = CreateToolbox();

            //菜单
            this.ExitCommand = new RelayCommand(this.ExecuteExit, CanExecuteExit);
            this.OpenCommand = new RelayCommand(this.ExecuteOpen, CanExecuteOpen);
            this.NewCommand = new RelayCommand(this.ExecuteNew, CanExecuteNew);
            this.SaveCommand = new RelayCommand(this.ExecuteSave, CanExecuteSave);
            this.SaveAsCommand = new RelayCommand(this.ExecuteSaveAs, CanExecuteSaveAs);
            this.RunCommand = new RelayCommand(this.ExecuteRun, CanExecuteRun);

            //暂定 开启后新建一个工程
            this.ExecuteNew(null);
        }

        #endregion 构造函数和析构函数

        #region 公共事件

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion 公共事件

        #region 公共方法

        public void ViewClosed(object sender, EventArgs e)
        {
        }

        public void ViewClosing(object sender, CancelEventArgs e)
        {
        }

        #endregion 公共方法

        #region 方法

        #region 基础

        public void NotifyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void WorkflowDesignerModelChanged(object sender, EventArgs e)
        {
            this.NotifyChanged("XAML");
        }

        #endregion 基础

        #region 工具栏

        private static void CreateToolboxBitmapAttributeForActivity(
       AttributeTableBuilder builder, IResourceReader resourceReader, Type builtInActivityType)
        {
            var bitmap = ExtractBitmapResource(
                resourceReader,
                builtInActivityType.IsGenericType ? builtInActivityType.Name.Split('`')[0] : builtInActivityType.Name);

            if (bitmap == null)
            {
                return;
            }

            var tbaType = typeof(ToolboxBitmapAttribute);

            var imageType = typeof(Image);

            var constructor = tbaType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { imageType, imageType }, null);

            if (constructor == null) return;
            var tba = constructor.Invoke(new object[] { bitmap, bitmap }) as ToolboxBitmapAttribute;

            builder.AddCustomAttributes(builtInActivityType, tba);
        }

        private static Bitmap ExtractBitmapResource(IResourceReader resourceReader, string bitmapName)
        {
            var dictEnum = resourceReader.GetEnumerator();

            Bitmap bitmap = null;

            while (dictEnum.MoveNext())
            {
                if (!Equals(dictEnum.Key, bitmapName)) continue;
                bitmap = dictEnum.Value as Bitmap;

                if (bitmap != null)
                {
                    var pixel = bitmap.GetPixel(bitmap.Width - 1, 0);

                    bitmap.MakeTransparent(pixel);
                }

                break;
            }

            return bitmap;
        }

        private static void LoadToolboxIconsForBuiltInActivities()
        {
            try
            {
                var sourceAssembly = Assembly.LoadFrom(@"Lib\Microsoft.VisualStudio.Activities.dll");

                var builder = new AttributeTableBuilder();

                {
                    var stream =
                        sourceAssembly.GetManifestResourceStream(
                            "Microsoft.VisualStudio.Activities.Resources.resources");
                    if (stream != null)
                    {
                        var resourceReader = new ResourceReader(stream);

                        foreach (var type in
                            typeof(Activity).Assembly.GetTypes().Where(
                                t => t.Namespace == "System.Activities.Statements"))
                        {
                            CreateToolboxBitmapAttributeForActivity(builder, resourceReader, type);
                        }
                    }
                }

                MetadataStore.AddAttributeTable(builder.CreateTable());
            }
            catch (FileNotFoundException exception)
            {
                // Ignore - will use default icons
            }
        }

        private static ToolboxControl CreateToolbox()
        {
            var toolboxControl = new ToolboxControl();

            toolboxControl.Categories.Add(
                new ToolboxCategory("Control Flow")
                {
                    new ToolboxItemWrapper(typeof(DoWhile)),
                    new ToolboxItemWrapper(typeof(ForEach<>)),
                    new ToolboxItemWrapper(typeof(If)),
                    new ToolboxItemWrapper(typeof(Parallel)),
                    new ToolboxItemWrapper(typeof(ParallelForEach<>)),
                    new ToolboxItemWrapper(typeof(Pick)),
                    new ToolboxItemWrapper(typeof(PickBranch)),
                    new ToolboxItemWrapper(typeof(Sequence)),
                    new ToolboxItemWrapper(typeof(Switch<>)),
                    new ToolboxItemWrapper(typeof(While)),
                });

            toolboxControl.Categories.Add(
                new ToolboxCategory("Primitives")
                {
                    new ToolboxItemWrapper(typeof(Assign)),
                    new ToolboxItemWrapper(typeof(Delay)),
                    new ToolboxItemWrapper(typeof(InvokeMethod)),
                    new ToolboxItemWrapper(typeof(WriteLine)),
                });

            toolboxControl.Categories.Add(
                new ToolboxCategory("Error Handling")
                {
                    new ToolboxItemWrapper(typeof(Rethrow)),
                    new ToolboxItemWrapper(typeof(Throw)),
                    new ToolboxItemWrapper(typeof(TryCatch)),
                });

            toolboxControl.Categories.Add(
                new ToolboxCategory("MyActivityLibrary")
                {
                    new ToolboxItemWrapper(typeof(SendMailActivity))
                });

            return toolboxControl;
        }

        #endregion 工具栏

        #region 菜单按钮

        #region 菜单事件命令

        public ICommand OpenCommand { get; set; }

        public ICommand SaveAsCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public ICommand NewCommand { get; set; }

        public ICommand RunCommand { get; set; }

        #endregion 菜单事件命令

        #region 菜单事件权限

        private static bool CanExecuteExit(object obj)
        {
            return true;
        }

        private static bool CanExecuteNew(object obj)
        {
            return true;
        }

        private static bool CanExecuteOpen(object obj)
        {
            return true;
        }

        private static bool CanExecuteSave(object obj)
        {
            return true;
        }

        private static bool CanExecuteSaveAs(object obj)
        {
            return true;
        }

        private static bool CanExecuteRun(object obj) => true;

        #endregion 菜单事件权限

        #region 菜单事件实现

        private void ExecuteExit(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteNew(object obj)
        {
            StatusClean();
            this.WorkflowDesigner = new WorkflowDesigner();
            this.WorkflowDesigner.ModelChanged += this.WorkflowDesignerModelChanged;

            DesignerConfigurationService configurationService = this.WorkflowDesigner.Context.Services.GetService<DesignerConfigurationService>();

            var fff = configurationService.TargetFrameworkName;

            configurationService.TargetFrameworkName = new FrameworkName(".NETFramework", new System.Version(4, 6));
            configurationService.LoadingFromUntrustedSourceEnabled = true;
            configurationService.AnnotationEnabled = true;


            if (File.Exists(TemplateXaml))
            {
                this.WorkflowDesigner.Load(TemplateXaml);
            }
            else
            {
                Variable<int> n = new Variable<int>
                {
                    Name = "n"
                };
                Activity wf = new Sequence
                {
                    Variables = { n },
                    Activities =
                    {
                        new Assign<int>
                        {
                            To = new CSharpReference<int>("n"),
                            Value = new CSharpValue<int>("new Random().Next(1, 101)")
                        },
                        new WriteLine
                        {
                            Text = new CSharpValue<string>("\"The number is \" + n")
                        }
                    }
                };
                CompileExpressions(wf);
             this.WorkflowDesigner.Load(wf);
            }

            this.WorkflowDesigner.Flush();

            AddExtensionCallBack();
            AddExtensionService();

            this.FileName = UntitledXaml;
            StatusAppend($"Created new workflow from template {TemplateXaml}");
        }

        private void ExecuteOpen(object obj)
        {
            StatusClean();
            var openFileDialog = new OpenFileDialog();
            if (!openFileDialog.ShowDialog(Application.Current.MainWindow).Value) return;
            this.LoadWorkflow(openFileDialog.FileName);
            StatusAppend($"Open workflow file {this.FileName}");
        }

        private void ExecuteSave(object obj)
        {
            if (this.FileName == UntitledXaml)
            {
                this.ExecuteSaveAs(obj);
            }
            else
            {
                this.Save();
            }
        }

        private void ExecuteSaveAs(object obj)
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "xaml",
                FileName = this.FileName,
                // ReSharper disable once StringLiteralTypo
                Filter = "xaml files (*.xaml) | *.xaml;*.xamlx| All Files | *.*"
            };

            // ReSharper disable once PossibleInvalidOperationException
            if (!saveFileDialog.ShowDialog().Value) return;
            this.FileName = saveFileDialog.FileName;
            this.Save();
        }

        private void ExecuteRun(object obj)
        {
            var workflowRun = new WorkflowRunner(this.WorkflowDesigner);
            workflowRun.Run();
        }

        #endregion 菜单事件实现

        #endregion 菜单按钮

        #region XAML文件操作

        /// <summary>
        /// 读取工作流
        /// </summary>
        /// <param name="name"></param>
        private void LoadWorkflow(string name)
        {
            this.ResolveImportedAssemblies(name);
            this.FileName = name;
            this.WorkflowDesigner = new WorkflowDesigner();
            this.WorkflowDesigner.ModelChanged += this.WorkflowDesignerModelChanged;

            DesignerConfigurationService configurationService = this.WorkflowDesigner.Context.Services.GetService<DesignerConfigurationService>();
            configurationService.TargetFrameworkName = new FrameworkName(".NETFramework", new System.Version(4, 5));
            configurationService.LoadingFromUntrustedSourceEnabled = true;
            configurationService.AnnotationEnabled = true;

            AddExtensionCallBack();
            AddExtensionService();

            this.WorkflowDesigner.Load(name);
        }

        private void Locate(XamlClrRef xamlClrRef)
        {
            StatusAppend($"Locate referenced assembly {xamlClrRef.CodeBase}");
            var openFileDialog = new OpenFileDialog
            {
                FileName = xamlClrRef.CodeBase,
                CheckFileExists = true,
                Filter = "Assemblies (*.dll;*.exe)|*.dll;*.exe|All Files|*.*",
                Title = this.Status
            };

            if (!openFileDialog.ShowDialog(Application.Current.MainWindow).Value) return;
            if (!xamlClrRef.Load(openFileDialog.FileName))
            {
                MessageBox.Show("Error loading assembly");
            }
        }

        private void Save()
        {
            this._workflowDesigner.Save(this.FileName);
            StatusAppend($"Saved workflow file {this.FileName}");
        }

        #endregion XAML文件操作

        #region 组件

        private void ResolveImportedAssemblies(string name)
        {
            var references = XamlClrReferences.Load(name);

            var query = from reference in references.References where !reference.Loaded select reference;
            foreach (var xamlClrRef in query)
            {
                this.Locate(xamlClrRef);
            }
        }

        #endregion 组件

        #region 扩展

        /// <summary>
        /// 服务
        /// </summary>
        private void AddExtensionService()
        {
            //表达式
            //var expressionEditorService = new ExpressionEditorService();
            //this.WorkflowDesigner.Context.Services.Publish<IExpressionEditorService>(expressionEditorService);

            //抓取错误信息
            var validationErrorService = new ValidationErrorService(WorkflowErrors);
            this.WorkflowDesigner.Context.Services.Publish<IValidationErrorService>(validationErrorService);
        }

        /// <summary>
        /// 事件回调
        /// </summary>
        private void AddExtensionCallBack()
        {
            this.WorkflowDesigner.Context.Items.Subscribe<Selection>(CallBackExtension.SelectionChanged);
        }

        #endregion 扩展

        #region 输出操作

        private void StatusAppend(string str)
        {
            this.Status = $"{DateTime.Now.ToString(CultureInfo.CurrentCulture)}--{str}\r\n{this.Status}";
        }

        private void StatusClean()
        {
            this.Status = "";
        }

        #endregion

        static void CompileExpressions(DynamicActivity dynamicActivity)
        {
            // activityName is the Namespace.Type of the activity that contains the
            // C# expressions. For Dynamic Activities this can be retrieved using the
            // name property , which must be in the form Namespace.Type.
            string activityName = dynamicActivity.Name;

            // Split activityName into Namespace and Type.Append _CompiledExpressionRoot to the type name
            // to represent the new type that represents the compiled expressions.
            // Take everything after the last . for the type name.
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            // Take everything before the last . for the namespace.
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());

            // Create a TextExpressionCompilerSettings.
            TextExpressionCompilerSettings settings = new TextExpressionCompilerSettings
            {
                Activity = dynamicActivity,
                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = true
            };

            // Compile the C# expression.
            TextExpressionCompilerResults results =
                new TextExpressionCompiler(settings).Compile();

            // Any compilation errors are contained in the CompilerMessages.
            if (results.HasErrors)
            {
                throw new Exception("Compilation failed.");
            }

            // Create an instance of the new compiled expression type.
            ICompiledExpressionRoot compiledExpressionRoot =
                Activator.CreateInstance(results.ResultType,
                    new object[] { dynamicActivity }) as ICompiledExpressionRoot;

            // Attach it to the activity.
            CompiledExpressionInvoker.SetCompiledExpressionRootForImplementation(
                dynamicActivity, compiledExpressionRoot);
        }

        static void CompileExpressions(Activity activity)
        {
            // activityName is the Namespace.Type of the activity that contains the
            // C# expressions.
            string activityName = activity.GetType().ToString();

            // Split activityName into Namespace and Type.Append _CompiledExpressionRoot to the type name
            // to represent the new type that represents the compiled expressions.
            // Take everything after the last . for the type name.
            string activityType = activityName.Split('.').Last() + "_CompiledExpressionRoot";
            // Take everything before the last . for the namespace.
            string activityNamespace = string.Join(".", activityName.Split('.').Reverse().Skip(1).Reverse());

            // Create a TextExpressionCompilerSettings.
            TextExpressionCompilerSettings settings = new TextExpressionCompilerSettings
            {
                Activity = activity,
                Language = "C#",
                ActivityName = activityType,
                ActivityNamespace = activityNamespace,
                RootNamespace = null,
                GenerateAsPartialClass = false,
                AlwaysGenerateSource = true,
                ForImplementation = false
            };

            // Compile the C# expression.
            TextExpressionCompilerResults results =
                new TextExpressionCompiler(settings).Compile();

            // Any compilation errors are contained in the CompilerMessages.
            if (results.HasErrors)
            {
                throw new Exception("Compilation failed.");
            }

            // Create an instance of the new compiled expression type.
            ICompiledExpressionRoot compiledExpressionRoot =
                Activator.CreateInstance(results.ResultType,
                    new object[] { activity }) as ICompiledExpressionRoot;

            // Attach it to the activity.
            CompiledExpressionInvoker.SetCompiledExpressionRoot(
                activity, compiledExpressionRoot);
        }
        #endregion 方法

    }
}