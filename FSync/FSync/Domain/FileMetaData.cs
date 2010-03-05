﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OneSync.Files;
using System.Xml;
using System.Xml.Serialization;

namespace OneSync.Synchronization
{
    
    public class FileMetaData:IMetaData
    {
        private string sourceId = "";
        private string sourcePath = "";
        /// <summary>
        /// Collection of attributes
        /// </summary>
        #region list of column names in METADATA TABLE
        public const string METADATA_TABLE = "METADATA_TABLE";
        public const string SOURCE_ID = "SOURCE_ID";
        public const string FULL_NAME = "FULL_NAME";
        public const string RELATIVE_PATH = "RELATIVE_PATH";
        public const string HASH_CODE = "HASH_CODE";
        public const string LAST_MODIFIED_TIME = "LAST_MODIFIED_TIME";
        public const string NTFS_ID1 = "NTFS_ID1";
        public const string NTFS_ID2 = "NTFS_ID2";
        #endregion list of column names in METADATA TABLE

        protected IList<IMetaDataItem> items = new List<IMetaDataItem>();              
        
        
        public FileMetaData(string sourceId, string sourcePath):base()
        {
            this.sourceId = sourceId;
            this.sourcePath = sourcePath;
        }

        public string SourcePath
        {
            get
            {
                return this.sourcePath;
            }
        }
        public string SourceId
        {
            set
            {
                this.sourceId = value;
            }
            get
            {
                return this.sourceId;
            }
        }

        public IList<IMetaDataItem> MetaDataItems
        {
            set
            {
                items = value;
            }
            get
            {
                return this.items;
            }
        }
    }
}
