using System;
using System.Collections.Generic;
using System.Text;

namespace FileReader.Abstractions;

public interface ILineFilter
{
    bool ShouldPublish(string line);
}