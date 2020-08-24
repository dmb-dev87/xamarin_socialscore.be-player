using System;
using Xamarin.Forms;
using SocialScore.Models;
using System.Diagnostics;

using System.Net.Http;
using Newtonsoft.Json;

using SocialScore.Services;

namespace SocialScore
{
    public partial class App : Application
    {
        public static ResultInfo appInfo { get; set; }

        public static bool ImInPinView;

        public static ManagePreferences mngPreferences;

        private static Stopwatch stopwatch = new Stopwatch();
        private const int defaultTimespan = 2;

        public App()
        {
            InitializeComponent();
            mngPreferences = new ManagePreferences();
        }

        protected override void OnStart()
        {
            bool isSuccess = mngPreferences.GetPreferenceBoolValue("key_IsSuccess");

            if (!isSuccess)
            {
                MainPage = new NavigationPage(new Views.PinPage());
            }
            else
            {
                var navPage = new NavigationPage(new Views.MainPage());
                MainPage = navPage;
            }

            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (stopwatch.IsRunning && stopwatch.Elapsed.Minutes >= defaultTimespan)
                {
                    CheckUpdate();
                    stopwatch.Restart();
                }
                return true;
            });
        }

        protected override void OnSleep()
        {
            stopwatch.Reset();
        }

        protected override void OnResume()
        {
            stopwatch.Start();
        }

        protected async void CheckUpdate()
        {
            string guid = mngPreferences.GetPrefereceStringValue("key_Guid");
            string lastUpdate = mngPreferences.GetPrefereceStringValue("key_LastUpdate");

            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://api.socialscore.be/api/v1/tv/projectinfo/" + guid);

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                var result = await response.Content.ReadAsStringAsync();

                ProjectInfo projectInfo = JsonConvert.DeserializeObject<ProjectInfo>(result);

                if (projectInfo.lastUpdate != lastUpdate)
                {
                    mngPreferences.SaveProjectInfo(projectInfo);

                    IFileService fileService = DependencyService.Get<IFileService>();
                    DownloadService downloadService = new DownloadService(fileService);
                    await downloadService.DownloadFileAsync(App.appInfo.projectInfo.url);

                    Type CurPageType = MainPage.GetType();

                    if (ImInPinView == false)
                    {
                        MainPage = new NavigationPage(new Views.MainPage());
                    }
                    
                } 
            }
            catch (Exception er)
            {
                var lb = er.ToString();
            }
        }
    }
}
