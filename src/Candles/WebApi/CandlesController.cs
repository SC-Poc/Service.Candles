using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Candles.Client.Models.Candles;
using Candles.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Candles.WebApi
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CandlesController : ControllerBase
    {
        private readonly ICandlesService _candlesService;
        private readonly IMapper _mapper;

        public CandlesController(ICandlesService candlesService, IMapper mapper)
        {
            _candlesService = candlesService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CandleModel[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(string assetPairId, CandleType candleType, DateTime startDate,
            DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(assetPairId))
                return NotFound();

            var candles = await _candlesService.GetAsync(assetPairId,
                Enum.Parse<Domain.Entities.CandleType>(candleType.ToString()), startDate, endDate);

            var model = _mapper.Map<List<CandleModel>>(candles);

            return Ok(model);
        }
    }
}
