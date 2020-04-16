using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
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
    public sealed partial class HomeScreen : Page
    {
        public DispatcherTimer timer = new DispatcherTimer();
        public string LiveTime => DateTime.Now.ToString("HH:mm:ss");
        public HomeScreen()
        {
            this.InitializeComponent();
            uxLogoutButton.Click += UxLogoutButton_Clicked;
            uxInventoryButton.Click += UxInventoryButton_Clicked;
            uxCouponButton.Click += UxCouponButton_Clicked;
            uxEmployeeButton.Click += UxEmployeeButton_Clicked;
            uxMenuEditButton.Click += UxMenuEditButton_Clicked;
            uxEmployeeListView.ItemClick += UxEmployeeListView_ItemClick;

            timer.Interval = TimeSpan.FromSeconds(1);  
            timer.Tick += Timer_Tick;  
            timer.Start();  

            RefreshEmployeeList();
        }

        private void Timer_Tick(object sender, object e)
        {
            secondHand.Angle = DateTime.Now.Second * 6;  
            minuteHand.Angle = DateTime.Now.Minute * 6;  
            hourHand.Angle = (DateTime.Now.Hour * 30);  
        }

        private void UxEmployeeListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxTimePopup.IsOpen = true;
        }

        public async void RefreshEmployeeList()
        {
            RealmManager.RemoveAll<EmployeeList>();
            RealmManager.RemoveAll<Employee>();
            await GetEmployeeListRequest.SendGetEmployeeListRequest();
            uxEmployeeListView.ItemsSource = RealmManager.All<EmployeeList>().FirstOrDefault().employees.ToList();
        }

        private void UxMenuEditButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuEdit), null);
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
