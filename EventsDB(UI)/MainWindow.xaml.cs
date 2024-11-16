using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Managment.Classes;
using Managment.Helpers;
using Managment.ApplicationContext;
using Models;

namespace EventsDB_UI_
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManageLocations_Click(object sender, RoutedEventArgs e)
        {
            EventWindow eventWindow = new EventWindow();
            eventWindow.Show();
        }

        private void ManageOrganizers_Click(object sender, RoutedEventArgs e)
        {
            OrganizersWindow organizersWindow = new OrganizersWindow();
            organizersWindow.Show();
        }

        private void ManageEvents_Click(object sender, RoutedEventArgs e)
        {
            EventManagmentWindow eventManagmentWindow = new EventManagmentWindow();
            eventManagmentWindow.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Close();
        }

        private void ManageCategories_Click(object sender, RoutedEventArgs e)
        {
            CategoriesWindow categoriesWindow = new CategoriesWindow();
            categoriesWindow.Show();
        }

        private void ManageStatus_Click(object sender, RoutedEventArgs e)
        {
            StatusWindow statusWindow = new StatusWindow();
            statusWindow.Show();
        }
    }
}
