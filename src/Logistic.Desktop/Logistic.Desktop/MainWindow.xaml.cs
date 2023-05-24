using Logistic.Core.Services;
using Logistic.DAL;
using Logistic.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Logistic.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private VehicleService vehicleService;
        private Vehicle _selectedVehicle;
        private VehicleType _selectedVehicleType;
        private ObservableCollection<Vehicle> _vehicleList;
        private ReportType _selectedReportType;
        public Array ReportTypes => Enum.GetValues(typeof(ReportType));
        public string ExportFilePath { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                if (_selectedVehicle != value)
                {
                    _selectedVehicle = value;
                    OnPropertyChanged(nameof(SelectedVehicle));
                }
            }
        }

        public static Array VehicleTypeValues => Enum.GetValues(typeof(VehicleType));

        public VehicleType SelectedVehicleType
        {
            get { return _selectedVehicleType; }
            set
            {
                if (_selectedVehicleType != value)
                {
                    _selectedVehicleType = value;
                    OnPropertyChanged(nameof(SelectedVehicleType));
                }
            }
        }

        public ReportType SelectedReportType
        {
            get { return _selectedReportType; }
            set
            {
                if (_selectedReportType != value)
                {
                    _selectedReportType = value;
                    OnPropertyChanged(nameof(SelectedReportType));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var vehicleRepository = new InMemoryRepository<Vehicle>();
            vehicleService = new VehicleService(vehicleRepository);
            SelectedReportType = ReportType.Json;

            RefreshVehicleList();
        }

        private void LoadCargoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Vehicle selectedVehicle = ListViewVehicle.SelectedItem as Vehicle;
                if (selectedVehicle == null)
                {
                    MessageBox.Show("Please select a vehicle", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                List<Cargo> existingCargos = selectedVehicle.Cargos;

                CargoWindow cargoWindow = new CargoWindow(existingCargos);
                cargoWindow.ShowDialog();

                if (cargoWindow.IsDataSet)
                {
                    CargoManagementResult cargoResult = cargoWindow.Result;
                    Cargo newCargo = cargoWindow.NewCargo;
                    Guid unloadedCargoId = cargoWindow.UnloadedCargoId;

                    switch (cargoResult)
                    {
                        case CargoManagementResult.LoadNewCargo:
                            try
                            {
                                vehicleService.LoadCargo(newCargo, selectedVehicle.Id);
                                RefreshVehicleList();
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            break;

                        case CargoManagementResult.UnloadExistingCargo:
                            try
                            {
                                vehicleService.UnloadCargo(unloadedCargoId, selectedVehicle.Id);
                                RefreshVehicleList();
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                            break;
                        case CargoManagementResult.None:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    ImportTextBox.Text = filePath;

                    try
                    {
                        ReportService<Vehicle> reportService = new ReportService<Vehicle>();
                        List<Vehicle> importedVehicles = reportService.LoadReport(filePath);

                        ImportListView.ItemsSource = importedVehicles;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing report: {ex.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedVehicle != null)
                {
                    int vehicleId = SelectedVehicle.Id;

                    vehicleService.Delete(vehicleId);

                    RefreshVehicleList();
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VehicleType selectedType = (VehicleType)ComboBoxVehicleType.SelectedItem;
                string number = TextBoxNumber.Text;
                int maxCargoWeight = int.Parse(TextBoxMaxWeight.Text);
                double maxCargoVolume = double.Parse(TextBoxMaxVolume.Text);

                Vehicle newVehicle = new Vehicle(selectedType, number, maxCargoWeight, maxCargoVolume);
                vehicleService.Create(newVehicle);

                RefreshVehicleList();
                ClearControls();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format. Please enter valid values.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshVehicleList() 
        {
            var listOfVehicles = vehicleService.GetAll();
            _vehicleList = new ObservableCollection<Vehicle>(listOfVehicles);
            ListViewVehicle.ItemsSource = _vehicleList;
            VehicleListBox.ItemsSource = _vehicleList;
            OnPropertyChanged(nameof(ListViewVehicle));
        }

        private void ClearControls()
        {
            TextBoxNumber.Text = string.Empty;
            TextBoxMaxWeight.Text = string.Empty;
            TextBoxMaxVolume.Text = string.Empty;
            ComboBoxVehicleType.SelectedItem = null;
        }

        private void ListViewVehicle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Vehicle selectedVehicle)
            {
                TextBoxNumber.Text = selectedVehicle.Number;
                TextBoxMaxWeight.Text = selectedVehicle.MaxCargoWeightKg.ToString();
                TextBoxMaxVolume.Text = selectedVehicle.MaxCargoVolume.ToString();
                ComboBoxVehicleType.SelectedItem = selectedVehicle.Type;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshVehicleList();
            ClearControls();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedVehicle != null)
                {
                    int vehicleId = SelectedVehicle.Id;

                    VehicleType selectedType = (VehicleType)ComboBoxVehicleType.SelectedItem;
                    string number = TextBoxNumber.Text;
                    int maxCargoWeight = int.Parse(TextBoxMaxWeight.Text);
                    double maxCargoVolume = double.Parse(TextBoxMaxVolume.Text);

                    Vehicle updatedVehicle = new Vehicle(selectedType, number, maxCargoWeight, maxCargoVolume);
                    updatedVehicle.Id = vehicleId;

                    vehicleService.Update(updatedVehicle);

                    RefreshVehicleList();
                    ClearControls();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid input format. Please enter valid values.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTab && selectedTab.Header.ToString() == "Report")
            {
                RefreshVehicleList();
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportType selectedReportType = SelectedReportType;
                List<Vehicle> selectedVehicles = _vehicleList.ToList();
                ReportService<Vehicle> reportService = new ReportService<Vehicle>();

                if (selectedVehicles.Count > 0)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = $"Report files (*.{selectedReportType.ToString().ToLower()})|*.{selectedReportType.ToString().ToLower()}";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string reportFilePath = saveFileDialog.FileName;
                        reportService.CreateReport(selectedVehicles, selectedReportType);
                        ExportTextBox.Text = reportFilePath;
                        MessageBox.Show($"{selectedReportType} report exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
