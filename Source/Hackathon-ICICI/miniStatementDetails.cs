using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "miniStatementDetails")]
    public class miniStatementDetails : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;
        public string transactionAmount, transactionDate, accountNo, creditDebitFlag, remark, closingBalance;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Create your application here

            SetContentView(Resource.Layout.miniStatement);
            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            string accountNumber = Intent.GetStringExtra("accountNumber") ?? "0";
            Console.WriteLine("AuthToken miniStatement Details : " + sessionToken);
            Console.WriteLine("custID miniStatement Details : " + custId);
            Console.WriteLine("accountNumber miniStatement Details : " + accountNumber);

            speaker.Main_speaker("Here is your previous transaction history.");
            InformationFetcher(sessionToken, custId, accountNumber);

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            FindViewById<EditText>(Resource.Id.transactionAmount).Text = transactionAmount;
            speaker.Main_speaker("Transaction Amount was Rupees " + transactionAmount.Replace(".00",""));
            FindViewById<EditText>(Resource.Id.transactionDate).Text = transactionDate;
            speaker.Main_speaker("Transaction was made on " + transactionDate);
            FindViewById<EditText>(Resource.Id.transactionAccount).Text = accountNo;
            speaker.Main_speaker("Transaction was made from account number " + accountNo.Aggregate(string.Empty, (c, i) => c + i + ' '));
            FindViewById<EditText>(Resource.Id.transactionCreditDebit).Text = creditDebitFlag;
            speaker.Main_speaker("It was made  " + creditDebitFlag.Replace("Dr.", "Debit").Replace("Cr.", "Credit"));
            FindViewById<EditText>(Resource.Id.transactionRemarks).Text = remark;
            speaker.Main_speaker("Transaction remark says " + remark);
            FindViewById<EditText>(Resource.Id.transactionClosingBalance).Text = closingBalance;
            speaker.Main_speaker("Closing balance after this transaction is Rupees " + closingBalance.Replace(".00", ""));
            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() => {
                speaker.Main_speaker("Do you want to go back?");
            });

            await System.Threading.Tasks.Task.Delay(25000);
            //Thread.Sleep(5000);
            await System.Threading.Tasks.Task.Run(() => {
                StartActivityForResult(voiceIntent, VOICE);
            });
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            //Button b_personalDetails = FindViewById<Button>(Resource.Id.servicePersonal);
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

        private void InformationFetcher(string sessionToken, string customerId, string accountNum)
        {
            string participantID = "kanojia24.10@gmail.com";
            //string accessCode = "TU9HO6QE";
            string baseURL = "https://retailbanking.mybluemix.net/banking/icicibank/recenttransaction?client_id="
                + participantID.ToString()
                + "&token="
                + sessionToken
                + "&custid="
                + customerId
                + "&accountno="
                + accountNum;

            using (var wb = new System.Net.WebClient())
            {
                Console.WriteLine("BaseURL : " + baseURL);
                //wb.DefaultRequestHeaders.Host = "mydomain.com";
                var response = wb.DownloadString(baseURL);
                Console.WriteLine("Response This : " + response);
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                Console.WriteLine("jsonData : " + jsonData);
                Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                this.transactionAmount = jsonData[1].transaction_amount;
                this.transactionDate = jsonData[1].transactiondate;
                this.accountNo = jsonData[1].accountno;
                this.creditDebitFlag = jsonData[1].credit_debit_flag;
                this.remark = jsonData[1].remark;
                this.closingBalance = jsonData[1].closing_balance;
            }

        }
    }


}