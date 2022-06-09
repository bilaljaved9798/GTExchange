using Microsoft.VisualBasic.CompilerServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;


namespace Test.wrBetEasyNG
{
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [GeneratedCode("System.Web.Services", "4.7.3056.0")]
    [WebServiceBinding(Name = "beteasynewngSoap", Namespace = "http://thebeteasynewbezy.org/")]
    public class beteasyng : SoapHttpClientProtocol
    {
        private SendOrPostCallback NGOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public beteasyng()
        {
             this.Url = "https://inforeborn.com/UnitedBet/beteasynewng.asmx";
            //this.Url = ConfigurationManager.AppSettings["URLForDataother"];
               if (this.IsLocalFileSystemWebService(this.Url))
               {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
                return;
              }
            this.useDefaultCredentialsSetExplicitly = true;
        }

       
        [SoapDocumentMethod("http://thebeteasynewbezy.org/NG", RequestNamespace = "http://thebeteasynewbezy.org/", ResponseNamespace = "http://thebeteasynewbezy.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string NG(int g, int o, string c, string T, string BName, string s1, string s2, string s3, string s4, string s5, string s6)
        {
            return Conversions.ToString(base.Invoke("NG", new object[] { g, o, c, T, BName, s1, s2, s3, s4, s5, s6 })[0]);
        }
      

        private bool IsLocalFileSystemWebService(string url)
        {
            bool result;
            if (url == null || url == string.Empty)
            {
                result = false;
            }
            else
            {
                Uri uri = new Uri(url);
                result = (uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0);
            }
            return result;
        }
    }
}
