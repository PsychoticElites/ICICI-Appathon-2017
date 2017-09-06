using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Hackathon_ICICI
{
    [Activity(Label = "SignUp to use ICICI m-Bank Services")]
    public class signUpScreen : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        public string mobileNo, token, customerId, bankAccountNumber, errCode;
        private readonly int VOICE = 10;
        EditText activeTextField;

        protected override void OnRestart()
        {
            base.OnRestart();
            Button b_OTPVerify = FindViewById<Button>(Resource.Id.signUpVerifyPhone);
            b_OTPVerify.Enabled = false;

            // Create your application here
            SetContentView(Resource.Layout.signUpScreen);
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            speaker.Main_speaker("Please speak the required information for sign up.");

            var t = Task.Factory.StartNew(() =>
            {
                speaker.Main_speaker("Please speak your customer ID.");
            });
            t.Wait();
            activeTextField = FindViewById<EditText>(Resource.Id.signUpCustID);
            StartActivityForResult(voiceIntent, VOICE);

            var t2 = Task.Factory.StartNew(() =>
            {
                speaker.Main_speaker("Please speak your account number.");
            });
            t2.Wait();
            activeTextField = FindViewById<EditText>(Resource.Id.signUpAccountNumber);
            StartActivityForResult(voiceIntent, VOICE);

            var t3 = Task.Factory.StartNew(() =>
            {
                speaker.Main_speaker("Please verify the details.");
                speaker.Main_speaker("Your Customer ID is " + FindViewById<EditText>(Resource.Id.signUpCustID).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                speaker.Main_speaker("Your account number is " + FindViewById<EditText>(Resource.Id.signUpAccountNumber).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                speaker.Main_speaker("Are these details correct?");
            });
            t3.Wait();
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.signUpScreen);
            Button b_OTPVerify = FindViewById<Button>(Resource.Id.signUpVerifyPhone);
            b_OTPVerify.Enabled = false;
            EditText custIDText = FindViewById<EditText>(Resource.Id.signUpCustID);
            EditText custaccountText = FindViewById<EditText>(Resource.Id.signUpAccountNumber);

            string custID = FindViewById<EditText>(Resource.Id.signUpCustID).Text;
            string accountNum = FindViewById<EditText>(Resource.Id.signUpAccountNumber).Text;

            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            speaker.Main_speaker("Please speak the required information for sign up.");

            Console.WriteLine("Active Text Field 1 : " + activeTextField);
            await System.Threading.Tasks.Task.Run(() =>
            {
                speaker.Main_speaker("Please speak your customer ID.");
            });

            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() =>
            {
                activeTextField = FindViewById<EditText>(Resource.Id.signUpCustID);
                StartActivityForResult(voiceIntent, VOICE);
            });
            //custIDText.Text = "33335329";
            Console.WriteLine("Active Text Field 2 : " + activeTextField);
            await System.Threading.Tasks.Task.Delay(10000);

            await System.Threading.Tasks.Task.Run(() =>
            {
                speaker.Main_speaker("Please speak your account number.");
            });

            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() =>
            {
                activeTextField = FindViewById<EditText>(Resource.Id.signUpAccountNumber);
                StartActivityForResult(voiceIntent, VOICE);
            });
            //custaccountText.Text = "4444777755550329";
            Console.WriteLine("Active Text Field : " + activeTextField);

            await System.Threading.Tasks.Task.Delay(10000);

            

            Console.WriteLine("Cust ID : " + custID);
            Console.WriteLine("accountNum : " + accountNum);


            await System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    speaker.Main_speaker("Please verify the details.");
                    speaker.Main_speaker("Your Customer ID is " + FindViewById<EditText>(Resource.Id.signUpCustID).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                    speaker.Main_speaker("Your account number is " + FindViewById<EditText>(Resource.Id.signUpAccountNumber).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                    speaker.Main_speaker("Are these details correct?");

                });
                await System.Threading.Tasks.Task.Delay(15000);
                StartActivityForResult(voiceIntent, VOICE);
                //if (Regex.IsMatch(custID, @"^\d+$") && Regex.IsMatch(accountNum, @"^\d+$"))
                //{
                //    await System.Threading.Tasks.Task.Run(() =>
                //    {
                //        speaker.Main_speaker("Please verify the details.");
                //        speaker.Main_speaker("Your Customer ID is " + FindViewById<EditText>(Resource.Id.signUpCustID).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                //        speaker.Main_speaker("Your account number is " + FindViewById<EditText>(Resource.Id.signUpAccountNumber).Text.Aggregate(string.Empty, (c, i) => c + i + "  "));
                //        speaker.Main_speaker("Are these details correct?");

                //    });
                //    await System.Threading.Tasks.Task.Delay(15000);
                //    StartActivityForResult(voiceIntent, VOICE);
                //}
                //else
                //{
                //    Console.WriteLine("Result custID : " + Regex.IsMatch(custID, @"^\d+$"));
                //    Console.WriteLine("Result accountNum : " + Regex.IsMatch(accountNum, @"^\d+$"));
                //    speaker.Main_speaker("Seems like an important field was left blank.");
                //    base.OnRestart();
                //}
            });

            // DO NOT TOUCH THIS CODE , RED HOT STUFF. 
            //Button b_OTPVerify = FindViewById<Button>(Resource.Id.signUpVerifyPhone);
            //b_OTPVerify.Click += delegate
            // {
            //     Button b_OTP = FindViewById<Button>(Resource.Id.signUpOTP);
            //     b_OTP.Enabled = true;
            // };

            //Button Config to go to Login Page After clicking SUBMIT button
            Button b_signUptoLogin = FindViewById<Button>(Resource.Id.signUpSubmit);

            b_signUptoLogin.Click += delegate
            {
                Poster(this.mobileNo, bankAccountNumber, customerId);
                if (errCode == "1")
                {
                    speaker.Main_speaker("You are already registered.");
                    speaker.Main_speaker("Please try again later.");
                    base.OnBackPressed();
                }
                else
                {
                    var signUptoLogin_Intent = new Intent(this, typeof(loginScreen));
                    StartActivity(signUptoLogin_Intent);
                }
            };


            EditText b_OTP = FindViewById<EditText>(Resource.Id.signUpOTP);
            
            b_OTPVerify.Click += async delegate
            {
                //b_OTP.Enabled = true;
                speaker.Main_speaker("Everything is FINE!");
                try
                {
                    Console.WriteLine("HERE I am 0!");
                    //speaker.Main_speaker("One Here");
                    string authToken = TokenFetcher();
                    //speaker.Main_speaker("Two Here");
                    Console.WriteLine("Mai auth Token : " + authToken);
                    custID = custIDText.Text;
                    accountNum = custaccountText.Text;

                    customerId = custID;
                    bankAccountNumber = accountNum;
                    InformationFetcher(authToken, custID, accountNum);
                    //speaker.Main_speaker("Three! Here");
                    Console.WriteLine("HERE I am!");
                    //TwilioClient.Init("AC97c83e0b5616af10e73d4fa6501f62f1", "6a71d77e9d57ecad604091c67c86da6f");// Dhruv
                    TwilioClient.Init("ACbf43f5c34981744e596c3fb24f48ad38", "944b2bac9a20e625456374393892532b");
                    Console.WriteLine("HERE I am! 2");
                    string randomOTP = otpGenerator();
                    //var call = CallResource.Create(
                    //    new PhoneNumber("+919717155636"),
                    //    from: new PhoneNumber("+19318419285"),
                    //    url: new Uri("https://my.twiml.here")
                    //);
                    //Console.WriteLine(call.Sid);

                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        var message = MessageResource.Create(
                        //new PhoneNumber("+919717155636"),// Dhruv
                        //from: new PhoneNumber("+19318419285"),// Dhruv
                        new PhoneNumber("+917503955048"), //Passi
                        from: new PhoneNumber("+19136866527"), //Passi
                        body: string.Format("Your OTP is {0}.", randomOTP)
                    );
                        Console.WriteLine("mai message : " + message.Sid);
                        //b_OTP.Text = message.Sid;

                    });
                    //await System.Threading.Tasks.Task.Delay(10000);
                    speaker.Main_speaker("Please Wait while we fetch your OTP.");
                    await System.Threading.Tasks.Task.Delay(4000);
                    b_OTP.Text = randomOTP;
                    await System.Threading.Tasks.Task.Delay(5000);
                    speaker.Main_speaker("Signing you up!");
                    b_signUptoLogin.PerformClick();


                }
                catch (Exception ex)
                {
                    Console.WriteLine("This error occured : " + ex);
                }

            };
        }

        private void Poster(string phoneNumber, string bankAccountNumber, string custId)
        {
            try
            {
                string registerUrl = "https://dmitra.000webhostapp.com/registration/iciciReg/register.php";

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["phoneNumber"] = phoneNumber;
                    data["bankAccount"] = bankAccountNumber;
                    data["custId"] = custId;

                    var response = wb.UploadValues(registerUrl, "POST", data);
                    Console.WriteLine("This is the shitnit : " + response);
                    //MessageBox.Show(response, "JSON Reply", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //string result = System.Text.Encoding.UTF8.GetString(response);
                    //Console.WriteLine("This is something amazhing : " + result);
                    var webPageResponse = Encoding.ASCII.GetString(response);
                    //resultShow.Text += "This is the data" + data + "\n";
                    //resultShow.Text += Encoding.ASCII.GetString(response);
                    //richTextBox1.Text += webPageResponse;
                    Console.WriteLine("WebPage Response : " + webPageResponse);
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(webPageResponse);
                    Console.WriteLine("OBJ2 : " + obj2);
                    this.errCode = obj2.error_code;
                    Console.WriteLine("Error Code : " + errCode);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ANother one : " + ex);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            Button otpButton = FindViewById<Button>(Resource.Id.signUpVerifyPhone);

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
                                otpButton.Enabled = true;
                                otpButton.PerformClick();
                            }

                            else if (item.Contains("no"))
                            {
                                base.OnRestart();
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
                                activeTextField.Text = item.Replace(" ", "").ToLower().Trim();
                            }
                        }

                    }
                    else
                        speaker.Main_speaker("I could not understand what you said.");
                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }

        private string TokenFetcher()
        {
            string participantID = "kanojia24.10@gmail.com";
            string accessCode = "TU9HO6QE";
            string baseURL = "https://corporateapiprojectwar.mybluemix.net/corporate_banking/mybank/authenticate_client?client_id="
                + participantID.ToString()
                + "&password="
                + accessCode.ToString();

            using (var wb = new System.Net.WebClient())
            {
                Console.WriteLine("BaseURL : " + baseURL);
                //wb.DefaultRequestHeaders.Host = "mydomain.com";
                var response = wb.DownloadString(baseURL);
                //Console.WriteLine("Response : " + response);
                dynamic jsonData = JsonConvert.DeserializeObject(response.Replace("[", "").Replace("]", ""));
                //Console.WriteLine("jsonData : " + jsonData);
                this.token = jsonData.token;
            }

            return token;
        }

        private string otpGenerator()
        {
            Random rnd = new Random();
            String otp = rnd.Next(0, 1000000).ToString("D6");

            return otp;
        }

        private void InformationFetcher(string sessionToken, string customerId, string accountNumber)
        {
            string participantID = "kanojia24.10@gmail.com";
            //string accessCode = "TU9HO6QE";
            string baseURL = "https://retailbanking.mybluemix.net/banking/icicibank/account_summary?client_id="
                + participantID.ToString()
                + "&token="
                + sessionToken
                + "&custid="
                + customerId
                + "&accountno="
                + accountNumber;
            Console.WriteLine("Mai base URL : " + baseURL);
            using (var wb = new System.Net.WebClient())
            {
                Console.WriteLine("BaseURL : " + baseURL);
                //wb.DefaultRequestHeaders.Host = "mydomain.com";
                var response = wb.DownloadString(baseURL);
                Console.WriteLine("Response This : " + response);
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                Console.WriteLine("jsonData : " + jsonData);
                Console.WriteLine("\njsonData 01 : " + jsonData[1]);
                this.mobileNo = jsonData[1].mobileno;
                Console.WriteLine("Mob Number : " + mobileNo);
            }
        }
    }
}