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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPromo : Page
    {
        public NewPromo()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxSubmitButton.Click += MakeNewPromoButton;

            _ = RefreshMenuItemLists();
        }

        private async void MakeNewPromoButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(uxTypeCombo.Text.ToString()) || String.IsNullOrEmpty(uxDescriptionBox.Text) || uxRequiredItemsList.SelectedItems.Count == 0 || uxAppliedItemsList.SelectedItems.Count == 0 || String.IsNullOrEmpty(uxPercentageBox.Text.ToString()))
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
                List<MenuItem> required = new List<MenuItem>();
                foreach (object o in uxRequiredItemsList.SelectedItems)
                {
                    required.Add((MenuItem)o);
                }

                List<MenuItem> applied = new List<MenuItem>();
                foreach (object o in uxAppliedItemsList.SelectedItems)
                {
                    applied.Add((MenuItem)o);
                }

                var validMakeNewPromo = await MakeNewPromo.SendMakeNewPromo(uxTypeCombo.SelectedItem.ToString(), uxDescriptionBox.Text, required, applied, uxPercentageBox.Text.ToString(), uxActiveChoice.IsEnabled.ToString(), uxRepeatableChoice.IsEnabled.ToString());

                if (validMakeNewPromo)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Successful",
                        Content = "Promotion was successfully added to the database",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Unsuccessful",
                        Content = "Promotion was not successfully added to the database",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
            }
        }

        public async Task RefreshMenuItemLists()
        {
            var validMenuItemList = await GetMenuItemsRequest.SendGetMenuItemsRequest();
            var menuItemsList = RealmManager.All<MenuItemList>().FirstOrDefault().menuItems;
            uxAppliedItemsList.ItemsSource = menuItemsList;
            uxRequiredItemsList.ItemsSource = menuItemsList;
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
