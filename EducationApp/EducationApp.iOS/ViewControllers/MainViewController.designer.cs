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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView CategoryTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem CoursesTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBarItem ProfileTabBarItem { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchBar SearchBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SearchResultsTable { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITabBar TabBar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvNoCoursesFound { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvSearchFaulted { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvSearching { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CategoryTable != null) {
				CategoryTable.Dispose ();
				CategoryTable = null;
			}
			if (CoursesTabBarItem != null) {
				CoursesTabBarItem.Dispose ();
				CoursesTabBarItem = null;
			}
			if (ProfileTabBarItem != null) {
				ProfileTabBarItem.Dispose ();
				ProfileTabBarItem = null;
			}
			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}
			if (SearchResultsTable != null) {
				SearchResultsTable.Dispose ();
				SearchResultsTable = null;
			}
			if (TabBar != null) {
				TabBar.Dispose ();
				TabBar = null;
			}
			if (TvNoCoursesFound != null) {
				TvNoCoursesFound.Dispose ();
				TvNoCoursesFound = null;
			}
			if (TvSearchFaulted != null) {
				TvSearchFaulted.Dispose ();
				TvSearchFaulted = null;
			}
			if (TvSearching != null) {
				TvSearching.Dispose ();
				TvSearching = null;
			}
		}
	}
}
