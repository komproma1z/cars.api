using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Cars.API.Data;
using Cars.API.Models;

namespace Cars.API.Utilities {
    // Функционал для логирования запросов к сервису
    public static class Logger {
        public static async Task LogData(DataContext context, HttpRequest req, DateTime reqTimestamp, dynamic res) {
            DateTime responseTimestamp;
            TimeSpan requestProcessingTime;
            responseTimestamp = DateTime.UtcNow;
            requestProcessingTime = responseTimestamp.Subtract(reqTimestamp);
            
            LogMetadata log = new LogMetadata(
                req.ContentType,
                req.Path,
                req.Method,
                reqTimestamp,
                res.StatusCode,
                responseTimestamp,
                $"{req.Path} was requested at {reqTimestamp} with {req.Method} method, request was processed in {requestProcessingTime.TotalMilliseconds} milliseconds, response status code {res.StatusCode}",
                requestProcessingTime.TotalMilliseconds
            );

            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}