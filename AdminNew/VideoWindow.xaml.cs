using Microsoft.Win32;
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
using System.Windows.Forms;
using mshtml;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            InitializeComponent();
        }
        int BrowserVer, RegVal;
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.WebBrowser Wb = new System.Windows.Forms.WebBrowser())
                BrowserVer = Wb.Version.Major;

            // set the appropriate IE version
            if (BrowserVer >= 11)
                RegVal = 11001;
            else if (BrowserVer == 10)
                RegVal = 10001;
            else if (BrowserVer == 9)
                RegVal = 9999;
            else if (BrowserVer == 8)
                RegVal = 8888;
            else
                RegVal = 7000;

            // set the actual key
            using (RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree))
                if (Key.GetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe") == null)
                    Key.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
            try
            {

                //LoggedinUserDetail.TvChannels=

                // this.Location = location;


                foreach (var item in LoggedinUserDetail.TvChannels)
                {
                    System.Windows.Controls.MenuItem toolitem = new System.Windows.Controls.MenuItem();
                    toolitem.Header = item.ChanelName.ToString();
                    toolitem.Tag = item.ChanelURL.ToString();
                    toolitem.Click += new RoutedEventHandler(changechanelurl);
                    menuChannels.Items.Add(toolitem);
                }




              //  wbNew.Navigate(LoggedinUserDetail.TvChannels[0].ChanelURL);
        wb.Navigate(LoggedinUserDetail.TvChannels[0].ChanelURL);
                wfhSample.Visibility = Visibility.Visible;
                
                //lbllogo.Background = Brushes.White;
                //lbllogo.Foreground = Brushes.Black;
            }
            catch (System.Exception ex)
            {

            }
        }
        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          
        }
        private void htmlDoc_Click(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = false;
        }
        //public void ResizeTV()
        //{

        //    if (wbNew.Document == null)
        //    {
        //        return;
        //    }
        //    htmlDocNew = (HTMLDocument) wbNew.Document;

        //    // htmlDocNew = wbNew.Document;
        //    wbNew.IsEnabled = false;
        //    //htmlDocNew.cli += htmlDoc_Click;
        //    //htmlDocNew.MouseDown += htmlDoc_MouseDown;

        //    //htmlDocNew.MouseMove += htmlDoc_MouseMove;

        //    //htmlDoc.ContextMenuShowing += htmlDoc_ContextMenuShowing;
        //    //wbNew.ScriptErrorsSuppressed = true;
        //    //wbNew.AllowNavigation = false;




        //    if (wbNew.url != null)
        //    {
        //        try
        //        {


        //            string url = wb.Url.AbsolutePath;
        //            if (wb.Url.AbsolutePath != "blank")
        //            {
        //                if (currentchanel.Contains("Race"))
        //                {
        //                    var divtoremove = wb.Document.Body.GetElementsByTagName("div");
        //                    HtmlElement firstdiv = (HtmlElement)divtoremove[0];

        //                    var allelements = firstdiv.Children;

        //                    foreach (HtmlElement link in allelements)
        //                    {
        //                        if (link.GetAttribute("className") == "close" || link.GetAttribute("className") == "banner")
        //                        {
        //                            link.OuterHtml = null;
        //                            firstdiv.OuterHtml = null;
        //                        }
        //                    }
        //                    //if (currentchanel == " Race TV 1")
        //                    //{

        //                    //}
        //                    //else
        //                    //{
        //                    //    var divtoremove = webBrowser1.Document.Body.GetElementsByTagName("div");


        //                    //    foreach (HtmlElement link in divtoremove)
        //                    //    {
        //                    //        if (link.GetAttribute("id") == "aaaab")
        //                    //        {
        //                    //            link.OuterHtml = null;
        //                    //        }
        //                    //        if (link.GetAttribute("id") == "player")
        //                    //        {
        //                    //            HtmlElement firstitem = (HtmlElement)link.FirstChild;
        //                    //           firstitem.SetAttribute("width", webBrowser1.Width.ToString());
        //                    //            firstitem.SetAttribute("height", (webBrowser1.Height - 35).ToString());
        //                    //        }

        //                    //    }

        //                    //}
        //                    // {HtmlElementCollection form = webBrowser1.Document.Body.Children;
        //                    // var maindiv = webBrowser1.Document.GetElementById("contenido");






        //                }
        //                else
        //                {

        //                    var form = wb.Document.Body.Children[1].GetElementsByTagName("embed"); //form element
        //                    if (form.Count > 0)
        //                    {
        //                        form[0].SetAttribute("width", (wb.Width - 5).ToString());
        //                        form[0].SetAttribute("height", (wb.Height - 5).ToString());
        //                    }
        //                    else
        //                    {
        //                        form = wb.Document.Body.Children[0].GetElementsByTagName("embed"); //form element
        //                        if (form.Count > 0)
        //                        {
        //                            form[0].SetAttribute("width", (wb.Width - 5).ToString());
        //                            form[0].SetAttribute("height", (wb.Height - 5).ToString());
        //                        }
        //                    }

        //                }

        //            }
        //        }
        //        catch (System.Exception ex)
        //        {

        //        }
        //    }
        //    wb.Visible = true;
        //    //wb.Dock= DockStyle.Left;
        //    //wb.Width =Convert.ToInt32( this.Width - 30);
        //    //wb.Height = Convert.ToInt32(this.Height - 70);
        //    resetPopup();
        //    wfhSample.Height = this.Height - 63;
        //    wfhSample.Width = this.Width - 10;

        //}
        public void ResizeTV1()
        {
           // wfhSample.Height = this.Height - 60;
           // wfhSample.Width = this.Width - 5;
            if (wb.Document == null)
            {
                return;
            }
            htmlDoc = wb.Document;

            htmlDoc = wb.Document;

            htmlDoc.Click += htmlDoc_Click;
            htmlDoc.MouseDown += htmlDoc_MouseDown;

            htmlDoc.MouseMove += htmlDoc_MouseMove;

            htmlDoc.ContextMenuShowing += htmlDoc_ContextMenuShowing;
            wb.ScriptErrorsSuppressed = true;
            wb.AllowNavigation = false;
         
           
         
           
            if (wb.Url != null)
            {
                try
                {


                    string url = wb.Url.AbsolutePath;
                    if (wb.Url.AbsolutePath != "blank")
                    {
                        if (currentchanel.Contains("Race"))
                        {
                            var divtoremove = wb.Document.Body.GetElementsByTagName("div");
                            HtmlElement firstdiv = (HtmlElement)divtoremove[0];

                            var allelements = firstdiv.Children;

                            foreach (HtmlElement link in allelements)
                            {
                                if (link.GetAttribute("className") == "close" || link.GetAttribute("className") == "banner")
                                {
                                    link.OuterHtml = null;
                                    firstdiv.OuterHtml = null;
                                }
                            }
                            //if (currentchanel == " Race TV 1")
                            //{

                            //}
                            //else
                            //{
                            //    var divtoremove = webBrowser1.Document.Body.GetElementsByTagName("div");


                            //    foreach (HtmlElement link in divtoremove)
                            //    {
                            //        if (link.GetAttribute("id") == "aaaab")
                            //        {
                            //            link.OuterHtml = null;
                            //        }
                            //        if (link.GetAttribute("id") == "player")
                            //        {
                            //            HtmlElement firstitem = (HtmlElement)link.FirstChild;
                            //           firstitem.SetAttribute("width", webBrowser1.Width.ToString());
                            //            firstitem.SetAttribute("height", (webBrowser1.Height - 35).ToString());
                            //        }

                            //    }

                            //}
                            // {HtmlElementCollection form = webBrowser1.Document.Body.Children;
                            // var maindiv = webBrowser1.Document.GetElementById("contenido");






                        }
                        else
                        {

                            var form = wb.Document.Body.Children[1].GetElementsByTagName("embed"); //form element
                            if (form.Count > 0)
                            {
                                form[0].SetAttribute("width",( wb.Width-5).ToString());
                                form[0].SetAttribute("height", (wb.Height-5).ToString());
                                form[0].SetAttribute("allowfullscreen", "false");
                                form[0].SetAttribute("muted", "true");

                            //    HtmlElement elem = wb.Document.CreateElement("DIV");
                            //  //  elem.SetAttribute("STYLE", "z-index: 5000;position: fixed; background: black;color: white;height: 18px; width: 96px; padding: 5px;font-size: 16px; ");
                            //    elem.InnerText = "Global Traders";
                            //    elem.Style = "z-index:5000;position:fixed;background:black;color:white;height:18px; width:96px;padding:5px;font-size:16px;top:0";
                            //  //  wb.Document.Body.AppendChild(elem);
                            //elem=    wb.Document.Body.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterBegin, elem);
                                
                                //HtmlElement newelement = new HtmlElement();

                                //wb.Document.Body.a
                            }
                            else
                            {
                                form = wb.Document.Body.Children[0].GetElementsByTagName("embed"); //form element
                                if (form.Count > 0)
                                {
                                    form[0].SetAttribute("width",( wb.Width-5).ToString());
                                    form[0].SetAttribute("height", (wb.Height-5).ToString());
                                    form[0].SetAttribute("allowfullscreen", "false");
                                    form[0].SetAttribute("muted", "true");
                                    //HtmlElement elem = wb.Document.CreateElement("LABEL");
                                    //elem.SetAttribute("STYLE", "z-index: 5000;position: fixed; background: black;color: white;height: 18px; width: 96px; padding: 5px;font-size: 16px; ");
                                    //elem.InnerText = "Global Traders";

                                    //wb.Document.Body.AppendChild(elem);
                                }
                            }
                           
                        }

                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            wb.Visible = true;
            wfhSample.IsEnabled = false;
            //wb.Dock= DockStyle.Left;
            //wb.Width =Convert.ToInt32( this.Width - 30);
            //wb.Height = Convert.ToInt32(this.Height - 70);
            resetPopup();
         

        }
        private void resetPopup()
        {
           try
            {
                var offset = popupheading.HorizontalOffset;
                popupheading.HorizontalOffset = offset + 1;
                popupheading.HorizontalOffset = offset;
                //if (this.ActualWidth - 20 > 0)
                //{
                //    popupheading.Width = this.ActualWidth - 15;
                //}
                popupheading.Width = 100;
               popupheading.PlacementRectangle = new Rect(0, 30, 0, 0);
                popupheading.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
                popupheading.IsOpen = false;
            }
            catch(System.Exception ex)
            {

            }
           

            // Resizing
           
        }
        string currentchanel = "";
        HtmlDocument htmlDoc;
        HTMLDocument htmlDocNew;
        void htmlDoc_ContextMenuShowing(object sender, HtmlElementEventArgs e)

        {

            // stop the right mouse Menu

            e.ReturnValue = false;

        }

        void htmlDoc_MouseMove(object sender, HtmlElementEventArgs e)

        {

            e.ReturnValue = false;

        }

        void htmlDoc_MouseDown(object sender, HtmlElementEventArgs e)

        {

            e.ReturnValue = false;

        }
        
        private void webBrowser1_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
           
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ResizeTV1();
        }

        private void wb_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ResizeTV1();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           // wfhSample.Height = this.Height - 60;
           // wfhSample.Width = this.Width-10;
            ResizeTV1();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.Topmost == true)
            {
                this.Topmost = false;
                menuitemoptop.Background = Brushes.Transparent;
                menuitemoptop.Foreground = Brushes.Black;
            }
            else
            {
                this.Topmost = true;
                menuitemoptop.Background = Brushes.Black;
                menuitemoptop.Foreground = Brushes.White;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LoggedinUserDetail.isTVON = false;
            wb.Dispose();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //wfhSample.Height = this.Height - 60;
            //wfhSample.Width = this.Width - 10;
            resetPopup();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            popupheading.IsOpen = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            popupheading.IsOpen = false;
        }

        private void wbNew_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ResizeTV1();
        }

        private void changechanelurl(object sender, RoutedEventArgs e)
        {
            try
            {
                wb.AllowNavigation = true;
                System.Windows.Controls.MenuItem toolitem = (System.Windows.Controls.MenuItem)sender;
                string chnlurl = (string)toolitem.Tag;
                currentchanel = toolitem.Header.ToString();
                wb.Navigate(chnlurl);
            }
            catch (System.Exception ex)
            {

            }

        }
    }
}
