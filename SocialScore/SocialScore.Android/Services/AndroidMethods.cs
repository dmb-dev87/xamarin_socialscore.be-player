using Xamarin.Forms;
using SocialScore.Services;

[assembly: Dependency(typeof(SocialScore.Droid.Services.AndroidMethods))]

namespace SocialScore.Droid.Services
{
    public class AndroidMethods : IAndroidMethods
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}