using System.Configuration;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.IO;
using CMS.SiteProvider;

namespace Sandbox.Data.Kentico.Infrastructure.Storage
{
    public class AzureStorageModule : Module
    {
        private readonly string containerName;
        private readonly bool isPublic;

        public AzureStorageModule() : base(nameof(AzureStorageModule))
        {
            isPublic = ValidationHelper.GetBoolean(ConfigurationManager.AppSettings["azure-storage:is-public"], false);
            containerName = ConfigurationManager.AppSettings["azure-storage:container-name"];
        }

        protected override void OnInit()
        {
            base.OnInit();

            if (string.IsNullOrWhiteSpace(containerName))
            {
                return;
            }

            var sites = SiteInfo.Provider.Get()
                .WhereEquals(nameof(SiteInfo.SiteIsOffline), 0)
                .WhereEquals("SiteStatus", "RUNNING")
                .GetEnumerableTypedResult();

            foreach (var site in sites)
            {
                StorageHelper.MapStoragePath($"~/{site.SiteName}/media", CreateAzureStorageProvider());
                StorageHelper.MapStoragePath($"~/{site.SiteName}/files", CreateAzureStorageProvider());
            }

            StorageHelper.MapStoragePath("~/App_Data/VersionHistory", CreateAzureStorageProvider());

            AbstractStorageProvider CreateAzureStorageProvider()
            {
                return new StorageProvider("Azure", "CMS.AzureStorage")
                {
                    CustomRootPath = containerName,
                    PublicExternalFolderObject = isPublic,
                };
            }
        }
    }
}
