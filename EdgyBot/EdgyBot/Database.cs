﻿using System.Data.SQLite;
using Discord;

namespace EdgyBot
{
    public class Database
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        /// <summary>
        /// File name of the Database.
        /// </summary>
        private readonly string _dbname = "database.db";

        /// <summary>
        /// Executes a SQL Command to the database.
        /// </summary>
        /// <param name="query"></param>
        public void ExecuteQuery (string query)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Insert a user into the database.
        /// </summary>
        /// <param name="uID"></param>
        public void InsertUser (ulong uID, string username)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            int startingXP = _lib.GetDefaultXP();
            cmd.CommandText = $"INSERT INTO users (userID, userName, userXP) VALUES ('{uID}', '{username}', '{startingXP}')";
            try
            {
                cmd.ExecuteNonQuery();
            } catch
            {
                _lib.EdgyLog(LogSeverity.Critical, "Could not add a user to the database. INFO: " + uID + " " + username);
            }         
            conn.Close();
        }
        public int GetXPFromUserID (ulong uID)
        {
            int currentXP = 0;
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"SELECT * FROM users WHERE userID='{uID}'";
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                currentXP = (int)r["userXP"];
            }
            conn.Close();
            return currentXP;
        }
        /// <summary>
        /// Excludes a server from recieving announcements etc...
        /// </summary>
        /// <param name="serverID"></param>
        public void BlacklistServer (ulong serverID)
        {
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"INSERT INTO blacklistedservers (serverID) VALUES ('{serverID}')";
            cmd.ExecuteNonQuery();
            conn.Clone();
        }

        /// <summary>
        /// Check if a server is blacklisted from recieving announcements.
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public bool IsServerBlacklisted (ulong serverID)
        {
            string isBlackListed = null;
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"SELECT * FROM blacklistedservers WHERE serverID='{serverID}'";
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                isBlackListed = (string)r["serverID"];
            }
            conn.Close();
            if (string.IsNullOrEmpty(isBlackListed))
            {
                return false;
            } else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if a user exists in the database.
        /// </summary>
        /// <param name="uID"></param>
        /// <returns></returns>
        public bool DoesUserExist(ulong uID)
        {
            string userExists = null;
            SQLiteConnection conn = new SQLiteConnection("DataSource=" + _dbname);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(conn);
            cmd.CommandText = $"SELECT * FROM users WHERE userID='{uID}'";
            conn.Close();
            SQLiteDataReader r = cmd.ExecuteReader();
            while (r.Read()) userExists = (string)r["userID"];
            if (string.IsNullOrEmpty(userExists))
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}