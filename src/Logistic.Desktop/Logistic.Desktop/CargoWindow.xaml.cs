using Logistic.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Logistic.Desktop
{
    /// <summary>
    /// Interaction logic for Cargo.xaml
    /// </summary>
    public partial class CargoWindow : Window
    {
        public string SomeData = "Constant data";
        public bool IsDataSet = false;
        public List<Cargo> ExistingCargos { get; }
        public CargoManagementResult Result { get; set; } = CargoManagementResult.None;
        public Guid UnloadedCargoId { get; set; } = Guid.Empty;
        public Cargo NewCargo { get; set; }

        public CargoWindow(List<Cargo> existingCargos)
        {
            InitializeComponent();
            ExistingCargos = existingCargos;
            DataContext = this;
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (double.TryParse(TextBoxVolume.Text, out double volume) &&
                    int.TryParse(TextBoxWeight.Text, out int weight) &&
                    !string.IsNullOrWhiteSpace(TextBoxCode.Text) &&
                    !string.IsNullOrWhiteSpace(TextBoxRecipientAddress.Text) &&
                    !string.IsNullOrWhiteSpace(TextBoxSenderAddress.Text) &&
                    !string.IsNullOrWhiteSpace(TextBoxRecipientPhoneNumber.Text) &&
                    !string.IsNullOrWhiteSpace(TextBoxSenderPhoneNumber.Text))
                {
                    string code = TextBoxCode.Text;
                    string recipientAddress = TextBoxRecipientAddress.Text;
                    string senderAddress = TextBoxSenderAddress.Text;
                    string recipientPhoneNumber = TextBoxRecipientPhoneNumber.Text;
                    string senderPhoneNumber = TextBoxSenderPhoneNumber.Text;

                    Invoice invoice = new Invoice(recipientAddress, senderAddress, recipientPhoneNumber, senderPhoneNumber);
                    Cargo newCargo = new Cargo(volume, weight, code, invoice);
                    Result = CargoManagementResult.LoadNewCargo;
                    IsDataSet = true;
                    NewCargo = newCargo;

                    Close();
                }
                else
                {
                    MessageBox.Show("Please enter all required information.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the cargo data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnloadSelectedCargoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListBoxExistingCargo.SelectedItem is Cargo selectedCargo)
                {
                    Guid cargoId = selectedCargo.Id;
                    UnloadedCargoId = cargoId;

                    Result = CargoManagementResult.UnloadExistingCargo;
                    IsDataSet = true;

                    Close();
                }
                else
                {
                    MessageBox.Show("Please select a cargo to unload.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while unloading the cargo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = CargoManagementResult.None;
            IsDataSet = true;

            Close();
        }
    }
}
