using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Infrastructure.Enums
{
    public enum FileStatus
    {
        Moved,
        Copied,
        Created,
        Corrupted,
        InProcess,
        Queued,
        Error
    }
}
