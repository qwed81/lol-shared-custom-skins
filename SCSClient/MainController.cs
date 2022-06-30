using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCSClient.UI;
using System.Net.WebSockets;

namespace SCSClient
{
    [Route("")]
    [ApiController]
    public class MainController : ControllerBase
    {

        private ActionThread _actionHandler;
        private DataResponseThread _dataResponseHandler;

        public MainController(ActionThread actionHandler, DataResponseThread dataResponseHandler)
        {
            _actionHandler = actionHandler;
            _dataResponseHandler = dataResponseHandler;
        }

        [HttpGet]
        [Route("/socket")]
        public async Task<IActionResult> AcceptSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest == false)
                return BadRequest();

            WebSocket socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _actionHandler.Restart(socket);
            _dataResponseHandler.Restart(socket);

            return Ok();
        }


        [HttpGet]
        [Route("/app")]
        public ActionResult App()
        {
            throw new NotImplementedException();
        }

        [Route("/image/{name}")]
        [HttpGet]
        public ActionResult Image(string name)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("/upload")]
        public ActionResult UploadMod()
        {
            throw new NotImplementedException();
        }




    }
}
