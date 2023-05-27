using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StructureUtil
{
    private static int _RADIX = 10;

    public static List<NoteData> RadixSort(this List<NoteData> list)
    {
        var maxLength = 0;

        // grab the max length
        for (var i = 1; i < list.Count(); i++)
            if (list[i].targetTimeStamp.ToString().Length > maxLength)
                maxLength = list[i].ToString().Length;

        // k
        for (var i = 0; i < maxLength; i++)
        {
            var buckets = new List<List<NoteData>>(_RADIX);

            // radix is const, drops from O(2nk) => O(nk)
            for (var j = 0; j < _RADIX; j++)
                buckets.Add(new List<NoteData>());

            // n
            foreach (var note in list)
                buckets[GetDigit(note.targetTimeStamp, i)].Add(note);

            list.Clear();

            // also const
            foreach (var bucket in buckets)
                list.AddRange(bucket);
        }

        return list;
    }

    // Start is called before the first frame update
    private static int GetDigit(int number, int digitIndex)
    {
        var divisor = (int) Math.Pow(10, digitIndex);
        return number / divisor % 10;
    }
}
