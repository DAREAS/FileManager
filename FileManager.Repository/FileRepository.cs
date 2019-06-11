using FileManager.Core.Models;
using FileManager.Core.Repositories;
using FileManager.Repository.Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Repository
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        private readonly ApiDbContext _context;

        public FileRepository(ApiDbContext context)
        {
            _context = context;
        }

        public File GetById(string idContato)
        {
            return _context.Files.Find(idContato);
        }

        public PagedResult<File> GetPagedFile(int page, int size)
        {
            var results = from file in _context.Files
                          where file.Status == FileManager.Infrastructure.Enums.FileStatus.Queued
                          select file;

            return GetPagedResultForQuery(results, page, size);
        }

        public File Insert(File file)
        {
            if (string.IsNullOrEmpty(file.Key))
                file.Key = Guid.NewGuid().ToString();

            _context.Files.Add(file);
            _context.SaveChanges();

            return file;
        }

        public File Update(File file)
        {
            _context.Files.Update(file);
            _context.SaveChanges();

            return _context.Files.Find(file.Key);
        }

        public int Delete(File file)
        {
            _context.Files.Remove(file);

            return _context.Files.Find(file.Key) == null ? 1 : 0;
        }
    }
}
