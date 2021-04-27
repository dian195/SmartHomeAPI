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
    public class ModeAppController : ControllerBase
    {
        public Appdb Db { get; }

        public ModeAppController(Appdb db)
        {
            Db = db;
        }

        // POST api/ChangeMode
        [HttpPost("ChangeMode")]
        public async Task<IActionResult> Post([FromBody] ModeAppDTO body)
        {
            try
            {
                await Db.Connection.OpenAsync();
                var query = new ModeAppQuery(Db);
                body.Db = Db;
                await body.UpdateInsertAsync();

                var response = new ResponseModeApp();

                response.message = "Data Berhasil Disimpan !";
                response.status = 0;
                return new OkObjectResult(response);
            }
            catch(Exception exc)
            {
                var response = new ResponseModeApp();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/GetModeApp
        [HttpGet("GetModeApp")]
        public async Task<IActionResult> GetModeApp()
        {
            try
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
                    var response = new ResponseModeApp();

                    response.message = "Data tidak ditemukan !";
                    response.status = 1;
                    return new OkObjectResult(response);
                }
            }
            catch (Exception exc)
            {
                var response = new ResponseModeApp();

                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }
    }
}