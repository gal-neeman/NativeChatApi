using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.WebSockets;

namespace NativeChat.Controllers
{
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        private List<WebSocket> _connections = new List<WebSocket>();

        [Route("/api/v1/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                string? userId = HttpContext.Items["UserId"] as string;
                var user = HttpContext.User;
                if (userId == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return;
                }
                Guid guid = Guid.Parse(userId);

                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Log.Information("Established a new WebSocket connection");
                WebSocketManager.AddSocket(webSocket, Guid.NewGuid());

                //await Task.Run(() => WebSocketManager.SendMessage(webSocket.);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
