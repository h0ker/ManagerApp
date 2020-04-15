using ManagerApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ManagerApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeScreen : Page
    {
        public HomeScreen()
        {
            this.InitializeComponent();
            uxLogoutButton.Click += UxLogoutButton_Clicked;
            uxInventoryButton.Click += UxInventoryButton_Clicked;
            uxCouponButton.Click += UxCouponButton_Clicked;
            uxEmployeeButton.Click += UxEmployeeButton_Clicked;
            uxMenuEditButton.Click += UxMenuEditButton_Clicked;
        }

        private void UxMenuEditButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuEdit), null);
            uxStatisticsButton.Click += uxStatisticsButton_Clicked;
        }

        private void UxEmployeeButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EmployeePage), null);
        }

        private void UxCouponButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Coupons), null);
        }

        private void UxInventoryButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Inventory), null);
        }
        private void uxStatisticsButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Statistics), null);
        }
        private void UxLogoutButton_Clicked(object sender, RoutedEventArgs e)
        {
            RealmManager.RemoveAll<Employee>();
            RealmManager.RemoveAll<Ingredient>();
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }
    }
}
