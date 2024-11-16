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
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow : Window
    {
        private readonly StatusParser _statusParser;
        private readonly StatusRepository _statusRepository;

        public StatusWindow()
        {
            InitializeComponent();
            var dataContext = new DataContext();
            _statusRepository = new StatusRepository(dataContext);
            _statusParser = new StatusParser(_statusRepository);
            LoadStatuses();
        }

        private void LoadStatuses()
        {
            var statuses = _statusRepository.Get().ToList();
            StatusDataGrid.ItemsSource = statuses;
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
                    _statusParser.ParseAndStoreStatuses(openFileDialog.FileName);
                    LoadStatuses();
                    MessageBox.Show("Statuses imported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing statuses: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddStatus_Click(object sender, RoutedEventArgs e)
        {
            var statusName = StatusNameTextBox.Text;
            if (!string.IsNullOrEmpty(statusName))
            {
                var status = new Status { Name = statusName };
                try
                {
                    _statusRepository.Create(status);
                    _statusRepository.SaveChanges();
                    LoadStatuses();
                    StatusNameTextBox.Clear();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a status name.");
            }
        }


        private void EditStatus_Click(object sender, RoutedEventArgs e)
        {
            if (StatusDataGrid.SelectedItem is Status selectedStatus)
            {
                selectedStatus.Name = StatusNameTextBox.Text;
                _statusRepository.Update(selectedStatus);
                _statusRepository.SaveChanges();
                LoadStatuses();
            }
        }

        private void DeleteStatus_Click(object sender, RoutedEventArgs e)
        {
            if (StatusDataGrid.SelectedItem is Status selectedStatus)
            {
                _statusRepository.Delete(selectedStatus.Id);
                _statusRepository.SaveChanges();
                LoadStatuses();
            }
        }

        private void GetStatusById_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(StatusIdTextBox.Text, out int id))
            {
                var status = _statusRepository.Get(id);
                if (status != null)
                {
                    MessageBox.Show($"ID: {status.Id}, Name: {status.Name}");
                }
                else
                {
                    MessageBox.Show("Status not found.");
                }
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }
        }
    }
}
