using System.Configuration;
using CMS.DataEngine;
using CMS.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace System.Web.Mvc
{
    public class DynamicOutputCacheAttribute : ResponseCacheAttribute
    {
        public static class SettingsKeys
        {
            public const string SANDBOX_OUTPUT_CACHE_GROUP = nameof(SANDBOX_OUTPUT_CACHE_GROUP);
            public const string SANDBOX_OUTPUT_CACHE_OVERRIDE_IS_ENABLED = nameof(SANDBOX_OUTPUT_CACHE_OVERRIDE_IS_ENABLED);
            public const string SANDBOX_OUTPUT_CACHE_PROFILE_NAME = nameof(SANDBOX_OUTPUT_CACHE_PROFILE_NAME);
            public const string SANDBOX_OUTPUT_CACHE_DURATION = nameof(SANDBOX_OUTPUT_CACHE_DURATION);
        }

        public DynamicOutputCacheAttribute() : base()
        {
            bool isOutputCacheEnabled = ValidationHelper.GetBoolean(ConfigurationManager.AppSettings["cache:response-cache:is-enabled"], false);

            if (!isOutputCacheEnabled)
            {
                CacheProfileName = "Disabled";

                return;
            }

            CacheProfileName = "Default";

            bool isOverrideEnabled = SettingsKeyInfoProvider.GetBoolValue(SettingsKeys.SANDBOX_OUTPUT_CACHE_OVERRIDE_IS_ENABLED);

            if (!isOverrideEnabled)
            {
                return;
            }

            string profileSettingValue = SettingsKeyInfoProvider.GetValue(SettingsKeys.SANDBOX_OUTPUT_CACHE_PROFILE_NAME);

            if (!string.IsNullOrEmpty(profileSettingValue))
            {
                CacheProfileName = profileSettingValue;
            }

            int durationSettingValue = SettingsKeyInfoProvider.GetIntValue(SettingsKeys.SANDBOX_OUTPUT_CACHE_DURATION);

            if (durationSettingValue >= 0)
            {
                Duration = durationSettingValue;
            }
        }
    }
}
