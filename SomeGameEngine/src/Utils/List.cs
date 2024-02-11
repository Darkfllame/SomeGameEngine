using System.Collections;

namespace SomeGameEngine.Utils;

public class List<T>
    : IEnumerable<T>
    where T : class
{
    public delegate void ForEachFunc(T elem, int index);

    private T?[] _array;

    public List(int baseCapacity = 0)
    {
        Size = 0;
        _array = new T[baseCapacity];
    }

    public int Size { get; private set; }

    public int Capacity
    {
        get => _array.Length;
        set
        {
            var nArr = new T?[value];
            for (var i = 0; i < Math.Min(Capacity, value); i++)
                nArr[i] = _array[i];
            _array = nArr;
            if (Size > value)
                Size = value;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index >= Size || index < 0)
                throw new IndexOutOfRangeException($"Tried to access index {index} on a {Size} element list");
            return _array[index] ?? throw new NullReferenceException("Unexpected null value");
        }
        set
        {
            if (index < Size || index < 0)
                _array[index] = value;
            else
                throw new IndexOutOfRangeException($"Tried to access index {index} on a {Size} element list");
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new ElementEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int FindElement(T value)
    {
        for (var i = 0; i < Size; i++)
        {
            var v = _array[i];
            if (v != null && v.Equals(value))
                return i;
        }

        return -1;
    }

    public void Append(T value)
    {
        if (Capacity > Size + 1)
        {
            // enough space in the buffer
            _array[Size] = value;
            Size++;
        }
        else
        {
            // not enough space in the buffer
            if (Size >= 1)
                Capacity = Size * 2;
            else
                Capacity = 1;
            _array[Size] = value;
            Size++;
        }
    }

    public void Remove(T value)
    {
        var index = FindElement(value);
        if (index != -1)
            _ = RemoveAt(index);
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= Size)
            throw new IndexOutOfRangeException($"Tried to access index {index} of {Size} element(s) array");
        var elem = _array[index] ?? throw new Exception("Unexpected null value");
        _array[index] = _array[Size - 1];
        _array[Size - 1] = null;
        Size--;
        return elem;
    }

    public void ShrinkToFit()
    {
        Capacity = Size;
    }

    public bool ElementExist(T value)
    {
        return FindElement(value) != -1;
    }

    /// <summary>
    ///     Append element 'value' to the list and shrink it to fit the size
    /// </summary>
    /// <param name="list">the list to append the element to</param>
    /// <param name="value">the element to append to the list</param>
    /// <returns>list</returns>
    public static List<T> operator <<(List<T> list, T value)
    {
        list.Append(value);
        list.ShrinkToFit();
        return list;
    }

    /// <summary>
    ///     Remove element 'value' from the list and shrink it to fit the size
    /// </summary>
    /// <param name="list">The list to remove element from</param>
    /// <param name="value">The element to remove</param>
    /// <returns>list</returns>
    public static List<T> operator >> (List<T> list, T value)
    {
        list.Remove(value);
        list.ShrinkToFit();
        return list;
    }

    public void ForEach(ForEachFunc callback)
    {
        for (var i = 0; i < Size; i++) callback(this[i], i);
    }

    public T[] ToArray()
    {
        var arr = new T[Size];
        for (var i = 0; i < Size; i++)
            arr[i] = _array[i] ?? throw new NullReferenceException("Unexpected null value");
        return arr;
    }

    private class ElementEnumerator : IEnumerator<T>
    {
        private readonly List<T> _list;
        private int _step = -1;

        public ElementEnumerator(List<T> list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            return ++_step < _list.Size;
        }

        public void Reset()
        {
            _step = 0;
        }

        public T Current => _list[_step];

        object IEnumerator.Current => Current;

        public void Dispose() {}
    }
}