using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBot.Services.WindowsService
{
    class ServiceBaseLifetime : ServiceBase, IHostLifetime
    {
        private readonly TaskCompletionSource<object> _delayStart = new TaskCompletionSource<object>();
        private IHostApplicationLifetime HostApplicationLifetime { get;}
        public ServiceBaseLifetime(IHostApplicationLifetime hostApplicationLifetime)
        {
            HostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        private void Run()
        {
            try
            {
                Run(this);
                _delayStart.TrySetException(new InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex)
            {
                _delayStart.TrySetException(ex);
            }
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            HostApplicationLifetime.ApplicationStopping.Register(Stop);
            new Thread(Run).Start();
            return _delayStart.Task;
        }

        protected override void OnStart(string[] args)
        {
            _delayStart.TrySetResult(null);
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            HostApplicationLifetime.StopApplication();
            base.OnStop();
        }
    }
}
