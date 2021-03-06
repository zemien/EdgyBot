﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using EdgyBot.Database;
using EdgyCore;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Discord;
using System.Text;
using System.Collections.Generic;

namespace EdgyBot.Modules.Categories
{
    public class OwnerCommands : InteractiveBase<ShardedCommandContext>
    {
        private LibEdgyBot _lib = new LibEdgyBot();

        [Command("ch"), Alias("checkup", "check", "status")]
        public async Task CheckupCmd ()
        {
            StringBuilder sb = new StringBuilder();
            int shardId = 0;
            foreach (DiscordSocketClient b in Context.Client.Shards)
            {
                sb.Append($"Shard {shardId} [{b.ConnectionState.ToString().ToUpper()}]\n");
                shardId++;
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("setstatus"), RequireOwner]
        public async Task SetStatusCmd([Remainder]string input = null)
        {
            if (input == "default") {
                EdgyCore.Handler.EventHandler.StatusIsCustom = false;
                await Context.Client.SetGameAsync("e!help | EdgyBot for " + Context.Client.Guilds.Count + " servers!");
                await ReplyAsync("Changed Status. **Custom Param: " + input + "**");

                return;
            }

            EdgyCore.Handler.EventHandler.StatusIsCustom = true;
            await Context.Client.SetGameAsync(input);
            await ReplyAsync("Changed Status.");
        }

        [Command("listservers", RunMode = RunMode.Async)]
        public async Task ListServersCmd ()
        {
            await ReplyAsync("ok my dude");

            StringBuilder sb = new StringBuilder();
            foreach (IGuild guild in Context.Client.Guilds)
            {
                if (guild == null)
                    continue;

                sb.Append(guild.Name + ':');
            }
            string[] pageStr = (sb.ToString()).Split(':');

            PaginatedMessage.Page[] pages = new PaginatedMessage.Page[pageStr.Length];
            for (int x = 0; x < pageStr.Length; x++)
            {
                pages[x] = new PaginatedMessage.Page
                {
                    Description = pageStr[x]
                };
            }

            await PagedReplyAsync(new PaginatedMessage(pages));
        }

        [Command("execquery")]
        [RequireOwner]
        public async Task ExecQueryCmd ([Remainder]string query)
        {
            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                try
                {
                    SQLProcessor sql = new SQLProcessor(connection.getConnObj());
                    await sql.ExecuteQueryAsync(query);
                }
                catch (Exception e)
                {
                    await ReplyAsync("Could not change the server prefix. Error: " + e.Message);
                }

                return;
            }
        }
    }
}
