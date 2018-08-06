using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Invent0ry.Model;

namespace Invent0ry.Forms
{
    /// <summary>
    /// Logique d'interaction pour MemosForm.xaml
    /// </summary>
    public partial class MemosForm : Window
    {
        private List<String> picturesPathsList;
        private int currentIndex = 0;

        public MemosForm(List<string> memosPaths)
        {
            InitializeComponent();
            picturesPathsList = memosPaths;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(picturesPathsList[currentIndex]));
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            if (currentIndex == picturesPathsList.Count) currentIndex = 0;
            image.Source = new BitmapImage(new Uri(picturesPathsList[currentIndex]));
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex--;
            if (currentIndex == -1) currentIndex = picturesPathsList.Count - 1;
            image.Source = new BitmapImage(new Uri(picturesPathsList[currentIndex]));
        }
    }
}