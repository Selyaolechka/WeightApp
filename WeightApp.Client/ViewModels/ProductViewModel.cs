using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Microsoft.Rest;

using WeightApp.Client.Internal;
using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private IWeightAppClient _client;
        private ProductModel _product = new ProductModel();

        public ProductViewModel()
        {
            var credentials = new BasicAuthenticationCredentials();
            credentials.UserName = "user@example.com";
            credentials.Password = "p";

            _client = new WeightAppClient(new Uri("https://localhost:5001"), credentials);
            var categories = _client.ReferenceData.GetProductCategories();
            Categories = new ObservableCollection<ProductCategoryModel>(categories);

            AddNewProduct = new RelayCommand(AddNewProductExecute, AddProductCanExecute);
            Cancel = new RelayCommand(CancelExecute);
        }

        private bool AddProductCanExecute(object obj)
        {
            if (SelectedCategory == null)
                return false;

            if (string.IsNullOrWhiteSpace(Name))
                return false;

            return true;
        }

        private void CancelExecute(object obj)
        {
            if (obj is NewProduct window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void AddNewProductExecute(object obj)
        {
            var result = _client.Products.AddProduct(
                new AddProductRequest(Name, SelectedCategory.Id, Calories, Carbohydrates, Proteins, Fats)
                );
            _product.Id = result.Id;

            if (obj is NewProduct window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand AddNewProduct { get; set; }
        public RelayCommand Cancel { get; set; }

        public ProductModel Product
        {
            get => _product;
        }

        public ObservableCollection<ProductCategoryModel> Categories { get; set; }

        public string Name
        {
            get => _product.Name;
            set
            {
                _product.Name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public ProductCategoryModel SelectedCategory
        {
            get => _product.ProductCategory;
            set
            {
                _product.ProductCategory = value;
                NotifyPropertyChanged(nameof(SelectedCategory));
            }
        }

        public int Calories
        {
            get => _product.Calories;
            set
            {
                _product.Calories = value;
                NotifyPropertyChanged(nameof(Calories));
            }
        }

        public int Carbohydrates
        {
            get => _product.Carbohydrates;
            set
            {
                _product.Carbohydrates = value;
                NotifyPropertyChanged(nameof(Carbohydrates));
            }
        }

        public int Proteins
        {
            get => _product.Proteins;
            set
            {
                _product.Proteins = value;
                NotifyPropertyChanged(nameof(Proteins));
            }
        }

        public int Fats
        {
            get => _product.Fats;
            set
            {
                _product.Fats = value;
                NotifyPropertyChanged(nameof(Fats));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}