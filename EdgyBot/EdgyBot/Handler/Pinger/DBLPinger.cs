﻿using System.Threading.Tasks;
using DiscordBotsList.Api;
using Discord;

namespace EdgyCore.Handler.Pinger
{
    public class DBLPinger
    {
        private LibEdgyBot _lib = new LibEdgyBot();
        private AuthDiscordBotListApi dblApi;

        public DBLPinger ()
        {
            string dblToken = _lib.GetDBLToken();
            if (string.IsNullOrEmpty(dblToken))
                return;
            
            dblApi = new AuthDiscordBotListApi(_lib.GetBotId(), dblToken);
        }

        public async Task UpdateDBLStatsAsync (int serverCount)
        {
            try
            {
                IDblSelfBot self = await dblApi.GetMeAsync();
                await self.UpdateStatsAsync(serverCount);
            } catch
            {
                await _lib.EdgyLog(LogSeverity.Warning, "DBL Token Empty, skipping DBL Refresh");
            }
        }
    }
}
