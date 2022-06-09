using bfnexchange.BettingServiceReference;
using bfnexchange.UsersServiceReference;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class FancyStatusController : Controller
    {
        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        // GET: FancyStatus
        public ActionResult Index()
        {
            return View();
        }
        List<ExternalAPI.TO.MarketBookForindianFancy> marketbook3 = new List<ExternalAPI.TO.MarketBookForindianFancy>();
        MarketBookForindianFancy marketbook2 = new MarketBookForindianFancy();
        List<RunnerForIndianFancy> lstMarketBookRunnersIndianFancy = new List<RunnerForIndianFancy>();
        GetDataFancy GetDataFancy = new GetDataFancy();
        public string LoadFancyMarketIN(string EventID, string MarketBookID)
        {
            try
            {
                //List<ExternalAPI.TO.LinevMarkets> linevmarketsfancy = (List<ExternalAPI.TO.LinevMarkets>)Session["linevmarkets"];
                //marketbook3.Clear();
                //var aaa = objBettingClient.GetRunnersForFancy(EventID, MarketBookID);
                GetDataFancy = JsonConvert.DeserializeObject<ExternalAPI.TO.GetDataFancy>(objBettingClient.GetRunnersForFancy(EventID, MarketBookID));
                GetDataFancy.session = GetDataFancy.session.Take(40).OrderBy(s => s.SelectionId).ToList();
                if (GetDataFancy.session != null)
                {
                    foreach (var runners in GetDataFancy.session)
                    {


                        var runner = new ExternalAPI.TO.RunnerForIndianFancy();
                        runner.StatusStr = runners.GameStatus;
                        runner.SelectionId = runners.SelectionId;
                        runner.MarketBookID = EventID;
                        lstMarketBookRunnersIndianFancy.Add(runner);

                    }

                    marketbook2.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>(lstMarketBookRunnersIndianFancy);
                    marketbook3.Add(marketbook2);

                    return LoggedinUserDetail.ConverttoJSONString(marketbook3);
                }
                else
                {
                    return LoggedinUserDetail.ConverttoJSONString(marketbook3);
                }
            }
            catch (System.Exception ex)
            {
                return LoggedinUserDetail.ConverttoJSONString(marketbook3);
            }
        }
    }
}