using bftradeline.Models;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for BookPostionForIN.xaml
    /// </summary>
    public partial class BookPostionForIN : Window
    {
        public BookPostionForIN()
        {
            InitializeComponent();
            timerCountdown.Tick += TimerCountdown_Tick;
            this.Height = 400;
            this.DGVBookitems.Height = 320;
            lstBookPosition = new ObservableCollection<BookPosition1>();
            this.DGVBookitems.DataContext = lstBookPosition;
            backgroundWorkerUpdateData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateData.DoWork += BackgroundWorkerUpdateData_DoWork;
            backgroundWorkerUpdateData.RunWorkerCompleted += BackgroundWorkerUpdateData_RunWorkerCompleted;
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private void BackgroundWorkerUpdateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //  LoadBets();
                GetBookPosition();
                backgroundWorkerUpdateData.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

            }


        }

        private void BackgroundWorkerUpdateData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {


                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                System.Threading.Thread.Sleep(500);
            }
            catch (System.Exception ex)
            {

            }
        }

        public BackgroundWorker backgroundWorkerUpdateData;
        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
            //   LoadBets();
            GetBookPosition();
        }

        public class BookPosition1 : INotifyPropertyChanged
        {
            public string Score { get; set; }
            public double Position { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public DispatcherTimer timerCountdown = new DispatcherTimer();
        public bool isopenedbyselecedagentfromadmin = false;
        ObservableCollection<BookPosition1> lstBookPosition;
        public void GetBookPosition()
        {
            try
            {
                List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
                if (UserTypeID == 1)
                {

                    List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                        ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                        objmarketbook.MarketId = marketBookID;

                        objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                        ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbook.Runners.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbook.Runners != null)
                            {
                                ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objmarketbook.Runners.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                objmarketbook.Runners.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objmarketbook.Runners.Add(objRunnerlast);
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                            decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                            bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                            int TransferAdminpercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                            foreach (var userbet in lstCurrentBetsbyUser)
                            {
                                var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminpercentage) / 100));

                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            decimal a = (Convert.ToDecimal(userbet.BetSize) / 100);
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }

                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;

                            }
                        }

                        objmarketbook.DebitCredit = lstDebitCredit;
                        List<BookPosition1> lstNewList = new List<BookPosition1>();

                        foreach (var runneritem in objmarketbook.Runners)
                        {
                            BookPosition1 objBook = new BookPosition1();
                            objBook.Score = runneritem.SelectionId;
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                            objBook.Position = -1 * runneritem.ProfitandLoss;

                            lstNewList.Add(objBook);
                        }
                        var result = lstNewList.Where(p => !lstBookPosition.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                        var result1 = lstBookPosition.Where(p => !lstNewList.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                        if (result.Count() > 0 || result1.Count() > 0)
                        {
                            lstBookPosition.Clear();
                            foreach (var item in lstNewList)
                            {
                                lstBookPosition.Add(new BookPosition1() { Score = item.Score, Position = item.Position });
                            }
                        }


                        // DGVBookitems.ItemsSource = lstBookPosition;

                    }
                    else
                    {
                        if (lstBookPosition.Count > 0)
                        {
                            lstBookPosition.Clear();
                        }

                        //  DGVBookitems.ItemsSource = new List<BookPosition1>();
                    }
                }
                else
                {
                    if (UserTypeID == 2)
                    {

                        List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = new List<UserBetsforAgent>();
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                        }
                        //else
                        //{
                        //    lstCurrentBetsAdmin = CurrentUserbetsAgent.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        //}

                        if (lstCurrentBetsAdmin.Count > 0)
                        {
                            lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                            objmarketbook.MarketId = marketBookID;
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                            objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                            objmarketbook.Runners.Add(objRunner1);
                            foreach (var userbet in lstCurrentBetsAdmin)
                            {
                                if (objmarketbook.Runners != null)
                                {
                                    ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                        objmarketbook.Runners.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                    objmarketbook.Runners.Add(objRunner);
                                }
                            }
                            ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                            objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                            objmarketbook.Runners.Add(objRunnerlast);
                            ///calculation
                            var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {
                                var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                                //decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                                //bool TransferAdminAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                                foreach (var userbet in lstCurrentBetsbyUser)
                                {
                                    decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;

                                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    if (userbet.BetType == "back")
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = totamount;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = totamount;
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }


                                    }

                                    //userbet.lstDebitCredit = new List<DebitCredit>();
                                    //userbet.lstDebitCredit = lstDebitCredit;

                                }
                            }
                            objmarketbook.DebitCredit = lstDebitCredit;
                            List<BookPosition1> lstNewList = new List<BookPosition1>();

                            foreach (var runneritem in objmarketbook.Runners)
                            {
                                BookPosition1 objBook = new BookPosition1();
                                objBook.Score = runneritem.SelectionId;
                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                objBook.Position = -1 * runneritem.ProfitandLoss;

                                lstNewList.Add(objBook);
                            }
                            var result = lstNewList.Where(p => !lstBookPosition.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                            var result1 = lstBookPosition.Where(p => !lstNewList.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                            if (result.Count() > 0 || result1.Count() > 0)
                            {
                                lstBookPosition.Clear();
                                foreach (var item in lstNewList)
                                {
                                    lstBookPosition.Add(new BookPosition1() { Score = item.Score, Position = item.Position });
                                }
                            }
                        }
                        else
                        {
                            if (lstBookPosition.Count > 0)
                            {
                                lstBookPosition.Clear();
                            }
                        }
                    }
                    else
                    {
                        if (UserTypeID == 3)
                        {

                            List<bftradeline.Models.UserBets> lstCurrentBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                            if (lstCurrentBets.Count > 0)
                            {
                                lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                                ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
                                objmarketbookin.MarketId = marketBookID;
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                                objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                                objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                                foreach (var userbet in lstCurrentBets)
                                {
                                    if (objmarketbookin.RunnersForindianFancy != null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                        if (objexistingrunner == null)
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                        }
                                    }
                                    else
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                                objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                                objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                                ///calculation
                                foreach (var userbet in lstCurrentBets)
                                {
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    var totamount = (Convert.ToDecimal(userbet.Amount));
                                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    if (userbet.BetType == "back")
                                    {
                                        double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;

                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                        {
                                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;

                                                objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                                // objDebitCredit.Debit = totamount;
                                                objDebitCredit.Credit = 0;

                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }
                                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;

                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                                //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                        {
                                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = totamount;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }
                                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;

                                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }


                                    }


                                }
                                objmarketbookin.DebitCredit = lstDebitCredit;
                                List<BookPosition1> lstNewList = new List<BookPosition1>();

                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    BookPosition1 objBook = new BookPosition1();
                                    objBook.Score = runneritem.SelectionId;
                                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                    objBook.Position = runneritem.ProfitandLoss;

                                    lstNewList.Add(objBook);
                                }
                                var result = lstNewList.Where(p => !lstBookPosition.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                                var result1 = lstBookPosition.Where(p => !lstNewList.Any(p2 => p2.Score == p.Score && p2.Position == p.Position));

                                if (result.Count() > 0 || result1.Count() > 0)
                                {
                                    lstBookPosition.Clear();
                                    foreach (var item in lstNewList)
                                    {
                                        lstBookPosition.Add(new BookPosition1() { Score = item.Score, Position = item.Position });
                                    }
                                }
                                //objmarketbook.DebitCredit = lstDebitCredit;
                                //lstBookPosition.Clear();
                                //foreach (var runneritem in objmarketbook.Runners)
                                //{
                                //    BookPosition1 objBook = new BookPosition1();
                                //    objBook.Score = runneritem.SelectionId;
                                //    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                //    objBook.Position = runneritem.ProfitandLoss;

                                //    lstBookPosition.Add(objBook);
                                //}

                                // DGVBookitems.ItemsSource = lstBookPosition;

                            }
                            else
                            {
                                if (lstBookPosition.Count > 0)
                                {
                                    lstBookPosition.Clear();
                                }
                                // lstBookPosition.Clear();

                                // DGVBookitems.ItemsSource = new List<BookPosition1>();
                            }
                        }
                        
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }
        public List<bftradeline.Models.UserBets> CurrentUserbets = new List<bftradeline.Models.UserBets>();
        public List<bftradeline.Models.UserBetsforAgent> CurrentUserbetsAgent = new List<bftradeline.Models.UserBetsforAgent>();

        public List<bftradeline.Models.UserBetsForAdmin> CurrentUserbetsAdmin = new List<bftradeline.Models.UserBetsForAdmin>();
        public string marketBookID = "";
        public string eventID = "";
        public string selectionID = "";

        public string marketbookName = "";
        public int UserTypeID = 0;
        public int userID = 0;
        public void LoadBets()
        {

            try
            {
                // System.Threading.Thread.Sleep(200);

                if (UserTypeID == 3)
                {
                    //string userbets = objUsersServiceCleint.GetUserbetsbyUserID(userID, LoggedinUserDetail.PasswordForValidate);
                    //List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(userbets);

                    //CurrentUserbets = lstUserBets;

                    //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
                    //  ViewData["liabality"] = Session["liabality"];
                    //  ViewBag.totliabality = Session["totliabality"];

                    //  return RenderRazorViewToString("UserBets", lstUserBets);

                }
                else
                {
                    if (UserTypeID == 2)
                    {
                        if (isopenedbyselecedagentfromadmin == true)
                        {
                            string userbets = objUsersServiceCleint.GetUserBetsbyAgentIDwithZeroReferer(userID, LoggedinUserDetail.PasswordForValidate);
                            var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(userbets);
                            CurrentUserbetsAgent = lstUserBet;
                        }
                        else
                        {
                            //string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(userID, LoggedinUserDetail.PasswordForValidate);
                            //var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(userbets);
                            //CurrentUserbetsAgent = lstUserBet;
                        }



                    }
                    else
                    {
                        if (UserTypeID == 1)
                        {
                            //string RestAPIPath = ConfigurationManager.AppSettings["RestAPIPath"];
                            //WebClient RESTProxy = new WebClient();
                            //byte[] data = RESTProxy.DownloadData(new Uri(RestAPIPath + "Services/BettingServiceRest.svc/GetUserbetsForAdmin"));
                            //Stream stream = new MemoryStream(data);
                            //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(string));
                            //string userbets = obj.ReadObject(stream).ToString();
                            ////string userbets = objUsersServiceCleint.GetUserbetsForAdmin();
                            //List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);
                            //CurrentUserbetsAdmin = lstUserBet;
                        }
                        else
                        {

                        }

                    }
                }

            }
            catch (System.Exception ex)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetBookPosition();
            this.Title = "Book - " + marketbookName;
            timerCountdown.Interval = TimeSpan.FromMilliseconds(300);
            timerCountdown.Start();

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerUpdateData.CancelAsync();
                backgroundWorkerUpdateData.Dispose();
                backgroundWorkerUpdateData = null;
                GC.Collect();
                timerCountdown.Tick -= TimerCountdown_Tick;
                timerCountdown.Stop();
            }
            catch (System.Exception ex)
            {

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadBets();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadBets();
        }
    }
}
