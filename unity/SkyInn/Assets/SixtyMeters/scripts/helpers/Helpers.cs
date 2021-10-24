using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.scripts.helpers
{
    public static class Helpers
    {
        /**
         * Returns a random entry from the list.
         */
        public static T GETRandomFromList<T>(IReadOnlyList<T> list)
        {
            return list[Random.Range (0, list.Count)];
        }
    }
}