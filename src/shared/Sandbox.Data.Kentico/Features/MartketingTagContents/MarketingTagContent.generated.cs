﻿//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.DocumentEngine.Types.Sandbox;
using CMS.DocumentEngine;

[assembly: RegisterDocumentType(MarketingTagsContent.CLASS_NAME, typeof(MarketingTagsContent))]

namespace CMS.DocumentEngine.Types.Sandbox
{
    /// <summary>
    /// Represents a content item of type MarketingTagsContent.
    /// </summary>
    public partial class MarketingTagsContent : TreeNode
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "Sandbox.MarketingTagsContent";


        /// <summary>
        /// The instance of the class that provides extended API for working with MarketingTagsContent fields.
        /// </summary>
        private readonly MarketingTagsContentFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// MarketingTagsContentID.
        /// </summary>
        [DatabaseIDField]
        public int MarketingTagsContentID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("MarketingTagsContentID"), 0);
            }
            set
            {
                SetValue("MarketingTagsContentID", value);
            }
        }


        /// <summary>
        /// Header Tags.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentHeaderTags
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentHeaderTags"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentHeaderTags", value);
            }
        }


        /// <summary>
        /// After Body Start Tags.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentAfterBodyStartTags
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentAfterBodyStartTags"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentAfterBodyStartTags", value);
            }
        }


        /// <summary>
        /// Before Body End Tags.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentBeforeBodyEndTags
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentBeforeBodyEndTags"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentBeforeBodyEndTags", value);
            }
        }


        /// <summary>
        /// Page Title Suffix.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentPageTitleSuffix
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentPageTitleSuffix"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentPageTitleSuffix", value);
            }
        }


        /// <summary>
        /// Default Page Description.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentDefaultPageDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentDefaultPageDescription"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentDefaultPageDescription", value);
            }
        }


        /// <summary>
        /// Default Open Graph Image Url.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentDefaultOpenGraphImageUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentDefaultOpenGraphImageUrl"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentDefaultOpenGraphImageUrl", value);
            }
        }


        /// <summary>
        /// Default Twitter Site.
        /// </summary>
        [DatabaseField]
        public string MarketingTagsContentDefaultTwitterSite
        {
            get
            {
                return ValidationHelper.GetString(GetValue("MarketingTagsContentDefaultTwitterSite"), @"");
            }
            set
            {
                SetValue("MarketingTagsContentDefaultTwitterSite", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with MarketingTagsContent fields.
        /// </summary>
        [RegisterProperty]
        public MarketingTagsContentFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with MarketingTagsContent fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class MarketingTagsContentFields : AbstractHierarchicalObject<MarketingTagsContentFields>
        {
            /// <summary>
            /// The content item of type MarketingTagsContent that is a target of the extended API.
            /// </summary>
            private readonly MarketingTagsContent mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="MarketingTagsContentFields" /> class with the specified content item of type MarketingTagsContent.
            /// </summary>
            /// <param name="instance">The content item of type MarketingTagsContent that is a target of the extended API.</param>
            public MarketingTagsContentFields(MarketingTagsContent instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// MarketingTagsContentID.
            /// </summary>
            public int ID
            {
                get
                {
                    return mInstance.MarketingTagsContentID;
                }
                set
                {
                    mInstance.MarketingTagsContentID = value;
                }
            }


            /// <summary>
            /// Header Tags.
            /// </summary>
            public string HeaderTags
            {
                get
                {
                    return mInstance.MarketingTagsContentHeaderTags;
                }
                set
                {
                    mInstance.MarketingTagsContentHeaderTags = value;
                }
            }


            /// <summary>
            /// After Body Start Tags.
            /// </summary>
            public string AfterBodyStartTags
            {
                get
                {
                    return mInstance.MarketingTagsContentAfterBodyStartTags;
                }
                set
                {
                    mInstance.MarketingTagsContentAfterBodyStartTags = value;
                }
            }


            /// <summary>
            /// Before Body End Tags.
            /// </summary>
            public string BeforeBodyEndTags
            {
                get
                {
                    return mInstance.MarketingTagsContentBeforeBodyEndTags;
                }
                set
                {
                    mInstance.MarketingTagsContentBeforeBodyEndTags = value;
                }
            }


            /// <summary>
            /// Page Title Suffix.
            /// </summary>
            public string PageTitleSuffix
            {
                get
                {
                    return mInstance.MarketingTagsContentPageTitleSuffix;
                }
                set
                {
                    mInstance.MarketingTagsContentPageTitleSuffix = value;
                }
            }


            /// <summary>
            /// Default Page Description.
            /// </summary>
            public string DefaultPageDescription
            {
                get
                {
                    return mInstance.MarketingTagsContentDefaultPageDescription;
                }
                set
                {
                    mInstance.MarketingTagsContentDefaultPageDescription = value;
                }
            }


            /// <summary>
            /// Default Open Graph Image Url.
            /// </summary>
            public string DefaultOpenGraphImageUrl
            {
                get
                {
                    return mInstance.MarketingTagsContentDefaultOpenGraphImageUrl;
                }
                set
                {
                    mInstance.MarketingTagsContentDefaultOpenGraphImageUrl = value;
                }
            }


            /// <summary>
            /// Default Twitter Site.
            /// </summary>
            public string DefaultTwitterSite
            {
                get
                {
                    return mInstance.MarketingTagsContentDefaultTwitterSite;
                }
                set
                {
                    mInstance.MarketingTagsContentDefaultTwitterSite = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingTagsContent" /> class.
        /// </summary>
        public MarketingTagsContent() : base(CLASS_NAME)
        {
            mFields = new MarketingTagsContentFields(this);
        }

        #endregion
    }
}