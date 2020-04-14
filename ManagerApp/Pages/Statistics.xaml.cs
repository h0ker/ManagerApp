using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ManagerApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Statistics : Page
    {
        public Statistics()
        {
            this.InitializeComponent();

            //clicked events
            uxBackButton.Click += UxBackButton_Clicked;
        }


        private void KPIComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add "using Windows.UI;" for Color and Colors.
            string viewSelection = e.AddedItems[0].ToString();
            
            switch (viewSelection)
            {
                case "Monthly View":
                    MonthlyViewPopup.IsOpen = true;
                    break;
                case "Weekly View":
                    //color = Colors.Green;
                    break;
                case "Yearly View":
                    //color = Colors.Blue;
                    break;
            }
            //colorRectangle.Fill = new SolidColorBrush(color);
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