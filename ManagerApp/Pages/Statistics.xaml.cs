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

            
            UxAllTimeCharts();

        }

        public Boolean monthlyDisplayed = false;
        public Boolean weeklyDisplayed = false;
        public Boolean yearlyDisplayed = false;


        //WEEKLY VIEW CHART CLASSES
        //revenue classes
        public class UxWeeklyChartDataModel
        {
            public string WeekDay { get; set; }

            public int Count { get; set; }
        }

        public class UxWeeklyChartViewModel
        {
            public List<UxWeeklyChartDataModel> Data { get; set; }

            public UxWeeklyChartViewModel(Dictionary<DateTime, int> revenueCalendar)
            {
                Data = new List<UxWeeklyChartDataModel>();
                foreach (DateTime weekday in revenueCalendar.Keys)
                {
                    UxWeeklyChartDataModel temp = new UxWeeklyChartDataModel();
                    temp.WeekDay = weekday.DayOfWeek.ToString();
                    temp.Count = revenueCalendar[weekday];
                    Data.Add(temp);
                }
            }
        }

        //MONTHLY VIEW CHART CLASSES
        //revenue classes
        public class UxMonthlyChartDataModel
        {
            public string WeekDate { get; set; }

            public int Count { get; set; }
        }

        public class UxMonthlyChartViewModel
        {
            public List<UxMonthlyChartDataModel> Data { get; set; }

            public UxMonthlyChartViewModel(Dictionary<DateTime, int> revenueCalendar)
            {
                Data = new List<UxMonthlyChartDataModel>();
                foreach (DateTime weekday in revenueCalendar.Keys)
                {
                    UxMonthlyChartDataModel temp = new UxMonthlyChartDataModel();
                    temp.WeekDate = weekday.ToString("d").TrimEnd();
                    temp.Count = revenueCalendar[weekday];
                    Data.Add(temp);
                }
            }
        }

        //YEARLY VIEW CHART CLASSES
        //revenue classes
        public class UxYearlyChartDataModel
        {
            public string Month { get; set; }

            public int Count { get; set; }
        }

        public class UxYearlyChartViewModel
        {
            public List<UxYearlyChartDataModel> Data { get; set; }

            public UxYearlyChartViewModel(Dictionary<DateTime, int> revenueCalendar)
            {
                Data = new List<UxYearlyChartDataModel>();
                foreach (DateTime weekday in revenueCalendar.Keys)
                {
                    UxYearlyChartDataModel temp = new UxYearlyChartDataModel();
                    temp.Month = weekday.Month.ToString();
                    temp.Count = revenueCalendar[weekday];
                    Data.Add(temp);
                }
            }
        }


        //Creating the datatype and list of datatype for the top right list view
        public class MenuItemPerformanceData
        {
            public string Name { get; set; }
            public int Count { get; set; }
        }

        public class UxListViewModel
        {
            public List<MenuItemPerformanceData> Data { get; set; }
            public UxListViewModel(Dictionary<string, int> menuItemCount)
            {
                Data = new List<MenuItemPerformanceData>();

                //going through each menuItem and adding the count and object to the data property.
                foreach (MenuItem m in RealmManager.All<MenuItemList>().FirstOrDefault().menuItems)
                {
                    //creating tempary MenuItemPerformanceData with information passed and stored in xaml
                    MenuItemPerformanceData temp = new MenuItemPerformanceData
                    {
                        Name = m.name,
                        Count = menuItemCount[m._id]
                    };
                    
                    //adding temp to the list
                    Data.Add(temp);
                }
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
                Dictionary<DateTime, int> revenueCalendar = new Dictionary<DateTime, int>();

                //creating a list of every menu item id for each order including duplicates
                List<string> menuItemIds = new List<string>();

                //String that will store the most popular item id
                string mostPopularMenuItemId;

                //Will store a date and the number of orders
                Dictionary<DateTime, int> orderCount = new Dictionary<DateTime, int>();

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
                                    orderCount[orderTime] = 0;
                                    revenueCalendar[orderTime] = 0;
                                }
                                //incrementing order count every order
                                orderCount[orderTime] = orderCount[orderTime] + 1;
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + Convert.ToInt32(oi.price);  //adding price of new menuitem 
                                }
                            }
                        }

                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            try
                            {
                                menuItemCounter[id] = menuItemCounter[id] + 1;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        //finding the largest value and storing the key
                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the MONTH

                        //this will only generate the charts once. After that the values you have been bound.
                        if (monthlyDisplayed == false)
                        {
                            UxMonthlyCharts(menuItemCounter, revenueCalendar, orderCount);
                            monthlyDisplayed = true;
                        }
                        uxMonthlyViewGrid.Visibility = Visibility.Visible;
                        uxWeeklyViewGrid.Visibility = Visibility.Collapsed;
                        uxYearlyViewGrid.Visibility = Visibility.Collapsed;
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
                                    if (revenueCalendar[orderTime] == 0 || orderCount[orderTime] == 0)
                                    {
                                    }
                                }
                                catch
                                {
                                    //initalizing each key of orderTime to zero
                                    revenueCalendar[orderTime] = 0;
                                    orderCount[orderTime] = 0;
                                }
                                //incrementing order count every order
                                orderCount[orderTime] = orderCount[orderTime] + 1;

                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + Convert.ToInt32(oi.price);  //adding price of new menuitem 
                                }
                            }

                        }
                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            try
                            {
                                menuItemCounter[id] = menuItemCounter[id] + 1;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        //finding the largest value and storing the key
                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the WEEK

                        //this will only generate the charts once. After that the values you have been bound.
                        if (weeklyDisplayed == false)
                        {
                            UxWeeklyCharts(menuItemCounter, revenueCalendar, orderCount);
                            weeklyDisplayed = true;
                        }

                        //makeing the other grids hidden
                        uxMonthlyViewGrid.Visibility = Visibility.Collapsed;
                        uxWeeklyViewGrid.Visibility = Visibility.Visible;
                        uxYearlyViewGrid.Visibility = Visibility.Collapsed;
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
                                    orderCount[orderTime] = 0;
                                    revenueCalendar[orderTime] = 0;
                                }
                                //incrementing order count every order
                                orderCount[orderTime] = orderCount[orderTime] + 1;
                                foreach (OrderItem oi in o.menuItems)
                                {
                                    menuItemIds.Add(oi._id); //add next menuitem id
                                    revenueCalendar[orderTime] = revenueCalendar[orderTime] + Convert.ToInt32(oi.price);  //adding price of new menuitem 
                                }
                            }

                        }
                        //updating menuItem map to see how often each was ordered
                        foreach (string id in menuItemIds)
                        {
                            try
                            {
                                menuItemCounter[id] = menuItemCounter[id] + 1;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        //finding the largest value and storing the key
                        mostPopularMenuItemId = menuItemCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //Getting the most popular menuItem of the YEAR

                        //this will only generate the charts once. After that the values you have been bound.
                        if  (yearlyDisplayed == false)
                        {
                            UxYearlyCharts(menuItemCounter, revenueCalendar, orderCount);
                            yearlyDisplayed = true;
                        }

                        //makeing the other grids hidden
                        uxMonthlyViewGrid.Visibility = Visibility.Collapsed;
                        uxWeeklyViewGrid.Visibility = Visibility.Collapsed;
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

        public void UxWeeklyCharts(Dictionary<string, int> menuItemCount, Dictionary<DateTime, int> revenueCalendar, Dictionary<DateTime, int> orderCount)
        {


            //Initialize the two series for SfChart
            ColumnSeries UxWeeklyRevenueData = new ColumnSeries();

            UxWeeklyRevenueData.ItemsSource = (new UxWeeklyChartViewModel(revenueCalendar)).Data;
            UxWeeklyRevenueData.XBindingPath = "WeekDay";
            UxWeeklyRevenueData.YBindingPath = "Count";

            //Adding Series to the revenue Series Collection
            UxWeeklyRevenueChart.Series.Add(UxWeeklyRevenueData);

            //Setting up and binding chart information weekly view
            ColumnSeries UxWeeklyOrderData = new ColumnSeries();

            UxWeeklyOrderData.ItemsSource = (new UxWeeklyChartViewModel(orderCount)).Data;
            UxWeeklyOrderData.XBindingPath = "WeekDay";
            UxWeeklyOrderData.YBindingPath = "Count";

            //Adding Series to the order count Series Collection
            UxWeeklyOrderChart.Series.Add(UxWeeklyOrderData);

            //populating the listview of menuItems
            WeeklyMenuItemPerformance.ItemsSource = (new UxListViewModel(menuItemCount)).Data.OrderByDescending(x => x.Count).ToList();
        }
        //MONTHLY CHART 
        public void UxMonthlyCharts(Dictionary<string, int> menuItemCount, Dictionary<DateTime, int> revenueCalendar, Dictionary<DateTime, int> orderCount)
        {
            //Initialize the two series for SfChart
            ColumnSeries UxMonthlyRevenueData = new ColumnSeries();

            UxMonthlyRevenueData.ItemsSource = (new UxMonthlyChartViewModel(revenueCalendar)).Data;
            UxMonthlyRevenueData.XBindingPath = "WeekDate";
            UxMonthlyRevenueData.YBindingPath = "Count";

            //Adding Series to the revenue Series Collection
            UxMonthlyRevenueChart.Series.Add(UxMonthlyRevenueData);

            //Setting up and binding chart information weekly view
            ColumnSeries UxMonthlyOrderData = new ColumnSeries();

            UxMonthlyOrderData.ItemsSource = (new UxMonthlyChartViewModel(orderCount)).Data;
            UxMonthlyOrderData.XBindingPath = "WeekDate";
            UxMonthlyOrderData.YBindingPath = "Count";

            //Adding Series to the order count Series Collection
            UxMonthlyOrderChart.Series.Add(UxMonthlyOrderData);

            //populating the listview of menuItems
            MonthlyMenuItemPerformance.ItemsSource = (new UxListViewModel(menuItemCount)).Data.OrderByDescending(x => x.Count).ToList();
        }

        public void UxYearlyCharts(Dictionary<string, int> menuItemCount, Dictionary<DateTime, int> revenueCalendar, Dictionary<DateTime, int> orderCount)
        {
            //Initialize the two series for SfChart
            ColumnSeries UxYearlyRevenueData = new ColumnSeries();

            UxYearlyRevenueData.ItemsSource = (new UxYearlyChartViewModel(revenueCalendar)).Data;
            UxYearlyRevenueData.XBindingPath = "Month";
            UxYearlyRevenueData.YBindingPath = "Count";

            //Adding Series to the revenue Series Collection
            UxYearlyRevenueChart.Series.Add(UxYearlyRevenueData);

            //Setting up and binding chart information weekly view
            ColumnSeries UxYearlyOrderData = new ColumnSeries();

            UxYearlyOrderData.ItemsSource = (new UxYearlyChartViewModel(orderCount)).Data;
            UxYearlyOrderData.XBindingPath = "Month";
            UxYearlyOrderData.YBindingPath = "Count";

            //Adding Series to the order count Series Collection
            UxYearlyOrderChart.Series.Add(UxYearlyOrderData);

            //populating the listview of menuItems
            YearlyMenuItemPerformance.ItemsSource = (new UxListViewModel(menuItemCount)).Data.OrderByDescending(x => x.Count).ToList();
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