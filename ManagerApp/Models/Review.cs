using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerApp.Models
{
    public class Review : RealmObject
    {
        public string _id { get; set; }
        public string employee_id { get; set; }
        public string order_id { get; set; }
        public int? question01_rating { get; set; }
        public int? question02_rating { get; set; }
        public int? question03_rating { get; set; }
        public string question01_reason { get; set; }
        public string question02_reason { get; set; }
        public string question03_reason { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

    public class ReviewList : RealmObject
    {
        public IList<Review> reviews { get; }
    }

    public class ReviewStatList
    {
        public List<OverallReviewStat> OverallReviewStats { get; set; }
        public List<WaitstaffReviewStat> WaitstaffReviewStats { get; set; }
        public List<FoodReviewStat> FoodReviewStats { get; set; }
        public double Average1 { get; set; }
        public double Average2 { get; set; }
        public double Average3 { get; set; }

        public ReviewStatList(IList<Review> reviews)
        {
            OverallReviewStats = new List<OverallReviewStat>();
            WaitstaffReviewStats = new List<WaitstaffReviewStat>();
            FoodReviewStats = new List<FoodReviewStat>();

            int total1 = 0;
            int numReviews1 = 0;
            int total2 = 0;
            int numReviews2 = 0;
            int total3 = 0;
            int numReviews3 = 0;
            foreach(Review review in reviews)
            {
                if(review.question01_rating != null)
                {
                    OverallReviewStat overallReviewStat = new OverallReviewStat();
                    total1 += (int)review.question01_rating;
                    numReviews1++;
                    overallReviewStat.Rating = (int)review.question01_rating;
                    overallReviewStat.DateTime = review.createdAt.Substring(11, 8) + " " + review.createdAt.Substring(5, 2) + "/" + review.createdAt.Substring(8, 2) + "/" + review.createdAt.Substring(0, 4);
                    overallReviewStat.Message = review.question01_reason;

                    //wire it into the list
                    OverallReviewStats.Add(overallReviewStat);
                }
                if(review.question02_rating != null)
                {
                    WaitstaffReviewStat waitstaffReviewStat = new WaitstaffReviewStat();
                    total2 += (int)review.question02_rating;
                    numReviews2++;
                    waitstaffReviewStat.Rating = (int)review.question02_rating;
                    waitstaffReviewStat.DateTime = review.createdAt.Substring(11, 8) + " " + review.createdAt.Substring(5, 2) + "/" + review.createdAt.Substring(8, 2) + "/" + review.createdAt.Substring(0, 4);
                    waitstaffReviewStat.Message = review.question02_reason;

                    //get employee name
                    var emp = RealmManager.Find<Employee>(review.employee_id);
                    if(emp != null)
                    {
                        waitstaffReviewStat.Server = emp.first_name;
                    }

                    //wire it into the list
                    WaitstaffReviewStats.Add(waitstaffReviewStat);
                }
                if(review.question03_rating != null)
                {
                    FoodReviewStat foodReviewStat = new FoodReviewStat();
                    total3 += (int)review.question03_rating;
                    numReviews3++;
                    foodReviewStat.Rating = (int)review.question03_rating;
                    foodReviewStat.DateTime = review.createdAt.Substring(11, 8) + " " + review.createdAt.Substring(5, 2) + "/" + review.createdAt.Substring(8, 2) + "/" + review.createdAt.Substring(0, 4);
                    foodReviewStat.Message = review.question03_reason;

                    //get food list
                    foodReviewStat.MenuItemNames = new List<OrderItem>();

                    var order = RealmManager.Find<Order>(review.order_id);
                    if(order != null)
                    {
                        foreach(OrderItem orderItem in order.menuItems)
                        {
                            foodReviewStat.MenuItemNames.Add(orderItem);
                        }
                    }

                    FoodReviewStats.Add(foodReviewStat);
                }

            }

            //get averages
            if(numReviews1 != 0)
            {
                Average1 = Convert.ToDouble(total1 / numReviews1);
            }
            if(numReviews2 != 0)
            {
                Average2 = Convert.ToDouble(total2 / numReviews2);
            }
            if(numReviews3 != 0)
            {
                Average3 = Convert.ToDouble(total3 / numReviews3);
            }
        }
    }

    public class OverallReviewStat
    {
        public string DateTime { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
    }

    public class WaitstaffReviewStat
    {
        public string DateTime { get; set; }
        public int Rating { get; set; }
        public string Server { get; set; }
        public string Message { get; set; }
    }

    public class FoodReviewStat
    {
        public string DateTime { get; set; }
        public int Rating { get; set; }
        public List<OrderItem> MenuItemNames { get; set; }
        public string Message { get; set; }
    }
}
