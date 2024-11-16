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
    /// Interaction logic for OrganizersWindow.xaml
    /// </summary>
    public partial class OrganizersWindow : Window
    {
        private readonly OrganizersHelper _organizersHelper;
        private readonly OrganizersRepository _organizersRepository;

        public OrganizersWindow()
        {
            InitializeComponent();
            var dataContext = new DataContext();
            _organizersRepository = new OrganizersRepository(dataContext);
            _organizersHelper = new OrganizersHelper(_organizersRepository);
            LoadOrganizers();
        }

        private void LoadOrganizers()
        {
            var organizers = _organizersRepository.Get().ToList();
            OrganizersDataGrid.ItemsSource = organizers;
        }

        private void ParseCsv_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select a CSV file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string csvFilePath = openFileDialog.FileName;
                _organizersHelper.ParseAndStoreOrganizers(csvFilePath);
                LoadOrganizers();
            }
        }


        private void AddOrganizer_Click(object sender, RoutedEventArgs e)
        {
            var organizerName = OrganizerNameTextBox.Text;
            if (!string.IsNullOrEmpty(organizerName))
            {
                var organizer = new Organizers { Name = organizerName };
                try
                {
                    _organizersRepository.Create(organizer);
                    _organizersRepository.SaveChanges();
                    LoadOrganizers();
                    OrganizerNameTextBox.Clear();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter an organizer name.");
            }
        }

        private void EditOrganizer_Click(object sender, RoutedEventArgs e)
        {
            if (OrganizersDataGrid.SelectedItem is Organizers selectedOrganizer)
            {
                selectedOrganizer.Name = OrganizerNameTextBox.Text;
                _organizersRepository.Update(selectedOrganizer);
                _organizersRepository.SaveChanges();
                LoadOrganizers();
            }
        }

        private void DeleteOrganizer_Click(object sender, RoutedEventArgs e)
        {
            if (OrganizersDataGrid.SelectedItem is Organizers selectedOrganizer)
            {
                _organizersRepository.Delete(selectedOrganizer.Id);
                _organizersRepository.SaveChanges();
                LoadOrganizers();
            }
        }

        private void GetOrganizerById_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(OrganizerIdTextBox.Text, out int id))
            {
                var organizer = _organizersRepository.Get(id);
                if (organizer != null)
                {
                    MessageBox.Show($"ID: {organizer.Id}, Name: {organizer.Name}");
                }
                else
                {
                    MessageBox.Show("Organizer not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }
        }
    }
}
