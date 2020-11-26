using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangmanApp.Adapter;
using HangmanApp.Data;

namespace HangmanApp
{
    [Activity(Label = "History")]
    public class HistoryActivity : Activity
    {
        ListView listHistory;
        DataManager manager;
        PlayerDataAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.history_layout);
            manager = new DataManager();
            // Create your application here
            listHistory = FindViewById<ListView>(Resource.Id.listHistory);
            adapter = new PlayerDataAdapter(this, manager.GetSortedPlayerList());
            listHistory.Adapter = adapter;
        }
    }
}