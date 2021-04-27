using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.Models;
using SmartHomeAPI.Models.Filter;
using SmartHomeAPI.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    public class GrafikController : Controller
    {
        public Appdb Db { get; }

        public GrafikController(Appdb db)
        {
            Db = db;
        }

        // Post api/GrafikWattInYear
        [HttpPost("GrafikWattInYear")]
        public async Task<IActionResult> GrafikWattInYear([FromBody] GrafikWattInYearFilter body)
        {
            var response = new ResponseGrafik();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new GrafikQuery(Db);
                var Year = body.Year;
                
                var result = await query.GetGrafikWattInYear(Year);
                if (result is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // Post api/GetTotalInYear
        [HttpPost("GetTotalInYear")]
        public async Task<IActionResult> GetTotalInYear([FromBody] GrafikWattInYearFilter body)
        {
            var response = new ResponseGrafik();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new GrafikQuery(Db);
                var Year = body.Year;

                var result = await query.GetTotalGrafikWattInYear(Year);
                if (result is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}
