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
using SQLite;

namespace HangmanApp.Model
{
    
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string PlayerName { get; set; }

        public int Won { get; set; }

        public int Lose { get; set; }
    }
}