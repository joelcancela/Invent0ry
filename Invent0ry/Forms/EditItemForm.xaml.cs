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
    /// Logique d'interaction pour EditItemForm.xaml
    /// </summary>
    public partial class EditItemForm : Window
    {
        private string categoriesSeparator = ", ";
        public Item Item { get; }

        private EditItemForm()
        {
            InitializeComponent();
        }

        public EditItemForm(Item item) : this()
        {
            Item = item;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Item.Name = NameTextBox.Text;
            Item.Quantity = int.Parse(QuantityTextBox.Text);
            List<string> itemCategories = CategoriesTextBox.Text
                .Split(new[] {categoriesSeparator}, StringSplitOptions.None).ToList();
            Item.Categories = new List<Category>();
            foreach (string category in itemCategories)
            {
                Category categoryEnum;
                if (Category.TryParse(category, out categoryEnum))
                {
                    Item.Categories.Add(categoryEnum);
                }
            }

            Item.Location = LocationTextBox.Text;
            if (!String.IsNullOrEmpty(LoansTextBox.Text))
                Item.Loans = LoansTextBox.Text.Split(new[] {Environment.NewLine}, StringSplitOptions.None).ToList();
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = Item.Name;
            QuantityTextBox.Text = Item.Quantity.ToString();
            CategoriesTextBox.Text = string.Join(categoriesSeparator, Item.Categories);
            LocationTextBox.Text = Item.Location;
            LoansTextBox.Text = string.Join(Environment.NewLine, Item.Loans);
        }
    }
}