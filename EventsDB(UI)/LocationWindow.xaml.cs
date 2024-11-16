using Managment.Classes;
using Managment.Helpers;
using Managment.ApplicationContext;
using Models;
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

namespace EventsDB_UI_
{
    /// <summary>
    /// Interaction logic for EventWindow.xaml
    /// </summary>
    public partial class EventWindow : Window
    {
        private readonly LocationParser _locationParser;
        private readonly LocationRepository _locationRepository;

        public EventWindow()
        {
            InitializeComponent();
            var dataContext = new DataContext();
            _locationRepository = new LocationRepository(dataContext);
            _locationParser = new LocationParser(_locationRepository);
            LoadLocations();
        }

        private void LoadLocations()
        {
            var locations = _locationRepository.Get().ToList();
            LocationsDataGrid.ItemsSource = locations;
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
                _locationParser.ParseAndStoreLocations(csvFilePath);
                LoadLocations();
            }
        }

        private void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            var locationName = LocationNameTextBox.Text;
            if (!string.IsNullOrEmpty(locationName))
            {
                var location = new Location { Name = locationName };
                _locationRepository.Create(location);
                _locationRepository.SaveChanges();
                LoadLocations();
                LocationNameTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a location name.");
            }
        }

        private void EditLocation_Click(object sender, RoutedEventArgs e)
        {
            if (LocationsDataGrid.SelectedItem is Location selectedLocation)
            {
                selectedLocation.Name = "Updated Location";
                _locationRepository.Update(selectedLocation);
                _locationRepository.SaveChanges();
                LoadLocations();
            }
        }

        private void DeleteLocation_Click(object sender, RoutedEventArgs e)
        {
            if (LocationsDataGrid.SelectedItem is Location selectedLocation)
            {
                _locationRepository.Delete(selectedLocation.Id);
                _locationRepository.SaveChanges();
                LoadLocations();
            }
        }

        private void GetLocationById_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(LocationIdTextBox.Text, out int id))
            {
                var location = _locationRepository.Get(id);
                if (location != null)
                {
                    MessageBox.Show($"ID: {location.Id}, Name: {location.Name}");
                }
                else
                {
                    MessageBox.Show("Location not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }
        }
    }
}
