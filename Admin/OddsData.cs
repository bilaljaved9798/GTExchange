using bftradeline;
using ExternalAPI.TO;
using globaltraders.Service123Reference;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace globaltraders
{
    public class OddsData
    {
        public MarketBook GetAllMarketBooks(string MarketBookID)
        {
            MarketBook currentmarketobject = new MarketBook();
            IList<bftradeline.wrBF.MarketBook> list;
            try
            {

                List<string> marketIdsNew = new List<string>();
                marketIdsNew.Add(MarketBookID);

               


                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {


                    string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataL/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                    request.Method = "GET";
                    request.Proxy = null;
                    request.Timeout =5000;
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLeaseTimeout = 5000;
                    request.ServicePoint.MaxIdleTime = 5000;
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    List<ExternalAPI.TO.MarketBook> test = new List<ExternalAPI.TO.MarketBook>();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {

                            System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<ExternalAPI.TO.MarketBook>));
                            test = (List<ExternalAPI.TO.MarketBook>)obj1.ReadObject(dataStream);

                            dataStream.Close();
                        }
                        response.Close();
                    }

                    //  var results1 = objBettingClient.GetAllMarketsBP(marketIdsNew.ToArray());
                    var results = (test);
                    foreach (var item in results)
                    {
                        try
                        {

                            if (item.MarketId != null)
                            {


                                currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault();

                                currentmarketobject = ConvertJsontoMarketObjectBFLive(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally);

                              
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }

                }
                else
                {

                   
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {
                        
                           
                         //  var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(objBettingClient.GetAllMarketsBP(string.Join(",", marketIdsNew.Distinct().ToArray())));

                       
                        string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataBP/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
                        //var client1 = new RestClient(RestAPIPath1);
                        //var request1 = new RestRequest(Method.GET);

                        //// request1.RequestFormat = RestSharp.DataFormat.Json;
                        ////request1.AddUrlSegment("marketID", string.Join(",", marketIdsNew.Distinct().ToArray()));

                        ////request1.JsonSerializer.ContentType = "application/json; charset=utf-8";
                        //request1.AddHeader("Accept-Encoding", "gzip");

                        //var response1 = client1.Execute(request1);
                        //var arr = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JValue>(response1.Content);
                        //var arr1 = JsonConvert.DeserializeObject<List<SampleResponse1>>(arr.Value.ToString());
                        //var results = arr1;

                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                        request.Method = "GET";
                        request.Proxy = null;
                        request.Timeout = 5000;
                        request.KeepAlive = false;
                        request.ServicePoint.ConnectionLeaseTimeout = 5000;
                        request.ServicePoint.MaxIdleTime = 5000;
                        //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0";
                        //request.ReadWriteTimeout = 30000;
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        String test = String.Empty;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {

                            using (Stream dataStream = response.GetResponseStream())
                            {
                                System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                                test = obj1.ReadObject(dataStream).ToString();

                                dataStream.Close();
                            }
                            response.Close();


                        }
                        var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(test);
                        foreach (var item in results)
                        {
                            try
                            {

                                if (item.MarketId != null)
                                {


                                    currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault();

                                    currentmarketobject = ConvertJsontoMarketObjectBF123(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally);

                                   
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }

                    }
                    else
                    {
                        string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataOther/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                        request.Method = "GET";
                        request.Proxy = null;
                        request.Timeout = 5000;
                        request.ReadWriteTimeout = 30000;
                        request.KeepAlive = false;
                        request.ServicePoint.ConnectionLeaseTimeout = 5000;
                        request.ServicePoint.MaxIdleTime = 5000;
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        List<ExternalAPI.TO.MarketBookString> test = new List<ExternalAPI.TO.MarketBookString>();
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (Stream dataStream = response.GetResponseStream())
                            {
                                System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<ExternalAPI.TO.MarketBookString>));
                                test = (List<ExternalAPI.TO.MarketBookString>)obj1.ReadObject(dataStream);

                                dataStream.Close();
                            }
                            response.Close();

                        }
                        // var results = (objBettingClient.GetAllMarketsOthers(marketIdsNew.ToArray()));
                        var results = test;
                        foreach (var item in results)
                        {
                            try
                            {

                                if (item.MarketBookId != null)
                                {


                                    currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault();
                                    // var index = LoggedinUserDetail.MarketBooks.IndexOf(currentmarketobject);


                                    currentmarketobject = ConvertJsontoMarketObjectBFNewSource(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally);

                                   
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }

                    }

                }
                return currentmarketobject;
            }
            catch (System.Exception ex)
            {
                return currentmarketobject;
            }
        }
        public static string FormatNumber(double n)
        {
            if (n < 1000)
                return n.ToString();

            if (n < 10000)
                return String.Format("{0:#,.##}K", n - 5);

            if (n < 100000)
                return String.Format("{0:#,.#}K", n - 50);

            if (n < 1000000)
                return String.Format("{0:#,.}K", n - 500);

            if (n < 10000000)
                return String.Format("{0:#,,.##}M", n - 5000);

            if (n < 100000000)
                return String.Format("{0:#,,.#}M", n - 50000);

            if (n < 1000000000)
                return String.Format("{0:#,,.}M", n - 500000);

            return String.Format("{0:#,,,.##}B", n - 5000000);
        }
        public MarketBook ConvertJsontoMarketObjectBFLive(ExternalAPI.TO.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally)
        {


            try
            {


                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.isOpenExternally = isopenexternally;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "IN-PLAY";
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "CLOSED";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "SUSPENDED";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "ACTIVE";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new ExternalAPI.TO.Runner();
                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        try
                        {
                            var runnermarketitem = OldRunners.Where(item => item.SelectionId == runneritem.SelectionId.ToString()).First();
                            runner.RunnerName = runnermarketitem.RunnerName;

                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }
                        catch (System.Exception ex)
                        {

                        }

                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {

                                try
                                {


                                    if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {

                                if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                            // selectionIDfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").FirstOrDefault().SelectionId;
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF123(SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally)
        {


            try
            {


                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.isOpenExternally = isopenexternally;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.Inplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "IN-PLAY";
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "CLOSED";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "SUSPENDED";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "ACTIVE";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new ExternalAPI.TO.Runner();
                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        try
                        {
                            var runnermarketitem = OldRunners.Where(item => item.SelectionId == runneritem.SelectionId.ToString()).First();
                            runner.RunnerName = runnermarketitem.RunnerName;

                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }
                        catch (System.Exception ex)
                        {

                        }

                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.Ex.AvailableToBack != null && runneritem.Ex.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {

                                try
                                {


                                    if (runneritem.Ex.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.Ex.AvailableToLay)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.Ex.AvailableToBack)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.Ex.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.Ex.AvailableToLay != null && runneritem.Ex.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {

                                if (runneritem.Ex.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.Ex.AvailableToBack)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.Ex.AvailableToLay)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.Ex.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                            // selectionIDfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").FirstOrDefault().SelectionId;
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public Service123Client objBettingClient = new Service123Client();
        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed, List<ExternalAPI.TO.Runner> OldRunners)
        {
            try
            {
                string[] marketIds = new string[]
                      {
                    marketid
                      };
                if (MainSportsCategory == "Cricket" && sheetname.Contains("Match Odds") && LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new ExternalAPI.TO.MarketBook();
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }
                    }
                    else
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }


                    }
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners)
        {


            try
            {


                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "IN-PLAY";
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "CLOSED";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "SUSPENDED";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "ACTIVE";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new ExternalAPI.TO.Runner();
                        if (OldRunners.Count > 0)
                        {
                            var runnermarketitem = OldRunners.Where(item2 => item2.SelectionId == runneritem.SelectionId.ToString()).FirstOrDefault();
                            runner.RunnerName = runnermarketitem.RunnerName;
                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }

                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                try
                                {


                                    if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }

                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }


                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }

                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                            // selectionIDfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").FirstOrDefault().SelectionId;
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) <= 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) <= 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally)
        {



            if (1 == 1)
            {
                var marketbook = new MarketBook();
                string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray();
                string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

                marketbook.MarketId = BFMarketbook.MarketBookId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = false;
                marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6]);
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                
                marketbook.isOpenExternally = isopenexternally;
                DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                DateTime CurrentDate = DateTime.Now;
                TimeSpan remainingdays = (CurrentDate - OpenDate);
                if (OpenDate < CurrentDate)
                {
                    marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                }
                else
                {
                    marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                }

                if (Convert.ToInt32(BFMarketBookDetail[5]) == 1 && BFMarketBookDetail[2].Trim().ToString() == "OPEN")
                {

                    marketbook.MarketStatusstr = "IN-PLAY";
                }
                else
                {
                    if (BFMarketBookDetail[2].Trim().ToString() == "CLOSED")
                    {
                        marketbook.MarketStatusstr = "CLOSED";
                    }
                    else
                    {
                        if (BFMarketBookDetail[2].Trim().ToString() == "SUSPENDED")
                        {
                            marketbook.MarketStatusstr = "SUSPENDED";
                        }
                        else
                        {
                            marketbook.MarketStatusstr = "ACTIVE";
                        }

                    }

                }

                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();


                for (int i = 1; i < newres.Count(); i++)
                {
                    string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                    string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray(); ;
                    string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    var runner = new ExternalAPI.TO.Runner();
                    if (OldRunners.Count > 0)
                    {
                        var runnermarketitem = OldRunners.Where(item2 => item2.SelectionId == runnerinfo[0].Trim().ToString()).FirstOrDefault();
                        runner.RunnerName = runnermarketitem.RunnerName;
                        runner.JockeyName = runnermarketitem.JockeyName;
                        runner.WearingURL = runnermarketitem.WearingURL;
                        runner.WearingDesc = runnermarketitem.WearingDesc;
                        runner.Clothnumber = runnermarketitem.Clothnumber;
                        runner.StallDraw = runnermarketitem.StallDraw;
                    }
                    try
                    {
                        runner.Handicap = Convert.ToDouble(runnerinfo[4].Trim());
                    }
                    catch (System.Exception)
                    {
                        runner.Handicap = 0;

                    }

                    runner.StatusStr = runnerinfo[6].Trim();
                    runner.SelectionId = runnerinfo[0].Trim().ToString();

                    //runner.LastPriceTraded = runnerinfo[3];
                    var lstpricelist = new List<PriceSize>();
                    if (runnerbackdata.Count() > 0)
                    {
                        if (newres.Count() == 2)
                        {
                            try
                            {


                                if (runnerbackdata[0].ToString().Contains("."))
                                {
                                    for (int j = 0; j < runnerlaydata.Count();)
                                    {
                                        if (j < runnerlaydata.Count())
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j]) + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < runnerbackdata.Count();)
                                    {
                                        if (j < runnerbackdata.Count())
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            for (int j = 0; j < runnerbackdata.Count();)
                            {
                                if (j < runnerbackdata.Count())
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

                                    lstpricelist.Add(pricesize);
                                    j = j + 4;

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int ii = 0; ii < 3; ii++)
                        {
                            var pricesize = new PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;

                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices = new ExchangePrices();
                    runner.ExchangePrices.AvailableToBack = lstpricelist;
                    lstpricelist = new List<PriceSize>();
                    if (runnerlaydata.Count() > 0)
                    {
                        if (newres.Count() == 2)
                        {
                            try
                            {


                                if (runnerlaydata[0].ToString().Contains("."))
                                {
                                    for (int j = 0; j < runnerbackdata.Count();)
                                    {
                                        if (j < runnerbackdata.Count())
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j]) + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < runnerlaydata.Count();)
                                    {
                                        if (j < runnerlaydata.Count())
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                            j = j + 4;

                                        }

                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            for (int j = 0; j < runnerlaydata.Count();)
                            {
                                if (j < runnerlaydata.Count())
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

                                    lstpricelist.Add(pricesize);
                                    j = j + 4;

                                }

                            }
                        }
                    }
                    else
                    {
                        for (int ii = 0; ii < 3; ii++)
                        {
                            var pricesize = new PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;

                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                    runner.ExchangePrices.AvailableToLay = lstpricelist;
                    lstRunners.Add(runner);
                }


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);
                double lastback = 0;
                double lastbackSize = 0;
                double lastLaySize = 0;
                double lastlay = 0;

                if (marketbook.MarketStatusstr != "SUSPENDED")
                {

                    double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                    string selectionIDfav = marketbook.Runners[0].SelectionId;
                    foreach (var favoriteitem in marketbook.Runners)
                    {
                        if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                            if (marketbook.MainSportsname.Contains("Racing"))
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }
                            else
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }

                    }
                    if (marketbook.MarketStatusstr == "CLOSED")
                    {
                        var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                        if (resultsfav != null && resultsfav.Count() > 0)
                        {
                            selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                        }
                        // selectionIDfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").FirstOrDefault().SelectionId;
                    }
                    var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                    string selectionname = favoriteteam.RunnerName;
                    if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                    {
                        lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                        lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                    }
                    if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                    {

                        lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                        lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                    }
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = selectionname;
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = selectionIDfav;
                }
                else
                {
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = "";
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = "0";
                }
                if (Convert.ToDouble(marketbook.FavoriteBack) <= 0)
                {
                    marketbook.FavoriteBack = "";
                    marketbook.FavoriteBackSize = "";
                }
                if (Convert.ToDouble(marketbook.FavoriteLay) <= 0)
                {
                    marketbook.FavoriteLay = "";
                    marketbook.FavoriteLaySize = "";
                }
                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
    }
}
