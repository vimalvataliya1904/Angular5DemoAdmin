using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.ServiceContract;
using AdminAPI.Services;
using AdminAPI.TokenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Currency")]
    public class CurrencyController : Controller
    {
        private ICurrency_Repository _ICurrency_Repository;

        public CurrencyController()
        {
            this._ICurrency_Repository = new Currency_Repository();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "All Currency List", APIOpStatus.Success, await _ICurrency_Repository.GetAll()));
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
        }

        // POST: api/Currency
        [HttpPost]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> PostCurrency([FromBody] CurrencyMaster obj)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());

            obj.IsActive = true;
            await _ICurrency_Repository.Insert(obj);
            return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Currency save successfully.", APIOpStatus.Success, obj));
        }

        // PUT: api/Currency/id
        [HttpPut("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> PutCurrency([FromRoute] int id, [FromBody] CurrencyMaster obj)
        {
            obj.CurrencyId = id;
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());

            if (id != obj.CurrencyId)
                return BadRequest();
            try
            {
                obj.IsActive = true;
                await _ICurrency_Repository.Update(obj);
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
            return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Currency updated successfully.", APIOpStatus.Success, obj));
        }

        // GET: api/Currency/id
        [HttpGet("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> GetCurrency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());
            try
            {
                var obj = await _ICurrency_Repository.GetById(id);
                if (obj == null)
                {
                    return NotFound();
                }
                return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Currency get successfully", APIOpStatus.Success, obj));
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
        }

        // DELETE: api/Currency/id
        [HttpDelete("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> DeleteCurrency([FromRoute] int id)
        {
            try
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Currency deleted successfully.", APIOpStatus.Success, await _ICurrency_Repository.Delete(id)));
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
        }
    }
}