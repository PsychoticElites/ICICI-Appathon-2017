using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using System.Text;
using System.Collections.Specialized;
using Android.Widget;
using System.Linq;
using Android.Speech;
using System.Threading.Tasks;

namespace Hackathon_ICICI
{
    [Activity(Label = "Personal Details")]
    public class personalDetails : Activity
    {
        public string custAge, custPhoneNumber, custAddress, custGender, custMarital, custSegment;
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            //Console.WriteLine("THIS IS CUST ID : " + custId);
            //Console.WriteLine("THIS IS TOKEN : " + sessionToken);
            //InformationFetcher(sessionToken, custId, out custAge);
            //Console.WriteLine("AGE : " + custAge);
            base.OnCreate(savedInstanceState);
            try
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 20000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 20000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 25000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                //StartActivityForResult(voiceIntent, VOICE);

                // Create your application here
                SetContentView(Resource.Layout.personalDetailsScreen);
                //string custId = Intent.GetStringExtra("custId") ?? "0";
                //string sessionToken = Intent.GetStringExtra("authToken") ?? "0";
                //Console.WriteLine("Past  THIS");
                //InfoPoster(sessionToken);
                string custName = FindViewById<EditText>(Resource.Id.editText2).Text;
                string custAge = FindViewById<EditText>(Resource.Id.personalAge).Text;
                string custPhone = FindViewById<EditText>(Resource.Id.personalPhone).Text;
                string custAddress = FindViewById<EditText>(Resource.Id.editText1).Text;
                string custGender = FindViewById<EditText>(Resource.Id.personalGender).Text;
                string custStatus = FindViewById<EditText>(Resource.Id.personalMarital).Text;
                string custSegment = FindViewById<EditText>(Resource.Id.personalSegment).Text;

                await Task.Run(() => {
                    speaker.Main_speaker("Welcome " + custName);
                    speaker.Main_speaker("Your age is " + custAge);
                    speaker.Main_speaker("You registered Phone number is " + custPhone.Aggregate(string.Empty, (c, i) => c + i + "  "));
                    speaker.Main_speaker("Your registered address is " + custAddress);
                    speaker.Main_speaker("You are a " + custGender);
                    speaker.Main_speaker("Your marital status is " + custStatus);
                    speaker.Main_speaker("You are a " + custSegment);
                });
                await System.Threading.Tasks.Task.Delay(25000);
                speaker.Main_speaker("Redirecting you back to main menu");
                await System.Threading.Tasks.Task.Delay(3000);
                base.OnBackPressed();
                //speaker.Main_speaker("Redirecting you back to main menu now.");
                //StartActivityForResult(voiceIntent, VOICE);
            }
            catch (Exception ex)
            {
                Console.WriteLine("This is the error : " + ex);
            }

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
                                if (item.Contains("back"))
                                {
                                    base.OnBackPressed();
                                    break;
                                }

                                else if (item.Contains("main"))
                                {
                                    base.OnBackPressed();
                                    break;
                                }
                                else if (item.Contains("menu"))
                                {
                                    base.OnBackPressed();
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

        //private void InfoPoster(string authToken)
        //{
        //    try
        //    {

        //        using (var wb = new System.Net.WebClient())
        //        {
        //            Console.WriteLine("INSIDE THIS!");
        //            //wb.Headers.Add("Content-Type", "application/json");
        //            wb.Headers["Content-Type"] = "application/json";
        //            string json = "{\r\n\r\n\t\"clientId\": \"kanojia24.10@gmail.com\",\r\n\r\n\t\"authToken\": \"8eb734862234\",\r\n\r\n\t\"policyNo\": \"00000099\",\r\n\t\"Dob\": \"\",\r\n\t\"Email\": \"\",\r\n\t\"mobno\": \"\",\r\n\t\"panNo\": \"\"\r\n}".Replace("8eb734862234", authToken);
        //            Console.WriteLine("Mui Jsun : " + json);
        //            var response = wb.UploadString("https://ipru.mybluemix.net/api/checkCustomerService", json);
        //            //var response = wb.UploadValues("https://ipru.mybluemix.net/api/checkCustomerService", "POST", json);
        //            Console.WriteLine("Been HERE");
        //            Console.WriteLine("LELWA : " + response);
        //            //string something = Encoding.ASCII.GetString(response);
        //            dynamic obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
        //            Console.WriteLine("OBJ2 : " + obj2);
        //            var message = obj2[0];
        //            Console.WriteLine("TestData : " + message);
        //            //int errCode = obj2.error_code;
        //            //string message = obj2.message;

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        //resultShow.Text += ex;
        //        Console.WriteLine("Exuption : " + ex);
        //    }
        //}

        //private void InformationFetcher(string sessionToken, string customerId, out string custAge)
        //{
        //    string participantID = "kanojia24.10@gmail.com";
        //    //string accessCode = "TU9HO6QE";
        //    string baseURL = "https://prudential.mybluemix.net/banking/icicibank/viewcustomerdetails?client_id="
        //        + participantID.ToString()
        //        + "&token="
        //        + sessionToken
        //        + "&custid="
        //        + customerId;

        //    using (var wb = new System.Net.WebClient())
        //    {
        //        Console.WriteLine("BaseURL : " + baseURL);
        //        //wb.DefaultRequestHeaders.Host = "mydomain.com";
        //        var response = wb.DownloadString(baseURL);
        //        //Console.WriteLine("Response : " + response);
        //        dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Replace("[", "").Replace("]", ""));
        //        //Console.WriteLine("jsonData : " + jsonData);
        //        custAge = jsonData.Age;
        //        custAddress = jsonData.Address;
        //        custGender = jsonData.Gender;
        //    }

        //}
    }
}