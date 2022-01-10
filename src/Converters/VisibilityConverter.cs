﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace fcrd
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = Visibility.Collapsed;
            if (value != null && (bool)value)
                visibility = Visibility.Visible;
            return (object)visibility;
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            return (object)((Visibility)value == Visibility.Visible);
        }
    }
}