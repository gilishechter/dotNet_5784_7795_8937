﻿using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
namespace PL;

internal class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
internal class ConvertIdToISEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if ((int)value <= 0)
        {
            return true;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
//    internal class ConvertStatusProjectToISEnable : IValueConverter
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if ((s_bl.CheckStatusProject() == BO.StatusProject.Mid))
//                return true;
//            return false;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//internal class ConvertStatusProjectPlanningToISEnable : IValueConverter
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        if ((s_bl.CheckStatusProject() == BO.StatusProject.Mid) || (s_bl.CheckStatusProject() == BO.StatusProject.Planning))
//            return true;
//        return false;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}

internal class ConvertBoolToVisability : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
            return Visibility.Visible;
        return Visibility.Collapsed;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertBoolToVisability2 : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
            return Visibility.Collapsed;
        return Visibility.Visible;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTimeToWidh: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).Days * 2;
      
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
public class TaskColorConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Status status)
        {

            if (BO.Status.Done == status)
                return Brushes.DeepPink;
            if (BO.Status.Scheduled == status)
                return Brushes.DarkBlue;
            if (BO.Status.Unscheduled == status)
                return Brushes.DarkBlue;
            if (BO.Status.InJeopardy == status)
                return Brushes.Red;
            return Brushes.LightBlue;
        }
        else return Brushes.LightBlue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}