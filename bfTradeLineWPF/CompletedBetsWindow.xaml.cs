using bftradeline.Models;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for CompletedBetsWindow.xaml
    /// </summary>
    public partial class CompletedBetsWindow : Window
    {
        public CompletedBetsWindow()
        {
            InitializeComponent();
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public string MarketBookID;
        public int UserID;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<CompletedUserBets> lstCompletedUserBets = JsonConvert.DeserializeObject<List<CompletedUserBets>>(objUsersServiceCleint.GetCompletedMatchedBetsbyUserID(UserID, MarketBookID)).ToList();
            //lstCompletedUserBets= lstCompletedUserBets.GroupBy(x=>x.MarketBookname).Select(g => g.First()).ToList();
            if (lstCompletedUserBets.Count > 0)
            {
                lblMarketname.Text = lstCompletedUserBets[0].MarketBookname + Environment.NewLine + "Winner: " + lstCompletedUserBets[0].Winnername;
                foreach (CompletedUserBets objbet in lstCompletedUserBets)
                {
                    if (objbet.BetType == "back")
                    {
                        if (objbet.SelectionName == objbet.Winnername)
                        {
                            objbet.Steak = (Convert.ToInt64(objbet.Amount) * (Convert.ToDouble(objbet.UserOdd) - 1)).ToString();
                        }
                        else
                        {
                            objbet.Steak = (Convert.ToInt64(objbet.Amount) * -1).ToString();
                        }
                    }
                    else
                    {
                        if (objbet.SelectionName == objbet.Winnername)
                        {
                            objbet.Steak = (-1 * (Convert.ToInt64(objbet.Amount) * (Convert.ToDouble(objbet.UserOdd) - 1))).ToString();
                        }
                        else
                        {
                            objbet.Steak = objbet.Amount;
                        }
                    }
                    objbet.Steak = Convert.ToDouble(objbet.Steak).ToString("F2");

                }
                DGVBetsCompleted.ItemsSource = lstCompletedUserBets;
            }
            else
            {
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
