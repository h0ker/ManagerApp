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
            
            switch (viewSelection)
            {
                case "Monthly View":
                    RealmManager.RemoveAll<OrderList>();
                    if (await GetOrdersRequest.SendGetOrdersRequest())
                    {
                        uxMonthlyViewPopup.IsOpen = true;
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

                    break;
                case "Weekly View":
                    //color = Colors.Green;
                    break;
                case "Yearly View":
                    //color = Colors.Blue;
                    break;
            }
            //colorRectangle.Fill = new SolidColorBrush(color);
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