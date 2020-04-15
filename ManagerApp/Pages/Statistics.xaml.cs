using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ManagerApp.Models.ServiceRequests;
using System;
using ManagerApp.Models;
using System.Linq;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ManagerApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Statistics : Page
    {
        public Statistics()
        {
            this.InitializeComponent();

            //clicked events
            uxBackButton.Click += UxBackButton_Clicked;
        }


        private async void KPIComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add "using Windows.UI;" for Color and Colors.
            string viewSelection = e.AddedItems[0].ToString();
            
            //clearing realm database from any old order list
            RealmManager.RemoveAll<OrderList>();
            RealmManager.RemoveAll<MenuItemList>();

            //excutes if the GET order request returns back okay
            if (await GetOrdersRequest.SendGetOrdersRequest() && await GetMenuItemsRequest.SendGetMenuItemsRequest())
            {
                //creating a dictionary to keep track of the count of each menuItem
                Dictionary<String, int> menuItemCounter = new Dictionary<String, int>();
                foreach (MenuItem m in RealmManager.All<MenuItemList>().FirstOrDefault().menuItems)
                {
                    menuItemCounter.Add(m._id, 0);
                }

                //will keep track of revenue made yearly, monthly and weekly
                Dictionary<DateTime, int> revenueCalendar = new Dictionary<DateTime, int>();

                //creating a list of every menu item id for each order including duplicates
                List<string> menuItemIds = new List<string>();
                
                //figuring out which view needs to be populated
                switch (viewSelection)
                {
                    case "Current Monthly View":
                        foreach (Order o in RealmManager.All<OrderList>().FirstOrDefault().orders)
                        {
                            //this will ignore all uncompleted orders
                            if(o.time_completed == null)
                            {
                                continue;
                            }

                            //initalize this month and last month
                            DateTime td = DateTime.Today;
                            DateTime monthStart = new DateTime(td.Year, td.Month, td.Day, 0, 0, 0);
                            DateTime orderTime = DateTime.ParseExact(o.time_completed.Replace('T',' ').TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff",
                                       System.Globalization.CultureInfo.InvariantCulture); ;
                            
                            //only added menuItems from orders for the current month
                            if (DateTime.Compare(monthStart, orderTime ) < 0) {
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id);
                                }
                            }


                        }
                        uxMonthlyViewGrid.Visibility = Visibility.Visible;
                        break;
                    case "Current Weekly View":
                        uxWeeklyViewGrid.Visibility = Visibility.Visible;
                        break;
                    case "Current Yearly View":
                        uxYearlyViewGrid.Visibility = Visibility.Visible;
                        break;
                }
                //uxMonthlyViewGridView.ItemsSource = RealmManager.All<OrderList>().FirstOrDefault().orders.ToList();
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Sorry",
                    Content = "Something went wrong.",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
        }

        //populating the monthly view popup
        public async void RefreshMonthlyView()
        {
            RealmManager.RemoveAll<OrderList>();
            await GetEmployeeListRequest.SendGetEmployeeListRequest();
            //uxMonthlyViewGridView.ItemsSource = RealmManager.All<OrderList>().ToList();
        }

        //Creating back button functionality
        private void UxBackButton_Clicked(object sender, RoutedEventArgs e)
        {
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