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
    public sealed partial class EmployeePage : Page
    {
        Employee selectedEmployee { get; set; }
        public EmployeePage()
        {
            this.InitializeComponent();

            //button wiring
            uxBackButton.Click += UxBackButton_Clicked;
            uxEmployeeListView.ItemClick += UxEmployeeListView_Clicked;
            uxAddEmployeeButton.Click += UxAddEmployeeButton_Clicked;
            uxAddEmployeeServiceRequestButton.Click += UxAddEmployeeServiceRequestButton_Clicked;
            uxDeleteEmployeeButton.Click += UxDeleteEmployeeButton_Clicked;
            uxUpdateEmployeeButton.Click += UxUpdateEmployeeButton_Clicked;

            RefreshEmployeeList();
        }

        private async void UxUpdateEmployeeButton_Clicked(object sender, RoutedEventArgs e)
        {
            var validUpdateEmployeeRequest = await UpdateEmployeeRequest.SendUpdateEmployeeRequest(selectedEmployee._id, selectedEmployee.position, selectedEmployee.pay);
        }

        private async void UxDeleteEmployeeButton_Clicked(object sender, RoutedEventArgs e)
        {
            var validDeleteEmployeeRequest = await DeleteEmployeeRequest.SendDeleteEmployeeRequest(selectedEmployee._id);
            if(validDeleteEmployeeRequest)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Successful",
                    Content = "Employee has been deleted from the database successfully",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Unsuccessful",
                    Content = "Employee has NOT been deleted from the database successfully",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            RefreshEmployeeList();
        }

        private async void UxAddEmployeeServiceRequestButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(uxPasswordEntry.Password == uxPasswordReentry.Password)
            {
                var validAddEmployeeRequest = await AddEmployeeRequest.SendAddEmployeeRequest(uxFirstNameEntry.Text, uxLastNameEntry.Text, uxUsernameEntry.Text, uxPasswordEntry.Password, 1);
                if(validAddEmployeeRequest)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Successful",
                        Content = "Employee has been added to the database successfully",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Unsuccessful",
                        Content = "Employee was not created successfully",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Password do not match",
                    Content = "Please try entering your password again",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            RefreshEmployeeList();
        }

        private void UxAddEmployeeButton_Clicked(object sender, RoutedEventArgs e)
        {
            uxEmployeeMenuPopup.IsOpen = true;
            uxFirstNameEntry.Text = String.Empty;
            uxLastNameEntry.Text = String.Empty;
            uxUsernameEntry.Text = String.Empty;
            uxPasswordEntry.Password = String.Empty;
            uxUpdateEmployeeButton.Visibility = Visibility.Collapsed;
            uxDeleteEmployeeButton.Visibility = Visibility.Collapsed;
            uxAddEmployeeServiceRequestButton.Visibility = Visibility.Visible;
            uxPasswordReentryStack.Visibility = Visibility.Visible;
            uxPasswordEntryStack.Visibility = Visibility.Visible;
        }

        private void UxEmployeeListView_Clicked(object sender, ItemClickEventArgs e)
        {
            uxEmployeeMenuPopup.IsOpen = true;
            selectedEmployee = (Employee)e.ClickedItem;
            uxFirstNameEntry.Text = selectedEmployee.first_name;
            uxLastNameEntry.Text = selectedEmployee.last_name;
            uxUsernameEntry.Text = selectedEmployee.username;
            uxPasswordEntry.Password = selectedEmployee.password;
            uxUpdateEmployeeButton.Visibility = Visibility.Visible;
            uxDeleteEmployeeButton.Visibility = Visibility.Visible;
            uxAddEmployeeServiceRequestButton.Visibility = Visibility.Collapsed;
            uxPasswordReentryStack.Visibility = Visibility.Collapsed;
            uxPasswordEntryStack.Visibility = Visibility.Collapsed;
        }

        public async void RefreshEmployeeList()
        {
            RealmManager.RemoveAll<EmployeeList>();
            RealmManager.RemoveAll<Employee>();
            await GetEmployeeListRequest.SendGetEmployeeListRequest();
            uxEmployeeListView.ItemsSource = RealmManager.All<EmployeeList>().FirstOrDefault().employees.ToList();
        }

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
