using System.Collections.Generic;
using Habby.CNUser;

namespace Habby.CNUser
{
    public class SyncOnlineDataRequest : Request
    {
        public const int ACCOUNT_NOT_LOGIN = 50601;
        public const int ACCOUNT_NOT_EXIST = 50602;
        public const int DATA_SAVE_FAILED = 50603;
        public List<UserOnlieSegment> activeTimeSegments;
    }
}