using Managment.Classes;
using Managment.Helpers;
using Managment.ApplicationContext;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EventsDB_UI_
{
    /// <summary>
    /// Interaction logic for CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : Window
    {
        private readonly CategoryParser _categoryParser;
        private readonly CategoryRepository _categoryRepository;

        public CategoriesWindow()
        {
            InitializeComponent();
            var dataContext = new DataContext();
            _categoryRepository = new CategoryRepository(dataContext);
            _categoryParser = new CategoryParser(_categoryRepository);
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _categoryRepository.Get().ToList();
            CategoriesDataGrid.ItemsSource = categories;
        }

        private void ParseCsv_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _categoryParser.ParseAndStoreCategories(openFileDialog.FileName);
                    LoadCategories();
                    MessageBox.Show("Categories imported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = CategoryNameTextBox.Text;
            if (!string.IsNullOrEmpty(categoryName))
            {
                var category = new Category { Name = categoryName };
                try
                {
                    _categoryRepository.Create(category);
                    _categoryRepository.SaveChanges();
                    LoadCategories();
                    CategoryNameTextBox.Clear();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a category name.");
            }
        }



        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category selectedCategory)
            {
                selectedCategory.Name = CategoryNameTextBox.Text;
                _categoryRepository.Update(selectedCategory);
                _categoryRepository.SaveChanges();
                LoadCategories();
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category selectedCategory)
            {
                _categoryRepository.Delete(selectedCategory.Id);
                _categoryRepository.SaveChanges();
                LoadCategories();
            }
        }

        private void GetCategoryById_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CategoryIdTextBox.Text, out int id))
            {
                var category = _categoryRepository.Get(id);
                if (category != null)
                {
                    MessageBox.Show($"ID: {category.Id}, Name: {category.Name}");
                }
                else
                {
                    MessageBox.Show("Category not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }
        }
    }
}
