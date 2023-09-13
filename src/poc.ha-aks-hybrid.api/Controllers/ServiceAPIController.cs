using Microsoft.AspNetCore.Mvc;

//This is NON PRODUCTION CODE
namespace PoC.HaAKSHybrid.API
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceAPIController : ControllerBase
    {
        private readonly ILogger<ServiceAPIController> _logger;
        private readonly IQueue _queue;

        public ServiceAPIController(ILogger<ServiceAPIController> logger, IQueue queue) 
        {
            _logger = logger;
            _queue = queue;
        }

        /// <summary>
        /// API health check
        /// </summary>
        /// <returns>200 Success</returns>
        [HttpGet]
        [Route("health")]
        public IActionResult Health()
        {
            return Ok();
        }

        /// <summary>
        /// Enqueue a message to send to server
        /// </summary>
        /// <param name="myMessage">The message to enqueue</param>
        /// <returns>Success or failure</returns>
        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> SendMessageToServer([FromBody] APIMessageIn message)
        {
            Console.WriteLine("message received.");

            var rtn = false;
            Message msg = new Message(message.SenderId, message.Value);
            
            if (!msg.IsValid())
                return BadRequest("message is invalid.");

            try
            {    
                rtn = await _queue.EnqueueAsync(msg);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return BadRequest("enqueue failed, please try later.");
            }

            return Ok(new APIMessageOut(msg.SenderId, msg.CorrelationId, msg.Value, rtn));
        }
    }
}