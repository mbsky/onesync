﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneSync.Synchronization
{
    public class FileSyncProvider:ISyncProvider
    {
        protected FileMetaData current;
        protected FileMetaData stored;      

        public FileSyncProvider()
        {

        }

        public FileSyncProvider(IMetaData current, IMetaData stored)
        {
            this.current = (FileMetaData) current;
            this.stored =(FileMetaData) stored;
        }

        public IMetaData CurrentMetaData
        {
            get
            {
                return this.current;
            }
        }

        public IMetaData StoredMetaData
        {
            get
            {
                return this.stored;
            }
        }
        
        /// <summary>
        /// Enumerate changes between 2 meta data
        /// and return a list of actions
        /// </summary>
        /// <returns></returns>
        public  IList<SyncAction> EnumerateChanges()
        {           
            IList<SyncAction> actions = new List<SyncAction>();
            
            //Get newly created items by comparing relative paths
            IEnumerable<IMetaDataItem> leftOnly = from left in current.MetaDataItems
                                                  where !stored.MetaDataItems.Contains(left, new FileMetaDataItemComparer())
                                                  select left;

            foreach (FileMetaDataItem left in leftOnly)
            {
                CreateAction createAction = new CreateAction(
                    current.SourcePath,
                    current.SourceId, ChangeType.NEWLY_CREATED, left.RelativePath, left.HashCode);
                actions.Add(createAction);
            }

            //Get deleted items 
            IEnumerable<IMetaDataItem> rightOnly = from right in stored.MetaDataItems
                                                  where !current.MetaDataItems.Contains(right, new FileMetaDataItemComparer())
                                                  select right;
            foreach (FileMetaDataItem right in rightOnly)
            {
                DeleteAction deleteAction = new DeleteAction(
                    current.SourcePath,
                    current.SourceId, ChangeType.DELETED, right.RelativePath, right.HashCode);
                actions.Add(deleteAction);
            }
            
            //get the items from 2 metadata with same relative paths but different hashes.
            IEnumerable<ChangeItem> bothModified =   from right in stored.MetaDataItems
                                                        from left in current.MetaDataItems
                                                        where ((FileMetaDataItem)right).RelativePath.Equals(((FileMetaDataItem)left).RelativePath)
                                                        && !((FileMetaDataItem)right).HashCode.Equals(((FileMetaDataItem)left).HashCode)
                                                        select new ChangeItem (right, left);
            foreach (ChangeItem item in bothModified) 
            {
                FileMetaDataItem oldItem =(FileMetaDataItem) item.OldItem;
                FileMetaDataItem newItem =(FileMetaDataItem) item.NewItem;
                ModifyAction modifyAction = new ModifyAction(current.SourcePath, current.SourceId, ChangeType.MODIFIED,
                    oldItem.RelativePath, oldItem.HashCode, newItem.HashCode); 
                actions.Add(modifyAction);
            }

            return  actions;
        }
    }
}
