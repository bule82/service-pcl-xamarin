using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using DSoft.Messaging;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private TextView mTextView;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Button buttonStop = FindViewById<Button>(Resource.Id.MyButtonStop); 

            mTextView = FindViewById<TextView>(Resource.Id.textView1);

            MessageBus.Default.Register(MessageHandler);

            button.Click += delegate {

                StartService(new Intent(this, typeof(MyService)));      
          
                Intent startIntent = new Intent(this,  typeof(MyService));
                startIntent.SetAction(MyService.ActionPlay);
                
                StartService(startIntent);
                
            };

            buttonStop.Click += delegate
            {

                Intent stopIntent = new Intent(this, typeof(MyService));
                stopIntent.SetAction(MyService.ActionStop);
                StartService(stopIntent);

            };
        }

      

        /// <summary>
        /// Messages the bus event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="evnt">Evnt.</param>
        public void MessageBusEventHandler(object sender, MessageBusEvent evnt)
        {
            //extrac the data
            //var data2 = evnt.Data[0] as String;

            //run on the ui thread
            RunOnUiThread(() =>
            {
                mTextView.Text += "\r\n " + evnt.Data[0].ToString();
            });

        }


        /// <summary>
        /// Gets the message handler.
        /// </summary>
        /// <value>The message handler.</value>
        /// 
        private MessageBusEventHandler mEvHandler;
        public MessageBusEventHandler MessageHandler
        {
            get
            {
                if (mEvHandler == null)
                {
                    mEvHandler = new MessageBusEventHandler()
                    {
                        EventId = "1234",
                        EventAction = MessageBusEventHandler,
                    };
                }

                return mEvHandler;
            }

        }
    }
}

