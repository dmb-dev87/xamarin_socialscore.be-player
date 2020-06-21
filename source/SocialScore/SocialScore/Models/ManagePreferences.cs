using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SocialScore.Models    
{
    public class ManagePreferences
    {
        public void SetPreferenceBoolValue(string key, bool value)
        {
            Preferences.Set(key, value);
        }
        public void SetPreferenceIntValue(string key, int value)
        {
            Preferences.Set(key, value);
        }
        public void SetPreferenceStringValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public bool GetPreferenceBoolValue(string key)
        {
            return Preferences.Get(key, false);
        }
        public string GetPrefereceStringValue(string key)
        {
            return Preferences.Get(key, "");
        }

        public void SaveAllInfo(ResultInfo appInfo)
        {
            SetPreferenceBoolValue("key_IsSuccess", appInfo.isSuccess);
            SetPreferenceStringValue("key_Guid", appInfo.guid);
            SetPreferenceStringValue("key_Name", appInfo.projectInfo.name);
            SetPreferenceStringValue("key_Url", appInfo.projectInfo.url);
            SetPreferenceIntValue("key_Id", appInfo.projectInfo.id);
            SetPreferenceStringValue("key_LastUpdate", appInfo.projectInfo.lastUpdate);
            SetPreferenceIntValue("key_Width", appInfo.projectInfo.width);
            SetPreferenceIntValue("key_Height", appInfo.projectInfo.height);
        }

        public void SaveProjectInfo(ProjectInfo projectInfo)
        {
            SetPreferenceStringValue("key_Name", projectInfo.name);
            SetPreferenceStringValue("key_Url", projectInfo.url);
            SetPreferenceIntValue("key_Id", projectInfo.id);
            SetPreferenceStringValue("key_LastUpdate", projectInfo.lastUpdate);
            SetPreferenceIntValue("key_Width", projectInfo.width);
            SetPreferenceIntValue("key_Height", projectInfo.height);
        }

        public void ClearAll()
        {
            SetPreferenceStringValue("video_Path", "");
            SetPreferenceBoolValue("key_IsSuccess", false);
            SetPreferenceStringValue("key_Guid", "");
            SetPreferenceStringValue("key_Name", "");
            SetPreferenceStringValue("key_Url", "");
            SetPreferenceIntValue("key_Id", 0);
            SetPreferenceStringValue("key_LastUpdate", "");
            SetPreferenceIntValue("key_Width", 0);
            SetPreferenceIntValue("key_Height", 0);
        }
    }
}
