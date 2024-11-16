using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Managment.Classes;
using Managment.Helpers;
using Managment.ApplicationContext;
using Models;
using Managment.Interfaces;

namespace EventsDB_UI_
{
    /// <summary>
    /// Interaction logic for EventManagmentWindow.xaml
    /// </summary>
    public partial class EventManagmentWindow : Window
    {
        private readonly EventRepository _eventRepository;
        private readonly LocationRepository _locationRepository;
        private readonly OrganizersRepository _organizersRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly StatusRepository _statusRepository;
        private readonly EventHelper _eventHelper;
        private readonly LocationParser _locationParser;
        private readonly EventHelper _eventParser;

        public EventManagmentWindow()
        {
            InitializeComponent();
            var dataContext = new DataContext();
            _eventRepository = new EventRepository(dataContext);
            _locationRepository = new LocationRepository(dataContext);
            _organizersRepository = new OrganizersRepository(dataContext);
            _categoryRepository = new CategoryRepository(dataContext);
            _statusRepository = new StatusRepository(dataContext);
            _eventHelper = new EventHelper(_eventRepository, _locationRepository, _organizersRepository, _categoryRepository, _statusRepository);
            _locationParser = new LocationParser(_locationRepository);
            _eventParser = new EventHelper(_eventRepository, _locationRepository, _organizersRepository, _categoryRepository, _statusRepository);

            LoadComboBoxes();
            LoadEventsDataGrid();
        }

        private void LoadComboBoxes()
        {
            LocationComboBox.ItemsSource = _locationRepository.Get().ToList();
            OrganizerComboBox.ItemsSource = _organizersRepository.Get().ToList();
            CategoryComboBox.ItemsSource = _categoryRepository.Get().ToList();
            StatusComboBox.ItemsSource = _statusRepository.Get().ToList();
        }

        private void LoadEventsDataGrid()
        {
            EventsDataGrid.ItemsSource = _eventRepository.Get().Select(e => new
            {
                e.Name,
                e.DateOfPerfomance,
                e.TimeOfPerfomance,
                LocationName = e.Location.Name,
                OrganizerName = e.Organizers.Name,
                CategoryName = e.Categoty.Name,
                StatusName = e.Status.Name,
                e.Participants,
                e.Budget
            }).ToList();
        }

        private void SaveEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var eventName = EventNameTextBox.Text;
                var eventDate = EventDatePicker.SelectedDate;
                var eventTime = EventTimeTextBox.Text;
                var location = LocationComboBox.SelectedItem as Location;
                var organizer = OrganizerComboBox.SelectedItem as Organizers;
                var category = CategoryComboBox.SelectedItem as Category;
                var status = StatusComboBox.SelectedItem as Status;
                var participants = int.Parse(ParticipantsTextBox.Text);
                var budget = decimal.Parse(BudgetTextBox.Text);

                if (string.IsNullOrEmpty(eventName) || !eventDate.HasValue || string.IsNullOrEmpty(eventTime) || location == null || organizer == null || category == null || status == null)
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                var newEvent = new Event
                {
                    Name = eventName,
                    DateOfPerfomance = eventDate.Value,
                    TimeOfPerfomance = DateTime.Parse(eventTime),
                    Location = location,
                    Organizers = organizer,
                    Categoty = category,
                    Status = status,
                    Participants = participants,
                    Budget = (int)budget
                };

                _eventRepository.Create(newEvent);
                _eventRepository.SaveChanges();
                MessageBox.Show("Event saved successfully.");
                LoadEventsDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving event: {ex.Message}");
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            EventNameTextBox.Clear();
            EventDatePicker.SelectedDate = null;
            EventTimeTextBox.Clear();
            LocationComboBox.SelectedItem = null;
            OrganizerComboBox.SelectedItem = null;
            CategoryComboBox.SelectedItem = null;
            StatusComboBox.SelectedItem = null;
            ParticipantsTextBox.Clear();
            BudgetTextBox.Clear();
        }

        private void ParseEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var csvFilePath = openFileDialog.FileName;
                    _eventHelper.ParseAndStoreEvents(csvFilePath);
                    MessageBox.Show("Events parsed and stored successfully.");
                    LoadEventsDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing events from file: {ex.Message}");
            }
        }
    }
}
