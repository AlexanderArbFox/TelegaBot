using Microsoft.Extensions.Logging;
using Quartz;
using ServiceBot.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegaBotConsole.Interfaces;

namespace ServiceBot.Quartz.Jobs
{
    internal class LoadBotJob : IJob
    {
        private readonly ILogger<LoadBotJob> _logger;
        private readonly IErrorsHandler _errorsHandler;
        private readonly IBotStarted _botStarted;
        public LoadBotJob(ILogger<LoadBotJob> logger, IErrorsHandler errorsHandler, IBotStarted botStarted)
        {
            _logger = logger;
            _errorsHandler = errorsHandler;
            _botStarted = botStarted;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            
            var loadTask = Task.Run(delegate
            {
                _logger.LogInformation("LoadBotJob начал работать");

                try
                {
                    _botStarted.StartBot();
                }
                catch (Exception ex)
                {
                    _errorsHandler.LogError($"Произошла ошибка : '{ ex.Message}'.", DateTime.Now);
                    _errorsHandler.SendMessageToMail($"Произошла ошибка : '{ ex.Message}'.", DateTime.Now);
                }
            });
            await loadTask;
            _logger.LogInformation("LoadBotJob завершил работу");
        }
    }
}
