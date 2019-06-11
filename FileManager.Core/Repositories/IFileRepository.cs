using FileManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Repositories
{
    public interface IFileRepository
    {
        File GetById(string idContato);

        PagedResult<File> GetPagedFile(int page, int size);

        File Insert(File file);

        File Update(File file);

        int Delete(File file);
    }
}
