using System;

namespace Template.Components.Logging
{
    public interface ILogger
    {
        void Log(String message);
        void Log(String accountId, String message);
    }
}
