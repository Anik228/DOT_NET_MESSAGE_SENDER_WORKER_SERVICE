using Microsoft.EntityFrameworkCore;
using SMSAPIBULKSMSBD.Models;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SMSAPIBULKSMSBD
{
    public class Worker : BackgroundService
    {
        private readonly String PULL_MESSAGE = "SELECT * FROM vas_robi.dbo.*** WHERE Status=0 ORDER BY Priority ASC";
        private readonly String API_URL = "http://***";
        private readonly String API_KEY = "****";
        private readonly String SENDER = "****";

        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var messages = await dbContext.Outbox_API_ADN_TEST
                            .FromSqlRaw(PULL_MESSAGE)
                            .ToListAsync();

                        if (messages == null || messages.Count == 0)
                        {
                           // _logger.LogInformation("No data found.");
                            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                            continue;
                        }

                        foreach (var message in messages)
                        {
                            if (message == null)
                            {
                                _logger.LogWarning("Encountered a null message. Skipping...");
                                continue;
                            }

                            string receiver = message.Receiver != null ? System.Uri.EscapeUriString(message.Receiver) : null;
                            string sender = message.Sender != null ? System.Uri.EscapeUriString(message.Sender) : null;
                            string encodedMessage = message.Message != null ? System.Uri.EscapeUriString(message.Message) : null;

                            DateTime? submitTime = message.DatabaseSubmitTime;

                            _logger.LogInformation($"Processing message for Receiver: {receiver}, Sender: {sender}, Message: {encodedMessage}");

                            if (string.IsNullOrEmpty(receiver))
                            {
                                _logger.LogWarning("Receiver is null or empty. Skipping this message.");
                                message.Status = -1;
                                await dbContext.SaveChangesAsync();
                                continue;
                            }

                            if (string.IsNullOrEmpty(encodedMessage))
                            {
                                _logger.LogWarning("Message is null or empty. Skipping this message.");
                                message.Status = -1;
                                await dbContext.SaveChangesAsync();
                                continue;
                            }

                            string url = $"{API_URL}?api_key={API_KEY}&senderid={SENDER}&number={receiver}&message={encodedMessage}";

                            message.SentTime = DateTime.Now;

                            try
                            {
                                using (var client = new HttpClient())
                                {
                                    var response = await client.GetAsync(url);

                                    if (response.IsSuccessStatusCode)
                                    {
                                        _logger.LogInformation($"Message sent to {receiver}, Status: {response.StatusCode}");

                                        message.Status = 1;
                                    }
                                    else
                                    {
                                        _logger.LogInformation($"Message is not sent to {receiver}, Status: {response.StatusCode}");
                                        message.Status = -1;
                                    }

                                    message.ApiResponse = response.StatusCode.ToString();
                                    message.ActualSentTime = DateTime.Now;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"Error while sending message to {receiver}: {ex.Message}");
                                message.Status = -1;
                                message.ApiResponse = $"Error: {ex.Message}";
                            }

                            await dbContext.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unexpected error: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
    }
}




