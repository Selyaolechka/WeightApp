using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

using Microsoft.Rest;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

using WeightApp.Client.Internal;
using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private IWeightAppClient _client;
        private ProductModel _selectedProduct;
        private GoalModel _selectedGoal;
        private ProgressViewModel _selectedProgress;
        private MealViewModel _selectedMeal;
        private DateTime _selectedDailyMeal = DateTime.Today;
        private IEnumerable<ProgressModel> _progress;
        private IEnumerable<MealTypeModel> _mealTypes;
        private ObservableCollection<DailyInfoViewModel> _dailyInfo;

        public UserViewModel()
        {
            var credentials = new BasicAuthenticationCredentials();
            credentials.UserName = "user@example.com";
            credentials.Password = "p";

            _client = new WeightAppClient(new Uri("https://localhost:5001"), credentials);
            var goals = _client.Goals.GetGoals();
            Goals = new ObservableCollection<GoalModel>(goals);

            var products = _client.Products.GetProducts();
            Products = new ObservableCollection<ProductModel>(products);

            var mealTypes = _client.ReferenceData.GetMealTypes();
            _mealTypes = new List<MealTypeModel>(mealTypes);

            var progressObj = _client.Goals.GetProgress();

            if (progressObj is IList<ProgressModel> progress)
            {
                _progress = progress;
            }

            if (products.Any())
            {
                SelectedProduct = products.First();
            }

            Meals = new ObservableCollection<MealViewModel>();
            MealsView = CollectionViewSource.GetDefaultView(Meals);
            MealsView.GroupDescriptions.Add(new PropertyGroupDescription("MealType"));
            MealsView.Filter = FilterMealsView;

            _dailyInfo = new ObservableCollection<DailyInfoViewModel>();
            DailyInfoView = CollectionViewSource.GetDefaultView(_dailyInfo);
            DailyInfoView.Filter = FilterDailyInfoView;

            AddProduct = new RelayCommand(AddProductExecute);
            RemoveProduct = new RelayCommand(RemoveProductExecute, p => SelectedProduct != null);

            TrackProgress = new RelayCommand(TrackProgressExecute, p => SelectedGoal != null);
            RemoveProgress = new RelayCommand(RemoveProgressExecute, p => SelectedProgress != null);

            AddNewMeal = new RelayCommand(AddNewMealExecute, p => SelectedGoal != null);
        }

        private void AddNewMealExecute(object obj)
        {
            var addNewMealModel = new MealInstanceModel { Date = DateTime.Now, GoalId = _selectedGoal.GoalId };
            var addNewMealViewModel = new AddMealViewModel(
                _client,
                addNewMealModel,
                Products,
                _mealTypes
                );
            var dialog = new AddNewMeal();
            dialog.DataContext = addNewMealViewModel;
            var result = dialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                UpdateMeals();
            }
        }

        private bool FilterDailyInfoView(object obj)
        {
            if (obj is DailyInfoViewModel mvm)
            {
                return (mvm.Date - SelectedDailyMeal).Days == 0;
            }

            return false;
        }

        private bool FilterMealsView(object obj)
        {
            if (obj is MealViewModel mvm)
            {
                return (mvm.Date - SelectedDailyMeal).Days == 0;
            }

            return false;
        }

        private void RemoveProgressExecute(object obj)
        {
            try
            {
                var result = _client.Goals.RemoveProgress(SelectedProgress.Model.ProgressId);
                Progress.Remove(SelectedProgress);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TrackProgressExecute(object obj)
        {
            var progressModel = new ProgressModel { GoalId = SelectedGoal.GoalId, Date = DateTime.Now };
            var newProgessViewModel = new AddProgressViewModel(progressModel, _client);
            
            var dialog = new TrackProgress();
            dialog.DataContext = newProgessViewModel;
            var result = dialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                Progress.Add(new ProgressViewModel(SelectedGoal, newProgessViewModel.Progress));
            }
        }

        private void RemoveProductExecute(object obj)
        {
            try
            {
                _client.Products.RemoveProduct(SelectedProduct.Id);
            }
            catch
            { }
        }

        private void AddProductExecute(object obj)
        {
            var newProduct = new ProductViewModel { Name = "Hi!" };
            var p = new NewProduct();
            p.DataContext = newProduct;
            var result = p.ShowDialog();

            if (result.GetValueOrDefault())
            {
                Products.Add(newProduct.Product);
            }
        }

        public double DailyNorm => ((DailyInfoViewModel)DailyInfoView.CurrentItem)?.DailyLimit ?? 0;

        public double Consumed => ((DailyInfoViewModel)DailyInfoView.CurrentItem)?.Consumed ?? 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand TrackProgress { get; set; }

        public RelayCommand RemoveProgress { get; set; }

        public RelayCommand AddProduct { get; set; }

        public RelayCommand RemoveProduct { get; set; }

        public RelayCommand AddNewMeal { get; set; }

        public RelayCommand RemoveMeal { get; set; }

        public ObservableCollection<MealViewModel> Meals { get; set; }

        public ICollectionView MealsView { get; }

        public ObservableCollection<ProgressViewModel> Progress { get; set; }

        public ObservableCollection<GoalModel> Goals { get; set; }

        public PlotModel ProgressPlot
        {
            get => BuildPlotModel();
        }

        private PlotModel BuildPlotModel()
        {
            var plot = new PlotModel { Title = "Your progress" };

            if (_selectedGoal == null)
                return plot;

            plot.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left, 
                    Maximum = _selectedGoal.Weight + 10,
                    Minimum = _selectedGoal.WeightGoal - 10
                });
            plot.Axes.Add(new DateTimeAxis {Position = AxisPosition.Bottom, IntervalType = DateTimeIntervalType.Days});

            var data = new Collection<ProgressModel>();

            var series = new LineSeries
            {
                Title = "Weight",
                DataFieldX = "Date",
                DataFieldY = "Weight",
                ItemsSource = data
            };

            foreach (var progress in _progress.OrderBy(p => p.Date))
            {
                data.Add(progress);
            }

            plot.Series.Add(series);

            return plot;
        }

        public DateTime SelectedDailyMeal
        {
            get => _selectedDailyMeal;
            set
            {
                _selectedDailyMeal = value;
                NotifyPropertyChanged(nameof(SelectedDailyMeal));
                MealsView.Refresh();
                DailyInfoView.Refresh();
                DailyInfoView.MoveCurrentToFirst();
                NotifyPropertyChanged(nameof(DailyNorm));
                NotifyPropertyChanged(nameof(Consumed));
            }
        }

        public ICollectionView DailyInfoView { get; set; }

        public MealViewModel SelectedMeal
        {
            get => _selectedMeal;
            set
            {
                _selectedMeal = value;
                NotifyPropertyChanged(nameof(SelectedMeal));
            }
        }

        public ProgressViewModel SelectedProgress
        {
            get => _selectedProgress;
            set
            {
                _selectedProgress = value;
                NotifyPropertyChanged(nameof(SelectedProgress));
            }
        }

        public GoalModel SelectedGoal
        {
            get => _selectedGoal;
            set 
            {
                _selectedGoal = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
                NotifyPropertyChanged(nameof(ProgressPlot));

                if (_selectedGoal != null)
                {
                    Progress = new ObservableCollection<ProgressViewModel>(
                        _progress
                            .Where(p => p.GoalId == _selectedGoal.GoalId)
                            .Select(p => new ProgressViewModel(_selectedGoal, p))
                        );
                    NotifyPropertyChanged(nameof(Progress));

                    UpdateMeals();
                }
            }
        }

        private void UpdateMeals()
        {
            var mealsObj = _client.Goals.GetMealsForGoal(_selectedGoal.GoalId);

            if (mealsObj is IList<DailyMealInstanceModel> meals)
            {
                _dailyInfo.Clear();
                Meals.Clear();

                foreach (var dm in meals)
                {
                    _dailyInfo.Add(new DailyInfoViewModel(dm));

                    foreach (var meal in dm.Meals)
                    {
                        var product = Products.First(p => p.Id == meal.Product.ProductId);
                        Meals.Add(new MealViewModel(meal, product));
                    }
                }

                DailyInfoView.MoveCurrentToFirst();
                NotifyPropertyChanged(nameof(DailyNorm));
                NotifyPropertyChanged(nameof(Consumed));
            }
        }

        public ObservableCollection<ProductModel> Products { get; set; }

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyPropertyChanged(nameof(SelectedProduct));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}