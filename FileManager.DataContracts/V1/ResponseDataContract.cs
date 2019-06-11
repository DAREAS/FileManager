using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FileManager.DataContracts.V1
{
    public class ResponseDataContract<T> where T : class
    {
        public bool Success { get; set; }
        public HttpStatusCode Code { get; set; }
        public T Data { get; set; }
    }
}
