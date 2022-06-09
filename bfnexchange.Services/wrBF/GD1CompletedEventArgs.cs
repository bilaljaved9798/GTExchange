namespace bfnexchange.wrBF
{

    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Microsoft.VisualBasic.CompilerServices;

    [GeneratedCode("System.Web.Services", "4.6.1038.0"), DebuggerStepThrough, DesignerCategory("code")]
    public class GD1CompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GD1CompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, RuntimeHelpers.GetObjectValue(userState))
        {
            this.results = results;
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return Conversions.ToString(this.results[0]);
            }
        }
    }
}

