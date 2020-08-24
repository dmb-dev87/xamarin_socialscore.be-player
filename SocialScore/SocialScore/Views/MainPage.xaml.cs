using System;
using Xamarin.Forms;
using FormsVideoLibrary;
using SocialScore.Services;

namespace SocialScore.Views
{
	public partial class MainPage : ContentPage
	{
        public string videoPath { get; set; }

		public MainPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);			
		}

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                string action = await DisplayActionSheet("What do you want?", "Cancel", null, "Disconnect", "Continue", "Close");

                if (action == "Disconnect")
                {
                    await Navigation.PopAsync();
                    Page page = new PinPage();
                    await Navigation.PushAsync(page);
                }
                else if (action == "Close")
                {
                    DependencyService.Get<IAndroidMethods>().CloseApp();                    
                    base.OnBackButtonPressed();
                }
            });

            return true;
        }

        protected override void OnAppearing()
        {
            videoPath = App.mngPreferences.GetPrefereceStringValue("video_Path");

            if (!String.IsNullOrWhiteSpace(videoPath))
            {
                videoPlayer.Source = new FileVideoSource
                {
                    File = videoPath
                };
            }
            else
            {
                Navigation.PopAsync();
                Page page = new PinPage();
                Navigation.PushAsync(page);
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}
