using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using Realms;
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
    public sealed partial class Inventory : Page
    {
        public Ingredient selectedIngredient { get; set; }
        public Inventory()
        {
            this.InitializeComponent();

            //clicked events
            uxBackButton.Click += UxBackButton_Clicked;
            uxClosePopupButton.Click += UxClosePopupButton;
            uxCloseAddIngredientPopupButton.Click += UxCloseAddIngredientPopupButton_Clicked;
            uxAddCountButton.Click += UxAddCountButton_Clicked;
            uxSubtractCountButton.Click += UxSubtractCountButton_Clicked;
            uxAddIngredientButton.Click += UxAddIngredientButton_Clicked;
            uxIngredientGridView.ItemClick += UxIngredientGridView_ItemClick;
            uxAddItemToInventoryButton.Click += UxAddItemToInventoryButton_Clicked;
            uxRemoveItemButton.Click += UxRemoveItemButton_Clicked;
        }

        //this method gets called everytime the page is loaded
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshIngredientList();
        }

        private async void UxRemoveItemButton_Clicked(object sender, RoutedEventArgs e)
        {
            var validDeleteIngredientRequest = await DeleteIngredientRequest.SendDeleteIngredientRequest(selectedIngredient._id);
            if(validDeleteIngredientRequest)
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Delete Successful",
                    Content = "Ingredient has been deleted from the remote database",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            else
            {
                ContentDialog responseAlert = new ContentDialog
                {
                    Title = "Delete Unsuccessful",
                    Content = "Ingredient failed to be removed from the remote database",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await responseAlert.ShowAsync();
            }
            await RefreshIngredientList();
            uxIngredientPopup.IsOpen = false;
        }

        private async Task RefreshIngredientList()
        {
            //send the GetIngredients service request
            var validSendGetIngredientsRequest = await GetIngredientsRequest.SendGetIngredientsRequest();
            //updates the itemsource with the newly populated data
            uxIngredientGridView.ItemsSource = RealmManager.All<IngredientList>().FirstOrDefault().doc;
        }

        private async void UxAddItemToInventoryButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(lblAddedItemName.Text)||(String.IsNullOrEmpty(lblAddedItemQuantity.Text)))
            {
                ContentDialog missingInput = new ContentDialog
                {
                    Title = "Missing Input",
                    Content = "Please make sure to enter both a name and quantity",
                    CloseButtonText = "Ok"
                };
                ContentDialogResult result = await missingInput.ShowAsync();
            }
            else
            {
                if(await AddIngredientRequest.SendAddIngredientRequest(lblAddedItemName.Text, lblAddedItemQuantity.Text))
                {
                    ContentDialog responseAlert = new ContentDialog
                    {
                        Title = "Add Successful",
                        Content = "Ingredient has been added to the remote database",
                        CloseButtonText = "Ok"
                    };
                    ContentDialogResult result = await responseAlert.ShowAsync();

                    uxAddIngredientPopup.IsOpen = false;

                    lblAddedItemName.Text = String.Empty;
                    lblAddedItemQuantity.Text = String.Empty;

                    await RefreshIngredientList();
                }
            }
        }

        private void UxCloseAddIngredientPopupButton_Clicked(object sender, RoutedEventArgs e)
        {
            uxAddIngredientPopup.IsOpen = false;
            lblAddedItemName.Text = String.Empty;
        }

        private void UxAddIngredientButton_Clicked(object sender, RoutedEventArgs e)
        {
            uxAddIngredientPopup.IsOpen = true;
        }

        private void UxSubtractCountButton_Clicked(object sender, RoutedEventArgs e)
        {
            RealmManager.Write(() =>
            {
                selectedIngredient.quantity--;
            });
            RealmManager.AddOrUpdate<Ingredient>(selectedIngredient);
            lblPopupItemTitle.Text = selectedIngredient.NameAndAmount;
        }

        private void UxAddCountButton_Clicked(object sender, RoutedEventArgs e)
        {
            RealmManager.Write(() =>
            {
                selectedIngredient.quantity++;
            });
            RealmManager.AddOrUpdate<Ingredient>(selectedIngredient);
            lblPopupItemTitle.Text = selectedIngredient.NameAndAmount;
        }

        private async void UxClosePopupButton(object sender, RoutedEventArgs e)
        {
            uxIngredientPopup.IsOpen = false;
            await UpdateIngredientRequest.SendUpdateIngredientRequest(selectedIngredient._id, "quantity", selectedIngredient.quantity.ToString());
            await RefreshIngredientList();
        }

        private void UxIngredientGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            uxIngredientPopup.IsOpen = true;
            selectedIngredient = (Ingredient)e.ClickedItem;
            lblPopupItemTitle.Text = selectedIngredient.NameAndAmount;
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
