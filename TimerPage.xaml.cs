using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace clock.UWP
{
    public sealed partial class TimerPage : Page
    {
        private DispatcherTimer _timer;
        private TimeSpan _remaining;
        private bool _running = false;

        public TimerPage()
        {
            this.InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            UpdateDisplay();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_remaining.TotalSeconds <= 0)
            {
                _timer.Stop();
                _running = false;
                TimerStateText.Text = "Finished";
                StartStopBtn.Content = "Start";

                MinutesInput.Text = "0";
                SecondsInput.Text = "0";

                // Show Windows toast notification
                ShowTimerFinishedNotification();

                return;
            }

            _remaining = _remaining.Subtract(TimeSpan.FromSeconds(1));
            UpdateDisplay();
        }

        private void StartStopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!_running)
            {
                // Starting or continuing
                if (_remaining.TotalSeconds <= 0)
                {
                    // First start: parse inputs
                    int.TryParse(MinutesInput.Text, out int minutes);
                    int.TryParse(SecondsInput.Text, out int seconds);

                    minutes = Math.Max(0, minutes);
                    seconds = Math.Max(0, Math.Min(59, seconds));

                    _remaining = TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);

                    if (_remaining.TotalSeconds <= 0)
                    {
                        TimerStateText.Text = "Enter a valid time";
                        return;
                    }
                }

                _timer.Start();
                _running = true;
                TimerStateText.Text = "Running";
                StartStopBtn.Content = "Stop";
            }
            else
            {
                // Pause
                _timer.Stop();
                _running = false;
                TimerStateText.Text = "Paused";
                StartStopBtn.Content = "Continue";
            }

            UpdateDisplay();
        }

        private void ResetTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _running = false;
            _remaining = TimeSpan.Zero;
            TimerStateText.Text = "";
            StartStopBtn.Content = "Start";

            MinutesInput.Text = "0";
            SecondsInput.Text = "0";

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            TimerDisplay.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", _remaining.Hours, _remaining.Minutes, _remaining.Seconds);
        }

        private void ShowTimerFinishedNotification()
        {
            string toastXmlString =
                $@"<toast>
                        <visual>
                            <binding template='ToastGeneric'>
                                <text>Timer Finished</text>
                                <text>Your timer has completed.</text>
                            </binding>
                        </visual>
                        <actions>
                            <action content='OK' activationType='foreground' arguments='action=ok'/>
                        </actions>
                    </toast>";

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastXmlString);

            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
