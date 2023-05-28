using System;
using System.Collections.Generic;

namespace _Dev._Mike.Scripts.Ground.PathFinding.DataStructure
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        public int Count => data.Count;
        
        private List<T> data;

        public PriorityQueue()
        {
            data = new List<T>();
        }

        public void Enqueue(T item)
        {
            data.Add(item);
            var childIdx = data.Count - 1;
            
            while (childIdx > 0)
            {
                var parentIdx = (childIdx - 1) / 2;

                if (EvaluateScore(childIdx, parentIdx) >= 0) break;

                SwapChildParentData(childIdx, parentIdx);
                childIdx = parentIdx;
            }
        }
        
        public T Dequeue()
        {
            var lastIdx = data.Count - 1;
            var front = data[0];
            
            data[0] = data[lastIdx];
            data.RemoveAt(lastIdx);
            
            lastIdx--;
            var parentIdx = 0;
            
            while (true)
            {
                var childIdx = parentIdx * 2 + 1;
                if (childIdx > lastIdx) break;
                
                
                var right = childIdx + 1;

                if (right < lastIdx && data[right].CompareTo(data[childIdx]) < 0)
                    childIdx = right;

                if (EvaluateScore(parentIdx, childIdx) <= 0) break;

                SwapChildParentData(parentIdx, childIdx);
                parentIdx = childIdx;
            }

            return front;
        }
        private void SwapChildParentData(int firstIdx, int secondIdx)
        {
            (data[firstIdx], data[secondIdx]) = (data[secondIdx], data[firstIdx]);
        }

        private bool EndOfSort(int childIdx, int lastIdx)
        {
            return childIdx > lastIdx;
        }

        private int EvaluateScore(int first, int second)
        {
            return data[first].CompareTo(data[second]);
        }
        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }
        public bool Contains(T item)
        {
            return data.Contains(item);
        }
        public List<T> ToList()
        {
            return data;
        }
    }
}
