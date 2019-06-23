using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

using WeightApp.Client.Internal;
using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class AddMealViewModel : INotifyPropertyChanged
    {
        private IWeightAppClient _client;
        private MealInstanceModel _model;
        private ProductModel _selectedProduct;
        private MealTypeModel _selectedMealType;

        public AddMealViewModel() : this(
            null, 
            new MealInstanceModel(), 
            Enumerable.Empty<ProductModel>(),
            Enumerable.Empty<MealTypeModel>())
        {
        }

        public AddMealViewModel(
            IWeightAppClient client, 
            MealInstanceModel model, 
            IEnumerable<ProductModel> products, 
            IEnumerable<MealTypeModel> mealTypes)
        {
            _client = client;
            _model = model;
            Products = new ObservableCollection<ProductModel>(products);
            MealTypes = new ObservableCollection<MealTypeModel>(mealTypes);

            AddMeal = new RelayCommand(
                AddMealExecute, 
                p => _selectedProduct != null && _selectedMealType != null
                );
            Cancel = new RelayCommand(CancelExecute);
        }

        private void CancelExecute(object obj)
        {
            if (obj is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        public int Amount
        {
            get => _model.Amount;
            set
            {
                _model.Amount = value;
                NotifyPropertyChanged(nameof(Amount));
            }
        }

        public DateTime Date
        {
            get => _model.Date;
            set
            {
                _model.Date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
            }
        }

        public MealTypeModel SelectedMealType
        {
            get => _selectedMealType;
            set
            {
                _selectedMealType = value;
                NotifyPropertyChanged(nameof(SelectedMealType));
            }
        }

        public ObservableCollection<ProductModel> Products { get; set; }
        public ObservableCollection<MealTypeModel> MealTypes { get; set; }

        public RelayCommand AddMeal { get; set; }
        public RelayCommand Cancel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddMealExecute(object obj)
        {
            _client.Goals.AddMeal(
                new AddMealRequest
                {
                    GoalId = _model.GoalId,
                    MealTypeId = _selectedMealType.Id,
                    ProductId = _selectedProduct.Id,
                    Amount = Amount,
                    Date = Date
                });

            if (obj is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}