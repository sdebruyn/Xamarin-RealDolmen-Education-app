// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
	[Register ("SessionViewController")]
	partial class SessionViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem ContactTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem FeedbackTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem ScheduleTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SessionScheduleTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem SubscribeTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBar TabBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvLocation { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvPreparation { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContactTabBarItem != null) {
				ContactTabBarItem.Dispose ();
				ContactTabBarItem = null;
			}
			if (FeedbackTabBarItem != null) {
				FeedbackTabBarItem.Dispose ();
				FeedbackTabBarItem = null;
			}
			if (ScheduleTabBarItem != null) {
				ScheduleTabBarItem.Dispose ();
				ScheduleTabBarItem = null;
			}
			if (SessionScheduleTable != null) {
				SessionScheduleTable.Dispose ();
				SessionScheduleTable = null;
			}
			if (SubscribeTabBarItem != null) {
				SubscribeTabBarItem.Dispose ();
				SubscribeTabBarItem = null;
			}
			if (TabBar != null) {
				TabBar.Dispose ();
				TabBar = null;
			}
			if (TvLocation != null) {
				TvLocation.Dispose ();
				TvLocation = null;
			}
			if (TvPreparation != null) {
				TvPreparation.Dispose ();
				TvPreparation = null;
			}
		}
	}
}
