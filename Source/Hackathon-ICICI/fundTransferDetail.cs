using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hackathon_ICICI
{
    [Activity(Label = "fundTransferDetail")]
    public class fundTransferDetail : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        public string destination_accountno, transaction_date, referance_no, transaction_amount, payee_name;
        EditText activeTextField;
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            speaker.Main_speaker("Welcome to funds transfer section.");
            base.OnCreate(savedInstanceState);
            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            string accountNumber = Intent.GetStringExtra("accountNumber") ?? "0";

            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.fundTransfer);
            // Create your application here

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            FindViewById<EditText>(Resource.Id.fundAccNumber).Text = accountNumber;

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please enter the destination account number.");
            });

            await System.Threading.Tasks.Task.Delay(8000);
            //Thread.Sleep(7000);
            Console.WriteLine("Active Field 1 : " + activeTextField);
            await System.Threading.Tasks.Task.Run(() => {
                activeTextField = FindViewById<EditText>(Resource.Id.fundDestAccount);
                StartActivityForResult(voiceIntent, VOICE);
            });
            Console.WriteLine("Active Field 2 : " + activeTextField);
            await System.Threading.Tasks.Task.Delay(10000);
            //Thread.Sleep(8000);
            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Please enter the amount to transfer.");
            });
            Console.WriteLine("Active Field 3 : " + activeTextField);
            await System.Threading.Tasks.Task.Delay(5000);
            //Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Run(() => {
                activeTextField = FindViewById<EditText>(Resource.Id.fundAmount);
                StartActivityForResult(voiceIntent, VOICE);
            });

            //Thread.Sleep(500);
            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("You want to Pay Rupees " + activeTextField.Text + " to account number " + FindViewById<EditText>(Resource.Id.fundDestAccount).Text.Aggregate(string.Empty, (c, i) => c + i + "  ").Replace("0", "Zero"));
                speaker.Main_speaker("Do you want me to submit this request?");
            });

            await System.Threading.Tasks.Task.Delay(15000);
            //Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Run(() => {
                activeTextField = FindViewById<EditText>(Resource.Id.fundAmount);
                StartActivityForResult(voiceIntent, VOICE);
            });

            Button submitButton = FindViewById<Button>(Resource.Id.fundTransferSubmit);
            
            //await System.Threading.Tasks.Task.Delay(5000);
            Button submitButton2 = FindViewById<Button>(Resource.Id.fundTransferSubmit);
            Console.WriteLine("I'm just here!");
            try
            {
                //submitButton.SetOnClickListener(this);
                submitButton.Click += delegate
                {
                    string sourceAccount = FindViewById<EditText>(Resource.Id.fundAccNumber).Text;
                    string destinationAccount = FindViewById<EditText>(Resource.Id.fundDestAccount).Text;
                    string amountToSend = FindViewById<EditText>(Resource.Id.fundAmount).Text;

                    Console.WriteLine("I'm just here 2!" + int.TryParse(destinationAccount, out int n));
                    Console.WriteLine("I'm just here 3!" + int.TryParse(amountToSend, out int m));

                    Console.WriteLine("Destination account Numbah : " + destinationAccount);

                    if (Regex.IsMatch(destinationAccount, @"^\d+$") && Regex.IsMatch(amountToSend, @"^\d+$"))
                    {
                        //speaker.Main_speaker("Burron Clicked!");
                        InformationFetcher(sessionToken, custId, sourceAccount, destinationAccount, amountToSend);
                        //speaker.Main_speaker("Information Fetched!");
                        var fundsComplete = new Intent(this, typeof(fundTransferCompleteScreen));
                        fundsComplete.PutExtra("destination_accountno", this.destination_accountno);
                        fundsComplete.PutExtra("transaction_date", this.transaction_date);
                        fundsComplete.PutExtra("referance_no", this.referance_no);
                        fundsComplete.PutExtra("transaction_amount", this.transaction_amount);
                        fundsComplete.PutExtra("payee_name", this.payee_name);
                        //Console.WriteLine("Running This!");
                        StartActivity(fundsComplete);
                        //speaker.Main_speaker("Funds successfully transfered!")
                    }
                    else
                    {
                        speaker.Main_speaker("Seems like an important field was left blank.");
                        base.OnRestart();
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Button Error : " + ex);
            }
        }

        //private async string dataCheck()
        //{
        //    TextView accountField = FindViewById<TextView>(Resource.Id.fundDestAccount);
        //    TextView amountField = FindViewById<TextView>(Resource.Id.fundAmount);
        //    if (accountField.Text == null)
        //    {
        //        speaker.Main_speaker("Seems like you did not enter the destination account number.");
        //        await System.Threading.Tasks.Task.Run(() => {
        //            speaker.Main_speaker("Please enter the destination account number.");
        //        });

        //        await System.Threading.Tasks.Task.Delay(8000);
        //        //Thread.Sleep(7000);
        //        Console.WriteLine("Active Field 1 : " + activeTextField);
        //        await System.Threading.Tasks.Task.Run(() => {
        //            activeTextField = FindViewById<EditText>(Resource.Id.fundDestAccount);
        //            StartActivityForResult(voiceIntent, VOICE);
        //        });
        //    }
        //}

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            Button b_personalDetails = FindViewById<Button>(Resource.Id.servicePersonal);
            Button submitButton = FindViewById<Button>(Resource.Id.fundTransferSubmit);

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
                                else if (item.Contains("no"))
                                {
                                    Finish();
                                    base.OnBackPressed();
                                }
                                else if (item.Contains("ok"))
                                {
                                    //Finish();
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

        private void InformationFetcher(string sessionToken, string customerId, string srcAccount, string destAccount, string amount)
        {
            string participantID = "kanojia24.10@gmail.com";
            //string accessCode = "TU9HO6QE";
            string baseURL = "https://retailbanking.mybluemix.net/banking/icicibank/fundTransfer?client_id="
                + participantID.ToString()
                + "&token="
                + sessionToken
                + "&srcAccount="
                + srcAccount
                + "&destAccount="
                + destAccount
                + "&amt="
                + amount
                + "&payeeDesc=NA&payeeId=1&type_of_transaction=school fee payment";

            using (var wb = new System.Net.WebClient())
            {
                Console.WriteLine("BaseURL : " + baseURL);
                //wb.DefaultRequestHeaders.Host = "mydomain.com";
                var response = wb.DownloadString(baseURL);
                Console.WriteLine("Response This : " + response);
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                Console.WriteLine("jsonData : " + jsonData);
                Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                this.destination_accountno = jsonData[1].destination_accountno;
                this.transaction_date = jsonData[1].transaction_date;
                this.referance_no = jsonData[1].referance_no;
                this.transaction_amount = jsonData[1].transaction_amount;
                this.payee_name = jsonData[1].payee_name;
            }

        }
        
    }
}