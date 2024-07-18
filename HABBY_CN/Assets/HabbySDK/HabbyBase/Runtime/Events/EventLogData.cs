using System;
using System.Text;

namespace Habby.Events
{
    public class EventLogData
    {
        public DateTime DateTime;
        public string EventName;
        public string Parameters;
        public bool Success;
        public Exception Exception;

        public string DateFormat = "";
        public string Log
        {
            get
            {
                if (string.IsNullOrEmpty(_log))
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(DateTime.ToString(DateFormat));
                    stringBuilder.Append("\t");
                    stringBuilder.Append(EventName);
                    stringBuilder.Append("\t");
                    stringBuilder.Append(Parameters);
                    stringBuilder.Append("\t");
                    if (!Success)
                    {
                        stringBuilder.Append("\n");
                        stringBuilder.Append(Exception);
                    }

                    _log = stringBuilder.ToString();
                }

                return _log;
            }
        }

        private string _log;

        public override string ToString()
        {
            return Log;
        }
    }
}