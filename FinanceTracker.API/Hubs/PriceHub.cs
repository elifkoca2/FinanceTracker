using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinanceTracker.API.Hubs
{
    [Authorize]
    public class PriceHub:Hub 
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
