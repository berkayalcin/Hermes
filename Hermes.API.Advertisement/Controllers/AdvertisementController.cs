using System;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Requests;
using Hermes.API.Advertisement.Domain.Services.Advertisement;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Advertisement.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAdvertisementRequest createAdvertisementRequest)
        {
            var advertisement = await _advertisementService.Create(createAdvertisementRequest);
            return Created("/v1/Advertisement", advertisement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,
            [FromBody] UpdateAdvertisementRequest updateAdvertisementRequest)
        {
            var advertisement = await _advertisementService.Update(updateAdvertisementRequest);
            return Ok(advertisement);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchAdvertisementRequest searchAdvertisementRequest)
        {
            var advertisements = await _advertisementService.GetAll(searchAdvertisementRequest);
            if (advertisements.Items == null || advertisements.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(advertisements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var advertisement = await _advertisementService.Get(id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return Ok(advertisement);
        }
    }
}