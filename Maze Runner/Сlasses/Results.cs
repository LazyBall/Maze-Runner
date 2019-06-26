using System;
using System.Collections;
using System.Collections.Generic;

namespace Maze_Runner
{
    [Serializable]
    public class Results : IEnumerable<Result>
    {
        private SortedSet<Result> _results;

        public Results()
        {
            _results = new SortedSet<Result>();
        }

        public void Add(Result result)
        {
            _results.Add(result);
        }

        public void Add(IEnumerable<Result> collection)
        {
            foreach (var item in collection)
            {
                _results.Add(item);
            }
        }

        public IEnumerator<Result> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in _results)
                yield return item;
        }
    }

    [Serializable]
    public class Result : IComparable<Result>
    {
        private string _playersName;
        public string PlayersName
        {
            get
            { return _playersName; }
            set
            {
                if (value != null) _playersName = value;
                else throw new ArgumentNullException();
            }
        }
        public int Level { get;  set; }

        public Result()
        {
            PlayersName = string.Empty;
            Level = 0;
        }

        public int CompareTo(Result other)
        {
            var result = -Level.CompareTo(other.Level);
            if (result == 0)
                result = PlayersName.CompareTo(other.PlayersName);
            return result;
        }
    }
}