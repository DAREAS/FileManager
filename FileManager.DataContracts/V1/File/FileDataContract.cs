using FileManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.DataContracts.V1.File
{
    public class FileDataContract
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime FinishProcessDate { get; set; }
        public FileStatus Status { get; set; }
        public string OriginPath { get; set; }
        public string DestinyPath { get; set; }
        public string Checksum { get; set; }

        public List<InfoDataContract> Error { get; set; }
    }
}
