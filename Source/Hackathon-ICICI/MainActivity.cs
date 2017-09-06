using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Android.Speech;

namespace Hackathon_ICICI
{
    [Activity(Label = "Hackathon ICICI", MainLauncher = false, Icon = "@drawable/pelogo1")]
    public class MainActivity : Activity
    {
        MainSpeaker speaker = new MainSpeaker();
        private readonly int VOICE = 10;

        protected async override void OnCreate(Bundle savedVariableInstance)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            speaker.Main_speaker("");
            speaker.Main_speaker("Welcome to I C I C I Special Banking");
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            Console.WriteLine("HERE 0");
            base.OnCreate(savedVariableInstance);
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);

                alert.SetTitle("Hi, how are you");

                alert.SetPositiveButton("Good", (senderAlert, args) => {
                    //change value write your own set of instructions
                    //you can also create an event for the same in xamarin
                    //instead of writing things here
                });

                alert.SetNegativeButton("Not doing great", (senderAlert, args) => {
                    //perform your own task for this conditional button click
                });
                //run the alert in UI thread to display in the screen
                RunOnUiThread(() => {
                    alert.Show();
                });
            }
            else
            {

                // Set our view from the "main" layout resource
                //speaker.Main_speaker("Welcome to I C I C I Special Banking");
                SetContentView(Resource.Layout.Main);
                try
                {
                    await System.Threading.Tasks.Task.Run(() => {
                        speaker.Main_speaker("Do you want to Log In or Sign Up?");
                    });

                    await System.Threading.Tasks.Task.Delay(7000);
                    
                    var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 9000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 9000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 35000);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                    voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                    StartActivityForResult(voiceIntent, VOICE);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Problem --> " + ex);
                }

                // Button Config for Going to Login Page
                ImageView b_Login = FindViewById<ImageView>(Resource.Id.mainLogin);
                b_Login.Click += delegate
                {
                    var login_Intent = new Intent(this, typeof(loginScreen));
                    StartActivity(login_Intent);
                };

                //Button Config for Going to Signup Page
                try
                {
                    ImageView b_SignUp = FindViewById<ImageView>(Resource.Id.mainSignup);
                    b_SignUp.Click += delegate
                    {
                        Console.WriteLine("Clicking the button!");
                        var signUp = new Intent(this, typeof(signUpScreen));
                        StartActivity(signUp);
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Mai Error : " + ex);
                }

                // Button config for Opening Psychotic Elites Website
                ImageView b_pelogo = FindViewById<ImageView>(Resource.Id.peLogo);
                b_pelogo.Click += delegate
                  {
                      var peLogo_Intent = Android.Net.Uri.Parse("http://www.psychoticelites.com");
                      var pe_intent = new Intent(Intent.ActionView, peLogo_Intent);
                      StartActivity(pe_intent);
                  };

                // Button config for Opening ICICI Website
                ImageView b_icici = FindViewById<ImageView>(Resource.Id.iciciLogo);
                b_icici.Click += delegate
                {
                    var iciciBank_Intent = Android.Net.Uri.Parse("https://www.icicibank.com/");
                    var icici_intent = new Intent(Intent.ActionView, iciciBank_Intent);
                    StartActivity(icici_intent);
                };

                // Button config for Opening ICICI ATM Website
                ImageView b_iciciATM = FindViewById<ImageView>(Resource.Id.iciciATM);
                b_iciciATM.Click += delegate
                {
                    var iciciATM_Intent = Android.Net.Uri.Parse("http://maps.icicibank.com/mobile/?m=1");
                    var icici2_intent = new Intent(Intent.ActionView, iciciATM_Intent);
                    StartActivity(icici2_intent);
                };
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
        {
            ImageView signUpButton = FindViewById<ImageView>(Resource.Id.mainSignup);
            ImageView loginButton = FindViewById<ImageView>(Resource.Id.mainLogin);
            if (requestCode == VOICE)
            {
                if (resultVal == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                    if (matches.Count != 0)
                    {
                        foreach (string item in matches)
                        {
                            Console.WriteLine("Mai Wurds : " + item);
                            if (item.Contains("login"))
                                loginButton.PerformClick();
                            else if (item.Contains("log in"))
                                loginButton.PerformClick();
                            else if (item.Contains("sign in"))
                                loginButton.PerformClick();

                            else if (item.Contains("signup"))
                                signUpButton.PerformClick();
                            else if (item.Contains("sign up"))
                                signUpButton.PerformClick();
                            else if (item.Contains("register"))
                                signUpButton.PerformClick();
                            else if (item.Contains("exit"))
                            {
                                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                            }
                            else
                            {
                                speaker.Main_speaker("That doesn't seem like an operation I can perform.");
                            }
                        }

                    }
                    else
                        speaker.Main_speaker("I could not understand what you said.");
                }
            }

            base.OnActivityResult(requestCode, resultVal, data);
        }
    }
}

