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
