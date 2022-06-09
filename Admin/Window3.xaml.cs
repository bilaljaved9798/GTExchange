using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        private ObservableCollection<Person> persons = new ObservableCollection<Person>();
        public Window3()
        {
            InitializeComponent();
            dgPerson.ItemsSource = persons;
            persons.CollectionChanged += this.OnCollectionChanged;
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Get the sender observable collection
            ObservableCollection<Person> obsSender = sender as ObservableCollection<Person>;
            NotifyCollectionChangedAction action = e.Action;
            if (action == NotifyCollectionChangedAction.Add)
                lblStatus.Content = "New person added";
            if (action == NotifyCollectionChangedAction.Remove)
                lblStatus.Content = "Person deleted";
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Person p = new Person();
            p.FirstName = txtFname.Text;
            p.LastName = txtLname.Text;
            persons.Add(p);

        }
    }
}
