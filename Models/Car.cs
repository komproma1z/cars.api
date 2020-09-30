using System;

namespace Cars.API.Models
{
    public class Car  {
        // Модель объекта машины
        public int Id {get; set;}
        public DateTime CreatedDate { get; set; }
        public string Number {get; set;}
        public string Brand {get; set;}
        public string Model {get; set;}
        public string Color {get; set;}
        public string ReleaseYear {get; set;}
        public int PriceInUSD {get; set;}

        public Car() {
            this.CreatedDate = DateTime.UtcNow;
        }
    }
}