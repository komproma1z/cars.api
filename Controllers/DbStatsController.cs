using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cars.API.Data;
using Cars.API.Models;
using Cars.API.Utilities;


namespace Cars.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DbStatsController: ControllerBase {
        private DataContext _context = null;
        public DbStatsController(DataContext context) {
            _context = context;
        }
        
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<ContentResult> GetStats() {
            ContentResult response;
            DateTime requestTimestamp = DateTime.UtcNow;
            if (!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                List<Car> carsList = _context.Cars.ToList();
                List<LogMetadata> logs = _context.Logs.ToList();

                if (carsList.Count + logs.Count != 0) { // Проверка на наличие записей во всех таблицах

                    // <алгоритм нахождения самой ранней и самой поздней записей>
                    // он не масштабируется, обусловлено это наличием лишь двух рабочих таблиц,
                    // #YAGNI
                    DateTime earliestRecordDate = logs[0].CreatedDate;
                    DateTime latestRecordDate = logs[logs.Count - 1].CreatedDate;

                    if (DateTime.Compare(carsList[0].CreatedDate, logs[0].CreatedDate) < 0) {
                        earliestRecordDate = carsList[0].CreatedDate;
                    }
                    if (DateTime.Compare(carsList[carsList.Count - 1].CreatedDate, logs[logs.Count - 1].CreatedDate) > 0) {
                        latestRecordDate = carsList[carsList.Count - 1].CreatedDate;
                    }
                    // </алгоритм нахождения самой ранней и самой поздней записей>

                    DbStats stats = new DbStats(earliestRecordDate, latestRecordDate, carsList.Count + logs.Count);
                    response = Content(JsonSerializer.Serialize(stats));
                } else {
                    response = Content("No statistics.");
                }
            }

            // логирование
            await Logger.LogData(_context, Request, requestTimestamp, response);

            return response;
        }
    }
}