using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using ManagerApp.Utilities;
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
    public sealed partial class MenuEdit : Page
    {
        public MenuItem SelectedMenuItem { get; set; }
        public MenuEdit()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxMenuItemListView.ItemClick += UxMenuItemListViewItem_Clicked;

            RefreshMenuItemList(); 
        }
         
        private async void UxMenuItemListViewItem_Clicked(object sender, ItemClickEventArgs e)
        {
            SelectedMenuItem = (MenuItem)e.ClickedItem;
            uxMenuPopup.IsOpen = true;
            uxMenuItemPhoto.Source = await ImageConverter.ConvertBase64ToImageSource(SelectedMenuItem.picture);
        }

        private async void RefreshMenuItemList()
        {
            RealmManager.RemoveAll<MenuItemList>();
            RealmManager.RemoveAll<MenuItem>();
            var validGetMenuItemsRequest = await GetMenuItemsRequest.SendGetMenuItemsRequest();
            if(validGetMenuItemsRequest)
            {
                uxMenuItemListView.ItemsSource = RealmManager.All<MenuItemList>().FirstOrDefault().menuItems.ToList();
            }
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
