using FIND_Breda.Common;
using FIND_Breda.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FIND_Breda.Screen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedbackView : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        public FeedbackView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FeedbackTextBlock.Text = LanguageModel.instance.getText(Text.feedbacktext);
            SendFeedbackButton.Content = LanguageModel.instance.getText(Text.sendfeedbackbutton);
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void ComposeEmail(string messageBody)
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = "developers@find-breda.com"
            };
            EmailMessage email = new EmailMessage();
            email.Body = messageBody;
            email.Subject = "FIND-Breda feedback";
            email.To.Add(sendTo);

            await EmailManager.ShowComposeNewEmailAsync(email);
        }

        private async void SendFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedbackTextBox.Text != string.Empty)
            {
                ComposeEmail(FeedbackTextBox.Text);
                FeedbackTextBox.Text = string.Empty;
            }
            else
            {
                MessageDialog dialog = new MessageDialog(LanguageModel.instance.getText(Text.feedbackerror));
                await dialog.ShowAsync();
            }
        }
    }
}
