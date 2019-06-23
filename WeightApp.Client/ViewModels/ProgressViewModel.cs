using System;
using System.ComponentModel;

using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        private ProgressModel _progress;
        private GoalModel _goal;

        public ProgressViewModel(GoalModel goal, ProgressModel progress)
        {
            _progress = progress;
            _goal = goal;
        }

        public ProgressModel Model => _progress;

        public DateTime Date
        {
            get => _progress.Date;
            set
            {
                _progress.Date = value;
                NotifyPropertyChanged(nameof(Date));
            }
        }

        public int Weight
        {
            get => _progress.Weight;
            set
            {
                _progress.Weight = value;
                NotifyPropertyChanged(nameof(Weight));
            }
        }

        public int WayToGo => _progress.Weight - _goal.WeightGoal;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}