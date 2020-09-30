using System.Collections.Generic;

namespace Cars.API.Models {
    // Модель объекта статистики запросов к сервису
    public class ServiceStats {
        public double AverageRequestProcessingTime { get; set; }
        public int RequestsCount { get; set; }
        public Dictionary<string, int> RequestsPerMethod { get; set; }

        public ServiceStats() {}
        public ServiceStats(double avgReqProcTime, int reqCount, Dictionary<string, int> reqPerMethod) {
            AverageRequestProcessingTime = avgReqProcTime;
            RequestsCount = reqCount;
            RequestsPerMethod = reqPerMethod;
        }
    }
}