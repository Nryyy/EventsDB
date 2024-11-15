using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Managment.Classes;
using Managment.Helpers;
using Managment.DataBaseContext;
using Models;

namespace EventsDB_UI_
{
    public partial class MainWindow : Window
    {
        private readonly LocationParser _locationParser;
        private readonly LocationRepository _locationRepository;

        public MainWindow()
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
            // Assuming the CSV file path is hardcoded or obtained from a file dialog
            string csvFilePath = @"C:\\Users\\Nazariy\\source\\repos\\EventsDB\\data.csv";
            _locationParser.ParseAndStoreLocations(csvFilePath);
            LoadLocations();
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
