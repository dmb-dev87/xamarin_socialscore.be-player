using System;
using Xamarin.Forms;
using SocialScore.Services;

[assembly: Dependency(typeof(SocialScore.Droid.Services.FileService))]

namespace SocialScore.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetStorageFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}