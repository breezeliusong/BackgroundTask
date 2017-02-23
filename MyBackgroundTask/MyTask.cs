using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace MyBackgroundTask
{
    public sealed class MyTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral; // Note: defined at class scope so we can mark it complete inside the OnCancel() callback if we choose to support cancellation
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            //
            // TODO: Insert code to start one or more asynchronous methods using the
            //       await keyword, for example:
            //
            // await ExampleMethodAsync();
            //
            Debug.WriteLine("Back");
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/test.txt"));
            await Windows.Storage.FileIO.WriteTextAsync(file, "Swift as a shadow");
            // Composite setting
            Windows.Storage.ApplicationDataContainer localSettings =
       Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.ApplicationDataCompositeValue composite =
               (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values["exampleCompositeSetting"];

            if (composite == null)
            {
                // No data
            }
            else
            {
                // Access data in composite["intVal"] and composite["strVal"]
                Debug.WriteLine(composite["intVal"]);
            }

            _deferral.Complete();

        }
    }
}
