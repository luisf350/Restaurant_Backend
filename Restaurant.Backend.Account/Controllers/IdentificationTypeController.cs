using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurant.Backend.Common.Constants;
using Restaurant.Backend.CommonApi.Base;
using Restaurant.Backend.Domain.Contract;
using Restaurant.Backend.Dto.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurant.Backend.Entities.Entities;

namespace Restaurant.Backend.Account.Controllers
{
    public class IdentificationTypeController : BaseController
    {
        private readonly IIdentificationTypeDomain _identificationTypeDomain;

        public IdentificationTypeController(ILogger<IdentificationTypeController> logger, IIdentificationTypeDomain identificationTypeDomain, IMapper mapper)
            : base(logger, mapper)
        {
            _identificationTypeDomain = identificationTypeDomain;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var resultTypes = await _identificationTypeDomain.GetAll();

            return Ok(Mapper.Map<IList<IdentificationTypeDto>>(resultTypes));
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var resultType = await _identificationTypeDomain.Find(id);

            return resultType == null ?
                (IActionResult)NotFound(string.Format(Constants.NotFound, id))
                : Ok(Mapper.Map<IdentificationTypeDto>(resultType));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(IdentificationTypeDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.ModelNotValid);
            }

            var model = Mapper.Map<IdentificationType>(modelDto);
            var result = await _identificationTypeDomain.Create(model);

            return result == 0 ?
                (IActionResult)BadRequest(Constants.OperationNotCompleted)
                : Created("GetAll", modelDto);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(IdentificationTypeDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.ModelNotValid);
            }

            var model = await _identificationTypeDomain.Find(modelDto.Id);
            if (model == null)
            {
                return NotFound(string.Format(Constants.NotFound, modelDto.Id));
            }

            model.Name = modelDto.Name;
            model.Description = modelDto.Description;

            var result = await _identificationTypeDomain.Update(model);

            return result ?
                Ok(Mapper.Map<IdentificationTypeDto>(model))
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(IdentificationTypeDto modelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Constants.ModelNotValid);
            }

            var model = await _identificationTypeDomain.Find(modelDto.Id);
            if (model == null)
            {
                return NotFound(string.Format(Constants.NotFound, modelDto.Id));
            }
            
            var result = await _identificationTypeDomain.Delete(model);

            return result == 1 ?
                Ok(Mapper.Map<IdentificationTypeDto>(model))
                : (IActionResult)BadRequest(Constants.OperationNotCompleted);
        }
    }
}