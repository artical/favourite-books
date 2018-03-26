using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFavouriteBooks.Models.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFavouriteBooks.Controllers
{
    [Route("api/[controller]")]
    public class LoggerController : Controller
    {
        private ILogger<LoggerController> _logger;

        public LoggerController(ILogger<LoggerController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Log Search, Adding and Removing books actions
        /// </summary>
        /// <param name="logEntry">Log entry to be saved</param>
        [HttpPost]
        public void Post([FromBody]LogEntry logEntry)
        {
            _logger.LogWarning(logEntry.Timestamp.ToString() + '\t' + logEntry.LogType + '\t' + logEntry.Details);
        }
        
    }
}
