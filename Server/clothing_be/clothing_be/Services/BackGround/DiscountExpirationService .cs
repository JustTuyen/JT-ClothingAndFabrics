using clothing_be.Data;
using Microsoft.EntityFrameworkCore;

namespace clothing_be.Services.BackGround
{
    public class DiscountExpirationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public DiscountExpirationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<MyDbContextApplication>();

                var closedStatus = await context.Statuses
                    .FirstOrDefaultAsync(s => s.Name == "Closed" 
                        && s.Type == "Discounts", stoppingToken);

                if(closedStatus != null)
                {
                    var now = DateTime.UtcNow;
                    var expiredDiscount = await context.Discounts
                        .Where(d =>
                            d.ExpireAt <= now &&
                            d.StatusId != closedStatus.Id)
                        .ToListAsync(stoppingToken);

                    foreach (var discount in expiredDiscount)
                    {
                        discount.StatusId = closedStatus.Id;
                    }

                    if (expiredDiscount.Count > 0)
                    {
                        await context.SaveChangesAsync(stoppingToken);
                    }
                }

                await Task.Delay(
                TimeSpan.FromMinutes(10),
                stoppingToken);

            }
        }
    }
}
