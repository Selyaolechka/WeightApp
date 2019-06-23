using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

using WeightApp.Client.Internal;
using WeightApp.Client.Models;

namespace WeightApp.Client.ViewModels
{
    public class AddProgressViewModel : INotifyPropertyChanged
    {
        private IWeightAppClient _client;
        private ProgressModel _progress;

        public AddProgressViewModel() : this(new ProgressModel(), null)
        {
        }

        public AddProgressViewModel(ProgressModel progress, IWeightAppClient client)
        {
            _progress = progress;
            _client = client;

            TrackProgress = new RelayCommand(TrackProgressExecute);
            Cancel = new RelayCommand(CancelExecute);
        }

        private void TrackProgressExecute(object obj)
        {
            var result = _client.Goals.TrackProgress(new TrackProgressRequest(_progress.GoalId, Weight, Date));

            if (result is ProgressModel progress)
            {
                _progress.ProgressId = progress.ProgressId;
            }

            if (result is IDictionary<string, object> err)
            {
                var message = "Failed to save progress.\n" + string.Join(
                    "\n",
                    err.Select(p => $"{p.Key}:{p.Value?.ToString()}")
                    );
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (obj is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void CancelExecute(object obj)
        {
            if (obj is NewProduct window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        public RelayCommand TrackProgress { get; set; }
        public RelayCommand Cancel { get; set; }

        public ProgressModel Progress => _progress;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}