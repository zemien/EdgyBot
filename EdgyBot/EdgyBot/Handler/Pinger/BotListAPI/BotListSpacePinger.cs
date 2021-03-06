﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdgyCore.Handler.Pinger
{
    public class BotListSpacePinger
    {
        private JsonHelper helper = new JsonHelper("https://botlist.space/api/bots/" + EdgyBot.Credentials.clientID);
        private LibEdgyBot _lib = new LibEdgyBot();

        public async Task PostServerCountAsync(int serverCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("server_count", serverCount);

            try { helper.postBotlist(dict, Botlist.LISTSPACE); }
            catch (Exception e)
            {
                await _lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to Bot List Space:\n" + e.Message);
            }
        }
    }
}
