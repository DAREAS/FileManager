using FileManager.Core.Models;
using System;
using System.Linq;

namespace FileManager.Repository
{
    public class BaseRepository<T>
    {
        public PagedResult<T> GetPagedResultForQuery(IQueryable<T> query, int page, int pageSize)
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }
    }
}
