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
    public sealed partial class PromosPage : Page
    {
        Coupon SelectedCoupon = new Coupon();
        public PromosPage()
        {
            this.InitializeComponent();

            uxPromotionListView.ItemClick += UxPromotionListView_ItemClick;
            uxSetActiveButton.Click += UxSetActiveButton_Click;
            uxSetInactiveButton.Click += UxSetInactiveButton_Click;
            uxDeleteCouponButton.Click += UxDeleteCouponButton_Click;
            uxBackButton.Click += UxBackButton_Clicked;
            uxAddPromoButton.Click += UxAddPromoButton_Click;

            _ = RefreshCouponList();
        }

        private void UxAddPromoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewPromo), null);
        }

        private async void UxDeleteCouponButton_Click(object sender, RoutedEventArgs e)
        {
            var validDeleteCouponRequest = await DeleteCouponRequest.SendDeleteCouponRequest(SelectedCoupon._id);
            if(validDeleteCouponRequest)
            {
                await RefreshCouponList();
            }
        }

        private async void UxSetInactiveButton_Click(object sender, RoutedEventArgs e)
        {
            var validSetCouponActiveRequest = await SetCouponActive.SendSetCouponActive(SelectedCoupon._id, false);
            if(validSetCouponActiveRequest)
            {
                await RefreshCouponList();
            }
        }

        private async void UxSetActiveButton_Click(object sender, RoutedEventArgs e)
        {
            var validSetCouponActiveRequest = await SetCouponActive.SendSetCouponActive(SelectedCoupon._id, true);
            if(validSetCouponActiveRequest)
            {
                await RefreshCouponList();
            }
        }

        private void UxPromotionListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedCoupon = (Coupon)e.ClickedItem;
        }

        public async Task RefreshCouponList()
        {
            var validGetCouponRequest = await GetCouponsRequest.SendGetCouponsRequest();
            uxPromotionListView.ItemsSource = RealmManager.All<CouponList>().FirstOrDefault().coupons.ToList();
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
