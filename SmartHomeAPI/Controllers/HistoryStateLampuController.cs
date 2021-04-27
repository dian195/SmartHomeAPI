using Microsoft.AspNetCore.Mvc;
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
    public class HistoryStateLampuController : Controller
    {
        public Appdb Db { get; }

        public HistoryStateLampuController(Appdb db)
        {
            Db = db;
        }

        // GET api/ActionLampu
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllData()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new HistoryStateLampuQuery(Db);
                var result = await query.AllDataSyn();
                return new OkObjectResult(result);
            }
            catch(Exception exc)
            {
                var response = new ResponseHistoryStateLampu();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/ActionLampu/GetData
        [HttpPost("GetData")]
        public async Task<IActionResult> GetDataWithFilter([FromBody] HistoryFilterBody body)
        {
            try
            {
                var response = new ResponseHistoryStateLampu();

                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new HistoryStateLampuQuery(Db);
                var DateFrom = body.Date_From;
                var DateTo = body.Date_To;

                var result = await query.SelectWithFilterSync(body.Lampu_ID, body.Lampu_State, DateFrom, DateTo);
                if (result is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }
                return new OkObjectResult(result);
            }
            catch(Exception exc)
            {
                var response = new ResponseHistoryStateLampu();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}
