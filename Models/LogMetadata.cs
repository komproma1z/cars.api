using System;

namespace Cars.API.Models {
    // Модель объекта лога
    public class LogMetadata {
        public int Id {get; set;}
        public DateTime CreatedDate {get; set;}
        public string RequestContentType { get; set; }
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public int? ResponseStatusCode { get; set; }
        public DateTime? ResponseTimestamp { get; set; }
        public string Message {get; set;}
        public double RequestProcessingTime {get; set;}

        // add LogLevel feild (maybe enum?)

        public LogMetadata() {}
        public LogMetadata(
            string reqContType, 
            string reqURI, 
            string reqMeth, 
            DateTime? reqTimestamp,
            int? respStatusCode,
            DateTime? respTimestamp,
            string msg,
            double reqProcTime
            ) {
                CreatedDate = DateTime.UtcNow;
                RequestContentType = reqContType;
                RequestUri = reqURI;
                RequestMethod = reqMeth;
                RequestTimestamp = reqTimestamp;
                ResponseStatusCode = respStatusCode;
                ResponseTimestamp = respTimestamp;
                Message = msg;
                RequestProcessingTime = reqProcTime;
        }
    }
}