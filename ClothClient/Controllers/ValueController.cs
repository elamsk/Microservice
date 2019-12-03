using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Products;

namespace ClothClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        public IClothingService _service;
        public ValueController()
        {
            _service = ServiceProxy.Create<IClothingService>(new Uri("fabric:/FlipCart/ClothingProduct"), new ServicePartitionKey(0));
        }


        // GET: api/Value
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var col = await _service.allClothesGet();
            return Ok(col);
        }

        // GET: api/Value/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Value
        [HttpPost]
        public void Post([FromBody] ClothProduct value)
        {
            _service.AddCloth(value);
        }

        // PUT: api/Value/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
