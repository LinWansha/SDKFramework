using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDKFramework.Account.DataSrc
{
    [Serializable]
    public class UserDataEntry
    {
        public string name;
        public int value;
    }
    [Serializable]
    public class UserLongDataEntry
    {
        public string name;
        public long value;
    }
    
    [Serializable]
    public class UserDoubleDataEntry
    {
        public string name;
        public double value;
    }
}
