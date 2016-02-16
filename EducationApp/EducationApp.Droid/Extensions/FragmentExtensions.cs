using System;
using Android.Support.V4.App;
using Android.Views;

namespace EducationApp.Droid.Extensions
{
    internal static class FragmentExtensions
    {
        internal static View GetObjectAdapter<T>(this Fragment fragment, Func<T, string> stringFunc, T obj, View view,
            int layoutId, int tvId)
            => ActivityExtensions.GetObjectAdapter(stringFunc, obj, view, layoutId, tvId, fragment.Activity);

        internal static View GetObjectAdapter<T>(this Android.App.Fragment fragment, Func<T, string> stringFunc, T obj,
            View view,
            int layoutId, int tvId)
            => ActivityExtensions.GetObjectAdapter(stringFunc, obj, view, layoutId, tvId, fragment.Activity);
    }
}