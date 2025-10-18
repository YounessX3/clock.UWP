using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace clock.UWP
{
    public sealed partial class StopwatchPage : Page
    {
        private Stopwatch _stopwatch = new Stopwatch();
        private DispatcherTimer _uiTimer;
        private ObservableCollection<string> _laps = new ObservableCollection<string>();


        public StopwatchPage()
        {
            this.InitializeComponent();
            _uiTimer = new DispatcherTimer();
            _uiTimer.Interval = TimeSpan.FromMilliseconds(100);
            _uiTimer.Tick += UiTimer_Tick;
            LapsListView.ItemsSource = _laps;
            UpdateDisplay();
        }


        private void UiTimer_Tick(object sender, object e)
        {
            UpdateDisplay();
        }


        private void UpdateDisplay()
        {
            var ts = _stopwatch.Elapsed;
            StopwatchDisplay.Text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D2}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }


        private void StartStopwatchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                _uiTimer.Start();
                StartStopwatchBtn.Content = "Stop";
            }
            else
            {
                _stopwatch.Stop();
                _uiTimer.Stop();
                StartStopwatchBtn.Content = "Start";
            }
        }


        private void LapBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_stopwatch.IsRunning) return;
            _laps.Insert(0, string.Format("Lap {0} — {1}", _laps.Count + 1, FormatTime(_stopwatch.Elapsed)));
        }


        private void ResetStopwatchBtn_Click(object sender, RoutedEventArgs e)
        {
            _stopwatch.Reset();
            _uiTimer.Stop();
            StartStopwatchBtn.Content = "Start";
            _laps.Clear();
            UpdateDisplay();
        }


        private string FormatTime(TimeSpan ts)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D2}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }
}