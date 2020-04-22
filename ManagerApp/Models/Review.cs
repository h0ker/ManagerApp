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
        public string question01_response { get; set; }
        public string question02_response { get; set; }
        public string question03_response { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

    public class ReviewList : RealmObject
    {
        public IList<Review> reviews { get; }
    }

    public class ReviewStatList
    {
        public List<ReviewStat> ReviewStats { get; set; }
        public double Average1 { get; set; }
        public double Average2 { get; set; }
        public double Average3 { get; set; }

        public ReviewStatList(IList<Review> reviews)
        {
            ReviewStats = new List<ReviewStat>();

            int total1 = 0;
            int numReviews1 = 0;
            int total2 = 0;
            int numReviews2 = 0;
            int total3 = 0;
            int numReviews3 = 0;
            foreach(Review review in reviews)
            {
                ReviewStat reviewStat = new ReviewStat();

                //start averages
                if(review.question01_rating != null)
                {
                    total1 += (int)review.question01_rating;
                    numReviews1++;
                }
                if(review.question02_rating != null)
                {
                    total2 += (int)review.question02_rating;
                    numReviews2++;
                }
                if(review.question03_rating != null)
                {
                    total3 += (int)review.question03_rating;
                    numReviews3++;
                }

                //get datetime
                reviewStat.DateTime = review.createdAt.Substring(11, 8) + " " + review.createdAt.Substring(5, 2) + "/" + review.createdAt.Substring(8, 2) + "/" + review.createdAt.Substring(0, 4);

                //get messages
                reviewStat.Message1 = review.question01_response;
                reviewStat.Message2 = review.question02_response;
                reviewStat.Message3 = review.question03_response;

                //get ratings
                if(review.question01_rating != null)
                {
                    reviewStat.Rating1 = (int)review.question01_rating;
                }
                if(review.question02_rating != null)
                {
                    reviewStat.Rating2 = (int)review.question02_rating;
                }
                if(review.question03_rating != null)
                {
                    reviewStat.Rating3 = (int)review.question03_rating;
                }

                //get employee name
                var emp = RealmManager.Find<Employee>(review.employee_id);
                if(emp != null)
                {
                    reviewStat.Server = emp.first_name;
                }

                //wire it into the list
                ReviewStats.Add(reviewStat);
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

    public class ReviewStat
    {
        public string DateTime { get; set; }
        public int Rating1 { get; set; }
        public int Rating2 { get; set; }
        public int Rating3 { get; set; }
        public string Server { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
    }
}
