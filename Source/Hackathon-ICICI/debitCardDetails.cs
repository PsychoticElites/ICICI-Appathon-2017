using System;
using Newtonsoft.Json;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "Debit Card Details")]
    public class debitCardDetails : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        public string custaccountNo, custCardType, custDebitCardNo, custExpDate;
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            
            // Create your application here
            SetContentView(Resource.Layout.debitCardDetails);
            string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
            string custId = Intent.GetStringExtra("custId") ?? "0";
            Console.WriteLine("AuthToken debitCard : " + sessionToken);
            Console.WriteLine("custID debitCard : " + custId);

            InformationFetcher(sessionToken, custId);

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            FindViewById<EditText>(Resource.Id.debitAccountNumber).Text = custaccountNo;
            speaker.Main_speaker("Your Account Number is " + custaccountNo.Aggregate(string.Empty, (c, i) => c + i + " "));
            FindViewById<EditText>(Resource.Id.debitCardNumber).Text = custDebitCardNo;
            speaker.Main_speaker("Your Debit Card Number is " + custDebitCardNo.Replace("-", "").Aggregate(string.Empty, (c, i) => c + i + ' '));
            FindViewById<EditText>(Resource.Id.debitCardType2).Text = custCardType;
            speaker.Main_speaker("Your Card Type is " + custCardType);
            FindViewById<EditText>(Resource.Id.debitExpiry).Text = custExpDate;
            //speaker.Main_speaker("Your Card Is Valid Till " + DateTime.ParseExact("1-" + custExpDate, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture));
            speaker.Main_speaker("Your Card Is Valid Till " + DateTime.Parse("1-" + custExpDate));

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
            EditText textBox = FindViewById<EditText>(Resource.Id.loginCustID);
            Button loginButton = FindViewById<Button>(Resource.Id.loginSubmit);

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
                                textBox.Text = item.Replace(" ", "").ToLower().Trim();
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
            string baseURL = "https://debitcardapi.mybluemix.net/debit/icicibank/getDebitDetails?client_id="
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
                dynamic jsonData = JsonConvert.DeserializeObject(response);
                Console.WriteLine("jsonData : " + jsonData);
                Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                this.custaccountNo = jsonData[1].accountno;
                this.custCardType = jsonData[1].Type_of_card;
                this.custDebitCardNo = jsonData[1].Debit_card_no;
                this.custExpDate = jsonData[1].exp_date;
                Console.WriteLine("custaccountNo : " + custaccountNo);
                Console.WriteLine("custCardType : " + custCardType);
                Console.WriteLine("custDebitCardNo : " + custDebitCardNo);
                Console.WriteLine("custExpDate : " + custExpDate);
            }

        }
    }
}