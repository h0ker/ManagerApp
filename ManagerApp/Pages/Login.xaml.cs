using ManagerApp;
using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using ManagerApp.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ManagerApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();

            CoreWindow.GetForCurrentThread().KeyDown += Login_KeyDown;
            uxLoginButton.Click += UxLoginButton_Clicked;
        }

        private void Login_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if(args.VirtualKey == Windows.System.VirtualKey.Enter && frame.BackStackDepth<1)
            {
                ValidateLogin();
            }
        }

        public async void ValidateLogin()
        {
            if (!String.IsNullOrEmpty(uxUsernameTextBox.Text) && !String.IsNullOrEmpty(uxPasswordTextBox.Password))
            {
                var validLoginRequest = await ValidateLoginRequest.SendValidateLoginRequest(uxUsernameTextBox.Text, uxPasswordTextBox.Password);

                if (validLoginRequest)
                {
                    this.Frame.Navigate(typeof(HomeScreen), null);
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Incorrect Username/Password",
                        Content = "No Employee user was found with that username and password",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Username or Password cannot be empty",
                    Content = "",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
        }
 
        private void UxLoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            ValidateLogin();
        }
    }
}
