using System;
using System.Collections;
using System.Collections.Generic;



public class SortedListDictionary<TKey, TValue> :
    IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    private bool _isSorted = false;
    private List<KeyValuePair<TKey, TValue>> _list;
    public SortedListDictionary(int capacity)
    {
        _list = new List<KeyValuePair<TKey, TValue>>(capacity);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _isSorted = false;
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _list.Contains(item);
    }

    private static int Comparison(KeyValuePair<TKey, TValue> a, KeyValuePair<TKey, TValue> b)
    {
        return a.Key.CompareTo(b.Key);
    }

    private static readonly Comparison<KeyValuePair<TKey, TValue>> _comparison = Comparison;
    public void SortSelf()
    {
        _list.Sort(_comparison);
        _isSorted = true;
    }

    private int BinarySearch(TKey key)
    {
        if (!_isSorted)
        {
            SortSelf();
        }

        int l = 0;
        int r = Count - 1;

        while (l <= r)
        {
            //int mid = (l + r) / 2;  //l和r都是大整型时，可能会溢出
            int mid = l + (r - l) / 2;

            if (key.CompareTo(_list[mid].Key) < 0)
                r = mid - 1;        //在keys[l...mid-1]查找key
            else if (key.CompareTo(_list[mid].Key) > 0)
                l = mid + 1;        //在keys[mid+1...r]查找key
            else
                return mid;         //找到key，并返回索引
        }
        return -1;
    }

    public bool ContainsKey(TKey key)
    {
        return BinarySearch(key) > -1;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return _list.Remove(item);
    }

    public int Count
    {
        get { return _list.Count; }
    }
    public bool IsReadOnly { get; }
    public void Add(TKey key, TValue value)
    {
        _isSorted = false;
        _list.Add(new KeyValuePair<TKey, TValue>(key, value));
    }

    public bool Remove(TKey key)
    {
        int index = BinarySearch(key);
        if (index > -1)
        {
            _list.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = BinarySearch(key);
        if (index > -1)
        {
            value = _list[index].Value;
            return true;
        }
        else
        {
            value = default(TValue);
            return false;
        }

    }

    public TValue this[TKey key]
    {
        get
        {
            int index = BinarySearch(key);
            if (index > -1)
            {
                return _list[index].Value;
            }
            else
            {
                return default(TValue);
            }
        }
        set
        {
           Add(key, value);;
        }
    }

    public ICollection<TKey> Keys {
        get
        {
            return new List<TKey>();
        }
    }
    public ICollection<TValue> Values {
        get
        {
            return new List<TValue>();
        }
    }

    public List<KeyValuePair<TKey, TValue>> ListValue
    {
        get
        {
            return _list;
        }
    }
}