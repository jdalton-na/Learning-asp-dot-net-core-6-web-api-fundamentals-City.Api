
using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/cities")]
    [ApiVersion("1.0")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// City collection represents all cities documented in CityInfo database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchQuery">Simple text search against name and description properties</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">Maximum page size is 20</param>
        /// <returns></returns>
        /// <response code="406">Only supports application/json as accept type</response>
        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            string? name,
            string? searchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            pageSize = (pageSize <= maxCitiesPageSize) ? pageSize : maxCitiesPageSize;
            var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery,
                pageNumber, pageSize);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
        }

        /// <summary>
        /// Returns a specified City.  By default will not return Points of Interest for that city, but this can be override with parameter includePointsOfInterest=true
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="includePointsOfInterest"></param>
        /// <returns>IActionResult</returns>
        /// <response code="404">requested City was not found in Cities database</response>
        /// <response code="400">Request is not well formed</response>
        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCity(int Id, bool includePointsOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(Id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }
            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
