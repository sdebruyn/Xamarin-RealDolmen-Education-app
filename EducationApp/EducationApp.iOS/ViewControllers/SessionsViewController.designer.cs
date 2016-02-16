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
	[Register ("SessionsViewController")]
	partial class SessionsViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView SessionsTable { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (SessionsTable != null) {
				SessionsTable.Dispose ();
				SessionsTable = null;
			}
		}
	}
}
