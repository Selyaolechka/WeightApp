using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class MealViewModel : INotifyPropertyChanged
    {
        private MealInstanceModel _model;
        private ProductModel _products;

        public MealViewModel(MealInstanceModel model, ProductModel products)
        {
            _model = model;
            _products = products;
        }

        public string MealType => _model.MealType.Name;

        public string Product => _model.Product.Name;

        // public int GoalId => _model.GoalId;

        public int Amount => _model.Amount;

        public int Calories => _model.Amount * _products.Calories / 100;

        public int Proteins => _model.Amount * _products.Proteins / 100;

        public int Carbohydrates => _model.Amount * _products.Carbohydrates / 100;

        public int Fats => _model.Amount * _products.Fats / 100;

        public DateTime Date => _model.Date;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}