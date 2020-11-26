using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Views;
using HangmanApp.Data;
using HangmanApp.Model;

namespace HangmanApp
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : AppCompatActivity
    {
        private Button _buttonPlay, _buttonHelp, _buttonExit, _buttonHistory;
        private DataManager _manager;
        private EditText _etPlayer;
        private Android.Support.V7.App.AlertDialog helpAlert;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            _buttonPlay = FindViewById<Button>(Resource.Id.btnPlay);
            _buttonPlay.Click += _buttonPlay_Click;
            _buttonHelp = FindViewById<Button>(Resource.Id.btnHelp);
            _buttonHelp.Click += _buttonHelp_Click;
            _buttonExit = FindViewById<Button>(Resource.Id.btnExit);
            _buttonExit.Click += _buttonExit_Click;
            _buttonHistory = FindViewById<Button>(Resource.Id.btnHistory);
            _buttonHistory.Click += _buttonHistory_Click;
            _etPlayer = FindViewById<EditText>(Resource.Id.etPlayerName);
            _manager = new DataManager();
        }

        private void _buttonHistory_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(HistoryActivity));            
            StartActivity(intent);
        }

        private void _buttonExit_Click(object sender, System.EventArgs e)
        {
            Finish();
        }

        private void _buttonHelp_Click(object sender, System.EventArgs e)
        {
            ShowHelp();
        }

        private void _buttonPlay_Click(object sender, System.EventArgs e)
        {
            string playername = _etPlayer.Text.Trim().ToUpper();
            
            if( playername.Equals(""))
            {
                Toast.MakeText(this, "Please Enter Any Player Name", ToastLength.Long).Show();
            }
            else
            {
                if(!_manager.CheckPlayerName(playername))
                {
                    Player player = new Player();
                    player.PlayerName = playername;
                    player.Won = 0;
                    player.Lose = 0;
                    _manager.AddNewPlayer(player);
                }
                Intent intent = new Intent(this, typeof(GameActivity));                
                intent.PutExtra("PlayerName", playername);
                StartActivity(intent);
            }
        }

        
    public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Inflate the menu; this adds items to the action bar if it is present.
            MenuInflater.Inflate(Resource.Menu.hangman, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.action_search)
            {
                ShowHelp();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void ShowHelp()
        {
            Android.Support.V7.App.AlertDialog.Builder helpBuild = new Android.Support.V7.App.AlertDialog.Builder(this);

            helpBuild.SetTitle("Help");
            helpBuild.SetMessage("Guess the word by selecting the letters.\n\n"
                + "You only have 6 wrong selections then it's game over!");
            helpBuild.SetPositiveButton("OK", (c, v) =>
             {
                 helpAlert.Dismiss();
             });
              
		  helpAlert = helpBuild.Create();
		 
		  helpBuild.Show();
		}

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}