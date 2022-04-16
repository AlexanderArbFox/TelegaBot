using Microsoft.Extensions.Logging;
using ServiceBot.Utility.Interfaces;
using ServiceDB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegaBotConsole.Interfaces;

namespace ServiceBot.Utility
{
    public class ErrorsHandler : IErrorsHandler
    {
        private readonly ILogger<ErrorsHandler> _errorsHandler;
        private readonly ApplicationSettings _applicationSettings;
        public ErrorsHandler(ILogger<ErrorsHandler> errorsHandler,ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
            _errorsHandler = errorsHandler;
        }
        public void LogError(string messageErr, DateTime dt)
        {
            try
            {
                using (var connection = new SqlConnection(_applicationSettings.ConnectionString))
                {
                    using (var cmd = new SqlCommand("insert into [TelegaBot].[dbo].[Logging_Error]([Info_error],[DateTimeError]) values (@errorMessage, @errorDateTime)", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@errorMessage", SqlDbType.NVarChar).Value = messageErr;
                        cmd.Parameters.Add("@errorDateTime", SqlDbType.DateTime).Value = dt;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageToMail($"Произошла ошибка : '{ ex.Message}'.", DateTime.Now);
            }
        }

        public void SendMessageToMail(string message, DateTime dt)
        {
            try
            {
                using (var connection = new SqlConnection(_applicationSettings.ConnectionString))
                {
                    using(var cmd = new SqlCommand("MailMessageAboutError", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@errorMessage", SqlDbType.NVarChar).Value = message;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _errorsHandler.LogError($"Произошла ошибка : '{ ex.Message}'.", DateTime.Now);
            }
        }
    }
}
