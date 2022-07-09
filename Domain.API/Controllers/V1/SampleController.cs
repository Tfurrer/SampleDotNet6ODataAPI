using AutoMapper;
using AutoMapper.AspNet.OData;
using Domain.API.Helpers;
using Domain.API.Models;
using Domain.API.Profiles;
using Domain.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SQLDataLayer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.API.Controllers.V2
{
    [ApiVersion("1.0")]
    //[Authorize]
    //[APIKey]
    [ApiController]
    [Route("api/v{version:apiVersion}/sample")]
    public class SampleController : ControllerBase
    {
        private DataContext _db;
        private readonly IMapper _mapper;
        private QuerySettings _settings = new QuerySettings { ODataSettings = new ODataSettings { HandleNullPropagation = HandleNullPropagationOption.Default } };

        public SampleController(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<ExampleDataContract> options)
        {
            var result = await _db.ExampleDatas.GetAsync(_mapper, options, _settings);
            if (options.SelectExpand != null)
                return Ok(options.SelectExpand.ApplyTo(result.AsQueryable<ExampleDataContract>(), new ODataQuerySettings()));
            else
                return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExampleDataContractNew dataNew)
        {
            try
            {
                var _record = _mapper.Map<ExampleDataContractNew, ExampleData>(dataNew);
                var data = await _db.ExampleDatas.AddAsync(_record);
                await _db.SaveChangesAsync();
                var result = _mapper.Map<ExampleData, ExampleDataContract>(data.Entity);
                return Ok(result);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(ODataQueryOptions<ExampleDataContract> options, [FromBody] JsonPatchDocument<ExampleDataContract> ExampleDataContractPatch)
        {
            if (!options.Filter.RawValue.ToLower().Contains("id eq"))
                return BadRequest("Must include an odata filter for the id");
            var ExampleData = (await _db.ExampleDatas.GetAsync(_mapper, options, new QuerySettings { ODataSettings = new ODataSettings { HandleNullPropagation = HandleNullPropagationOption.Default } })).FirstOrDefault();
            if (ExampleData == null)
                return NotFound();
            var _record = _db.ExampleDatas.FirstOrDefault(x => x.Id == ExampleData.Id);

            ExampleDataContractPatch.Operations.MapPatchPaths();
            var ExampleDataPatch = _mapper.Map<JsonPatchDocument<ExampleDataContract>, JsonPatchDocument<ExampleData>>(ExampleDataContractPatch);
            ExampleDataPatch.ApplyTo(_record);
            _db.Entry(_record).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _db.SaveChangesAsync();
            var result = _mapper.Map<ExampleData, ExampleDataContract>(_record);
            return Accepted(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ODataQueryOptions<ExampleDataContract> options, [FromBody] ExampleDataContract ExampleDataContractUpdate)
        {
            if (!options.Filter.RawValue.ToLower().Contains("id eq"))
                return BadRequest("Must include an odata filter for the id");

            var ExampleData = (await _db.ExampleDatas.GetAsync(_mapper, options, _settings)).FirstOrDefault();
            if (ExampleData == null)
                return NotFound();

            var dataUpdate = _mapper.Map<ExampleDataContract, ExampleData>(ExampleDataContractUpdate);
            _db.Entry(dataUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _db.SaveChangesAsync();
            var result = _mapper.Map<ExampleData, ExampleDataContract>(dataUpdate);
            return Accepted(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var _record = _db.ExampleDatas.FirstOrDefault(x => x.Id == id);

            if (_record == null)
                return NotFound();
            _db.ExampleDatas.Remove(_record);
            await _db.SaveChangesAsync();
            var result = _mapper.Map<ExampleData, ExampleDataContract>(_record);
            return Accepted(result);
        }
    }
}

