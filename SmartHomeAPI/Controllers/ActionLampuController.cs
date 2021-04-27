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
    public class ActionLampuController : ControllerBase
    {
        public Appdb Db { get; }

        public ActionLampuController(Appdb db)
        {
            Db = db;
        }

        //// GET api/ActionLampu
        //[HttpGet]
        //public async Task<IActionResult> GetAllData()
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new MasterLampuQuery(Db);
        //    var result = await query.AllDataSyn();
        //    return new OkObjectResult(result);
        //}

        // POST api/ActionLampu
        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody] ActionLampuBody body)
        {
            try
            {
                var response = new ResponseActionLampu();

                await Db.Connection.OpenAsync();
                var query = new MasterLampuQuery(Db);
                var result = await query.FindOneAsync(body.Lampu_ID);

                if (result is null)
                {
                    response.message = "Data tidak ditemukan !";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                //Check Last State
                result = await query.CheckLastState(body.Lampu_ID, body.Lampu_State);

                if (result != null)
                {
                    response.message = "State Lampu tidak benar !";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                body.Db = Db;
                await body.InsertAsync();

                response.message = "Data berhasil disimpan ! ";
                response.status = 0;

                return new OkObjectResult(response);
            }
            catch(Exception exc)
            {
                var response = new ResponseActionLampu();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}