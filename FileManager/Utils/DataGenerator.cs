using FileManager.Core.Models;
using FileManager.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManager.WebApi.Utils
{
    public class DataGenerator
    {
        public static void Generator(ApiDbContext context)
        {
            if (context.Files.Any())
            {
                return;
            }

            context.Files.AddRange(
                new File
                {
                    Key = Guid.NewGuid().ToString(),
                    Name = "MOV.TV",
                    OriginPath = @"C:\temp\ORG\",
                    DestinyPath = @"C:\temp\DEST\",
                    Status = Infrastructure.Enums.FileStatus.Queued,
                    InitialDate = DateTime.Now,
                },

                new File
                {
                    Key = Guid.NewGuid().ToString(),
                    Name = "FILME.TV",
                    OriginPath = @"C:\temp\ORG\",
                    DestinyPath = @"C:\temp\DEST\",
                    Status = Infrastructure.Enums.FileStatus.Queued,
                    InitialDate = DateTime.Now,
                },

                new File
                {
                    Key = Guid.NewGuid().ToString(),
                    Name = "SERIE.TV",
                    OriginPath = @"C:\temp\ORG\",
                    DestinyPath = @"C:\temp\DEST\",
                    Status = Infrastructure.Enums.FileStatus.Queued,
                    InitialDate = DateTime.Now,
                },

                new File
                {
                    Key = Guid.NewGuid().ToString(),
                    Name = "PROGRAMA.TV",
                    OriginPath = @"C:\temp\ORG\",
                    DestinyPath = @"C:\temp\DEST\",
                    Status = Infrastructure.Enums.FileStatus.Queued,
                    InitialDate = DateTime.Now,
                }
            );

            context.SaveChanges();
        }
    }
}
