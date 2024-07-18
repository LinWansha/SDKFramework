using System;
using System.Collections.Generic;

namespace Habby.Base
{
    // link list object pool with Get and Return function and implement IRuse interface,cache object in a weekReference LinkedList
    // with max cache size
    // Get function:if cache size is bigger than max cache size, will destroy the last object from LinkedList then add object to the frist of linkedList
    // Get function:when get object from LinkedList,will call Reset function
    // when return object to LinkedList,will call Reset function
  
    public class HabbyLinkListObjectPool<T> where T : class, IReuse, new()
    {
        private int mMaxCacheSize = 50;
        private LinkedList<T> mCacheList = new LinkedList<T>();
        private bool mCollectionChecks = true;

        public HabbyLinkListObjectPool(int maxCacheSize = 50, bool collectionChecks = true)
        {
            mMaxCacheSize = maxCacheSize;
            mCollectionChecks = collectionChecks;
        }

        public T Get()
        {
            T target = null;
            if (mCacheList.Count > 0)
            {
                target = mCacheList.First.Value;
                mCacheList.RemoveFirst();
            }
            else
            {
                target = new T();
            }

            target.Reset();
            return target;
        }
        
        // Size :get the cache size of LinkedList ,check list empty or not
        public int Size
        {
            get { return mCacheList.Count; }
        }
        public void Return(T target)
        {
            if (mCollectionChecks && mCacheList.Contains(target))
            {
                throw new Exception("Object already released to pool!");
            }

            target.Reset();
            mCacheList.AddFirst(target);
            if (mCacheList.Count > mMaxCacheSize)
            {
                mCacheList.Last.Value.Dispose();
                mCacheList.RemoveLast();
            }
        }

        public void Clear()
        {
            foreach (var item in mCacheList)
            {
                item.Dispose();
            }

            mCacheList.Clear();
        }
 
    }
}

