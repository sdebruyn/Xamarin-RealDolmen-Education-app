using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EducationApp.Extensions
{
    public static class ObservableCollectionExtensions
    {
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void Fill<T>(this ObservableCollection<T> collection, IEnumerable<T> source)
        {
            collection.Clear();
            var list = source.ToList();
            list.Sort();
            list.ForEach(collection.Add);
        }

        public static void Sort<T>(this ObservableCollection<T> collection)
        {
            var content = new SortedSet<T>(collection);
            collection.Clear();
            content.ForEach(collection.Add);
        }
    }
}