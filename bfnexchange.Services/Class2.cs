using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using bfnexchange.wrBF;
using bfnexchange.Services;

namespace TestBF
{
   
    [System.Web.Services.WebServiceBindingAttribute(Name = "wsnewSoap", Namespace = "http://forexdatawsnew.org/")]
    public class wsnew : SoapHttpClientProtocol
    {
        public wsnew()
        {
            this.Url = "http://rsexch.bfbetpro.com/theService/MainSvc.asmx";
            this.UseDefaultCredentials = false;

        }


        [SoapDocumentMethod("http://forexdatawsnew.org/GDLBF", RequestNamespace = "http://forexdatawsnew.org/", ResponseNamespace = "http://forexdatawsnew.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public MarketBook[] GDLBF(string Scd, string[] marketIds, int n)
        {
            return (MarketBook[])base.Invoke("GDLBF", new object[]
            {
                Scd,
                marketIds,
                n
            })[0];
        }
        [SoapDocumentMethod("http://forexdatawsnew.org/GD", RequestNamespace = "http://forexdatawsnew.org/", ResponseNamespace = "http://forexdatawsnew.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public MarketBook[] GD(string Scd, string[] marketIds, int n)
        {
            return (MarketBook[])base.Invoke("GD", new object[]
            {
                Scd,
                marketIds,
                n
            })[0];
        }
        [SoapDocumentMethod("http://bfbetpro.com/getMarketBook", RequestNamespace = "http://bfbetpro.com/", ResponseNamespace = "http://bfbetpro.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string getMarketBook(string marketId, sessionHeader header)
        {

            return (string)base.Invoke("getMarketBook", new object[]
            {
                marketId,
                header
            })[0];
        }
        [SoapDocumentMethod("http://bfbetpro.com/UserLogin", RequestNamespace = "http://bfbetpro.com/", ResponseNamespace = "http://bfbetpro.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public BetterInfo UserLogin(string securitycode, string userName, string password, string softwareId)
        {
            return (BetterInfo)base.Invoke("UserLogin", new object[]
            {
              null,
                userName,
                password,
                null
            })[0];
        }
        public string Url
        {
            get { return base.Url; }
              
            set
            {
                base.Url = value;
            }
            }


        }

    }
