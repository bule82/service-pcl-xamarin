using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClassLibrary1;
using Android.Util;
using System.Threading;
using DSoft.Messaging;

namespace App1
{
    [Service]
    [IntentFilter(new[] { ActionPlay, ActionStop })]
    class MyService : Service
    {

        public const string ActionPlay = "com.xamarin.action.PLAY";
        public const string ActionStop = "com.xamarin.action.STOP";
        private bool run = true;
        private Portable p;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {

            

            base.OnStart(intent, startId);


            if (intent.Action != null)
            {

                if (intent.Action.Equals(ActionPlay))
                {


                    ThreadPool.QueueUserWorkItem(o => SlowMethod());
                    

                    fg();

                }
                else if (intent.Action.Equals(ActionStop))
                {                    
                    //Process.KillProcess(Process.MyPid());
                    run = false;
                    if (p != null)
                        p.stop();

                    StopForeground(true);
                    StopSelf();
                }
            }


            

            return StartCommandResult.Sticky;
        
        }


        void fg()
        {
            Notification notification = new Notification(Resource.Drawable.Icon, "Logging On", Java.Lang.JavaSystem.CurrentTimeMillis());
            Intent notificationIntent = new Intent(this, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);
            notification.SetLatestEventInfo(this, "Logger", "Logger Running", pendingIntent);
            StartForeground((int)NotificationFlags.ForegroundService, notification);
        }

        private void SlowMethod()
        {
            p = new Portable();
            p.start();

            while (run)
            {
                Thread.Sleep(20000);
                Console.WriteLine("--> service " + p.i);
                
                var aEvent = new CoreMessageBusEvent("1234")
                {
                    Sender = null,
                    Data = new object[] { p.i },
                };

                //send it
                MessageBus.Default.Post(aEvent);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

          

            Log.Debug("SimpleService", "SimpleService stopped");
        }

     

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            throw new NotImplementedException();
        }
          
 
    }
}