using System;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

// Multithreading : http://codereview.stackexchange.com/questions/59147/running-2-sets-of-tasks-at-the-same-time
namespace Hackathon_ICICI
{
    [Activity(Label = "LOGIN to enjoy ICICI Services")]
    public class loginScreen : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        string token;
        public string authenticatedUser, errCode, customerID, accountNumber, randomOTPMain;
        //private bool isRecording;
        private readonly int VOICE = 10;

        //public override void OnBackPressed()
        //{
        //    //base.OnBackPressed();
        //    speaker.Main_speaker("This functionality has been disabled.");
        //}

        private void clicker()
        {
            Button loginButton = FindViewById<Button>(Resource.Id.loginSubmit);
            loginButton.PerformClick();
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            //speaker.Main_speaker("Please Enter Your Credentials");
            
            //await System.Threading.Tasks.Task.Delay(5000);
            // Create your application here

            SetContentView(Resource.Layout.loginScreen);
            authenticatedUser = "false";

            await Task.Run(() => {
                speaker.Main_speaker("Please Enter Your Credentials");
            });
            //await System.Threading.Tasks.Task.Delay(1500);
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 10000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            //StartActivityForResult(voiceIntent, VOICE);
            EditText textBox = FindViewById<EditText>(Resource.Id.loginCustID);
            EditText otpBox = FindViewById<EditText>(Resource.Id.otpbox);
            

            await System.Threading.Tasks.Task.Run(() =>
            {
                speaker.Main_speaker("Please speak your customer ID.");
            });

            await System.Threading.Tasks.Task.Delay(5000);

            await System.Threading.Tasks.Task.Run(() =>
            {
                //activeTextField = FindViewById<EditText>(Resource.Id.signUpAccountNumber);
                StartActivityForResult(voiceIntent, VOICE);
            });

            await System.Threading.Tasks.Task.Delay(10000);
            await System.Threading.Tasks.Task.Run(() =>
            {
                speaker.Main_speaker("Please wait while we verify you..");
            });

            await System.Threading.Tasks.Task.Run(() =>
            {
                Poster(textBox.Text);
            });

            if (errCode == "1")
            {
                speaker.Main_speaker("Please verify your custID and try again later.");
                authenticatedUser = "false";
                await System.Threading.Tasks.Task.Delay(7000);
                base.OnBackPressed();
            }
            else if (errCode == "0")
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    speaker.Main_speaker("Please wait while we send and OTP to your registered phone number.");
                });
                await System.Threading.Tasks.Task.Delay(5000);
                //TwilioClient.Init("AC97c83e0b5616af10e73d4fa6501f62f1", "6a71d77e9d57ecad604091c67c86da6f"); // Dhruv
                 TwilioClient.Init("ACbf43f5c34981744e596c3fb24f48ad38", "944b2bac9a20e625456374393892532b");  // Passi
                Console.WriteLine("HERE I am! 2");
                string randomOTP = otpGenerator();
                randomOTPMain = randomOTP;
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
                    //from: new PhoneNumber("+19318419285 "),// Dhruv
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
                otpBox.Text = randomOTP;
                await System.Threading.Tasks.Task.Delay(5000);
                speaker.Main_speaker("Signing you in!");
                //loginButton.PerformClick();
                authenticatedUser = "true";
                //speaker.Main_speaker("kliking the button");
                clicker();
                //speaker.Main_speaker("booton kilked!");

            }

            // Uncomment this

            Button loginButton = FindViewById<Button>(Resource.Id.loginSubmit);

            loginButton.Click += async delegate
            {
                //speaker.main_speaker("please wait while we try to log you in.");
                //speaker.Main_speaker("Inside this une, nigga");
                if (otpBox.Text == randomOTPMain)
                {
                    if (authenticatedUser == "true")
                    {
                        string authtoken = TokenFetcher();
                        //string custid = "33335329";
                        //string accountnumber = "4444777755550329";
                        //console.writeline("token : " + authtoken);
                        var loginsubmit_intent = new Intent(this, typeof(Services));
                        loginsubmit_intent.PutExtra("authToken", authtoken);
                        loginsubmit_intent.PutExtra("custId", customerID);
                        loginsubmit_intent.PutExtra("accountNumber", accountNumber);

                        Console.WriteLine("AuthToken LoginScreen : " + authtoken);
                        Console.WriteLine("customerID LoginScreen : " + customerID);
                        Console.WriteLine("accountNumber LoginScreen : " + accountNumber);

                        //var loginsubmit_intent = new intent(this, typeof(services));
                        StartActivity(loginsubmit_intent);
                    }
                    else if (authenticatedUser == "false")
                    {
                        await System.Threading.Tasks.Task.Run(() =>
                        {
                            speaker.Main_speaker("You need to log in to be able to access this application.");
                        });
                        await System.Threading.Tasks.Task.Delay(8000);
                        base.OnBackPressed();
                    }
                }
                else
                {
                    speaker.Main_speaker("Seems like the OTP entered is wrong.");
                }

            };


            //Button backButton = FindViewById<Button>(Resource.Id.imageButton1);
            //backButton.Click += delegate
            //{
            //    var backButton_Intent = new Intent(this, typeof(MainActivity));
            //    StartActivity(backButton_Intent);
            //};
        }

        private void Poster(string custId)
        {
            try
            {
                string registerUrl = "https://dmitra.000webhostapp.com/registration/iciciReg/login.php";

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["custId"] = custId;

                    var response = wb.UploadValues(registerUrl, "POST", data);
                    Console.WriteLine("This is the shitnit : " + response);
                    
                    var webPageResponse = Encoding.ASCII.GetString(response);

                    Console.WriteLine("WebPage Response : " + webPageResponse);
                    if (webPageResponse == "")
                    {
                        webPageResponse = "{'error_code':'1'}";
                    }
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(webPageResponse);
                    Console.WriteLine("OBJ2 : " + obj2);
                    try
                    {
                        this.errCode = obj2.error_code;
                        this.accountNumber = obj2.bankAccount;
                        this.customerID = obj2.custId;
                        Console.WriteLine("Error Code : " + errCode);
                    }
                    catch (Exception ex)
                    {
                        this.errCode = "1";
                        Console.WriteLine("Error Code : " + errCode);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ANother one : " + ex);
            }
        }

        private string otpGenerator()
        {
            Random rnd = new Random();
            String otp = rnd.Next(0, 1000000).ToString("D6");

            return otp;
        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            EditText textBox  = FindViewById<EditText>(Resource.Id.loginCustID);
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
                                loginButton.PerformClick();
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
                token = jsonData.token;
            }

            return token;
        }

        private string Login(string state)
        {
            return state;
        }
    }
}