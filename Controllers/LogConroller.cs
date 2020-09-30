using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cars.API.Data;
using Cars.API.Models;
using Cars.API.Utilities;


namespace Cars.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LogController: ControllerBase {
        private DataContext _context = null;
        public LogController(DataContext context) {
            _context = context;
        }
        
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<LogMetadata>> GetLog(string orderBy, bool reversed) {
            DateTime requestTimestamp = DateTime.UtcNow;
            ActionResult response;

            if (!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                List<LogMetadata> allRecords = _context.Logs.ToList();
                if (allRecords.Count != 0) { // Проверка на наличие записей в таблице
                    response = Ok(allRecords);
                } else {
                    response = Content("No statistics");
                }
            }

            await Logger.LogData(_context, Request, requestTimestamp, response);

            return response;
        }
    }
}