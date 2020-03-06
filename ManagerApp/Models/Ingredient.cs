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
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string NameAndAmount
        {
            get
            {
                if (Quantity > 1)
                {
                    return Name + ": " + Quantity + " units";
                }
                else if (Quantity == 1)
                {
                    return Name + ": " + Quantity + " unit";
                }
                else
                {
                    return Name + ": No units left";
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
                Name = "Onions",
                Quantity = 42,
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "Potatoes",
                Quantity = 30
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "10 Oz Steak",
                Quantity = 25
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "12 Oz Steak",
                Quantity = 21
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "Salad Greens",
                Quantity = 67
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "Asparagas",
                Quantity = 0
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "72 Oz Steak",
                Quantity = 5
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "Mac 'N Cheese",
                Quantity = 34
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);

            ingredient = new Ingredient
            {
                Name = "Tomatoes",
                Quantity = 1
            };

            RealmManager.AddOrUpdate<Ingredient>(ingredient);
        }
    }
}
