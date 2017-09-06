using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "Transaction Details")]
    public class transactionDetails : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;

        protected override void OnRestart()
        {
            base.OnRestart();
            speaker.Main_speaker("Please select the transaction details type.");
            speaker.Main_speaker("Mini Statement.");
            speaker.Main_speaker("Interval Statement.");
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            //speaker.Main_speaker("Please select the transaction details type.");
            //speaker.Main_speaker("Mini Statement.");
            //speaker.Main_speaker("Interval Statement.");

            // Create your application here

            SetContentView(Resource.Layout.transactionScreen);

            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            string accountNumber = Intent.GetStringExtra("accountNumber") ?? "0";
            Console.WriteLine("Account Number Transaction Details : " + accountNumber);

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please select the transaction details type.");
                speaker.Main_speaker("Mini Statement.");
                speaker.Main_speaker("Interval Statement.");
            });

            await System.Threading.Tasks.Task.Delay(9000);
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);

            //Button Config for MINI STATEMENT
            //Button b_miniStatement = FindViewById<Button>(Resource.Id.transactionMini);
            //b_miniStatement.Click += delegate
            //{
            //    var miniStatement_Intent = new Intent(this, typeof(miniStatementDetails));
            //    miniStatement_Intent.PutExtra("custId", custId);
            //    miniStatement_Intent.PutExtra("authToken", sessionToken);
            //    miniStatement_Intent.PutExtra("accountNumber", accountNumber);
            //    StartActivity(miniStatement_Intent);
            //};


            //Button Config for INTERVAL STATEMENT
            //Button b_intervalStatement = FindViewById<Button>(Resource.Id.transactionInterval);
            //b_intervalStatement.Click += delegate
            //{
            //    var intervalStatement_Intent = new Intent(this, typeof(intervalStatementDetails));
            //    StartActivity(intervalStatement_Intent);
            //};
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            Button miniStatement = FindViewById<Button>(Resource.Id.transactionMini);
            //Button intervalStatement = FindViewById<Button>(Resource.Id.transactionInterval);

            try
            {
                if (requestCode == VOICE)
                {
                    if (resultVal == Result.Ok)
                    {
                        var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                        if (matches.Count != 0)
                        {
                            Console.WriteLine("what I SPOKE : " + matches[0].Substring(0, 5).ToLower());
                            /*
                             * Here I'm using IF ELSE, because I wanted to accept the long sentences as well.
                             * SWITCH statements had only one phrase. So, if a person said "Bill Details" or "BILL",
                             * they will be 2 separate cases. Speaker can say anything along with the word "BILL".
                             * So, as long as we hear "BILL", we want to show the user the "BILL" details.
                             * This will save us from generating various scenarios and phrases.
                             */
                            foreach (string item in matches)
                            {
                                if (item.Contains("mini"))
                                {
                                    speaker.Main_speaker("Selecting Mini Statement Details.");
                                    miniStatement.PerformClick();
                                    break;
                                }

                                else if (item.Contains("interval"))
                                {
                                    speaker.Main_speaker("Selecting Intrval Statement details");
                                    //intervalStatement.PerformClick();
                                    break;
                                }
                                else if (item.Contains("back"))
                                {
                                    Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("menu"))
                                {
                                    Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("exit"))
                                {
                                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                                }
                                else
                                {
                                    speaker.Main_speaker("I could not understand what you said.");
                                }
                                
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