using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ManagerApp.Models
{
    public class Ingredient : RealmObject
    {
        [PrimaryKey]
        public string id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string NameAndAmount
        {
            get
            {
                if (quantity > 1)
                {
                    return name + ": " + quantity + " units";
                }
                else if (quantity == 1)
                {
                    return name + ": " + quantity + " unit";
                }
                else
                {
                    return name + ": No units left";
                }
            }
            set
            {
                NameAndAmount = value;
            }
        }

        public static void StashDummyIngredientData()
        {
            Ingredient ingredient = new Ingredient
            {
                name = "Onions",
                quantity = 42,
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "Potatoes",
                quantity = 30
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "10 Oz Steak",
                quantity = 25
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "12 Oz Steak",
                quantity = 21
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "Salad Greens",
                quantity = 67
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "Asparagas",
                quantity = 0
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "72 Oz Steak",
                quantity = 5
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "Mac 'N Cheese",
                quantity = 34
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                name = "Tomatoes",
                quantity = 1
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);
        }
    }
}
