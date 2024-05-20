using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();
        public Window1()
        {
            InitializeComponent();
            //dataGrid1.DataContext = users;

            users.Add(new User() { UserName = "Prem", FirstName = "Prem Bdr" });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            users.Add(new User() { UserName = "Prem", FirstName = "Prem Bdr" });
        }
    }
    public class User : INotifyPropertyChanged

    {

        private string _userName;

        public User()

        {

            UserName = "New user";

        }

        public string UserName

        {

            get { return _userName; }

            set

            {

                _userName = value;

                FirePropertyChangedEvent("UserName");

            }
        }

        private string _firstName;

        public string FirstName

        {

            get { return _firstName; }

            set

            {

                _firstName = value;

                FirePropertyChangedEvent("FirstName");

            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChangedEvent(string propertyName)

        {

            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
