using FileManager.Infrastructure.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Models
{
    public class File
    {
        [Key]
        public string Key { get; set; }
        public string Name { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime ProcessDate { get; set; }
        public DateTime FinishProcessDate { get; set; }
        public FileStatus Status { get; set; }
        public string OriginPath { get; set; }
        public string DestinyPath { get; set; }
        public string Checksum { get; set; }
    }
}
