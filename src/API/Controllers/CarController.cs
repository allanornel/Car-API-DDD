using Application.DTOs;
using Application.Services.Car;
using Application.Exceptions;
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
                var paginationModel = await _carService.GetCars(query, page);
                return Ok(paginationModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something wrong happened.");
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
        public async Task<IActionResult> AddCar([FromForm] string name, [FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");

                CarDTO carDTO = await ProcessCarDTO(name, file);
                var car = await _carService.AddCar(carDTO);
                var uri = new Uri($"/api/cars/{car.Id}", UriKind.Relative);
                return Created(uri, car);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error adding car");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar([FromRoute] int id, [FromForm] string? name, [FromForm] IFormFile? file)
        {
            try
            {
                CarDTO carDTO = await ProcessCarDTO(name, file);
                await _carService.UpdateCar(id, carDTO);
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

        private async Task<CarDTO> ProcessCarDTO(string? name, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                if (!file.ContentType.StartsWith("image/"))
                    throw new BadHttpRequestException("File is not an image");

                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    return new CarDTO(name, $"data:{file.ContentType};base64,{Convert.ToBase64String(fileBytes)}");
                }
            }
            else
            {
                return new CarDTO(name, "");
            }
        }
    }
}
