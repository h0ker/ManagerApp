using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ManagerApp.Models.ServiceRequests;
using System;
using ManagerApp.Models;
using System.Linq;

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
            //excutes if the GET order request returns back okay
            if (await GetOrdersRequest.SendGetOrdersRequest())
            {
                switch (viewSelection)
                {
                    case "Monthly View":
                        var temp = RealmManager.All<OrderList>();
                        uxMonthlyViewGrid.Visibility = Visibility.Visible;
                        break;
                    case "Weekly View":
                        uxWeeklyViewGrid.Visibility = Visibility.Visible;
                        break;
                    case "Yearly View":
                        uxYearlyViewGrid.Visibility = Visibility.Visible;
                        break;
                }
                //uxMonthlyViewGridView.ItemsSource = RealmManager.All<OrderList>().FirstOrDefault().orders.ToList();
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Successful",
                    Content = "Employee has been added to the database successfully",
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