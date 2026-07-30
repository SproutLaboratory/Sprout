using System;

namespace SproutInterpreter
{
    public class DynValue
    {
        private static int _refIdCounter = 0;
        private int _refId = ++_refIdCounter;
        private int _hashCode = -1;
        private bool _readOnly;
        private double _number;
        private object _object;
        private DataType _type;

        public int ReferenceID => _refId;
        public DataType Type => _type;
        public double Number => _number;
        public string String => _object as string;
        public Table Table => _object as Table;
        public bool Boolean => _number != 0;
        public bool ReadOnly => _readOnly;

        public static DynValue Nil { get; } = new DynValue { _type = DataType.Nil }.AsReadOnly();
        public static DynValue Void { get; } = new DynValue { _type = DataType.Void }.AsReadOnly();
        public static DynValue True { get; } = new DynValue { _number = 1, _type = DataType.Boolean }.AsReadOnly();
        public static DynValue False { get; } = new DynValue { _number = 0, _type = DataType.Boolean }.AsReadOnly();

        public static DynValue NewNil() => new DynValue { _type = DataType.Nil };
        public static DynValue NewBoolean(bool v) => new DynValue { _number = v ? 1 : 0, _type = DataType.Boolean };
        public static DynValue NewNumber(double num) => new DynValue { _number = num, _type = DataType.Number };
        public static DynValue NewString(string str) => new DynValue { _object = str, _type = DataType.String };
        public static DynValue NewTable(Table table) => new DynValue { _object = table, _type = DataType.Table };

        public DynValue AsReadOnly()
        {
            if (_readOnly) return this;
            return Clone(true);
        }

        public DynValue Clone(bool readOnly = false)
        {
            return new DynValue
            {
                _object = _object,
                _number = _number,
                _hashCode = _hashCode,
                _type = _type,
                _readOnly = readOnly
            };
        }

        public bool IsNil() => _type == DataType.Nil || _type == DataType.Void;
        public bool IsNotNil() => !IsNil();

        public string CastToString()
        {
            if (_type == DataType.Number) return _number.ToString();
            if (_type == DataType.String) return String;
            return null;
        }

        public override string ToString()
        {
            switch (_type)
            {
                case DataType.Nil: return "nil";
                case DataType.Boolean: return Boolean ? "true" : "false";
                case DataType.Number: return _number.ToString();
                case DataType.String: return "\"" + String + "\"";
                case DataType.Table: return "(Table)";
                default: return "(???)";
            }
        }
    }
}