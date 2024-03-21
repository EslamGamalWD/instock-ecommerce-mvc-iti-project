using Microsoft.AspNetCore.SignalR;

namespace InStockWebAppPL.Hubs
{
    public class UserCount:Hub
    {
        public static int TotalViews { get; set; } = 0;
        public static int UserViews { get; set; } = 0;

        public override Task OnConnectedAsync()
        {
            UserViews++;
            Clients.All.SendAsync("updateTotalUserViews", UserViews).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            UserViews--;
            Clients.All.SendAsync("updateTotalUserViews", UserViews).GetAwaiter().GetResult();

            return base.OnDisconnectedAsync(exception);
        }
        public async Task NewWindowLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("updateTotalViews", TotalViews);

        }
    }
}
