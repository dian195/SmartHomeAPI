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
    public class ReportInfoController : ControllerBase
    {
        public Appdb Db { get; }

        public ReportInfoController(Appdb db)
        {
            Db = db;
        }

        // GET api/GetDataLampuForDDLInfo
        [HttpGet("ListDataLampuForDDLInfo")]
        public async Task<IActionResult> ListDataLampuForDDLInfo()
        {
            var response = new ResponseReportInfo();

            try
            {
                await Db.Connection.OpenAsync();
                var query = new ReportInfoQuery(Db);
                var result = await query.GetDataLampuForDDLInfo();
                return new OkObjectResult(result);
            }
            catch (Exception exc)
            {
                response.message = "Error ! " + exc.ToString();
                response.status = 1;
                return new OkObjectResult(response);
            }
        }

        // Post api/InfoWaktuLampuHidup
        [HttpPost("InfoWaktuLampuHidup")]
        public async Task<IActionResult> InfoWaktuLampuHidup([FromBody] InfoWaktuLampuHidupFilter body)
        {
            var response = new ResponseReportInfo();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new ReportInfoQuery(Db);
                var DateFrom = body.Date_From;
                var DateTo = body.Date_To;

                var result = await query.GetReportInfoLampuByWaktuHidup(body.Date_From, body.Date_To, body.Lampu_ID);
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

        // Post api/InfoTotalWaktuLampuHidup
        [HttpPost("InfoTotalWaktuLampuHidup")]
        public async Task<IActionResult> InfoTotalWaktuLampuHidup([FromBody] InfoWaktuLampuHidupFilter body)
        {
            var response = new ResponseReportInfo();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new ReportInfoQuery(Db);
                var DateFrom = body.Date_From;
                var DateTo = body.Date_To;

                var result = await query.InfoTotalWaktuLampuHidup(body.Date_From, body.Date_To, body.Lampu_ID);
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

        // Post api/InfoWattLampuHidup
        [HttpPost("InfoWattLampuHidup")]
        public async Task<IActionResult> InfoWattLampuHidup([FromBody] InfoWaktuLampuHidupFilter body)
        {
            var response = new ResponseReportInfo();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new ReportInfoQuery(Db);
                var DateFrom = body.Date_From;
                var DateTo = body.Date_To;

                var result = await query.GetInfoWattLampuHidup(body.Date_From, body.Date_To, body.Lampu_ID);
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

        // Post api/InfoTotalWattLampuHidup
        [HttpPost("InfoTotalWattLampuHidup")]
        public async Task<IActionResult> InfoTotalWattLampuHidup([FromBody] InfoWaktuLampuHidupFilter body)
        {
            var response = new ResponseReportInfo();

            try
            {
                if (body is null)
                {
                    response.message = "Data tidak ditemukan ! ";
                    response.status = 1;
                    return new OkObjectResult(response);
                }

                await Db.Connection.OpenAsync();
                var query = new ReportInfoQuery(Db);
                var DateFrom = body.Date_From;
                var DateTo = body.Date_To;

                var result = await query.InfoTotalWattLampuHidup(body.Date_From, body.Date_To, body.Lampu_ID);
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