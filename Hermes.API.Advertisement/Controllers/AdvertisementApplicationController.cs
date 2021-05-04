using System;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Authentication;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Requests;
using Hermes.API.Advertisement.Domain.Services.AdvertisementApplication;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Advertisement.Controllers
{
    [ApiController, Route("v1/[controller]")]
    [HermesAuthorize]
    public class AdvertisementApplicationController : Controller
    {
        private readonly IAdvertisementApplicationService _advertisementApplicationService;

        public AdvertisementApplicationController(IAdvertisementApplicationService advertisementApplicationService)
        {
            _advertisementApplicationService = advertisementApplicationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] SearchAdvertisementApplicationRequest searchAdvertisementApplicationRequest)
        {
            var applications = await _advertisementApplicationService.GetAll(searchAdvertisementApplicationRequest);
            if (applications == null || applications.Count == 0)
            {
                return NotFound();
            }

            return Ok(applications);
        }


        [HttpGet("{advertisementId}")]
        public async Task<IActionResult> Get(Guid advertisementId, [FromQuery] long applicantId)
        {
            var advertisementApplication =
                await _advertisementApplicationService.GetByAdvertisementIdAndApplicantId(advertisementId, applicantId);
            if (advertisementApplication == null)
                return NotFound();
            return Ok(advertisementApplication);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(long id)
        {
            var advertisementApplication =
                await _advertisementApplicationService.GetById(id);
            if (advertisementApplication == null)
                return NotFound();
            return Ok(advertisementApplication);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] AdvertisementApplicationDto advertisementApplicationDto)
        {
            var response = await _advertisementApplicationService.Apply(advertisementApplicationDto);
            return Created("/v1/AdvertisementApplication", response);
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(long id)
        {
            await _advertisementApplicationService.Approve(id);
            return Ok();
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(long id)
        {
            await _advertisementApplicationService.Reject(id);
            return Ok();
        }
    }
}