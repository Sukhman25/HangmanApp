using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangmanApp.Adapter;
using HangmanApp.Data;
using Java.Interop;

namespace HangmanApp
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        private String[] words;
        private Random rand;
        private String currWord;
        private LinearLayout wordLayout;
        private TextView[] charViews;
        //body part images
        private ImageView[] bodyParts;
        //number of body parts
        private int numParts = 6;
        private GridView letters;
        private LetterAdapter ltrAdapt;
        //current part - will increment when wrong answers are chosen
        private int currPart;
        //number of characters in current word
        private int numChars;
        //number correctly guessed
        private int numCorr;

        private DataManager manager;
        private string playername;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.game_layout);

            playername = Intent.GetStringExtra("PlayerName");
            manager = new DataManager();

            words = Resources.GetStringArray(Resource.Array.words);
            rand = new Random();
            currWord = "";

            wordLayout = FindViewById<LinearLayout>(Resource.Id.word);

            letters = FindViewById<GridView>(Resource.Id.letters);

            bodyParts = new ImageView[numParts];
            bodyParts[0] = FindViewById<ImageView>(Resource.Id.head);
            bodyParts[1] = FindViewById<ImageView>(Resource.Id.body);
            bodyParts[2] = FindViewById<ImageView>(Resource.Id.arm1);
            bodyParts[3] = FindViewById<ImageView>(Resource.Id.arm2);
            bodyParts[4] = FindViewById<ImageView>(Resource.Id.leg1);
            bodyParts[5] = FindViewById<ImageView>(Resource.Id.leg2);

            StartGame();

        }

        private void StartGame()
        {
            string newWord = words[rand.Next(words.Length)];

            while (newWord.Equals(currWord))
            {
                newWord = words[rand.Next(words.Length)];
            }
            currWord = newWord;

            charViews = new TextView[currWord.Count()];
            
            wordLayout.RemoveAllViews();

            for (int index = 0; index < currWord.Count(); index++)
            {
                charViews[index] = new TextView(this);
                charViews[index].Text = currWord[index] + "";

                charViews[index].Gravity = GravityFlags.Center;
                charViews[index].SetTextColor(Color.White);
                charViews[index].SetBackgroundResource(Resource.Drawable.letter_bg);
                charViews[index].LayoutParameters = new ViewGroup.LayoutParams(WindowManagerLayoutParams.WrapContent,
                    WindowManagerLayoutParams.WrapContent);
                
                //add to layout
                wordLayout.AddView(charViews[index]);

            }

            ltrAdapt = new LetterAdapter(this);
            letters.Adapter = ltrAdapt;
            currPart = 0;
            numChars = currWord.Count();
            numCorr = 0;

            for (int p = 0; p < numParts; p++)
            {
                bodyParts[p].Visibility = ViewStates.Invisible;
            }
        }


        [Export("letterPressed")]
        public void letterPressed(View view)
        {
            //user has pressed a letter to guess
            String ltr = ((TextView)view).Text;
            char letterChar = ltr[0];

            view.Enabled = false;
            view.SetBackgroundResource(Resource.Drawable.letter_down);

            bool correct = false;
            for (int k = 0; k < currWord.Count(); k++)
            {
                if (currWord[k] == letterChar)
                {
                    correct = true;
                    numCorr++;
                    charViews[k].SetTextColor(Color.Black);
                }
            }
            if (correct)
            {
                //correct guess
                if (numCorr == numChars)
                {
                    manager.UpdatePlayerData(playername, true);
                    //user has won
                    // Disable Buttons
                    disableBtns();
                    // Display Alert Dialog
                    AlertDialog.Builder winBuild = new AlertDialog.Builder(this);
                    winBuild.SetTitle("Yay, well done!");
                    winBuild.SetMessage("You won!\n\nThe answer was:\n\n" + currWord);
                    winBuild.SetPositiveButton("Play Again", (c, v) =>
                    {
                        StartGame();
                    });
                    winBuild.SetNegativeButton("Exit", (c, v) =>
                    {
                        Finish();
                    });
                    winBuild.Show();
                }
            }
            else if (currPart < numParts)
            {
                //some guesses left
                bodyParts[currPart].Visibility = ViewStates.Visible;
                currPart++;
            }
            else
            {
                manager.UpdatePlayerData(playername, false);
                //user has lost
                disableBtns();
                // Display Alert Dialog
                AlertDialog.Builder loseBuild = new AlertDialog.Builder(this);
                loseBuild.SetTitle("Oopsie");
                loseBuild.SetMessage("You lose!\n\nThe answer was:\n\n" + currWord);
                loseBuild.SetPositiveButton("Play Again", (c, v) =>
                {
                    StartGame();
                });
                loseBuild.SetNegativeButton("Exit", (c, v) =>
                {
                    Finish();
                });
                loseBuild.Show();
            }
        }
        public void disableBtns()
        {
            int numLetters = letters.ChildCount;
            for (int l = 0; l < numLetters; l++)
            {
                letters.GetChildAt(l).Enabled = false;
            }
        }
    }
}