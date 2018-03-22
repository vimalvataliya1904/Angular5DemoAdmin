using System;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.ServiceContract;
using AdminAPI.Services;
using AdminAPI.TokenModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Country")]
    public class CountryController : Controller
    {
        private ICountry_Repository _iCountryRepository;

        public CountryController()
        {
            _iCountryRepository = new Country_Repository();
        }

        [HttpGet]
        public IActionResult GetCountryMaster()
        {
            var data = _iCountryRepository.GetAll().Result;
            return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Country List", APIOpStatus.Success, data));
        }     

        // POST: api/Country
        [HttpPost]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> PostCountry([FromBody] CountryMaster obj)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());

            obj.IsActive = true;
            await _iCountryRepository.Insert(obj);
            return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Country save successfully.", APIOpStatus.Success, obj));
        }

        // PUT: api/Country/id
        [HttpPut("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> PutCountry([FromRoute] int id, [FromBody] CountryMaster obj)
        {
            obj.CountryId = id;
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());

            if (id != obj.CountryId)
                return BadRequest();
            try
            {
                obj.IsActive = true;
                await _iCountryRepository.Update(obj);
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
            return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Country updated successfully.", APIOpStatus.Success, obj));
        }

        // GET: api/Country/id
        [HttpGet("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> GetCountry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return Ok(ModelState.ResponseValidation());
            try
            {
                var obj = await _iCountryRepository.GetById(id);
                if (obj == null)
                {
                    return NotFound();
                }
                return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Country get successfully", APIOpStatus.Success, obj));
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
        }

        // DELETE: api/Country/id
        [HttpDelete("{id}")]
        [AuthorizeUser(Roles = "A")]
        public async Task<IActionResult> DeleteCountry([FromRoute] int id)
        {
            try
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Ok, "Country deleted successfully.", APIOpStatus.Success, await _iCountryRepository.Delete(id)));
            }
            catch (Exception ex)
            {
                return Ok(APIResponse.SetResponse(APIResponseStatus.Error, ex.Message, APIOpStatus.Error, null));
            }
        }
    }
}
