using LeituraArquivos.Core;

namespace LeituraDeArquivos
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("AppSettings.Json").Build();
            Arquivo arquivo = new Arquivo(configuration);
            arquivo.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(int.Parse(configuration.GetSection("Configuration:intervaloEntreEnvios").Value), stoppingToken);
            }
        }
    }
}