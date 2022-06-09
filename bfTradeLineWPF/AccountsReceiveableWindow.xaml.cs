using bftradeline.Models;
using globaltraders.Models;
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
    /// Interaction logic for AccountsReceiveableWindow.xaml
    /// </summary>
    public partial class AccountsReceiveableWindow : Window
    {
        public AccountsReceiveableWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetUsersbyUsersType();
            Getdatabydate(DateTime.Now);
            dtpDueDateAdd.Value = DateTime.Now;
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public void GetUsersbyUsersType()
        {
            LoggedinUserDetail.CheckifUserLogin();
           
            if (1==1)
            {
                List<UserIDandUserType> lstUsers =LoggedinUserDetail.AllUsers;

               
               
                cmbPunters.ItemsSource = lstUsers;
                cmbPunters.DisplayMemberPath = "UserName";
                cmbPunters.SelectedValuePath = "ID";





            }
            else
            {
#pragma warning disable CS0162 // Unreachable code detected
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
#pragma warning restore CS0162 // Unreachable code detected

            }
        }
        public void Getdatabydate(DateTime currdate)
        {
            var results = JsonConvert.DeserializeObject<List<AmountReceivables>>(objUsersServiceCleint.GetAllPendingAmountsbyDate(currdate));
            if (results != null)
            {
             
                foreach (var item in results)
                {
                    
                 item.UserName= Crypto.Decrypt(item.UserName);
                
                    item.Balance = item.Amount - item.AmountReceived;
                   item.DueDateStr= item.DueDate.ToString("dd/MM/yyyy hh:mm:ss");
                   
                }
                dgvPunters.ItemsSource = results;
            }
        }

        private void btnAddReceiveables_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (cmbPunters.SelectedIndex > -1 && txtAmoundAdd.Text != "")
                {
                    objUsersServiceCleint.AddAmountReceviables(Convert.ToInt32(cmbPunters.SelectedValue), Convert.ToDecimal(txtAmoundAdd.Text), dtpDueDateAdd.Value.Value, "Pending", 0);
                   Xceed.Wpf.Toolkit. MessageBox.Show("Successfully Added.");
                    txtPunterName.Content = "";
                    txtAmoundAdd.Text = "";
                   
                    txtPunterName.Tag = "";
                    Getdatabydate(dtpDueDateAdd.Value.Value);

                }

            }
        }

        private void btnSearchReceiveables_Click(object sender, RoutedEventArgs e)
        {
            Getdatabydate(dtpDueDateAdd.Value.Value);
        }

        private void btnSearchReceiveables_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtPunterName.Tag != null)
            {
                if (txtAmountReceived.Text != "")
                {
                    decimal reaminingamount = Convert.ToDecimal(txtAmountActual.Text) - Convert.ToDecimal(txtAmountReceived.Text);
                    string status = "";
                    if (reaminingamount > 0)
                    {
                        status = "Pending";
                    }
                    else
                    {
                        status = "Received";
                    }
                    objUsersServiceCleint.UpdateAmountReceviables(Convert.ToInt32(txtPunterName.Tag), Convert.ToDecimal(txtAmountReceived.Text), dateTimePicker2.Value.Value, status);
                  Xceed.Wpf.Toolkit.  MessageBox.Show("Successfully Updated.");
                    txtPunterName.Content = "";
                    txtAmountActual.Text = "";
                    dateTimePicker2.Value = DateTime.Now;
                    txtPunterName.Tag = null;
                    txtAmountReceived.Text = "";
                    Getdatabydate(dateTimePicker2.Value.Value);
                }
            }
            else
            {
                MessageBox.Show("Please select username from data.");
            }
        }

        private void dgvPunters_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dgvPunters.Items.Count > 0)
            {
                AmountReceivables objSelectedRow = (AmountReceivables)dgvPunters.SelectedItem;
                txtPunterName.Content = objSelectedRow.UserName;
                txtAmountActual.Text = objSelectedRow.Balance.ToString();
                try
                {
                    dateTimePicker2.Value = DateTime.Now;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                }
                txtPunterName.Tag = objSelectedRow.ID;
            }
        }

        private void cmbPunters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtAmoundAdd.Focus();
        }
    }
}
