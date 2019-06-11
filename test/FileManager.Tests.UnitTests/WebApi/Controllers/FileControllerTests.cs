using FileManager.Core.Operations.FileOperations;
using FileManager.DataContracts;
using FileManager.DataContracts.V1;
using FileManager.DataContracts.V1.File;
using FileManager.WebApi.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Linq;
using System.Net;
using Xunit;
using static Xunit.Assert;

namespace FileManager.Tests.UnitTests.WebApi.Controllers
{

    public class FileControllerTests : BaseControllerTests<FilesController>
    {
        [Fact]
        [Trait("Category", "FilesController")]
        public void Should_return_200_OK_for_GET_Paged_with_success()
        {
            //arrange
            var pagedResult = new PagedResultDataContract<FileDataContract>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                RowCount = 10,
                Results = Enumerable.Range(1, 10).Select(x => new FileDataContract())
            };
             
            var mockOperation = Substitute.For<IFileOperations>();
            mockOperation.GetPaged(1, 10).Returns(pagedResult);

            var response = new ResponseDataContract<object> {
                Success = true,
                Code = HttpStatusCode.OK,
                Data = pagedResult
            };

            //act
            var result = IsType<OkObjectResult>(Controller.Get(1, 10, mockOperation));

            //asset
            Same(response.Data,((ResponseDataContract<object>)(result.Value)).Data);
        }

        [Fact]
        [Trait("Category", "FilesController")]
        public void Should_return_200_OK_for_Get_by_ID_with_success()
        {
            //arrange
            var key = Guid.NewGuid();

            var resultById = Enumerable.First(Enumerable.Range(1,10).Select(x => new FileDataContract()));
            resultById.Key = key;

            var mockOperation = Substitute.For<IFileOperations>();
            mockOperation.GetById(key.ToString()).Returns(resultById);

            var response = new ResponseDataContract<object>
            {
                Success = true,
                Code = HttpStatusCode.OK,
                Data = resultById
            };

            //act
            var result = IsType<OkObjectResult>(Controller.Get(key.ToString(), mockOperation));

            //asset
            Same(response.Data,((ResponseDataContract<object>)(result.Value)).Data);
        }

        [Fact]
        [Trait("Category", "FilesController")]
        public void Should_return_200_OK_for_Delete_by_ID_with_success()
        {
            // arrange
            var key = Guid.NewGuid();

            int? resultDel = 1;

            var mockOperation = Substitute.For<IFileOperations>();
            mockOperation.Delete(key.ToString()).Returns(resultDel.Value);

            var response = new ResponseDataContract<object>
            {
                Success = true,
                Code = HttpStatusCode.OK,
                Data = resultDel.Value
            };

            //act
            var result = IsType<OkObjectResult>(Controller.Delete(key.ToString(), mockOperation));
            var dataResult = ((int?)((ResponseDataContract<object>)(result.Value)).Data).Value == ((int?)response.Data).Value;

            //asset
            True(dataResult);
        }

        [Fact]
        [Trait("Category", "FilesController")]
        public void Should_return_404_BadRequest_for_Get_Paged_Not_found()
        {
            // arrange
            var pagedResult = new PagedResultDataContract<FileDataContract>();

            var response = new ResponseDataContract<object>
            {
                Success = false,
                Code = HttpStatusCode.NotFound,
                Data = null
            };

            var mockOperation = Substitute.For<IFileOperations>();
            mockOperation.GetPaged(-1,10).Returns(pagedResult);

            // act
            var result = IsType<NotFoundObjectResult>(Controller.Get(1, 10, mockOperation));

            // assert
            Same(response.Data, ((ResponseDataContract<object>)result.Value).Data);
        }
    }
}
