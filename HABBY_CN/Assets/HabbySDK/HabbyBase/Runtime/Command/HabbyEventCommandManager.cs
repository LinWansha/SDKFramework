using System.Collections.Generic;
using Habby.Base;
using UnityEngine;
namespace Habby.Command
{
    public class HabbyEventCommandManager:UnitySingletonBase<HabbyEventCommandManager>
    {
        private Dictionary<string,ICommand> mCommandMap = new Dictionary<string, ICommand>();
        public bool AddCommand(ICommand command)
        {
            if (!mCommandMap.ContainsKey(command.ExcuteEventName))
            {
                mCommandMap[command.ExcuteEventName] = command;
                
            }
            return false;
        }

        protected override void OnAwakeInitialized()
        {
            mCommandMap = new Dictionary<string, ICommand>();
        }
    }
}