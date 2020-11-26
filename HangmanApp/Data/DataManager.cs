using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HangmanApp.Model;
using SQLite;

namespace HangmanApp.Data
{
    public class DataManager
    {
        private SQLiteConnection conn;

        public DataManager()
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            conn = new SQLiteConnection(Path.Combine(path, "players.db"));
            if (!ConfirmTableExists())
            {
                conn.CreateTable<Player>();
            }

        }

        public bool CheckPlayerName(string playername)
        {
            List<Player> players = conn.Query<Player>("Select * from Player");
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].PlayerName.Equals(playername))
                {
                    return true;
                }

            }
            return false;
        }

        public bool UpdatePlayerData(string playername,bool status)
        {
            try
            {
                var players = conn.Table<Player>();
                var player = (from data in players
                              where data.PlayerName == playername
                              select data).Single();
                if( status )
                {
                    player.Won += 1;
                }
                else
                {
                    player.Lose += 1;
                }
                conn.Update(player);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Player> GetSortedPlayerList()
        {
            List<Player> players = conn.Query<Player>("Select * from Player order by Won desc");
            return players;
        }

        public bool AddNewPlayer(Player player)
        {
            try
            {
                conn.Insert(player);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ConfirmTableExists()
        {
            try
            {
                conn.Get<Player>(1);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}