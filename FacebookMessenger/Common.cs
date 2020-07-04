using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger
{
    public static class FacebookApiURL
    {
        public const string Message_V26URL = "https://graph.facebook.com/v2.6/me/messages";
        public const string Message_V70URL = "https://graph.facebook.com/v7.0/me/messages";
        public const string Profile_V70URL = "https://graph.facebook.com/v7.0/me/messenger_profile";
        public const string CustomUserSettings_V70URL = "https://graph.facebook.com/v7.0/me/custom_user_settings";
        public const string GraphURL = "https://graph.facebook.com";
    }

    public class ContentType
    {
        public const string Text = "text";
        public const string Json = "application/json";
        public const string Location = "location";
    }


}
