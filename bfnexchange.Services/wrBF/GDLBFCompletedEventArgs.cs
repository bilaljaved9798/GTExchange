namespace bfnexchange.wrBF
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Web.Services", "4.6.1038.0"), DebuggerStepThrough, DesignerCategory("code")]
    public class GDLBFCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GDLBFCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, RuntimeHelpers.GetObjectValue(userState))
        {
            this.results = results;
        }

        public MarketBook Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (MarketBook) this.results[0];
            }
        }
    }
}

