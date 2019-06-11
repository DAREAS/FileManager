using System;
using System.Linq;
using FileManager.DataContracts.V1.File;
using Microsoft.AspNetCore.Mvc;
using FileManager.Core.Operations.FileOperations;

namespace FileManager.WebApi.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/file")]
    [Produces("application/json")]
    public class FilesController : AbstractController
    {
        /// <summary>
        /// Get paged all files informations in process to copy or move
        /// If not set page or size, will be considered page = 1 and size = 10
        /// </summary>
        /// <param name="page">Current selected page</param>
        /// <param name="size">Elements in the page</param>
        /// <returns>Retrivies files with process status is Queued<see cref="FileDataContract"/></returns>
        [HttpGet]
        [HttpGet("{page}/{size}")]
        public ActionResult Get(int page, int size, [FromServices] IFileOperations operations)
        {
            try
            {
                var result = operations.GetPaged(page, size);

                if (result == null)
                    return NotFound("Files", "");

                return OkWithSuccess(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get file information by Id
        /// </summary>
        /// <param name="idFile">File identification</param>
        /// <returns>Retrieve specified file by id</returns>
        [HttpGet("{idFile}")]
        public ActionResult Get(string idFile, [FromServices] IFileOperations operations)
        {
            try
            {
                var result = operations.GetById(idFile);

                if (result == null)
                    return OkWithSuccess(result, System.Net.HttpStatusCode.NotFound);

                return OkWithSuccess(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Code = HttpContext.Response.StatusCode, Message = ex.Message });
            }
        }

        /// <summary>
        /// Add file to process of manager
        /// </summary>
        /// <param name="file"><see cref="FileDataContract"/></param>
        [HttpPost]
        public ActionResult Post([FromBody] FileDataContract file, [FromServices] IFileOperations operations)
        {
            try
            {
                var result = operations.Insert(file);

                if (result.Error.Any())
                {
                    return BadRequest(result.Error);
                }

                return Created("", result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Code = HttpContext.Response.StatusCode, Message = ex.Message });
            }
        }

        /// <summary>
        /// Update file to return to process
        /// </summary>
        /// <param name="idFile">File identification</param>
        /// <param name="file"><see cref="FileDataContract"/></param>
        /// <returns>Retrieves file updated</returns>
        [HttpPut("{idFile}")]
        public ActionResult Put(string idFile, [FromBody] FileDataContract file, [FromServices] IFileOperations operations)
        {
            try
            {
                var result = operations.Update(idFile, file);

                if (result == null)
                    return OkWithSuccess(result, System.Net.HttpStatusCode.NotFound);

                if (result.Error.Any())
                    return BadRequest(result.Error);


                return OkWithSuccess(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Code = HttpContext.Response.StatusCode, Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete file of process queued
        /// </summary>
        /// <param name="idFile">File identification</param>
        /// <returns>Returns 1 if OK or 0 if not OK</returns>
        [HttpDelete("{idFile}")]
        public ActionResult Delete(string idFile, [FromServices] IFileOperations operations)
        {
            try
            {
                var result = operations.Delete(idFile);

                if (result == null)
                    return OkWithSuccess(result, System.Net.HttpStatusCode.NotFound);

                return OkWithSuccess(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Code = HttpContext.Response.StatusCode, Message = ex.Message });
            }
        }
    }
}
