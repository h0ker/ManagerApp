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
        public Employee SelectedEmployee = new Employee();
        public DispatcherTimer timer = new DispatcherTimer();
        public HomeScreen()
        {
            this.InitializeComponent();
            uxLogoutButton.Click += UxLogoutButton_Clicked;
            uxInventoryButton.Click += UxInventoryButton_Clicked;
            uxCouponButton.Click += UxCouponButton_Clicked;
            uxEmployeeButton.Click += UxEmployeeButton_Clicked;
            uxMenuEditButton.Click += UxMenuEditButton_Clicked;
            uxFeedbackButton.Click += UxFeedbackButton_Click;
            uxPromosButton.Click += UxPromoButton_Clicked;
            uxEmployeeListView.ItemClick += UxEmployeeListView_ItemClick;
            uxClockInButton.Click += UxClockInButton_Click;
            uxClockOutButton.Click += UxClockOutButton_Click;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateHotItem();

            RefreshEmployeeList();
        }

        private async void UpdateHotItem()
        {
            RealmManager.RemoveAll<OrderList>();
            RealmManager.RemoveAll<MenuItemList>();
            List<MenuItem> somelist = new List<MenuItem>();
            //finding each distinct category and adding it 
            await GetOrdersRequest.SendGetOrdersRequest();
            await GetMenuItemsRequest.SendGetMenuItemsRequest();

            //creating a list of every menu item id for each order including duplicates
            List<OrderItem> menuItemIds = new List<OrderItem>();
            //creating a dictionary to keep track of the count of each menuItem
            Dictionary<String, Dictionary<MenuItem, int>> menuItemCounter = new Dictionary<String, Dictionary<MenuItem, int>>();

            foreach (Order o in RealmManager.All<OrderList>().FirstOrDefault().orders)
            {
                //this will ignore all uncompleted orders
                if (o.time_completed == null)
                {
                    continue;
                }

                //initalize this week and last week
                DateTime td = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); //sets td to the beginning of the week
                DateTime lastWeekStart = new DateTime(td.Year, td.Month, td.Day, 0, 0, 0).AddDays(-7);
                DateTime orderTime = DateTime.ParseExact(o.time_completed.Replace('T', ' ').TrimEnd('Z'), "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);

                //Makes it easier for keying the revenue map by WEEK
                orderTime = orderTime.AddDays(-(int)orderTime.DayOfWeek);
                orderTime = new DateTime(orderTime.Year, orderTime.Month, orderTime.Day, 0, 0, 0);

                //only added menuItems from orders for the current month
                if (DateTime.Compare(lastWeekStart, orderTime) == 0)
                {
                    foreach (OrderItem oi in o.menuItems)
                    {
                        menuItemIds.Add(oi); //add next menuitem id
                    }
                }
            }

            List<MenuItem> tempList = RealmManager.All<MenuItem>().ToList();
            List<MenuItem> tempMList = RealmManager.All<MenuItemList>().FirstOrDefault().menuItems.ToList();

            //updating menuItem map to see how often each was ordered
            foreach (OrderItem o in menuItemIds)
            {
                MenuItem tempMenuItem = tempList.Find(x => x._id == o._id);
                if (tempMenuItem == null)
                {
                    continue;
                }
                if (menuItemCounter.ContainsKey(tempMenuItem.category))
                {
                    try
                    {
                        menuItemCounter[tempMenuItem.category][tempMenuItem] = menuItemCounter[tempMenuItem.category][tempMenuItem] + 1;
                    }
                    catch
                    {
                        menuItemCounter[tempMenuItem.category].Add(tempMenuItem, 1);
                    }
                }
                else
                {
                    menuItemCounter[tempMenuItem.category] = new Dictionary<MenuItem, int> { { tempMenuItem, 1 } };
                }
            }
            foreach(string key in menuItemCounter.Keys)
            {
                KeyValuePair<MenuItem,int> topMenuItem;
                topMenuItem = menuItemCounter[key].Aggregate((x, y) => x.Value > y.Value ? x : y);
                HotItem tempItem = RealmManager.Find<HotItem>(topMenuItem.Key.category);
                
                //if hotitem is in realm yet
                if(tempItem == null)
                {
                    HotItem tempHotItem = new HotItem();
                    tempHotItem.category = topMenuItem.Key.category;
                    tempHotItem.createdAt = DateTime.Today.ToString();
                    tempHotItem._id = topMenuItem.Key._id;


                    RealmManager.AddOrUpdate<HotItem>(tempHotItem);
                } 
                else
                {
                    if(tempItem._id != topMenuItem.Key._id)
                    {
                        HotItem tempHotItem = new HotItem();
                        tempHotItem.category = topMenuItem.Key.category;
                        tempHotItem.createdAt = DateTime.Today.ToString();
                        tempHotItem._id = topMenuItem.Key._id;

                        RealmManager.AddOrUpdate<HotItem>(tempHotItem);
                    }
                }
            }
            if (1 == 1) {
                
            }
        }

        private void UxFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Feedback), null);
        }

        private void UxPromoButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PromosPage), null);
        }

        private async void UxClockOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee.current_shift == null)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Not Clocked In",
                    Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " has yet to be clocked in",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else
            {
                var validClockOut = await ClockOut.SendClockOutRequest(SelectedEmployee.current_shift);
                if (validClockOut)
                {
                    var validResetShift = await ResetEmployeeShift.SendResetEmployeeShift(SelectedEmployee._id);
                    if (validResetShift)
                    {
                        ContentDialog responseAlert = new ContentDialog
                        {
                            Title = "Successful",
                            Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " has been clocked out",
                            CloseButtonText = "Ok"
                        };
                        ContentDialogResult result = await responseAlert.ShowAsync();
                    }
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Unsuccessful",
                        Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " has not been clocked out",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                RefreshEmployeeList();
                uxTimePopup.IsOpen = false;
            }
        }

        private async void UxClockInButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEmployee.current_shift != null)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Already Clocked In",
                    Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " is already clocked in",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else
            {
                var validClockIn = await ClockIn.SendClockInRequest(SelectedEmployee._id);
                if (validClockIn)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Successful",
                        Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " has been clocked in",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Unsuccessful",
                        Content = SelectedEmployee.first_name + " " + SelectedEmployee.last_name + " has not been clocked in",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                RefreshEmployeeList();
                uxTimePopup.IsOpen = false;
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            secondHand.Angle = DateTime.Now.Second * 6;  
            minuteHand.Angle = DateTime.Now.Minute * 6;  
            hourHand.Angle = (DateTime.Now.Hour * 30);
            lblDigitalClock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private async void UxEmployeeListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxTimePopup.IsOpen = true;
            SelectedEmployee = (Employee)e.ClickedItem;

            var validShiftRequest = await GetAllShiftsRequest.SendGetAllShiftsRequest(SelectedEmployee._id);
            uxShiftListView.ItemsSource = RealmManager.All<ShiftList>().FirstOrDefault().shifts.ToList();
        }

        public async void RefreshEmployeeList()
        {
            RealmManager.RemoveAll<EmployeeList>();
            RealmManager.RemoveAll<Employee>();
            await GetEmployeeListRequest.SendGetEmployeeListRequest();
            uxEmployeeListView.ItemsSource = RealmManager.All<EmployeeList>().FirstOrDefault().employees.ToList();
            uxStatisticsButton.Click += uxStatisticsButton_Clicked;
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
