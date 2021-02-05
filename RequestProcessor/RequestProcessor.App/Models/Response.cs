﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RequestProcessor.App.Models
{
    internal class Response : IResponse
    {
        public bool Handled { get; set; }

        public int Code { get; set; }

        public string Content { get; set; }
    }
}