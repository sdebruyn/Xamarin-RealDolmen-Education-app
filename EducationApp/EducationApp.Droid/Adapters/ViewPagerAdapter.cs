using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Java.Lang;

namespace EducationApp.Droid.Adapters
{
    public class ViewPagerAdapter : FragmentPagerAdapter
    {
        public ViewPagerAdapter(FragmentManager supportFragmentManager) : base(supportFragmentManager)
        {
            Fragments = new Dictionary<Fragment, string>();
        }

        private IDictionary<Fragment, string> Fragments { get; }

        public override int Count => Fragments.Count;

        public override Fragment GetItem(int position) => Fragments.ElementAt(position).Key;

        public void AddFragment(Fragment fragment, string title) => Fragments.Add(fragment, title);

        public override ICharSequence GetPageTitleFormatted(int position)
            => new String(Fragments.ElementAt(position).Value);
    }
}