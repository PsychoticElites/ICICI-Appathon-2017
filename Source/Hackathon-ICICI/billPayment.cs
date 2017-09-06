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
using Android.Speech;
using System.Threading;

namespace Hackathon_ICICI
{
    [Activity(Label = "billPayment")]
    public class billPayment : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        public string errCode;
        EditText activeTextField;
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            speaker.Main_speaker("Welcome To Bill Payment.");
            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            
            SetContentView(Resource.Layout.billPaymentScreen);

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            // Create your application here

            FindViewById<EditText>(Resource.Id.billCustID).Text = custId;

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please speak the nickname of the biller.");
            });
            await System.Threading.Tasks.Task.Delay(5000);

            Console.WriteLine("My Active TextField 1 : " + activeTextField);
            await System.Threading.Tasks.Task.Run(() => {
                activeTextField = FindViewById<EditText>(Resource.Id.billNickname);
                StartActivityForResult(voiceIntent, VOICE);
            });
            Console.WriteLine("My Active TextField 2 : " + activeTextField);

            await System.Threading.Tasks.Task.Delay(5000);
            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please speak the amount you want to pay.");
            });
            await System.Threading.Tasks.Task.Delay(5000);
            try
            {
                await System.Threading.Tasks.Task.Run(() => {
                    activeTextField = FindViewById<EditText>(Resource.Id.billAmount);
                    StartActivityForResult(voiceIntent, VOICE);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("My Errur 2 : " + ex);
            }

            await System.Threading.Tasks.Task.Delay(2000);
            Console.WriteLine("My Active TextField 3 : " + activeTextField);

            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("You want to Pay Rupees " + activeTextField.Text + " to " + FindViewById<EditText>(Resource.Id.billNickname).Text.Aggregate(string.Empty, (c, i) => c + i + "  ").Replace("0", "Zero"));
                speaker.Main_speaker("Do you want me to submit this request?");
            });

            await System.Threading.Tasks.Task.Delay(7000);
            //Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Run(() => {
                activeTextField = FindViewById<EditText>(Resource.Id.fundAmount);
                StartActivityForResult(voiceIntent, VOICE);
            });
            //Button submitBUtton = FindViewById<Button>(Resource.Id.imageButton1);
            //submitBUtton.PerformClick();

            Button submitButon = FindViewById<Button>(Resource.Id.finalSubmitButton);
            submitButon.Click += async delegate
            {
                string billerName = FindViewById<EditText>(Resource.Id.billNickname).Text;
                string billAmount = FindViewById<EditText>(Resource.Id.billAmount).Text;
                InformationFetcher(sessionToken, custId, billerName, billAmount);
                Console.Write("Error Code : " + this.errCode);
                if (this.errCode == "200")
                {
                    speaker.Main_speaker("Your Billpayment was successful!");
                    await System.Threading.Tasks.Task.Delay(10000);
                    base.OnBackPressed();
                }
                else if (this.errCode == "400")
                {
                    speaker.Main_speaker("Something went wrong.");
                }

            };

        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            Button submitButton = FindViewById<Button>(Resource.Id.finalSubmitButton);
            //Button b_personalDetails = FindViewById<Button>(Resource.Id.servicePersonal);
            //speaker.Main_speaker("I'm here!!!!!");
            Console.WriteLine("Inside HERE");

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
                            Console.WriteLine("Match " + matches);
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
                                Console.WriteLine("Item " + item);
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
                                    //Finish();
                                    submitButton.PerformClick();
                                }
                                else if (item.Contains("back"))
                                {
                                    //Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("menu"))
                                {
                                    //Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("exit"))
                                {
                                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                                }
                                else
                                    activeTextField.Text = item.Replace(" ", "").ToLower().Trim();
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

        private void InformationFetcher(string sessionToken, string customerId, string nickname, string amount)
        {
            string participantID = "kanojia24.10@gmail.com";
            //string accessCode = "TU9HO6QE";
            string baseURL = "https://biller.mybluemix.net/biller/icicibank/billpay?client_id="
                + participantID.ToString()
                + "&token="
                + sessionToken
                + "&custid="
                + customerId
                + "&nickname="
                + nickname
                + "&amount="
                + amount;

            try
            {
                using (var wb = new System.Net.WebClient())
                {
                    Console.WriteLine("BaseURL : " + baseURL);
                    //wb.DefaultRequestHeaders.Host = "mydomain.com";
                    var response = wb.DownloadString(baseURL);
                    Console.WriteLine("Response This : " + response);
                    dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    Console.WriteLine("jsonData : " + jsonData);
                    //Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                    //Console.WriteLine("\njsonData 0 : " + jsonData[0]);
                    this.errCode = jsonData[0].code;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Internet Error : " + ex);
            }
            
        }
    }
}