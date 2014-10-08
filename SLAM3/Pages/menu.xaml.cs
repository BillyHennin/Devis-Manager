using System;
using System.Windows;

namespace SLAM3.Pages
{
    /// <summary>
    /// Logique d'interaction pour amortissements.xaml
    /// </summary>
    public partial class amortissements
    {
        public amortissements()
        {
            InitializeComponent();
        }

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Convert.ToString(Menu.ActualHeight));
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Menu.ActualWidth > 390)
            {
                BorderBlack.Width = Menu.ActualWidth - 389;
            }
            else
            {
                BorderBlack.Width = 1;
            }
            
            BorderBlack.Height = Menu.ActualHeight - 60;
        }
    }
}
