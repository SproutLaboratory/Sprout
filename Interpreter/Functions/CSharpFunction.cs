using System;
using System.Collections.Generic;

namespace SproutInterpreter
{
    public class CSharpFunction
    {
        public string Name { get; set; }
        public Func<List<SproutValue>, SproutValue> Func { get; set; }
        public CSharpFunction(string name, Func<List<SproutValue>, SproutValue> func)
        { Name = name; Func = func; }
    }
}