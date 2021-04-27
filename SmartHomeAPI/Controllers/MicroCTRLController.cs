using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHomeAPI.Models;
using SmartHomeAPI.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    public class MicroCTRLController : ControllerBase
    {
        public Appdb Db { get; }

        public MicroCTRLController(Appdb db)
        {
            Db = db;
        }

        // GET api/MicroCTRL/GetLastState
        [HttpGet("GetLastState")]
        public async Task<IActionResult> GetLastState()
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new HistoryStateLampuQuery(Db);
                var result = await query.AllDataSyn();

                if (result != null)
                {
                    int hsil = 1;
                    var returnedJson = "{";

                    foreach (var hasil in result)
                    {
                        returnedJson += "\"" + hsil + "\": " + hasil.Lampu_State + ",";
                        hsil++;
                    }

                    returnedJson += "}";

                    // parse the json response so that we can get at the key/value pairs
                    dynamic api = JObject.Parse(returnedJson);
                    return new OkObjectResult(api);
                }
                else
                {
                    var response = new ResponsePushDataMicroCTRL();

                    response.message = "Data tidak ditemukan !";
                    response.status = 1;
                    return new OkObjectResult(response);
                }
            }
            catch (Exception exc)
            {
                var response = new ResponsePushDataMicroCTRL();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/MicroCTRL/PushState
        [HttpGet("PushState")]
        public async Task<IActionResult> Post([FromQuery] int lmp, [FromQuery] int stat, [FromQuery] double date)
        {
            var response = new ResponsePushDataMicroCTRL();

            if (stat < 1 && stat > 2)
            {
                response.message = "Status tidak terdaftar ! 1 untuk non aktif, 2 untuk aktif";
                response.status = 1;
                return new OkObjectResult(response);
            }

            await Db.Connection.OpenAsync();
            var query = new MicroCTRLDTO(Db);
            var queryCheck = new MasterLampuQuery(Db);
            var result = await queryCheck.FindOneAsync(lmp);

            if (result is null)
            {
                response.message = "Lampu tidak terdaftar !";
                response.status = 1;
                return new OkObjectResult(response);
            }

            //Check Last State
            result = await queryCheck.CheckLastState(lmp, stat);

            if (result != null)
            {
                response.message = "State Lampu tidak benar !";
                response.status = 1;
                return new OkObjectResult(response);
            }

            query.Lampu_ID = lmp;
            query.Lampu_State = stat;
            query.Change_Date = date;

            try
            {
                await query.InsertAsync();
                response.message = "Berhasil";
                response.status = 0;
                return new OkObjectResult(response);
            }
            catch (Exception exc)
            {
                response.message = exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        //// PUT api/MicroCTRL/5
        //[HttpPut("Update/{id}")]
        //public async Task<IActionResult> PutOne(int id, [FromBody] MasterLampuInsertBody body)
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new MasterLampuQuery(Db);
        //    var result = await query.FindOneAsync(id);
        //    if (result is null)
        //        return new NotFoundResult();
        //    result.ID = id;
        //    result.Lampu_Name = body.Lampu_Name;
        //    result.Is_Active = body.Is_Active;
        //    await result.UpdateAsync();
        //    return new OkObjectResult(result);
        //}

        // GET api/MicroCTRL/GetModeApp
        [HttpGet("GetModeApp")]
        public async Task<IActionResult> GetModeApp()
        {
            await Db.Connection.OpenAsync();
            var query = new ModeAppQuery(Db);
            var result = await query.FindOneAsync();

            if (result != null)
            {
                var returnedJson = "{";
                returnedJson += "\"" + "mode" + "\": " + result.Mode + ",";
                returnedJson += "}";

                // parse the json response so that we can get at the key/value pairs
                dynamic api = JObject.Parse(returnedJson);
                return new OkObjectResult(api);
            }
            else
            {
                var response = new ResponsePushDataMicroCTRL();

                response.message = "Data tidak ditemukan !";
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}