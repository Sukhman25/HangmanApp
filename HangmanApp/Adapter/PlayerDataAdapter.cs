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
using HangmanApp.Model;

namespace HangmanApp.Adapter
{
    public class PlayerDataAdapter : BaseAdapter<Player>
    {
        private Activity context;
        private List<Player> players;

        public PlayerDataAdapter(Activity context, List<Player> players)
        {
            this.players = players;
            this.context = context;
        }

        public override int Count
        {
            get { return players.Count; }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Player this[int position]
        {
            get { return players[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.row_layout, null, false);
            }

            TextView textPlayerName = row.FindViewById<TextView>(Resource.Id.textPlayerName);
            TextView textStatus = row.FindViewById<TextView>(Resource.Id.textStatus);

            textPlayerName.Text = "Player Name: " + players[position].PlayerName;
            
            string output = "Game Win: " + players[position].Won + " Game Lost: " + players[position].Lose;
            textStatus.Text = output;

            return row;
        }
    }
}