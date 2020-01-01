using System.Collections.Generic;
using System.Text;
using System;
using log4net.Core;

namespace BankBros.Backend.Core.CrossCuttingConcerns.Logging.Log4Net
{
    [Serializable]
    public class SerializableLogEvent
    {
        private LoggingEvent _logginEvent;

        public SerializableLogEvent(LoggingEvent logginEvent)
        {
            _logginEvent = logginEvent;
        }

        public object Message => _logginEvent.MessageObject;
    }
}
