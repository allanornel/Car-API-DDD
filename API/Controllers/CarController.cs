using Application.DTOs;
using Application.Services.Car;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars([FromQuery] string query = "", [FromQuery] int page = 0)
        {
            var carsModel = await _carService.GetCars(query, page);
            return Ok(carsModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar([FromRoute] int id)
        {
            try
            {
                var car = await _carService.GetCar(id);
                return Ok(car);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(CarDTO carDTO)
        {
            var carResult = await _carService.AddCar(carDTO);
            var uri = new Uri($"/api/cars/{carResult.Id}", UriKind.Relative);
            return Created(uri, carResult);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCar([FromRoute] int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar([FromRoute] int id)
        {
            try
            {
                await _carService.DeleteCar(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
