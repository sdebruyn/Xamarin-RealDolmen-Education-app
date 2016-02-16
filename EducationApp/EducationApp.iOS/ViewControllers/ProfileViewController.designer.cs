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
	[Register ("ProfileViewController")]
	partial class ProfileViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem CoursesTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView PleaseLogin { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView ProfileData { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem ProfileTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBar TabBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvFirstName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvLastName { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CoursesTabBarItem != null) {
				CoursesTabBarItem.Dispose ();
				CoursesTabBarItem = null;
			}
			if (PleaseLogin != null) {
				PleaseLogin.Dispose ();
				PleaseLogin = null;
			}
			if (ProfileData != null) {
				ProfileData.Dispose ();
				ProfileData = null;
			}
			if (ProfileTabBarItem != null) {
				ProfileTabBarItem.Dispose ();
				ProfileTabBarItem = null;
			}
			if (TabBar != null) {
				TabBar.Dispose ();
				TabBar = null;
			}
			if (TvEmail != null) {
				TvEmail.Dispose ();
				TvEmail = null;
			}
			if (TvFirstName != null) {
				TvFirstName.Dispose ();
				TvFirstName = null;
			}
			if (TvLastName != null) {
				TvLastName.Dispose ();
				TvLastName = null;
			}
		}
	}
}
