
using Android.App;
using Android.OS;
using System.Linq;
using Android.Speech;
using System.Threading;
using Android.Widget;
using System;
using Android.Content;

namespace Hackathon_ICICI
{
    [Activity(Label = "fundTransferCompleteScreen" , MainLauncher = false)]
    public class fundTransferCompleteScreen : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
            speaker.Main_speaker("This functionality has been disabled.");
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.fundTransferComplete);

            speaker.Main_speaker("Your funds have been successfully transfered.");
            speaker.Main_speaker("These are the transaction details.");
            // Create your application here
            string destination_accountno = Intent.GetStringExtra("destination_accountno") ?? "0";
            string transaction_date = Intent.GetStringExtra("transaction_date") ?? "0";
            string referance_no = Intent.GetStringExtra("referance_no") ?? "0";
            string transaction_amount = Intent.GetStringExtra("transaction_amount") ?? "0";
            string payee_name = Intent.GetStringExtra("payee_name") ?? "0";

            speaker.Main_speaker("Reference ID of the transaction is " + referance_no.Aggregate(string.Empty, (c, i) => c + i + "  ").Replace("0", "Zero"));
            FindViewById<Android.Widget.EditText>(Resource.Id.fundCompleteRefID).Text = referance_no;

            speaker.Main_speaker("Destination account number is " + destination_accountno.Aggregate(string.Empty, (c, i) => c + i + "  ").Replace("0", "Zero"));
            FindViewById<Android.Widget.EditText>(Resource.Id.fundCompleteDest).Text = destination_accountno;

            speaker.Main_speaker("Payee name is " + payee_name.Replace("0", "Zero"));
            FindViewById<Android.Widget.EditText>(Resource.Id.fundCompletePayee).Text = payee_name;

            speaker.Main_speaker("transaction amount was Rupees " + transaction_amount.Aggregate(string.Empty, (c, i) => c + i + " ").Replace(".00", ""));
            FindViewById<Android.Widget.EditText>(Resource.Id.fundCompleteAmount).Text = transaction_amount;

            speaker.Main_speaker("Transaction date is " + transaction_date);
            FindViewById<Android.Widget.EditText>(Resource.Id.fundCompleteDate).Text = transaction_date;

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            Button b_backToServices = FindViewById<Button>(Resource.Id.fundTransferComplete);
            Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Do you want to go back to Main Menu?");
            });

            await System.Threading.Tasks.Task.Delay(30000);
            //Thread.Sleep(5000);
            StartActivityForResult(voiceIntent, VOICE);

            b_backToServices.Click += delegate
            {
                Console.WriteLine("Clicked meh!");
                var backToServices_Intent = new Android.Content.Intent(this, typeof(Services));
                StartActivity(backToServices_Intent);
            };

            //b_backToServices.PerformClick();
        }

        

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            Button submitButton = FindViewById<Button>(Resource.Id.fundTransferComplete);

            //Console.WriteLine("Inside HERE");

            try
            {
                if (requestCode == VOICE)
                {
                    Console.WriteLine("requestCode and VOICE" + requestCode + " --> " + VOICE);
                    if (resultVal == Result.Ok)
                    {
                        var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                        Console.WriteLine("resultVal " + resultVal + " --> " + Result.Ok);

                        if (matches.Count != 0)
                        {
                            Console.WriteLine("matches Count " + matches.Count);
                            //Console.WriteLine("what I SPOKE : " + matches[0].Substring(0, 5).ToLower());
                            /*
                             * Here I'm using IF ELSE, because I wanted to accept the long sentences as well.
                             * SWITCH statements had only one phrase. So, if a person said "Bill Details" or "BILL",
                             * they will be 2 separate cases. Speaker can say anything along with the word "BILL".
                             * So, as long as we hear "BILL", we want to show the user the "BILL" details.
                             * This will save us from generating various scenarios and phrases.
                             */
                            foreach (string item in matches)
                            {
                                if (item.Contains("submit"))
                                {
                                    //Finish();
                                    submitButton.PerformClick();
                                }
                                else if (item.Contains("yes"))
                                {
                                    //Finish();
                                    submitButton.PerformClick();
                                }
                                else if (item.Contains("ok"))
                                {
                                    Finish();
                                    submitButton.PerformClick();
                                }
                                else if (item.Contains("back"))
                                {
                                    Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("menu"))
                                {
                                    Finish();
                                    submitButton.PerformClick();
                                }
                                else if (item.Contains("exit"))
                                {
                                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                                }
                                else
                                    speaker.Main_speaker("I could not understand what you said.");
                            }
                        }
                        else
                            speaker.Main_speaker("I could not understand what you said.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("My Errur : " + ex);
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

    }
}