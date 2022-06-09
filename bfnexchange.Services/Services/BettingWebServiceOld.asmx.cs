using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace bfnexchange.Services.Services
{
    /// <summary>
    /// Summary description for BettingWebServiceOld
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BettingWebServiceOld : System.Web.Services.WebService
    {

        [WebMethod]
        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyID(string marketID, string sheetname, string OrignalOpenDate, string MainSportsCategory)
        {

            if (1 == 1)
            {


                try
                {
                    var marketbooks = new List<MarketBook>();
                   
                        if (marketID != "")
                        {


                            // string JsonResultsArr = "";


                            // JsonResultsArr = dbEntities.SP_OddsData_GetData().FirstOrDefault();
                            var marketbook1 = new RootObject();
                            if (MainSportsCategory == "Tennis")
                            {
                                marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrTennis);
                            }
                            else
                            {
                                if (MainSportsCategory == "Soccer")
                                {
                                    marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrSoccer);
                                }
                                else
                                {
                                    if (MainSportsCategory == "Horse Racing")
                                    {
                                        if (sheetname.Contains("To Be Placed"))
                                        {
                                            marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrHorseRacePlace);
                                        }
                                        else
                                        {
                                            marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrHorseRaceWin);
                                        }

                                    }
                                    else
                                    {
                                        if (MainSportsCategory == "Greyhound Racing")
                                        {
                                            if (sheetname.Contains("To Be Placed"))
                                            {
                                                marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrGrayHoundPlace);
                                            }
                                            else
                                            {
                                                marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrGrayHoundWin);
                                            }

                                        }
                                        else
                                        {
                                            if (MainSportsCategory == "Cricket")
                                            {
                                                if (sheetname.Contains("Match Odds"))
                                                {
                                                    marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrCricketMatchOdds);
                                                }
                                                else
                                                {

                                                    if (sheetname.Contains("Completed Match"))
                                                    {
                                                        marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrCricketCompletedMatch);
                                                    }
                                                    else
                                                    {
                                                        marketbook1 = JsonConvert.DeserializeObject<RootObject>(APIConfigforResults.JsonResultsArrCricketInningsRuns);
                                                    }

                                                }

                                            }
                                        }
                                    }
                                }
                            }


                            MarketNode marketbookorignal = marketbook1.eventTypes.SelectMany(eventtype => eventtype.eventNodes)
                                 .SelectMany(eventnode => eventnode.marketNodes).FirstOrDefault(marketnode => marketnode.marketId == marketID);



                            if (marketbookorignal != null)
                            {


                                var marketbook = new MarketBook();
                                marketbook.marketsopened = marketbook1.eventTypes.SelectMany(eventtype => eventtype.eventNodes)
                                 .SelectMany(eventnode => eventnode.marketNodes).Count();
                                marketbook.MarketId = marketbookorignal.marketId;
                                marketbook.SheetName = "";

                                marketbook.PoundRate = APIConfigforResults.PoundRate;
                                if (APIConfigforResults.PoundRate == 0)
                                {
                                    APIConfigService objAPIConfigCleint = new APIConfigService();
                                    marketbook.PoundRate = Convert.ToDecimal(Crypto.Decrypt(objAPIConfigCleint.GetPoundRate()));
                                }
                                marketbook.NumberOfWinners = marketbookorignal.state.numberOfWinners;
                                marketbook.MainSportsname = "";
                                if (sheetname != "" && OrignalOpenDate != "")
                                {
                                    marketbook.MarketBookName = sheetname;
                                    marketbook.MainSportsname = MainSportsCategory;
                                    marketbook.OrignalOpenDate = Convert.ToDateTime(OrignalOpenDate);
                                    marketbook.OpenDate = DateTime.Now.TimeOfDay.ToString();
                                    //DateTime OpenDate = marketbook.OrignalOpenDate.AddHours(5);
                                    //DateTime CurrentDate = DateTime.Now;
                                    //TimeSpan remainingdays = (CurrentDate - OpenDate);
                                    //if (OpenDate < CurrentDate)
                                    //{
                                    //    marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                    //}
                                    //else
                                    //{
                                    //    marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                    //}
                                }

                                if (marketbookorignal.state.inplay == true && marketbookorignal.state.status == "OPEN")
                                {
                                    //marketbook.MarketStatusstr = marketbook1.eventTypes[0].eventNodes[0].marketNodes[0].state.status.ToString() + marketbook1.eventTypes[0].eventNodes[0].marketNodes[0].state.inplay.ToString();
                                    marketbook.MarketStatusstr = "In Play";
                                }
                                else
                                {
                                    if (marketbookorignal.state.status == "CLOSED")
                                    {
                                        marketbook.MarketStatusstr = "Closed";
                                    }
                                    else
                                    {
                                        if (marketbookorignal.state.status == "SUSPENDED")
                                        {
                                            marketbook.MarketStatusstr = "Suspended";
                                        }
                                        else
                                        {
                                            marketbook.MarketStatusstr = "Active";
                                        }

                                    }

                                }

                                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                                foreach (var runneritem in marketbookorignal.runners)
                                {
                                    var runner = new ExternalAPI.TO.Runner();
                                    runner.RunnerName = runneritem.description.runnerName;
                                    runner.Handicap = runneritem.handicap;
                                    runner.StatusStr = runneritem.state.status.ToString();
                                    //if (MainSportsCategory.Contains("Racing"))
                                    //{
                                    //    runner.JockeyName = runneritem.description.metadata.JOCKEY_NAME;
                                    //    runner.WearingURL = "http://content-cache.betfair.com/feeds_images/Horses/SilkColours/"+ runneritem.description.metadata.COLOURS_FILENAME;
                                    //    runner.WearingDesc = runneritem.description.metadata.COLOURS_DESCRIPTION;
                                    //    runner.Clothnumber = runneritem.description.metadata.CLOTH_NUMBER;
                                    //}

                                    // runner.SelectionId = Regex.Replace(runneritem.description.runnerName, @"[^0-9a-zA-Z]+", "");
                                    runner.SelectionId = runneritem.selectionId.ToString();
                                    runner.LastPriceTraded = runneritem.state.lastPriceTraded;
                                    var lstpricelist = new List<PriceSize>();
                                    if (runneritem.exchange.availableToBack != null)
                                    {
                                        foreach (var backitems in runneritem.exchange.availableToBack)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.size * Convert.ToDouble(marketbook.PoundRate));

                                            pricesize.Price = backitems.price;

                                            lstpricelist.Add(pricesize);
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
                                    if (runneritem.exchange.availableToLay != null)
                                    {
                                        foreach (var backitems in runneritem.exchange.availableToLay)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.size * Convert.ToDouble(marketbook.PoundRate));

                                            pricesize.Price = backitems.price;

                                            lstpricelist.Add(pricesize);
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

                                if (marketbook.Runners[0].ExchangePrices.AvailableToBack.Count > 0)
                                {
                                    double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                                    string selectionIDfav = marketbook.Runners[0].SelectionId;
                                    foreach (var favoriteitem in marketbook.Runners)
                                    {
                                        if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                            if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                            {
                                                favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                                selectionIDfav = favoriteitem.SelectionId;
                                            }
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
                                marketbooks.Add(marketbook);

                            }
                        }
                    
                    return marketbooks;
                }
                catch (System.Exception ex)
                {
                    APIConfig.LogError(ex);
                    var marketbooks = new List<MarketBook>();
                    return marketbooks;
                }
            }
            else
            {
                var marketbooks = new List<MarketBook>();
                return marketbooks;
            }
        }
    }
}
