﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager.Core.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}
