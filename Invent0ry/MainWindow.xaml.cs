using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Invent0ry.Forms;
using Invent0ry.Model;
using Invent0ry.Persistence;
using TextBox = System.Windows.Controls.TextBox;

namespace Invent0ry
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Inventory _inventory;
        private List<String> memosPaths;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _inventory = InventoryManager.DeserializeInventory();
            SetItemsSource();
            memosPaths = InventoryManager.GetMemosPaths();
        }

        private void SetItemsSource()
        {
            inventoryGrid.ItemsSource = _inventory.Items;
            inventoryGrid.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            Item selectedItem = (Item) inventoryGrid.SelectedItem;
            if (selectedItem == null)
                return;
            EditItemForm dialog = new EditItemForm(selectedItem);
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                inventoryGrid.ItemsSource = null;
                inventoryGrid.Items.Clear();
                int index = _inventory.Items.IndexOf(selectedItem);
                _inventory.Items[index] = dialog.Item;
                SetItemsSource();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryManager.SerializeInventory(_inventory);
            ChangesSavedLabel.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(new Action(HideChangesSavedLabel));
            });
        }

        internal void HideChangesSavedLabel()
        {
            ChangesSavedLabel.Visibility = Visibility.Hidden;
        }

        private void FilterTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FilterTextBox.Text = "";
            FilterTextBox.Foreground = Brushes.Black;
        }

        private void FilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterTextBox.Text == "")
            {
                FilterTextBox.Text = "Filter items...";
                FilterTextBox.Foreground = Brushes.DimGray;
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterTextBox.Text != "Filter items..." && FilterTextBox.Text != "")
            {
                inventoryGrid.ItemsSource = null;
                inventoryGrid.Items.Clear();
                foreach (Item item in _inventory.Items)
                {
                    if (item.Name.IndexOf(FilterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        item.Location.IndexOf(FilterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                        item.Categories.Any(element =>
                            element.ToString().IndexOf(FilterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >=
                            0) ||
                        item.Loans.Any(element =>
                            element.IndexOf(FilterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0))
                    {
                        inventoryGrid.Items.Add(item);
                    }
                }

                inventoryGrid.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }

            if (FilterTextBox.Text == "")
            {
                inventoryGrid.ItemsSource = null;
                inventoryGrid.Items.Clear();
                SetItemsSource();
            }
        }

        private void MemosButton_Click(object sender, RoutedEventArgs e)
        {
            MemosForm memosForm = new MemosForm(memosPaths);
            memosForm.Show();
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (inventoryGrid.SelectedItems.Count > 0)
            {
                List<Item> itemsToRemove = new List<Item>();
                foreach (Item item in inventoryGrid.SelectedItems)
                {
                    itemsToRemove.Add(item);
                }

                foreach (Item item in itemsToRemove)
                {
                    _inventory.Items.Remove(item);
                }
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            EditItemForm dialog = new EditItemForm(new Item());
            dialog.Title = "Add item";
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value && !string.IsNullOrWhiteSpace(dialog.Item.Name))
            {
                _inventory.Items.Add(dialog.Item);
                SetItemsSource();
            }
        }

        private void inventoryGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditItem_Click(null,null);
        }
    }
}