using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
using Realms;
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
    public sealed partial class Inventory : Page
    {
        public Ingredient selectedIngredient { get; set; }
        public Inventory()
        {
            this.InitializeComponent();

            uxBackButton.Click += UxBackButton_Clicked;
            uxClosePopupButton.Click += UxClosePopupButton;
            uxAddCountButton.Click += UxAddCountButton_Clicked;
            uxSubtractCountButton.Click += UxSubtractCountButton_Clicked;

            uxIngredientGridView.ItemClick += UxIngredientGridView_ItemClick;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var validSendGetIngredientsRequest = await GetIngredientsRequest.SendGetIngredientsRequest();
            uxIngredientGridView.ItemsSource = RealmManager.All<IngredientList>().FirstOrDefault().doc;
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

        private void UxClosePopupButton(object sender, RoutedEventArgs e)
        {
            uxIngredientPopup.IsOpen = false;
            uxIngredientGridView.ItemsSource = RealmManager.All<Ingredient>().ToList();
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
