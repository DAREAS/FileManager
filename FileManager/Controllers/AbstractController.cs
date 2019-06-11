using FileManager.DataContracts.V1;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FileManager.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AbstractController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        protected OkObjectResult OkWithSuccess(object result, HttpStatusCode code = HttpStatusCode.OK)
        {
            var response = new ResponseDataContract<object>
            {
                Success = true,
                Code = code,
                Data = result
            };

            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected new BadRequestObjectResult BadRequest()
        {
            var response = new ResponseDataContract<object>
            {
                Success = true,
                Code = HttpStatusCode.BadRequest,
                Data = null
            };
            
            return base.BadRequest(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected NotFoundObjectResult NotFound(string resource, object key)
        {
            var response = new ResponseDataContract<object>
            {
                Success = true,
                Code = HttpStatusCode.NotFound,
                Data = null
            };
            
            return base.NotFound(response);
        }
    }
}
