using MSScriptControl;
using System;

namespace InvokeJSDemo
{
    public class RunScriptHelper
    {
        /// <summary>
        /// 脚本类型
        /// </summary>
        public enum ScriptLanguage
        {
            /// <summary>
            /// JScript脚本语言
            /// </summary>
            JScript,

            /// <summary>
            /// VbScript脚本语言
            /// </summary>
            VbScript,

            /// <summary>
            /// JavaScript脚本语言
            /// </summary>
            JavaScript
        }

        /// <summary>
        /// 脚本运行错误代理
        /// </summary>
        public delegate void RunErrorHandler();

        /// <summary>
        /// 脚本运行超时代理
        /// </summary>
        public delegate void RunTimeoutHandler();

        /// <summary>
        /// ScriptEngine类
        /// </summary>
        public class ScriptEngine
        {
            private ScriptControl msc;

            /// <summary>
            /// 定义脚本运行错误事件
            /// </summary>
            public event RunErrorHandler RunError;

            /// <summary>
            /// 定义脚本运行超时事件
            /// </summary>
            public event RunTimeoutHandler RunTimeout;

            /// <summary>
            ///构造函数 默认为 VbScript 脚本类型
            /// </summary>
            public ScriptEngine()
                : this(ScriptLanguage.VbScript)
            { }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="language">脚本类型</param>
            public ScriptEngine(ScriptLanguage language)
            {
                this.msc = new ScriptControlClass {UseSafeSubset = true, Language = language.ToString()};
                ((DScriptControlSource_Event)this.msc).Error += new DScriptControlSource_ErrorEventHandler(OnError);
                ((DScriptControlSource_Event)this.msc).Timeout += new DScriptControlSource_TimeoutEventHandler(OnTimeout);
            }

            /// <summary>
            /// 运行Eval方法
            /// </summary>
            /// <param name="expression">表达式</param>
            /// <param name="codeBody">函数体</param>
            /// <returns>返回值object</returns>
            public object Eval(string expression, string codeBody)
            {
                msc.AddCode(codeBody);
                return msc.Eval(expression);
            }

            /// <summary>
            /// 运行Eval方法
            /// </summary>
            /// <param name="language">脚本语言</param>
            /// <param name="expression">表达式</param>
            /// <param name="codeBody">函数体</param>
            /// <returns>返回值object</returns>
            public object Eval(ScriptLanguage language, string expression, string codeBody)
            {
                if (this.Language != language)
                    this.Language = language;
                return Eval(expression, codeBody);
            }

            /// <summary>
            /// 运行Run方法
            /// </summary>
            /// <param name="mainFunctionName">入口函数名称 例如:add</param>
            /// <param name="parameters">参数:例如:new object(){"",""}</param>
            /// <param name="codeBody">函数体 例如：function add(int a,int b){return a+b;}</param>
            /// <returns>返回值object:获取object.ToString()</returns>
            public object Run(string mainFunctionName, object[] parameters, string codeBody)
            {
                this.msc.AddCode(codeBody);
                return msc.Run(mainFunctionName, parameters);
            }

            /// <summary>
            /// 运行Run方法
            /// </summary>
            /// <param name="language">脚本语言</param>
            /// <param name="mainFunctionName">入口函数名称</param>
            /// <param name="parameters">参数</param>
            /// <param name="codeBody">函数体</param>
            /// <returns>返回值object</returns>
            public object Run(ScriptLanguage language, string mainFunctionName, object[] parameters, string codeBody)
            {
                if (this.Language != language)
                    this.Language = language;
                return Run(mainFunctionName, parameters, codeBody);
            }

            /// <summary>
            /// 放弃所有已经添加到 ScriptControl 中的 Script 代码和对象
            /// </summary>
            public void Reset()
            {
                this.msc.Reset();
            }

            /// <summary>
            /// 获取或设置脚本语言
            /// </summary>
            public ScriptLanguage Language
            {
                get => (ScriptLanguage)Enum.Parse(typeof(ScriptLanguage), this.msc.Language, false);
                set => this.msc.Language = value.ToString();
            }

            /// <summary>
            /// 获取或设置脚本执行时间，单位为毫秒
            /// </summary>
            public int Timeout => 0;

            /// <summary>
            /// 设置是否显示用户界面元素
            /// </summary>
            public bool AllowUI
            {
                get => this.msc.AllowUI;
                set => this.msc.AllowUI = value;
            }

            /// <summary>
            /// 宿主应用程序是否有保密性要求
            /// </summary>
            public bool UseSafeSubset
            {
                get => this.msc.UseSafeSubset;
                set => this.msc.UseSafeSubset = true;
            }

            /// <summary>
            /// RunError事件激发
            /// </summary>
            private void OnError()
            {
                RunError?.Invoke();
            }

            /// <summary>
            /// OnTimeout事件激发
            /// </summary>
            private void OnTimeout()
            {
                RunTimeout?.Invoke();
            }
        }
    }
}