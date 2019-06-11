using FileManager.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Operations
{
    public interface IOperation<T> where T : class
    {
        T GetById(string key);

        PagedResultDataContract<T> GetPaged(int page, int size);

        T Insert(T data);

        T Update(string key, T data);

        int? Delete(string key);
    }
}
