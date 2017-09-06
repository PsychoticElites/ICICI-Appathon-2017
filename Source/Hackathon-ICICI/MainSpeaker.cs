using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System;
using System.Linq;

namespace Hackathon_ICICI
{
    class MainSpeaker
    {
        //Java.Util.Locale lang;
        //Context context;

        //TextToSpeech textToSpeech;

        async public void Main_speaker(string Text)
        {
            //textToSpeech = new TextToSpeech(cuntext, cuntext, "com.google.android.tts");
            //var crossLocale = true;
            //CrossTextToSpeech.Current.Speak("I'm am Dhruv Kanojia, Devesh Shyngle, Ankit Passi, Nishit Bose", crossLocale);
            Plugin.TextToSpeech.TextToSpeech ttse = new Plugin.TextToSpeech.TextToSpeech();

            CrossLocale localeen;
            var items = CrossTextToSpeech.Current.GetInstalledLanguages();
            //var lang = items.ToString();
            var evenScores = items.Where(i => true).ToList();
            Console.WriteLine("This is ITEM : " + evenScores);
            var lang = evenScores[110];
            Console.WriteLine("This is ITEM2 : " + lang);

            //foreach (var item in items)
            //{
            //    Console.WriteLine("This is ITEM : " + item);
            //}

            //localeen.Country = "en-IN";
            //localeen.Language = "en-IN";
            ttse.Speak(Text, false, lang);

        }


    }
}