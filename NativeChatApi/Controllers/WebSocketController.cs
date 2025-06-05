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
        public async Task Get([FromQuery] string token)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                string idStr = TokenUtils.GetIdFromToken(token);
                Guid id = Guid.Parse(idStr);
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Log.Information("Established a new WebSocket connection");
                WebSocketManager.AddSocket(webSocket, id);

                //await Task.Run(() => WebSocketManager.SendMessage(webSocket.);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
