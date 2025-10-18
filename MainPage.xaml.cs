using System;
using Windows.UI.Xaml.Controls;

namespace clock.UWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Navigate to Timer page by default
            ContentFrame.Navigate(typeof(TimerPage));

            // Highlight Timer item in NavigationView
            NavView.SelectedItem = TimerNavItem;
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            var invoked = args.InvokedItemContainer as NavigationViewItem;
            if (invoked == null) return;
            var tag = invoked.Tag?.ToString();

            switch (tag)
            {
                case "timer":
                    ContentFrame.Navigate(typeof(TimerPage));
                    NavView.SelectedItem = TimerNavItem;
                    break;
                case "stopwatch":
                    ContentFrame.Navigate(typeof(StopwatchPage));
                    NavView.SelectedItem = StopwatchNavItem;
                    break;
            }
        }
    }
}
