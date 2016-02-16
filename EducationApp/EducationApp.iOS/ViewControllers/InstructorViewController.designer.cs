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
	[Register ("InstructorViewController")]
	partial class InstructorViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtSendInstructorEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvEmployer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvEmployerDepartment { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvEmployerDesc { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvFirstName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvInstructorNameDesc { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView TvLastName { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (BtSendInstructorEmail != null) {
				BtSendInstructorEmail.Dispose ();
				BtSendInstructorEmail = null;
			}
			if (TvEmployer != null) {
				TvEmployer.Dispose ();
				TvEmployer = null;
			}
			if (TvEmployerDepartment != null) {
				TvEmployerDepartment.Dispose ();
				TvEmployerDepartment = null;
			}
			if (TvEmployerDesc != null) {
				TvEmployerDesc.Dispose ();
				TvEmployerDesc = null;
			}
			if (TvFirstName != null) {
				TvFirstName.Dispose ();
				TvFirstName = null;
			}
			if (TvInstructorNameDesc != null) {
				TvInstructorNameDesc.Dispose ();
				TvInstructorNameDesc = null;
			}
			if (TvLastName != null) {
				TvLastName.Dispose ();
				TvLastName = null;
			}
		}
	}
}
