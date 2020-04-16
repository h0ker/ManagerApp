using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ManagerApp.Models.ServiceRequests;
using System;
using ManagerApp.Models;
using System.Linq;
using System.Collections.Generic;
using LinqToDB;
using Syncfusion.UI.Xaml.Charts;

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
            this.DataContext = new ViewModel();

            UxAllTimeCharts();

        }

        //Creating the first chart
        public class Person
        {
            public string Name { get; set; }

            public double Height { get; set; }
        }

        public class ViewModel
        {
            public List<Person> Data { get; set; }

            public ViewModel()
            {
                Data = new List<Person>()
            {
                new Person { Name = "David", Height = 180 },
                new Person { Name = "Michael", Height = 170 },
                new Person { Name = "Steve", Height = 160 },
                new Person { Name = "Joel", Height = 182 }
            };
            }
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
                Dictionary<DateTime, double> revenueCalendar = new Dictionary<DateTime, double>();

                //creating a list of every menu item id for each order including duplicates
                List<string> menuItemIds = new List<string>();

                string mostPopularMenuItemId;

                //figuring out which view needs to be populated
                switch (viewSelection)
                {
                    //MONTHLY VIEW
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
                            DateTime monthStart = new DateTime(td.Year, td.Month, 1, 0, 0, 0);
                            DateTime orderTime = DateTime.ParseExact(o.time_completed.Replace('T',' ').TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                            
                            //Makes it easier for keying the revenue map by WEEK
                            orderTime = orderTime.AddDays(-(int)orderTime.DayOfWeek);
                            orderTime = new DateTime(orderTime.Year, orderTime.Month, orderTime.Day, 0, 0, 0);

                            //only added menuItems from orders for the current month
                            if (DateTime.Compare(monthStart, orderTime ) == 0 || DateTime.Compare(monthStart, orderTime) < 0) {
                                //adding a key and setting it to 0 if it doesn't exist
                                try
                                {
                                    if (revenueCalendar[orderTime] == 0)
                                    {
                                    }
                                }
                                catch
                                {
                                    revenueCalendar[orderTime] = 0;
                                }
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + oi.price;  //adding price of new menuitem 
                                }
                            }
                        }

                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            menuItemCounter[id] = menuItemCounter[id] + 1;
                        }
                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the MONTH
                        uxMonthlyViewGrid.Visibility = Visibility.Visible;
                        break;

                    //WEEKLY VIEW
                    case "Current Weekly View":
                        foreach (Order o in RealmManager.All<OrderList>().FirstOrDefault().orders)
                        {
                            //this will ignore all uncompleted orders
                            if (o.time_completed == null)
                            {
                                continue;
                            }

                            //initalize this month and last month
                            DateTime td = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); //sets td to the beginning of the week
                            DateTime weekStart = new DateTime(td.Year, td.Month, td.Day, 0, 0, 0);
                            DateTime orderTime = DateTime.ParseExact(o.time_completed.Replace('T', ' ').TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff",  System.Globalization.CultureInfo.InvariantCulture);

                            //Makes it easier for keying the revenue map by DAY 
                            orderTime = new DateTime(orderTime.Year, orderTime.Month, orderTime.Day, 0, 0, 0);

                            //only added menuItems from orders for the current week
                            if (DateTime.Compare(weekStart, orderTime) < 0)
                            {
                                //adding a key and setting it to 0 if it doesn't exist
                                try
                                {
                                    if (revenueCalendar[orderTime] == 0)
                                    {
                                    }
                                }
                                catch
                                {
                                    revenueCalendar[orderTime] = 0;
                                }
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + oi.price;  //adding price of new menuitem 
                                }
                            }

                        }
                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            menuItemCounter[id] = menuItemCounter[id] + 1;
                        }

                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the WEEK
                        //UxWeeklyCharts();
                        uxWeeklyViewGrid.Visibility = Visibility.Visible;
                        break;

                    //YEARLY VIEW
                    case "Current Yearly View":
                        foreach (Order o in RealmManager.All<OrderList>().FirstOrDefault().orders)
                        {
                            //this will ignore all uncompleted orders
                            if (o.time_completed == null)
                            {
                                continue;
                            }

                            //initalize this month and last month
                            DateTime td = DateTime.Today;
                            DateTime weekStart = new DateTime(td.Year, 1, 1, 0, 0, 0);
                            DateTime orderTime = DateTime.ParseExact(o.time_completed.Replace('T', ' ').TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture); ;

                            //makeing it easier to key the revenue map by MONTH
                            orderTime = new DateTime(orderTime.Year, orderTime.Month, 1, 0, 0, 0);

                            

                            //only added menuItems from orders for the current week
                            if (DateTime.Compare(weekStart, orderTime) < 0)
                            {
                                //adding a key and setting it to 0 if it doesn't exist
                                try
                                {
                                    if (revenueCalendar[orderTime] == 0)
                                    {
                                    }
                                }
                                catch
                                {
                                    revenueCalendar[orderTime] = 0;
                                }
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + oi.price;  //adding price of new menuitem 
                                }
                            }

                        }
                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            menuItemCounter[id] = menuItemCounter[id] + 1;
                        }
                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the YEAR
                        uxYearlyViewGrid.Visibility = Visibility.Visible;
                        break;
                }
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

        public void UxWeeklyCharts(Dictionary<string, int> menuItemCount, Dictionary<DateTime, int> RevenueCalendar)
        {

            SfChart chart = new SfChart();

            //Adding horizontal axis to the chart 

            CategoryAxis primaryAxis = new CategoryAxis();

            primaryAxis.Header = "Name";

            chart.PrimaryAxis = primaryAxis;


            //Adding vertical axis to the chart 

            NumericalAxis secondaryAxis = new NumericalAxis();

            secondaryAxis.Header = "Height(in cm)";

            chart.SecondaryAxis = secondaryAxis;


            //Initialize the two series for SfChart
            ColumnSeries series = new ColumnSeries();

            series.ItemsSource = (new ViewModel()).Data;
            series.XBindingPath = "Name";
            series.YBindingPath = "Height";

            //Adding Series to the Chart Series Collection
            chart.Series.Add(series);
        }

        public void UxMonthlyCharts()
        {

        }

        public void UxYearlyCharts()
        {

        }

        public void UxAllTimeCharts()
        {

        }


        //populating the monthly view popup
        public async void RefreshMonthlyView()
        {
            RealmManager.RemoveAll<OrderList>();
            await GetEmployeeListRequest.SendGetEmployeeListRequest();
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