using SqlServiceBrokerListener;

namespace SQLTableListener
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private SqlDependencyEx? _listener;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await ObserveJSTypeInDB(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
            await Task.CompletedTask;
        }

        public async Task ObserveJSTypeInDB(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() =>
                {
                    string connectionString = _configuration.GetValue<string>("Db:Connection");
                    string database = GetConnectionStringItem(connectionString, "Database");
                    string schema = _configuration.GetValue<string>("Db:DefaultSchema");
                    string observableTable = _configuration.GetValue<string>("Db:TableToListen");
                    string version = _configuration.GetValue<string>("Version");
                    _listener = new SqlDependencyEx(connectionString, database, observableTable, schema);

                    _listener.TableChanged += async (o, e) =>
                    {
                        await Task.Run(() =>
                        {
                            //Write Code here for manipulation.
                            switch (e.NotificationType)
                            {
                                case SqlDependencyEx.NotificationTypes.Insert:
                                    Console.WriteLine($"{observableTable} data Insert detected by listener version: {version}");
                                    break;
                                case SqlDependencyEx.NotificationTypes.Update:
                                    Console.WriteLine($"{observableTable} data Update detected by listener version: {version}");
                                    break;
                                case SqlDependencyEx.NotificationTypes.Delete:
                                    Console.WriteLine($"{observableTable} data Delete detected by listener version: {version}");
                                    break;
                            }

                        }, cancellationToken);
                    };
                });

                if (_listener != null)
                {
                    _listener.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex, "ObserveJSTypeInDB");
            }
        }

        private string? GetConnectionStringItem(string connectionstring, string itemName)
        {
            string[] parts = connectionstring.Split(';');
            return parts.FirstOrDefault(part => part.StartsWith(itemName + "="))?.Replace(itemName + "=", "");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {

            if (_listener != null)
            {
                _listener.Stop();
                _listener.Dispose();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}