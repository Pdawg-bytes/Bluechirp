using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Bluechirp.Core;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services;
using Bluechirp.Services;
using Bluechirp.View;

namespace Bluechirp
{
    /// <summary>
    /// The core class of Bluechirp.
    /// </summary>
    sealed partial class App : Application
    {
        /// <inheritdoc/>
        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;
        }


        /// <inheritdoc/>
        /// <param name="E">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs E)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            await SetupAppAsync();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (E.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter

                if (ClientDataHelper.GetLastUsedProfile() != null)
                {
                    ClientHelper.LoadLastUsedProfile();
                    rootFrame.Navigate(typeof(ShellView), E.Arguments);
                }
                else
                {
                    rootFrame.Navigate(typeof(LoginView), E.Arguments);
                }

                NavService.CreateInstance(rootFrame);
            }

            if (E.PrelaunchActivated == false)
                CoreApplication.EnablePrelaunch(true);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <inheritdoc/>
        protected override async void OnActivated(IActivatedEventArgs Args)
        {
            base.OnActivated(Args);

            if (Args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs eventArgs = Args as ProtocolActivatedEventArgs;
                string fullUri = eventArgs.Uri.Query;

                Debug.WriteLine(fullUri);

                await AuthHelper.Instance.FinishOAuth(fullUri);
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;
                await SetupAppAsync();
                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    rootFrame.NavigationFailed += OnNavigationFailed;

                    if (Args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Load state from previously suspended application
                    }

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter

                    if (ClientDataHelper.GetLastUsedProfile() != null)
                    {
                        ClientHelper.LoadLastUsedProfile();
                        rootFrame.Navigate(typeof(ShellView), null);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(LoginView), null);
                    }

                    NavService.CreateInstance(rootFrame);
                }

                Window.Current.Activate();
            }
        }

        /// <summary>
        ///     Initializes the application.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        private async Task SetupAppAsync()
        {
            await ClientDataHelper.StartUpAsync();

            ApplicationView appView = ApplicationView.GetForCurrentView();

            appView.SetPreferredMinSize(new Size(400, 500));
            ApiConstants.SetApiConstants();
            GlobalKeyboardShortcutService.Initialize();

            try
            {
                await CacheService.LoadKeyboardShortcutsContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        ///     Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="Sender">The Frame which failed navigation</param>
        /// <param name="E">Details about the navigation failure</param>
        private void OnNavigationFailed(object Sender, NavigationFailedEventArgs E)
        {
            throw new Exception("Failed to load Page " + E.SourcePageType.FullName);
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="Sender">The source of the suspend request.</param>
        /// <param name="E">Details about the suspend request.</param>
        private void OnSuspending(object Sender, SuspendingEventArgs E)
        {
            SuspendingDeferral deferral = E.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}