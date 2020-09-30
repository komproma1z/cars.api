using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Cars.API.Data;
using Cars.API.Models;
using Cars.API.Utilities;


namespace Cars.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceStatsController: ControllerBase {
        private DataContext _context = null;
        public ServiceStatsController(DataContext context) {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ContentResult> GetServiceStats() {
            DateTime requestTimestamp = DateTime.UtcNow;
            IStatusCodeActionResult response;

            if (!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                List<LogMetadata> allRequests = _context.Logs.ToList();

                if (allRequests.Count != 0) { // Проверка на наличие записей в таблице
                    double allRequestsProcessingTime = 0;
                    double avgRequestProcessingTime;
                    Dictionary<string, int> requestsPerMethod = new Dictionary<string, int>() {{"GET", 0}, {"POST", 0}, {"DELETE", 0}};
                    foreach (LogMetadata req in allRequests) {
                        allRequestsProcessingTime += req.RequestProcessingTime;
                        requestsPerMethod[req.RequestMethod] += 1;
                    }
                    avgRequestProcessingTime = allRequestsProcessingTime/allRequests.Count;

                    ServiceStats stats = new ServiceStats(Math.Round(avgRequestProcessingTime, 5), allRequests.Count, requestsPerMethod);

                    response = Content(JsonSerializer.Serialize(stats));
                } else {
                    response = NotFound("No statistics");
                }
            }

            await Logger.LogData(_context, Request, requestTimestamp, response);

            return (ContentResult)response;
        }
    }
}