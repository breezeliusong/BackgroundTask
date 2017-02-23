using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SimpleBackgroundTask
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
        private void Register(object sender, RoutedEventArgs e)
        {

            Windows.Storage.ApplicationDataCompositeValue composite =
    new Windows.Storage.ApplicationDataCompositeValue();
            composite["intVal"] = 1;
            composite["strVal"] = "string";
            localSettings.Values["exampleCompositeSetting"] = composite;

            var taskRegistered = false;
            var exampleTaskName = "ExampleBackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == exampleTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }
            if (!taskRegistered)
            {
                var builder = new BackgroundTaskBuilder();

                builder.Name = exampleTaskName;
                builder.TaskEntryPoint = "MyBackgroundTask.MyTask";
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                BackgroundTaskRegistration task = builder.Register();
                task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
                task.Progress += new BackgroundTaskProgressEventHandler(Onprogress);
            }
        }

        private void Onprogress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var key = task.TaskId.ToString();
            var message = settings.Values[key].ToString();
        }

        private async void ReadFile(object sender, RoutedEventArgs e)
        {
            //string imageFile = @"Assets\test.txt";
            //var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(imageFile);
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/test.txt"));
            string text = await Windows.Storage.FileIO.ReadTextAsync(file);
        }
    }
}
