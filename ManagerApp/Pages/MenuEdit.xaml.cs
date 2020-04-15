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
using Windows.UI.Xaml.Media.Imaging;
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
        public bool Editing = false;
        public bool Creating = false;
        public string DescriptionText;
        public string NutritionText;
        public string Picture;
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
            uxAddMenuItemButton.Click += AddMenuItem;
            uxAddImageButton.Click += PickPicture;

            RefreshMenuItemList();
        }

        private async void PickPicture(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            Picture = await ImageConverter.ConvertStorageFileToBase64(file);
            uxMenuItemPhoto.Source = await ImageConverter.ConvertBase64ToImageSource(Picture);
        }

        private void AddMenuItem(object sender, RoutedEventArgs e)
        {
            uxMenuPopup.IsOpen = true;
            Creating = true;

            uxEditInfoButton.Visibility = Visibility.Collapsed;
            uxDoneInfoButton.Visibility = Visibility.Visible;

            //name
            uxDisplayName.Visibility = Visibility.Collapsed;
            uxDisplayNameEntry.Visibility = Visibility.Visible;
            uxDisplayNameEntry.PlaceholderText = "Item Name";

            //category
            uxDisplayCategoryEntry.Visibility = Visibility.Visible;
            uxDisplayCategoryName.Visibility = Visibility.Collapsed;

            //price
            uxDisplayPrice.Visibility = Visibility.Collapsed;
            uxDisplayPriceEntry.Visibility = Visibility.Visible;

            //image
            uxMenuItemPhoto.Source = new BitmapImage(new Uri("ms-appx:///Assets/userIcon.png"));
            uxAddImageButton.Visibility = Visibility.Visible;
        }

        private async void DoneEditing(object sender, RoutedEventArgs e)
        {
            if (Editing == true)
            {
                var successfulUpdateMenuItemRequest = await UpdateMenuItemRequest.SendUpdateMenuItemRequest(SelectedMenuItem._id, uxDisplayCategoryEntry.Text, Convert.ToDouble(uxDisplayPriceEntry.Text), uxDisplayNameEntry.Text, NutritionText, DescriptionText);
                if (successfulUpdateMenuItemRequest)
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Successful",
                        Content = "Menu Item has been updated successfully",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }
                else
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Unsuccessful",
                        Content = "Menu Item has not been updated successfully",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();
                }

                Editing = false;
            }

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

            uxMenuPopup.IsOpen = false;

            RefreshMenuItemList();
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
            DescriptionText = SelectedMenuItem.description;
            NutritionText = SelectedMenuItem.nutrition;
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
