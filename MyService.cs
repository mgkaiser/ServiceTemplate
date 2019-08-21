using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Hosting;

namespace ServiceTemplate
{
    public class MyService : IHostedService
    {
        private readonly ILogger _logger;        

        public Dispatcher(IBusControl busControl, ILoggerFactory loggerFactory)
		{
            _logger = loggerFactory.CreateLogger<Dispatcher>();            
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {          
            _logger.LogInformation("Starting Dispatcher Service");
			
			// Setup initial state
			// Start your service.
			// 		Do one or more of these
			// 		Attach to queue to listen.  
			//		Spin up worker threads.   
			
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Dispatcher Service");
			// Stop threads
			// Cleanup resources
            return Task.CompletedTask
        }

    }
}