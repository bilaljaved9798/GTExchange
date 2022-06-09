using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAPIConfigService" in both code and config file together.
    [ServiceContract]
    public interface IAPIConfigService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAPIConfigData(int ID);
        [OperationContract]
        void UpdateSession(string session, int ID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetPoundRate();
        [OperationContract]
        void SetPoundRate(string Poundrate);

    }
}
