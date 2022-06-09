using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBettingServiceCricketCompletedMatch" in both code and config file together.
    [ServiceContract]
    public interface IBettingServiceCricketCompletedMatch
    {
        [OperationContract]
        void GetDataFromBetfairReadOnly();
        [OperationContract]
        void GetCurrentMarketBookCricket(string Password);
    }
}
