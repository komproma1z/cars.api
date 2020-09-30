using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cars.API.Data;
using Cars.API.Models;
using Cars.API.Utilities;


namespace Cars.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController: ControllerBase {
        private DataContext _context = null;
        public CarsController(DataContext context) {
            _context = context;
        }
        
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<Car>> GetCars(string orderBy, bool reversed) {
            
            DateTime requestTimestamp = DateTime.UtcNow;
            ActionResult response;

            if (!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                var carsList = _context.Cars.ToList();

                if (carsList.Count == 0) { // Проверка на наличие записей в таблице
                    response = NotFound("There are no records yet");
                } else {
                    switch (orderBy) { // Логика сортировки
                        case "year":
                            if (!reversed) {
                                response = Ok(carsList.OrderBy(c => c.ReleaseYear));
                                break;
                            } else {
                                response = Ok(carsList.OrderByDescending(c => c.ReleaseYear));
                                break;
                            }
                        case "price":
                            if (!reversed) {
                                response = Ok(carsList.OrderBy(c => c.PriceInUSD));
                                break;
                            } else {
                                response = Ok(carsList.OrderByDescending(c => c.PriceInUSD));
                                break;
                            }
                        default:
                            response = Ok(carsList);
                            break;
                    }
                }
            }

            // логирование
            await Logger.LogData(_context, Request, requestTimestamp, response);

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id) {
            DateTime requestTimestamp = DateTime.UtcNow;
            ActionResult response;
            
            if(!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                var car = await _context.Cars.FindAsync(id);

                if (car == null) { // Проверка на наличие искомой записи
                    response = NotFound("Item does not exist");
                } else {
                    response = Ok(car);
                }
            }

            // логирование
            await Logger.LogData(_context, Request, requestTimestamp, response);

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car) {

            DateTime requestTimestamp = DateTime.UtcNow;
            ActionResult response;

            if (!_context.Database.CanConnect()) { // Проверка на доступность дб 
                response = Content("No connection to database.");
            } else {
                var carsList = _context.Cars.ToList();
                bool alreadyExists = false;
    
                if (carsList.Count > 0) { // Проверка на наличие записей в таблице
                    foreach (Car c in carsList) {
                        if (car.Number == c.Number) alreadyExists = true;
                    }
                }
                
                if (!alreadyExists) { // проверка на наличие записи с указанным номером (номер машины уникален)
                    _context.Cars.Add(car);
                    await _context.SaveChangesAsync();
                    response = Created("Item successfully added.", CreatedAtAction(nameof(GetCar), new { id = car.Id }, car));
                } else {
                    response = Conflict("A car with given number already exists. There cannot be cars with the same number.");
                }
            }

            // логирование
            await Logger.LogData(_context, Request, requestTimestamp, response);
        
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> DeleteCar(int id) {
            
            DateTime requestTimestamp = DateTime.UtcNow;
            ActionResult response;

            if (!_context.Database.CanConnect()) { // Проверка на доступность дб
                response = Content("No connection to database.");
            } else {
                var car = await _context.Cars.FindAsync(id);

                if (car == null) { // Проверка на наличие искомой записи
                    response = NotFound("The item does not exist");
                } else {
                    _context.Cars.Remove(car);
                    await _context.SaveChangesAsync();
                    response = Ok("Item successfully deleted");
                }
            }

            // логирование
            await Logger.LogData(_context, Request, requestTimestamp, response);

            return response;
        }
    }
}
