using System;

namespace Cars.API.Models {
    // Модель объекта статистики базы
    public class DbStats {
        public DateTime FirstRecordCreated { get; set; }
        public DateTime LastRecordCreated { get; set; }
        public int RecordsCount { get; set; }

        public DbStats() {}
        public DbStats(DateTime firstRecDate, DateTime lastRecDate, int recCount) {
            FirstRecordCreated = firstRecDate;
            LastRecordCreated = lastRecDate;
            RecordsCount = recCount;
        }
    }
}
