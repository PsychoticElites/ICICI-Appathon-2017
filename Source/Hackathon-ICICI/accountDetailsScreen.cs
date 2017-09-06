using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "Account Details")]
    public class accountDetailsScreen : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        public string accountType, accountStatus, balance, accountNo, custId, mobileNo;
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.accountDetails);
            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";

            Console.WriteLine("AuthToken Account Details : " + sessionToken);
            Console.WriteLine("custID Account Details : " + custId);
            InformationFetcher(sessionToken, custId);

            speaker.Main_speaker("This is your account's current information.");

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);


            try
            {
                FindViewById<EditText>(Resource.Id.accountBalance).Text = balance;
                speaker.Main_speaker("Your Account's balance is Rupees " + balance.Replace(".00", ""));
                FindViewById<EditText>(Resource.Id.accountNumber).Text = accountNo;
                speaker.Main_speaker("Your Account Number is " + accountNo.Aggregate(string.Empty, (c, i) => c + i + ' '));
                FindViewById<EditText>(Resource.Id.accountStatus).Text = accountStatus;
                speaker.Main_speaker("Your account status is " + accountStatus);
                FindViewById<EditText>(Resource.Id.accountType).Text = accountType;
                speaker.Main_speaker("and your account type is " + accountType);
                FindViewById<EditText>(Resource.Id.accountCustomerID).Text = custId;
                speaker.Main_speaker("Your customer ID is " + custId.Aggregate(string.Empty, (c, i) => c + i + ' '));
                FindViewById<EditText>(Resource.Id.accountPhone).Text = mobileNo;
                speaker.Main_speaker("Your registered mobile number with us is " + mobileNo.Aggregate(string.Empty, (c, i) => c + i + ' '));
            }
            catch (Exception ex)
            {
                Console.Write("Exeption is : " + ex);
            }

            await System.Threading.Tasks.Task.Delay(25000);

            await System.Threading.Tasks.Task.Run(() =>
            {
                speaker.Main_speaker("Do you want to go back to main menu?");
            });
            await System.Threading.Tasks.Task.Delay(5000);
            StartActivityForResult(voiceIntent, VOICE);
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            //EditText textBox = FindViewById<EditText>(Resource.Id.loginCustID);
            //Button loginButton = FindViewById<Button>(Resource.Id.loginSubmit);

            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                    if (matches.Count != 0)
                    {
                        foreach (string item in matches)
                        {
                            if (item.Contains("yes"))
                            {
                                //loginButton.Enabled = true;
                                base.OnBackPressed();
                            }
                            else if (item.Contains("exit"))
                            {
                                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                            }
                            else
                            {
                                //textBox.Text = item.Replace(" ", "").ToLower().Trim();
                                speaker.Main_speaker("I could not understand what you said.");
                            }
                        }

                    }
                    else
                        speaker.Main_speaker("I could not understand what you said.");
                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

        private void InformationFetcher(string sessionToken, string customerId)
        {
            string participantID = "kanojia24.10@gmail.com";
            //string accessCode = "TU9HO6QE";
            string baseURL = "https://retailbanking.mybluemix.net/banking/icicibank/account_summary?client_id="
                + participantID.ToString()
                + "&token="
                + sessionToken
                + "&custid="
                + customerId;

            using (var wb = new System.Net.WebClient())
            {
                Console.WriteLine("BaseURL : " + baseURL);
                //wb.DefaultRequestHeaders.Host = "mydomain.com";
                var response = wb.DownloadString(baseURL);
                Console.WriteLine("Response This : " + response);
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                Console.WriteLine("jsonData : " + jsonData);
                Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                this.accountType = jsonData[1].accounttype;
                this.accountStatus = jsonData[1].account_status;
                this.balance = jsonData[1].balance;
                this.accountNo = jsonData[1].accountno;
                this.custId = jsonData[1].custid;
                this.mobileNo = jsonData[1].mobileno;

                Console.WriteLine("Mobile : " + mobileNo);
                Console.WriteLine("accountType : " + accountType);
                Console.WriteLine("accountStatus : " + accountStatus);
                Console.WriteLine("balance : " + balance);
                Console.WriteLine("custId : " + custId);
                Console.WriteLine("accountNo : " + accountNo);
            }

        }
    }
}