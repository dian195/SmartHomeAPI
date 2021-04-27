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
    public class MasterLampuController : ControllerBase
    {
        public Appdb Db { get; }

        public MasterLampuController(Appdb db)
        {
            Db = db;
        }

        // GET api/MasterLampu
        [HttpGet("GetAllData")]
        public async Task<IActionResult> GetAllData()
        {
            var response = new ResponseMasterLampu();

            try
            {
                await Db.Connection.OpenAsync();
                var query = new MasterLampuQuery(Db);
                var result = await query.AllDataSyn();
                return new OkObjectResult(result);
            }
            catch(Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // GET api/MasterLampu/5
        [HttpGet("GetData/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var response = new ResponseMasterLampu();

            try
            {
                await Db.Connection.OpenAsync();
                var query = new MasterLampuQuery(Db);
                var result = await query.FindOneAsync(id);
                
                if (result is null)
                {
                    response.message = "Data tidak ditemukan !";
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

        // POST api/MasterLampu
        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody] MasterLampuInsertBody body)
        {
            var response = new ResponseMasterLampu();

            try
            {
                await Db.Connection.OpenAsync();
                body.Db = Db;
                await body.InsertAsync();
                //return new OkObjectResult(body);
                response.message = "Data berhasil disimpan !";
                response.status = 0;
                return new OkObjectResult(response);
            }
            catch(Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // PUT api/MasterLampu/5
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] MasterLampuInsertBody body)
        {
            var response = new ResponseMasterLampu();

            try
            {
                await Db.Connection.OpenAsync();
                var query = new MasterLampuQuery(Db);
                var result = await query.FindOneAsync(id);

                if (result is null)
                {
                    //return new NotFoundResult();
                    response.message = "Data tidak ditemukan !";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                result.ID = id;
                result.Lampu_Name = body.Lampu_Name;
                result.Is_Active = body.Is_Active;
                result.Watt = body.Watt;
                await result.UpdateAsync();

                //return new OkObjectResult(result);
                response.message = "Data Berhasil disimpan!";
                response.status = 0;
                return new OkObjectResult(response);
            }
            catch (Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // PUT api/MasterLampu/5
        [HttpPut("UpdateNominalBiaya/{biaya}")]
        public async Task<IActionResult> UpdateNominalBiaya(decimal biaya)
        {
            var response = new ResponseMasterLampu();

            try
            {
                await Db.Connection.OpenAsync();
                var query = new MasterLampuQuery(Db);
                await query.UpdateBiayaAsync(biaya);

                //return new OkObjectResult(result);
                response.message = "Data Berhasil disimpan!";
                response.status = 0;
                return new OkObjectResult(response);
            }
            catch (Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        //// DELETE api/MasterLampu/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOne(int id)
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new MasterLampuQuery(Db);
        //    var result = await query.FindOneAsync(id);
        //    if (result is null)
        //        return new NotFoundResult();
        //    await result.DeleteAsync();
        //    return new OkResult();
        //}

        //// DELETE api/MasterLampu
        //[HttpDelete]
        //public async Task<IActionResult> DeleteAll()
        //{
        //    await Db.Connection.OpenAsync();
        //    var query = new MasterLampuQuery(Db);
        //    await query.DeleteAllAsync();
        //    return new OkResult();
        //}
    }
}
