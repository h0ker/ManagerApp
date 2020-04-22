using ManagerApp.Models;
using ManagerApp.Models.ServiceRequests;
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

namespace ManagerApp.Pages
{
    public sealed partial class Feedback : Page
    {
        public ReviewStatList ReviewStatList { get; set; }
        public Feedback()
        {
            this.InitializeComponent();

            GetReviewContext();
            uxOverallCategoryButton.Click += UxOverallCategoryButton_Click;
            uxFoodCategoryButton.Click += UxFoodCategoryButton_Click;
            uxWaitstaffCategoryButton.Click += UxWaitstaffCategoryButton_Click;
            uxOverallReviewList.ItemClick += UxOverallReviewList_ItemClick;
            uxFoodReviewList.ItemClick += UxFoodReviewList_ItemClick;
            uxWaitstaffReviewList.ItemClick += UxWaitstaffReviewList_ItemClick;

            uxBackButton.Click += UxBackButton_Clicked;
        }

        private void UxWaitstaffReviewList_ItemClick(object sender, ItemClickEventArgs e)
        {
            WaitstaffReviewStat waitstaffReviewStat = (WaitstaffReviewStat)e.ClickedItem;
            if(!String.IsNullOrEmpty(waitstaffReviewStat.Message))
            {
                uxDescriptionBlock.Text = waitstaffReviewStat.Message;
            }
            else
            {
                uxDescriptionBlock.Text = "No description provided by customer.";
            }
        }

        private void UxFoodReviewList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FoodReviewStat foodReviewStat = (FoodReviewStat)e.ClickedItem;
            if(!String.IsNullOrEmpty(foodReviewStat.Message))
            {
                uxDescriptionBlock.Text = foodReviewStat.Message;
            }
            else
            {
                uxDescriptionBlock.Text = "No description provided by customer.";
            }
        }

        private void UxOverallReviewList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OverallReviewStat overallReviewStat = (OverallReviewStat)e.ClickedItem;
            if(!String.IsNullOrEmpty(overallReviewStat.Message))
            {
                uxDescriptionBlock.Text = overallReviewStat.Message;
            }
            else
            {
                uxDescriptionBlock.Text = "No description provided by customer.";
            }
        }

        private void UxWaitstaffCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            uxDescriptionBlock.Text = String.Empty;
            uxOverallReviewList.Visibility = Visibility.Collapsed;
            uxFoodReviewList.Visibility = Visibility.Collapsed;
            uxWaitstaffReviewList.Visibility = Visibility.Visible;
        }

        private void UxFoodCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            uxDescriptionBlock.Text = String.Empty;
            uxOverallReviewList.Visibility = Visibility.Collapsed;
            uxFoodReviewList.Visibility = Visibility.Visible;
            uxWaitstaffReviewList.Visibility = Visibility.Collapsed;
        }

        private void UxOverallCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            uxDescriptionBlock.Text = String.Empty;
            uxOverallReviewList.Visibility = Visibility.Visible;
            uxFoodReviewList.Visibility = Visibility.Collapsed;
            uxWaitstaffReviewList.Visibility = Visibility.Collapsed;
        }

        private async void GetReviewContext()
        {
            var validGetReviews = await GetReviews.SendGetReviews();
            await GetOrdersRequest.SendGetOrdersRequest();
            if(validGetReviews)
            {
                var reviewList = RealmManager.All<ReviewList>().FirstOrDefault().reviews;
                ReviewStatList = new ReviewStatList(reviewList);

                uxOverallReviewList.ItemsSource = ReviewStatList.OverallReviewStats;
                uxWaitstaffReviewList.ItemsSource = ReviewStatList.WaitstaffReviewStats;
                uxFoodReviewList.ItemsSource = ReviewStatList.FoodReviewStats;

                uxScore1ReviewNumber.Text = "Dijkstra's Average Rating: " + ReviewStatList.Average1.ToString() + " Stars";

                if(ReviewStatList.Average1 <=1)
                {
                    uxScore1Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average1 > 1 && ReviewStatList.Average1 <= 2)
                {
                    uxScore1Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average1 > 2 && ReviewStatList.Average1 <= 3)
                {
                    uxScore1Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore1Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average1 > 3 && ReviewStatList.Average1 <= 4)
                {
                    uxScore1Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average1 > 4 && ReviewStatList.Average1 <= 5)
                {
                    uxScore1Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore1Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                }
 
                uxScore2ReviewNumber.Text = "Dijkstra's Average Rating: " + ReviewStatList.Average2.ToString() + " Stars";

                if(ReviewStatList.Average2 <=1)
                {
                    uxScore2Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average2 > 1 && ReviewStatList.Average2 <= 2)
                {
                    uxScore2Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average2 > 2 && ReviewStatList.Average2 <= 3)
                {
                    uxScore2Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore2Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average2 > 3 && ReviewStatList.Average2 <= 4)
                {
                    uxScore2Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average2 > 4 && ReviewStatList.Average2 <= 5)
                {
                    uxScore2Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore2Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                }
 
                uxScore3ReviewNumber.Text = "Dijkstra's Average Rating: " + ReviewStatList.Average3.ToString() + " Stars";

                if(ReviewStatList.Average3 <=1)
                {
                    uxScore3Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average3 > 1 && ReviewStatList.Average3 <= 2)
                {
                    uxScore3Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average3 > 2 && ReviewStatList.Average3 <= 3)
                {
                    uxScore3Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                    uxScore3Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average3 > 3 && ReviewStatList.Average3 <= 4)
                {
                    uxScore3Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/greyStar.png"));
                }
                if(ReviewStatList.Average3 > 4 && ReviewStatList.Average3 <= 5)
                {
                    uxScore3Star1.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star2.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star3.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star4.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                    uxScore3Star5.Source = new BitmapImage(new Uri("ms-appx:///Assets/goldStar.png"));
                }
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
