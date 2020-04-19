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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ManagerApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Coupons : Page
    {

        public Coupons()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxBackground1.Click += UxBackground1_Clicked;
            uxBackground2.Click += UxBackground2_Clicked;
            uxBackground3.Click += UxBackground3_Clicked;
            uxDiscountButton.Click += uxDiscountButton_Clicked;
            uxSubmitButton.Click += MakeNewPromoButton;
            uxCancelDiscountButton.Click += uxCancelDiscountButton_Clicked;
            uxPrintButton.Click += uxPrintButton_Clicked;
        }

        private void UxBackground1_Clicked(object sender, RoutedEventArgs e)
        {
            lblCouponImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/managerLogin.png"));
        }

        private void UxBackground2_Clicked(object sender, RoutedEventArgs e)
        {
            lblCouponImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/birthdayCoupon.png"));
        }

        private void UxBackground3_Clicked(object sender, RoutedEventArgs e)
        {

            lblCouponImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/americaCoupon.png"));
        }

        private void uxDiscountButton_Clicked(object sender, RoutedEventArgs e)
        {
            _ = RefreshMenuItemLists();
            uxDiscountPopup.IsOpen = true;
        }
        
        private async void uxCancelDiscountButton_Clicked(object send, RoutedEventArgs e)
        {
            uxDiscountPopup.IsOpen = false;
        }

        private async void uxPrintButton_Clicked(object send, RoutedEventArgs e)
        {
            if (uxDiscountPopup.IsOpen)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Close discount box",
                    Content = "Please close the discount specification popup before continuing",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else // Discount pop-up is closed
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Sent to printer",
                    Content = "Coupon was sent to printer.",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            
        }

        private async void MakeNewPromoButton(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(uxDescriptionBox.Text) || uxRequiredItemsList.SelectedItems.Count == 0 || uxAppliedItemsList.SelectedItems.Count == 0 || String.IsNullOrEmpty(uxPercentageBox.Text.ToString()))
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

                var validMakeNewPromo = await MakeNewPromo.SendMakeNewPromo("Customer", uxDescriptionBox.Text, required, applied, uxPercentageBox.Text.ToString(), uxActiveChoice.IsEnabled.ToString(), uxRepeatableChoice.IsEnabled.ToString());

                if (validMakeNewPromo != null)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Successful",
                        Content = "Promotion was successfully added to the database",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();

                    var writer = new ZXing.BarcodeWriter();

                    writer.Format = ZXing.BarcodeFormat.QR_CODE;

                    writer.Options = new ZXing.Common.EncodingOptions() { Height = 300, Width = 300, PureBarcode = true };

                    QRCode.Source = writer.Write(validMakeNewPromo);

                    uxDiscountPopup.IsOpen = false;
                    uxDiscountButton.Visibility = Visibility.Collapsed; // Remove option to bring back this screen
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
