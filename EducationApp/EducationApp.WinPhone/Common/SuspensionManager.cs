using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EducationApp.WinPhone.Common
{
    /// <summary>
    ///     SuspensionManager captures global session state to simplify process lifetime management
    ///     for an application.  Note that session state will be automatically cleared under a variety
    ///     of conditions and should only be used to store information that would be convenient to
    ///     carry across sessions, but that should be discarded when an application crashes or is
    ///     upgraded.
    /// </summary>
    internal sealed class SuspensionManager
    {
        private const string SessionStateFilename = "_sessionState.xml";

        private static readonly DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof (string), typeof (SuspensionManager),
                null);

        private static readonly DependencyProperty FrameSessionBaseKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionBaseKeyParams", typeof (string),
                typeof (SuspensionManager), null);

        private static readonly DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_FrameSessionState", typeof (Dictionary<string, object>),
                typeof (SuspensionManager), null);

        private static readonly List<WeakReference<Frame>> RegisteredFrames = new List<WeakReference<Frame>>();

        /// <summary>
        ///     Provides access to global session state for the current session.  This state is
        ///     serialized by <see cref="SaveAsync" /> and restored by
        ///     <see cref="RestoreAsync" />, so values must be serializable by
        ///     <see cref="DataContractSerializer" /> and should be as compact as possible.  Strings
        ///     and other self-contained data types are strongly recommended.
        /// </summary>
        public static Dictionary<string, object> SessionState { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        ///     List of custom types provided to the <see cref="DataContractSerializer" /> when
        ///     reading and writing session state.  Initially empty, additional types may be
        ///     added to customize the serialization process.
        /// </summary>
        private static IEnumerable<Type> KnownTypes { get; } = new List<Type>();

        /// <summary>
        ///     Save the current <see cref="SessionState" />.  Any <see cref="Frame" /> instances
        ///     registered with <see cref="RegisterFrame" /> will also preserve their current
        ///     navigation stack, which in turn gives their active <see cref="Page" /> an opportunity
        ///     to save its state.
        /// </summary>
        /// <returns>An asynchronous task that reflects when session state has been saved.</returns>
        /// <exception cref="SuspensionManagerException">Could not save session state.</exception>
        public static async Task SaveAsync()
        {
            try
            {
                // Save the navigation state for all registered frames
                foreach (var weakFrameReference in RegisteredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        SaveFrameNavigationState(frame);
                    }
                }

                // Serialize the session state synchronously to avoid asynchronous access to shared
                // state
                var sessionData = new MemoryStream();
                var serializer = new DataContractSerializer(typeof (Dictionary<string, object>), KnownTypes);
                serializer.WriteObject(sessionData, SessionState);

                // Get an output stream for the SessionState file and write the state asynchronously
                var file =
                    await
                        ApplicationData.Current.LocalFolder.CreateFileAsync(SessionStateFilename,
                            CreationCollisionOption.ReplaceExisting);
                using (var fileStream = await file.OpenStreamForWriteAsync().ConfigureAwait(true))
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream).ConfigureAwait(true);
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        /// <summary>
        ///     Restores previously saved <see cref="SessionState" />.  Any <see cref="Frame" /> instances
        ///     registered with <see cref="RegisterFrame" /> will also restore their prior navigation
        ///     state, which in turn gives their active <see cref="Page" /> an opportunity restore its
        ///     state.
        /// </summary>
        /// <param name="sessionBaseKey">
        ///     An optional key that identifies the type of session.
        ///     This can be used to distinguish between multiple application launch scenarios.
        /// </param>
        /// <returns>
        ///     An asynchronous task that reflects when session state has been read.  The
        ///     content of <see cref="SessionState" /> should not be relied upon until this task
        ///     completes.
        /// </returns>
        public static async Task RestoreAsync(string sessionBaseKey = null)
        {
            SessionState = new Dictionary<string, object>();

            try
            {
                // Get the input stream for the SessionState file
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionStateFilename);
                using (var inStream = await file.OpenSequentialReadAsync())
                {
                    // Deserialize the Session State
                    var serializer = new DataContractSerializer(typeof (Dictionary<string, object>), KnownTypes);
                    SessionState = (Dictionary<string, object>) serializer.ReadObject(inStream.AsStreamForRead());
                }

                // Restore any registered frames to their saved state
                foreach (var weakFrameReference in RegisteredFrames)
                {
                    Frame frame;
                    if (weakFrameReference.TryGetTarget(out frame) &&
                        (string) frame.GetValue(FrameSessionBaseKeyProperty) == sessionBaseKey)
                    {
                        frame.ClearValue(FrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }
            catch (Exception e)
            {
                throw new SuspensionManagerException(e);
            }
        }

        /// <summary>
        ///     Provides storage for session state associated with the specified <see cref="Frame" />.
        ///     Frames that have been previously registered with <see cref="RegisterFrame" /> have
        ///     their session state saved and restored automatically as a part of the global
        ///     <see cref="SessionState" />.  Frames that are not registered have transient state
        ///     that can still be useful when restoring pages that have been discarded from the
        ///     navigation cache.
        /// </summary>
        /// <remarks>
        ///     Apps may choose to rely on <see cref="NavigationHelper" /> to manage
        ///     page-specific state instead of working with frame session state directly.
        /// </remarks>
        /// <param name="frame">The instance for which session state is desired.</param>
        /// <returns>
        ///     A collection of state subject to the same serialization mechanism as
        ///     <see cref="SessionState" />.
        /// </returns>
        private static Dictionary<string, object> SessionStateForFrame(Frame frame)
        {
            var frameState = (Dictionary<string, object>) frame.GetValue(FrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (string) frame.GetValue(FrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    // Registered frames reflect the corresponding session state
                    if (!SessionState.ContainsKey(frameSessionKey))
                    {
                        SessionState[frameSessionKey] = new Dictionary<string, object>();
                    }
                    frameState = (Dictionary<string, object>) SessionState[frameSessionKey];
                }
                else
                {
                    // Frames that aren't registered have transient state
                    frameState = new Dictionary<string, object>();
                }
                frame.SetValue(FrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        private static void RestoreFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((string) frameState["Navigation"]);
            }
        }

        private static void SaveFrameNavigationState(Frame frame)
        {
            var frameState = SessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
    }

    public class SuspensionManagerException : Exception
    {
        public SuspensionManagerException()
        {
        }

        public SuspensionManagerException(Exception e)
            : base("SuspensionManager failed", e)
        {
        }
    }
}