using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Analizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalizerController : ControllerBase
    {
        // GET: api/<AnalizerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AnalizerController>/5
        [HttpGet("{name}")]
        public async Task<ActionResult<string>> Get(string name)
        {
            var analizer = Program.Analizers.FirstOrDefault(p => p.Name.Equals(name));
            if (analizer == null)
                return BadRequest($"Analizer with name '{name}' not found");
            if(analizer.IsBusy == false)
                return BadRequest($"Analyzer is not working");

            return Ok(Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress);
        }

        // POST api/<AnalizerController>
        [HttpPost("{name}")]
        public async Task<ActionResult<string>> Post( string name, int patientId, List<string> services)
        {
            var analizer = Program.Analizers.FirstOrDefault(p => p.Name.Equals(name));
            if (analizer == null)
                return BadRequest($"Analizer with name '{name}' not found");
            if (analizer.IsBusy == true)
                return BadRequest($"Analizer is busy");
            foreach (var item in services)
            {
                if(services.FirstOrDefault(p=>p.Equals(item)) == null)
                    return BadRequest($"Analyzer can not do this order.May be order contains services which analyzer does not support");
            }

            Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).IsBusy = true;
            Program.Timer.Elapsed += async (sender, e) => await HandleTimer(name);
            Program.Timer.Start();
           // var b = Program.Analizers.FirstOrDefault(p => p.Name.Equals(name));
            return Ok();
        }
        private Task HandleTimer(string name)
        {
            return Task.Run(() =>
            {
                Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress += 10;
                if (Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress == 100)
                {
                    Program.Timer.Stop();
                    Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).Progress = 0;
                    Program.Analizers.FirstOrDefault(p => p.Name.Equals(name)).IsBusy = false;
                }
            });
        }

    }
}
