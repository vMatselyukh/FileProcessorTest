﻿using Domain.Models;
using System.IO;

namespace Domain.Interfaces
{
    public interface IFileParser
    {
        FileParseResult ParseFile(string content);
    }
}
