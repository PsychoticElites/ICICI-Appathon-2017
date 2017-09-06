using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "Services")]
    public class Services : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;
        //string text = Intent.GetStringExtra("MyData") ?? "Data not available";

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
            speaker.Main_speaker("This functionality has been disabled.");
        }

        protected async override void OnRestart()
        {
            base.OnRestart();
            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please select the service you would like to know about.");
                speaker.Main_speaker("View Personal Details.");
                speaker.Main_speaker("Card Details.");
                speaker.Main_speaker("Fund Transfer");
                speaker.Main_speaker("Bill Payment");
                speaker.Main_speaker("Account Details.");
                speaker.Main_speaker("Previous Transaction Details");
            });

            await System.Threading.Tasks.Task.Delay(11000);
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);
            //speaker.Main_speaker("Branch Details.");
        }

        protected override void OnPause()
        {
            base.OnPause();
            //speaker.Main_speaker("I've been PAUSED!");
            // Code can be added here to stop the MainSpeaker Class!
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            speaker.Main_speaker("Welcome to I C I C I Services");
            //await System.Threading.Tasks.Task.Delay(5000);
            // speaker.Main_speaker("Branch Details.");

            // Create your application here
            SetContentView(Resource.Layout.servicesScreen);

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please select the service you would like to know about.");
                speaker.Main_speaker("View Personal Details.");
                speaker.Main_speaker("Card Details.");
                speaker.Main_speaker("Fund Transfer");
                speaker.Main_speaker("Bill Payment");
                speaker.Main_speaker("Account Details.");
                speaker.Main_speaker("Previous Transaction Details");
            });

            await System.Threading.Tasks.Task.Delay(12500);
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, VOICE);

            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            string accountNumber = Intent.GetStringExtra("accountNumber") ?? "0";
            Console.WriteLine("AuthToken Services : " + sessionToken);
            Console.WriteLine("custID Services : " + custId);

            Console.WriteLine("Account Number Services : " + accountNumber);
            //Console.WriteLine("THIS IS THE TOKEN : " + sessionToken);

            //Button Config for PERSONAL DETAILS
            ImageButton b_personalDetails = FindViewById<ImageButton>(Resource.Id.servicePersonal);
            b_personalDetails.Click += delegate
            {
                var personalDetails_Intent = new Intent(this, typeof(personalDetails));
                personalDetails_Intent.PutExtra("custId", custId);
                personalDetails_Intent.PutExtra("authToken", sessionToken);
                StartActivity(personalDetails_Intent);
            };

            //Button Config for CARD DETAILS
            ImageButton b_cardDetails = FindViewById<ImageButton>(Resource.Id.serviceCard);
            b_cardDetails.Click += delegate
            {
                var cardDetails_Intent = new Intent(this, typeof(debitCardDetails));
                cardDetails_Intent.PutExtra("custId", custId);
                cardDetails_Intent.PutExtra("authToken", sessionToken);
                StartActivity(cardDetails_Intent);
            };

            //Button Config for ACCOUNT DETAILS
            ImageButton b_accountDetails = FindViewById<ImageButton>(Resource.Id.serviceAccount);
            b_accountDetails.Click += delegate
            {
                var accountDetails_Intent = new Intent(this, typeof(accountDetailsScreen));
                accountDetails_Intent.PutExtra("custId", custId);
                accountDetails_Intent.PutExtra("authToken", sessionToken);
                StartActivity(accountDetails_Intent);
            };

            //Button Config for Transaction Details
            ImageButton b_transactionDetails = FindViewById<ImageButton>(Resource.Id.serviceTransaction);
            b_transactionDetails.Click += delegate
            {
                var transactionDetails_Intent = new Intent(this, typeof(miniStatementDetails));
                transactionDetails_Intent.PutExtra("custId", custId);
                transactionDetails_Intent.PutExtra("authToken", sessionToken);
                transactionDetails_Intent.PutExtra("accountNumber", accountNumber);
                StartActivity(transactionDetails_Intent);
            };


            //Button Config for Fund Transfer
            ImageButton b_fundTransferDetails = FindViewById<ImageButton>(Resource.Id.serviceFund);
            b_fundTransferDetails.Click += delegate
              {
                  var fundTransferDetails_Intent = new Intent(this, typeof(fundTransferDetail));
                  fundTransferDetails_Intent.PutExtra("custId", custId);
                  fundTransferDetails_Intent.PutExtra("authToken", sessionToken);
                  fundTransferDetails_Intent.PutExtra("accountNumber", accountNumber);
                  StartActivity(fundTransferDetails_Intent);
              };

            //Button Config for Bill Payment
            ImageButton b_billPaymentDetails = FindViewById<ImageButton>(Resource.Id.serviceBill);
            b_billPaymentDetails.Click += delegate
            {
                var billPaymentDetails_Intent = new Intent(this, typeof(billPayment));
                billPaymentDetails_Intent.PutExtra("custId", custId);
                billPaymentDetails_Intent.PutExtra("authToken", sessionToken);
                StartActivity(billPaymentDetails_Intent);
            };
            //Button Config for Branch/ATM Details
            //Button b_branchDetails = FindViewById<Button>(Resource.Id.serviceBranch);
            //b_branchDetails.Click += delegate
            //{
            //    var branchDetails_Intent = new Intent(this, typeof(branchDetailScreen));
            //    branchDetails_Intent.PutExtra("custId", custId);
            //    branchDetails_Intent.PutExtra("authToken", sessionToken);
            //    branchDetails_Intent.PutExtra("accountNumber", accountNumber);
            //    StartActivity(branchDetails_Intent);
            //};
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            ImageButton b_personalDetails = FindViewById<ImageButton>(Resource.Id.servicePersonal);
            ImageButton b_cardDetails = FindViewById<ImageButton>(Resource.Id.serviceCard);
            ImageButton b_accountDetails = FindViewById<ImageButton>(Resource.Id.serviceAccount);
            ImageButton b_transactionDetails = FindViewById<ImageButton>(Resource.Id.serviceTransaction);
            ImageButton b_fundTransferDetails = FindViewById<ImageButton>(Resource.Id.serviceFund);
            ImageButton b_billPaymentDetails = FindViewById<ImageButton>(Resource.Id.serviceBill);

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
                                if (item.Contains("personal"))
                                {
                                    speaker.Main_speaker("Selecting Personal Details.");
                                    b_personalDetails.PerformClick();
                                    break;
                                }

                                else if (item.Contains("card"))
                                {
                                    speaker.Main_speaker("Selecting card details");
                                    b_cardDetails.PerformClick();
                                    break;
                                }

                                else if (item.Contains("account"))
                                {
                                    speaker.Main_speaker("Selecting account details");
                                    b_accountDetails.PerformClick();
                                    break;
                                }

                                else if (item.Contains("previous"))
                                {
                                    speaker.Main_speaker("Selecting Previous Transactions");
                                    b_transactionDetails.PerformClick();
                                    break;
                                }

                                else if (item.Contains("fund"))
                                {
                                    speaker.Main_speaker("Selecting funds transfer");
                                    b_fundTransferDetails.PerformClick();
                                    break;
                                }

                                else if (item.Contains("bill"))
                                {
                                    speaker.Main_speaker("Selecting bill payment");
                                    b_billPaymentDetails.PerformClick();
                                    break;
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