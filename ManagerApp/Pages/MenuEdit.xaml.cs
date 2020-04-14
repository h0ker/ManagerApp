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
        public bool Editing;
        public string DescriptionText;
        public string NutritionText;
        public MenuEdit()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxMenuItemListView.ItemClick += UxMenuItemListViewItem_Clicked;
            uxNutritionButton.Click += UxNutritionButton_Clicked;
            uxIngredientsButton.Click += UxIngredientsButton_Clicked;
            uxDescriptionButton.Click += UxDescriptionButton_Clicked;
            uxInfoDoneButton.Click += HideInfoPopup;
            uxEditInfoButton.Click += UxEditInfoButton_Clicked;
            uxDoneInfoButton.Click += DoneEditing;

            RefreshMenuItemList();
        }

        private void DoneEditing(object sender, RoutedEventArgs e)
        {
            Editing = false;

            uxEditInfoButton.Visibility = Visibility.Visible;
            uxDoneInfoButton.Visibility = Visibility.Collapsed;

            //name
            uxDisplayName.Visibility = Visibility.Visible;
            uxDisplayNameEntry.Visibility = Visibility.Collapsed;

            //category
            uxDisplayCategoryEntry.Visibility = Visibility.Collapsed;
            uxDisplayCategoryName.Visibility = Visibility.Visible;

            //price
            uxDisplayPrice.Visibility = Visibility.Visible;
            uxDisplayPriceEntry.Visibility = Visibility.Collapsed;

            //clear all
            uxDisplayPriceEntry.Text = String.Empty;
            uxDisplayCategoryEntry.Text = String.Empty;
        }

        private void UxEditInfoButton_Clicked(object sender, RoutedEventArgs e)
        {
            Editing = true;

            uxEditInfoButton.Visibility = Visibility.Collapsed;
            uxDoneInfoButton.Visibility = Visibility.Visible;

            //name
            uxDisplayName.Visibility = Visibility.Collapsed;
            uxDisplayNameEntry.Visibility = Visibility.Visible;
            uxDisplayNameEntry.Text = uxDisplayName.Text;

            //category
            uxDisplayCategoryEntry.Visibility = Visibility.Visible;
            uxDisplayCategoryName.Visibility = Visibility.Collapsed;
            uxDisplayCategoryEntry.Text = uxDisplayCategoryName.Text;

            //price
            uxDisplayPrice.Visibility = Visibility.Collapsed;
            uxDisplayPriceEntry.Visibility = Visibility.Visible;
            uxDisplayPriceEntry.Text = uxDisplayPrice.Text;
        }

        private void HideInfoPopup(object sender, RoutedEventArgs e)
        {
            //save text
            if(uxDescriptionTextBox.Text != String.Empty)
            {
                DescriptionText = uxDescriptionTextBox.Text;
            }
            if(uxNutritionTextBox.Text != String.Empty)
            {
                NutritionText = uxNutritionTextBox.Text;
            }

            uxInfoPopup.IsOpen = false;
            uxDescriptionTextBox.Visibility = Visibility.Collapsed;
            uxNutritionTextBox.Visibility = Visibility.Collapsed;
            uxDescriptionNutritionTextBlock.Visibility = Visibility.Collapsed;
            uxIngredientInfoListView.Visibility = Visibility.Collapsed;
            uxDescriptionTextBox.Text = String.Empty;
            uxNutritionTextBox.Text = String.Empty;
        }

        private void UxNutritionButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(Editing)
            {
                uxInfoPopup.IsOpen = true;
                uxDescriptionNutritionTextBlock.Visibility = Visibility.Collapsed;
                uxNutritionTextBox.Visibility = Visibility.Visible;
                uxNutritionTextBox.Text = SelectedMenuItem.nutrition;
            }
            uxInfoPopup.IsOpen = true;
            uxDescriptionNutritionTextBlock.Visibility = Visibility.Visible;
            uxDescriptionNutritionTextBlock.Text = SelectedMenuItem.nutrition;
        }

        private void UxIngredientsButton_Clicked(object sender, RoutedEventArgs e)
        {
            uxInfoPopup.IsOpen = true;
            uxIngredientInfoListView.Visibility = Visibility.Visible;
            uxIngredientInfoListView.ItemsSource = SelectedMenuItem.ingredients.ToList();
        }

        private void UxDescriptionButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(Editing)
            {
                uxInfoPopup.IsOpen = true;
                uxDescriptionNutritionTextBlock.Visibility = Visibility.Collapsed;
                uxDescriptionTextBox.Visibility = Visibility.Visible;
                uxDescriptionTextBox.Text = SelectedMenuItem.description;
            }
            else
            {
                uxInfoPopup.IsOpen = true;
                uxDescriptionNutritionTextBlock.Visibility = Visibility.Visible;
                uxDescriptionNutritionTextBlock.Text = SelectedMenuItem.description;
            }
        }
        
        private async void UxMenuItemListViewItem_Clicked(object sender, ItemClickEventArgs e)
        {
            SelectedMenuItem = (MenuItem)e.ClickedItem;
            uxMenuPopup.IsOpen = true;
            uxMenuItemPhoto.Source = await ImageConverter.ConvertBase64ToImageSource(SelectedMenuItem.picture);
            uxDisplayCategoryName.Text = SelectedMenuItem.category;
            uxDisplayName.Text = SelectedMenuItem.name;
            uxDisplayPrice.Text = SelectedMenuItem.price.ToString();
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
