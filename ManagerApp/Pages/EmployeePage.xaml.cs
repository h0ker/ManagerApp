using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
            uxTipPopupButton.Click += UxTipPopupButton_Click;
            uxCompPopupButton.Click += UxCompPopupButton_Click;
            uxCloseTipComp.Click += UxCloseTipComp_Click;
            uxProcessTips.Click += UxProcessTips_Click;
            uxClearComps.Click += UxClearComps_Click;
            uxUpdateEmployeeButton.Click += UxUpdateEmployeeButton_Click;

            RefreshEmployeeList();
        }

        private async void UxUpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(uxUsernameEntry.Text)||String.IsNullOrEmpty(uxWageBox.Text)||String.IsNullOrEmpty(uxFirstNameEntry.Text)||String.IsNullOrEmpty(uxLastNameEntry.Text)||String.IsNullOrEmpty(uxPositionComboBox.SelectedItem.ToString()))
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Required fields are missing",
                    Content = "Please make sure that all fields are filled out",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else
            {
                var validUpdate = await UpdateEmployeeRequest.SendUpdateEmployeeRequest(selectedEmployee._id, uxWageBox.Text.ToString(), uxFirstNameEntry.Text, uxLastNameEntry.Text, uxUsernameEntry.Text, uxPositionComboBox.SelectedItem.ToString());
                if(validUpdate)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Update Successful",
                        Content = "Employee information was updated",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Update Unsuccessful",
                        Content = "Employee information was updated",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
            }
            RefreshEmployeeList();
            uxEmployeeMenuPopup.IsOpen = false;
        }

        private async void UxClearComps_Click(object sender, RoutedEventArgs e)
        {
            foreach(Comp comp in RealmManager.All<CompList>().FirstOrDefault().comp)
            {
                await DeleteComp.SendDeleteComp(comp._id); 
            }

            var validGetComps = await GetAllComps.SendGetAllCompsEmployee(selectedEmployee._id);
            uxCompListView.ItemsSource = await SyncCompData(RealmManager.All<CompList>().FirstOrDefault().comp);
        }

        private async void UxProcessTips_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            foreach(Tip tip in uxTipListView.Items)
            {
                success = await DeleteTipRequest.SendDeleteTipRequest(tip._id);
            }
            if(success)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Processed",
                    Content = "Employee tips have been processed",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
                var validGetTips = await GetEmployeeTips.SendGetEmployeeTips(selectedEmployee._id);
                uxTipListView.ItemsSource = RealmManager.All<Tips>().FirstOrDefault().tip;
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Unsuccessful",
                    Content = "Tip processing was unsuccessful",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
        }

        private async Task<List<CompDisplay>> SyncCompData(IList<Comp> comps)
        {
            List<CompDisplay> compDisplays = new List<CompDisplay>();
            if(comps.Count == 0)
            {
                return compDisplays;
            }
            var validGetMenuItems = await GetMenuItemsRequest.SendGetMenuItemsRequest();
            foreach(Comp comp in comps)
            {
                CompDisplay compDisplay = new CompDisplay();
                MenuItem menuItem = RealmManager.Find<MenuItem>(comp.menuItem_id);
                compDisplay.MenuItemName = menuItem.name;
                compDisplay.Price = menuItem.price;
                compDisplay.Reason = comp.reason;
                compDisplays.Add(compDisplay);
            }
            return compDisplays;
        }

        private void UxCloseTipComp_Click(object sender, RoutedEventArgs e)
        {
            uxTipCompPopup.IsOpen = false;
            uxTipListView.Visibility = Visibility.Collapsed;
            uxCompListView.Visibility = Visibility.Collapsed;
            uxProcessTips.Visibility = Visibility.Collapsed;
            uxClearComps.Visibility = Visibility.Collapsed;
        }

        private async void UxCompPopupButton_Click(object sender, RoutedEventArgs e)
        {
            uxTipCompPopup.IsOpen = true;
            uxClearComps.Visibility = Visibility.Visible;
            uxCompListView.Visibility = Visibility.Visible;
            var validGetComps = await GetAllComps.SendGetAllCompsEmployee(selectedEmployee._id);
            uxCompListView.ItemsSource = await SyncCompData(RealmManager.All<CompList>().FirstOrDefault().comp);
        }

        private async void UxTipPopupButton_Click(object sender, RoutedEventArgs e)
        {
            uxTipCompPopup.IsOpen = true;
            uxTipListView.Visibility = Visibility.Visible;
            uxProcessTips.Visibility = Visibility.Visible;
            var validGetTips = await GetEmployeeTips.SendGetEmployeeTips(selectedEmployee._id);
            uxTipListView.ItemsSource = RealmManager.All<Tips>().FirstOrDefault().tip;
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
                var validAddEmployeeRequest = await AddEmployeeRequest.SendAddEmployeeRequest(uxFirstNameEntry.Text, uxLastNameEntry.Text, uxUsernameEntry.Text, uxPasswordEntry.Password, Convert.ToInt32(uxPositionComboBox.SelectedItem), Convert.ToDouble(uxWageBox.Text));
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
            uxCompStack.Visibility = Visibility.Collapsed;
            uxTipStack.Visibility = Visibility.Collapsed;
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
            uxPositionComboBox.SelectedItem = selectedEmployee.position.ToString();
            uxPositionComboBox.Text = selectedEmployee.position.ToString();
            uxFirstNameEntry.Text = selectedEmployee.first_name;
            uxLastNameEntry.Text = selectedEmployee.last_name;
            uxUsernameEntry.Text = selectedEmployee.username;
            uxWageBox.Text = selectedEmployee.pay.ToString();
            uxTipStack.Visibility = Visibility.Visible;
            uxCompStack.Visibility = Visibility.Visible;
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
            await GetMenuItemsRequest.SendGetMenuItemsRequest();
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
