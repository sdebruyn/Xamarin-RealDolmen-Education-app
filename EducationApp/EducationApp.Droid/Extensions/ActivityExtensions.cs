using System;
using Android.App;
using Android.Views;
using Android.Widget;

namespace EducationApp.Droid.Extensions
{
    internal static class ActivityExtensions
    {
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        internal static View GetObjectAdapter<T>(Func<T, string> stringFunc, T obj, View view, int layoutId, int tvId,
            Activity activity)
        {
            var template = view ?? activity.LayoutInflater.Inflate(layoutId, null);

            var tv = template.FindViewById<TextView>(tvId);
            tv.Text = stringFunc(obj);

            return template;
        }

        internal static View GetObjectAdapter<T>(this Activity activity, Func<T, string> stringFunc, T obj, View view,
            int layoutId, int tvId) => GetObjectAdapter(stringFunc, obj, view, layoutId, tvId, activity);
    }
}