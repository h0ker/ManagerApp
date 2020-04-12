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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeePage : Page
    {
        public EmployeePage()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxEmployeeListView.ItemClick += UxEmployeeListView_Clicked;

            RefreshEmployeeList();
        }

        private void UxEmployeeListView_Clicked(object sender, ItemClickEventArgs e)
        {
            uxEmployeeMenuPopup.IsOpen = true;
            Employee employee = (Employee)e.ClickedItem;
            uxFirstNameEntry.Text = employee.first_name;
            uxLastNameEntry.Text = employee.last_name;
            uxUsernameEntry.Text = employee.username;
            uxPasswordEntry.Password = employee.password;
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
