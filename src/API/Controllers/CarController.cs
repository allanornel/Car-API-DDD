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
        public async Task<IActionResult> GetCars([FromQuery] string? query = "", [FromQuery] int page = 1)
        {
            try
            {
                var carsModel = await _carService.GetCars(query, page);
                return Ok(carsModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        public async Task<IActionResult> AddCar(string name, IFormFile file)
        {
            CarDTO carDTO;
            if (file != null && file.Length > 0)
            {
                if (!file.ContentType.StartsWith("image/"))
                    return BadRequest("File is not an image");

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    carDTO = new CarDTO(name, Convert.ToBase64String(fileBytes));
                }

                var car = await _carService.AddCar(carDTO);
                var uri = new Uri($"/api/cars/{car.Id}", UriKind.Relative);
                return Created(uri, car);
            }

            return BadRequest("File is empty");

        }

        [HttpPut("{id}")]
        public IActionResult UpdateCar([FromRoute] int id, [FromBody] CarDTO carDTO)
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
