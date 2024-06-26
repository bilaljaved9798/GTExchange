﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace globaltraders
{
    class ClosableTab:TabItem
    {
        UserServiceReference.UserServicesClient objUsersServiceCleint = new UserServiceReference.UserServicesClient();
        // Constructor
        public ClosableTab()
        {
           
            // Create an instance of the usercontrol
            CloseableHeader closableTabHeader = new CloseableHeader();

            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;

            // Attach to the CloseableHeader events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave += new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
        }



        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string Title
        {
            set
            {
                ((CloseableHeader)this.Header).label_TabTitle.Content = value;
            }
        }




        //
        // - - - Overrides  - - -
        //


        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
            ((CloseableHeader)this.Header).label_TabTitle.Foreground = Brushes.White;
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            ((CloseableHeader)this.Header).label_TabTitle.Foreground = Brushes.Black;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }





        //
        // - - - Event Handlers  - - -
        //


        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }


        // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
          
            ExternalAPI.TO.MarketBook markettoremove = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == this.Tag.ToString()).First();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.SetMarketBookClosedbyUserAsync(73, markettoremove.MarketId);
            }
            else
            {
                objUsersServiceCleint.SetMarketBookClosedbyUserAsync(LoggedinUserDetail.GetUserID(), markettoremove.MarketId);
            }
            LoggedinUserDetail.MarketBooks.Remove(markettoremove);
            LoggedinUserDetail.OpenMarkets.Remove("markettab" + markettoremove.MarketId.Replace(".", ""));
            
            ((TabControl)this.Parent).Items.Remove(this);
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Margin = new Thickness(((CloseableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }


    }

}
