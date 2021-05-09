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

        [HttpPut("lender-approve/{id}")]
        public async Task<IActionResult> LenderApprove(long id)
        {
            await _advertisementApplicationService.LenderApprove(id);
            return Ok();
        }

        [HttpPut("lender-reject/{id}")]
        public async Task<IActionResult> LenderReject(long id)
        {
            await _advertisementApplicationService.LenderReject(id);
            return Ok();
        }

        [HttpPut("borrower-approve/{id}")]
        public async Task<IActionResult> BorrowerApprove(long id)
        {
            await _advertisementApplicationService.BorrowerApprove(id);
            return Ok();
        }

        [HttpPut("borrower-reject/{id}")]
        public async Task<IActionResult> BorrowerReject(long id)
        {
            await _advertisementApplicationService.BorrowerReject(id);
            return Ok();
        }

        [HttpPut("given-back-to-lender/{id}")]
        public async Task<IActionResult> GivenBackToLender(long id)
        {
            await _advertisementApplicationService.GivenBackToLender(id);
            return Ok();
        }

        [HttpPut("lender-took-item-back/{id}")]
        public async Task<IActionResult> LenderTookItemBack(long id)
        {
            await _advertisementApplicationService.LenderTookBack(id);
            return Ok();
        }
    }
}