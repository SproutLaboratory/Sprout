namespace SproutInterpreter
{
    public struct TablePair
    {
        public DynValue Key { get; }
        public DynValue Value { get; set; }

        public TablePair(DynValue key, DynValue val)
        {
            Key = key;
            Value = val;
        }

        public static TablePair Nil = new TablePair(DynValue.Nil, DynValue.Nil);
    }
}