using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace globaltraders
{
    public class TextInputToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Always test MultiValueConverter inputs for non-null
            // (to avoid crash bugs for views in the designer)
            if (values[0] is bool && values[1] is bool)
            {
                bool hasText = !(bool)values[0];
                bool hasFocus = (bool)values[1];

                if (hasFocus || hasText)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ProfitLossColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
              
                var PL = value.ToString();
                if (PL.Contains("-") || PL.Contains("K"))
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
            else
            {
                return "Green";
            }
           
        }

      

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
    public class ProfitLossColorConverterDecimal : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {

                var PL = value.ToString();
                if (PL.Contains("-"))
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
            else
            {
                return "Green";
            }

        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }

    public class TextAmounttoNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                double num;
                string strvalue = value as string;
                if (double.TryParse(strvalue, out num))
                {
                    return num.ToString("N0");
                }
                return value;
           
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
    public class Css
    {

        public static string GetClass(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (string)element.GetValue(ClassProperty);
        }

        public static void SetClass(DependencyObject element, string value)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(ClassProperty, value);
        }


        public static readonly DependencyProperty ClassProperty =
            DependencyProperty.RegisterAttached("Class", typeof(string), typeof(Css),
                new PropertyMetadata(null, OnClassChanged));

        private static void OnClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ui = d as FrameworkElement;
            Style newStyle = new Style();

            if (e.NewValue != null)
            {
                var names = e.NewValue as string;
                var arr = names.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in arr)
                {
                    Style style = ui.FindResource(name) as Style;
                    foreach (var setter in style.Setters)
                    {
                        newStyle.Setters.Add(setter);
                    }
                    foreach (var trigger in style.Triggers)
                    {
                        newStyle.Triggers.Add(trigger);
                    }
                }
            }
            ui.Style = newStyle;
        }
    }
}
