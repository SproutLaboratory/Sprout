using System;
using System.Collections.Generic;
using System.Linq;

namespace SproutInterpreter
{
    public class Table
    {
        private LinkedList<TablePair> _values = new LinkedList<TablePair>();
        private Dictionary<string, LinkedListNode<TablePair>> _stringMap = new Dictionary<string, LinkedListNode<TablePair>>();
        private Dictionary<int, LinkedListNode<TablePair>> _arrayMap = new Dictionary<int, LinkedListNode<TablePair>>();
        private Dictionary<DynValue, LinkedListNode<TablePair>> _valueMap = new Dictionary<DynValue, LinkedListNode<TablePair>>(new DynValueComparer());

        private int _cachedLength = -1;
        #pragma warning disable CS0414
        private bool _containsNilEntries = false;
        #pragma warning restore CS0414

        public Table() { }

        public Table(params DynValue[] arrayValues) : this()
        {
            for (int i = 0; i < arrayValues.Length; i++)
                Set(DynValue.NewNumber(i + 1), arrayValues[i]);
        }

        public int Length
        {
            get
            {
                if (_cachedLength < 0)
                {
                    _cachedLength = 0;
                    for (int i = 1; _arrayMap.ContainsKey(i) && !_arrayMap[i].Value.Value.IsNil(); i++)
                        _cachedLength = i;
                }
                return _cachedLength;
            }
        }

        public void Set(string key, DynValue value)
        {
            if (key == null) throw new Exception("Table index is nil");
            PerformTableSet(_stringMap, key, DynValue.NewString(key), value, false);
        }

        public void Set(int key, DynValue value)
        {
            PerformTableSet(_arrayMap, key, DynValue.NewNumber(key), value, true);
        }

        public void Set(DynValue key, DynValue value)
        {
            if (key.IsNil()) throw new Exception("Table index is nil");

            if (key.Type == DataType.String)
            {
                Set(key.String, value);
                return;
            }

            if (key.Type == DataType.Number)
            {
                int idx = GetIntegralKey(key.Number);
                if (idx > 0)
                {
                    Set(idx, value);
                    return;
                }
            }

            PerformTableSet(_valueMap, key, key, value, false);
        }

        private void PerformTableSet<T>(Dictionary<T, LinkedListNode<TablePair>> map, T key, DynValue keyDynValue, DynValue value, bool isNumber)
        {
            if (map.TryGetValue(key, out LinkedListNode<TablePair> node))
            {
                node.Value = new TablePair(keyDynValue, value);
            }
            else
            {
                var newNode = _values.AddLast(new TablePair(keyDynValue, value));
                map[key] = newNode;

                if (isNumber)
                {
                    int intKey = Convert.ToInt32(key);
                    if (intKey > _cachedLength) _cachedLength = -1;
                }
            }

            if (value.IsNil())
            {
                _containsNilEntries = true;
                if (isNumber) _cachedLength = -1;
            }
        }

        public DynValue Get(string key) => RawGet(key) ?? DynValue.Nil;
        public DynValue Get(int key) => RawGet(key) ?? DynValue.Nil;
        public DynValue Get(DynValue key) => RawGet(key) ?? DynValue.Nil;

        public DynValue RawGet(string key)
        {
            if (_stringMap.TryGetValue(key, out LinkedListNode<TablePair> node))
                return node.Value.Value;
            return null;
        }

        public DynValue RawGet(int key)
        {
            if (_arrayMap.TryGetValue(key, out LinkedListNode<TablePair> node))
                return node.Value.Value;
            return null;
        }

        public DynValue RawGet(DynValue key)
        {
            if (key.Type == DataType.String) return RawGet(key.String);
            if (key.Type == DataType.Number)
            {
                int idx = GetIntegralKey(key.Number);
                if (idx > 0) return RawGet(idx);
            }
            if (_valueMap.TryGetValue(key, out LinkedListNode<TablePair> node))
                return node.Value.Value;
            return null;
        }

        public bool Remove(string key) => PerformTableRemove(_stringMap, key, false);
        public bool Remove(int key) => PerformTableRemove(_arrayMap, key, true);
        public bool Remove(DynValue key)
        {
            if (key.Type == DataType.String) return Remove(key.String);
            if (key.Type == DataType.Number)
            {
                int idx = GetIntegralKey(key.Number);
                if (idx > 0) return Remove(idx);
            }
            return PerformTableRemove(_valueMap, key, false);
        }

        private bool PerformTableRemove<T>(Dictionary<T, LinkedListNode<TablePair>> map, T key, bool isNumber)
        {
            if (map.TryGetValue(key, out LinkedListNode<TablePair> node))
            {
                _values.Remove(node);
                map.Remove(key);
                if (isNumber) _cachedLength = -1;
                return true;
            }
            return false;
        }

        public IEnumerable<TablePair> Pairs => _values;

        private int GetIntegralKey(double d)
        {
            int v = (int)d;
            if (d >= 1.0 && d == v) return v;
            return -1;
        }

        public void Clear()
        {
            _values.Clear();
            _stringMap.Clear();
            _arrayMap.Clear();
            _valueMap.Clear();
            _cachedLength = -1;
        }

        public override string ToString()
        {
            var items = new List<string>();
            foreach (var pair in _values)
            {
                string keyStr = pair.Key.ToString();
                string valStr = pair.Value.ToString();
                items.Add($"{keyStr}: {valStr}");
            }
            return "{" + string.Join(", ", items) + "}";
        }

        private class DynValueComparer : IEqualityComparer<DynValue>
        {
            public bool Equals(DynValue x, DynValue y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                return x.ReferenceID == y.ReferenceID;
            }

            public int GetHashCode(DynValue obj) => obj.ReferenceID;
        }
    }
}