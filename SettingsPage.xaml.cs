using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Storage;
using Windows.ApplicationModel;

namespace clock.UWP
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            ApplySavedTheme();
            LoadAppInfo();
        }

        private void LoadAppInfo()
        {
            var version = Package.Current.Id.Version;
            AppVersionTextBlock.Text = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private void ApplySavedTheme()
        {
            var settings = ApplicationData.Current.LocalSettings;
            string saved = settings.Values["AppTheme"] as string;

            ElementTheme themeToApply = ElementTheme.Default;

            if (saved == "Light") themeToApply = ElementTheme.Light;
            else if (saved == "Dark") themeToApply = ElementTheme.Dark;
            else if (saved == "System")
            {
                var uiSettings = new UISettings();
                var bg = uiSettings.GetColorValue(UIColorType.Background);
                bool isDark = bg.R + bg.G + bg.B < 384;
                themeToApply = isDark ? ElementTheme.Dark : ElementTheme.Light;
            }

            ((FrameworkElement)Window.Current.Content).RequestedTheme = themeToApply;

            switch (saved)
            {
                case "Light":
                    ThemeComboBox.SelectedIndex = 1;
                    break;
                case "Dark":
                    ThemeComboBox.SelectedIndex = 2;
                    break;
                default:
                    ThemeComboBox.SelectedIndex = 0;
                    break;
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ThemeComboBox.SelectedItem as ComboBoxItem;
            string selected = selectedItem?.Content.ToString();
            if (selected == null) return;

            ApplicationData.Current.LocalSettings.Values["AppTheme"] = selected;

            ElementTheme newTheme = ElementTheme.Default;

            switch (selected)
            {
                case "System Default":
                    var uiSettings = new UISettings();
                    var bg = uiSettings.GetColorValue(UIColorType.Background);
                    bool isDark = bg.R + bg.G + bg.B < 384;
                    newTheme = isDark ? ElementTheme.Dark : ElementTheme.Light;
                    break;

                case "Light":
                    newTheme = ElementTheme.Light;
                    break;

                case "Dark":
                    newTheme = ElementTheme.Dark;
                    break;
            }

            ((FrameworkElement)Window.Current.Content).RequestedTheme = newTheme;

            App.ApplyTitleBarButtonColors(); // 🔥 Sync title bar
        }
    }
}
