using System;
using System.Threading;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Authentication;
using Hermes.API.Advertisement.Domain.Constants;
using Hermes.API.Advertisement.Domain.Requests;
using Hermes.API.Advertisement.Domain.Services.Advertisement;
using Hermes.API.Advertisement.Domain.Services.AdvertisementSeeder;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Advertisement.Controllers
{
    [ApiController, Route("v1/[controller]")]
    [HermesAuthorize]
    public class AdvertisementController : Controller
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IAdvertisementSeederService _advertisementSeederService;

        public AdvertisementController(IAdvertisementService advertisementService,
            IAdvertisementSeederService advertisementSeederService)
        {
            _advertisementService = advertisementService;
            _advertisementSeederService = advertisementSeederService;
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

        [HttpDelete("{id}")]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _advertisementService.Delete(id);
            return Ok();
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

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            await _advertisementSeederService.DoWork();
            return Ok();
        }
    }
}