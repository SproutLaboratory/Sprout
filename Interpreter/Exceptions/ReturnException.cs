using System;

namespace SproutInterpreter
{
    public class ReturnException : Exception
    {
        public SproutValue Value { get; set; }
        
        public ReturnException(SproutValue value) : base("Return") 
        { 
            Value = value; 
        }
    }
}