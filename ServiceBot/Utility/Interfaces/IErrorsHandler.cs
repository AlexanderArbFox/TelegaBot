using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBot.Utility.Interfaces
{
    interface IErrorsHandler
    {
        void LogError(string messageErr, DateTime dt);
        void SendMessageToMail(string message, DateTime dt);
    }
}
