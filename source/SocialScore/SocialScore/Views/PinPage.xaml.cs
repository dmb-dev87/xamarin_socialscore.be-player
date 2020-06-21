using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

using Newtonsoft.Json;
using SocialScore.Models;
using SocialScore.Services;
using System.IO;

namespace SocialScore.Views
{
    public partial class PinPage : ContentPage
    {
        private string pin_code;
        public PinPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            bool isSuccess = App.mngPreferences.GetPreferenceBoolValue("key_IsSuccess");

            if (isSuccess)
            {
                string video_path = App.mngPreferences.GetPrefereceStringValue("video_Path");
                
                if (File.Exists(video_path))
                {
                    File.Delete(video_path);
                }

                App.mngPreferences.ClearAll();
            }
            
            entry_pin_code.Focus();
            entry_pin_code.CursorPosition = 0;

            App.ImInPinView = true;
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<IAndroidMethods>().CloseApp();
            return base.OnBackButtonPressed();
        }

        protected async override void OnDisappearing()
        {
            App.ImInPinView = false;
            base.OnDisappearing();
        }

        void OnPinEditorChanged(object sender, TextChangedEventArgs e)
        {
            pin_code = e.NewTextValue;
        }

        public void OnEnterPressed(object sender, EventArgs e)
        {
            PostPinCode();
        }

        private async void PostPinCode()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://api.socialscore.be/api/v1/tv/Pincode/" + pin_code);

            try
            {
                StringContent content = new StringContent("", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);

                var result = await response.Content.ReadAsStringAsync();

                App.appInfo = JsonConvert.DeserializeObject<ResultInfo>(result);

                IFileService fileService = DependencyService.Get<IFileService>();
                DownloadService downloadService = new DownloadService(fileService);
                await downloadService.DownloadFileAsync(App.appInfo.projectInfo.url);

                if (App.appInfo.isSuccess)
                {
                    await Navigation.PopAsync();
                    App.mngPreferences.SaveAllInfo(App.appInfo);
                    Page page = new MainPage();
                    await Navigation.PushAsync(page);
                } else {
                    await DisplayAlert(null, "Your PIN code is not valid. Please rewrite your PIN code.", "OK");
                    entry_pin_code.Focus();
                    entry_pin_code.CursorPosition = 5;
                }
            }
            catch (Exception er)
            {
                var lb = er.ToString();
                await DisplayAlert(null, "The server is not responding. Just a second and retry.", "OK");
                entry_pin_code.Focus();
                entry_pin_code.CursorPosition = 5;
            }
        }
    }    
}