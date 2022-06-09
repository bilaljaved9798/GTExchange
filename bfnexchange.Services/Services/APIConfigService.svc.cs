using bfnexchange.Services.DBModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "APIConfigService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select APIConfigService.svc or APIConfigService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class APIConfigService : IAPIConfigService
    {
        NExchangeEntities dbEntities = new NExchangeEntities();
        
        public string GetAPIConfigData(int ID)
        {
            var results = dbEntities.SP_APIConfig_GetData(ID).First<SP_APIConfig_GetData_Result>();
            return ConverttoJSONString(results);
        }
        public string GetPoundRate()
        {
            var results = dbEntities.SP_APIConfig_GetPoundRate().First<SP_APIConfig_GetPoundRate_Result>();
            return results.PoundRate.ToString();
        }
        public void SetPoundRate(string Poundrate)
        {
            dbEntities.SP_APIConfig_UpdatePoundRate(Poundrate);
        }
        public void UpdateSession(string session,int ID)
        {
            dbEntities.SP_APIConfig_UpdateSession(session,ID);
        }
        public string GetCommisionRate()
        {
            var results = dbEntities.SP_APIConfig_GetCommisionRate().FirstOrDefault<SP_APIConfig_GetCommisionRate_Result>();
            return results.ComissionRate.ToString();
        }
        public string ConverttoJSONString(object result)
        {
            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }
    }
}
