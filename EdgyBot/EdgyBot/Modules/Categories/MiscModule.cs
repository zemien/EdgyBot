﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using EdgyCore;
using EdgyCore.Lib;

namespace EdgyBot.Modules.Categories
{
    [Name("Miscellanious Commands"), Summary("Commands that don't fit in any other categories.")]
    public class MiscCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private LibEdgyCore _core = new LibEdgyCore();

        [Command("invite")][Name("invite")][Summary("Gives the Invite Link for EdgyBot.")]
        public async Task InviteCmd ()
        {
            Embed e = _lib.CreateEmbedWithText("Invite Link", _core.GetInviteLink() + "\nThanks for inviting EdgyBot! :smile:");
            await ReplyAsync("", embed: e);
        }

        [Command("source"), Alias("sourcecode", "github")]
        [Name("source")][Summary("Links the Source Code of EdgyBot")]
        public async Task SourceCmd ()
            => await ReplyAsync("", embed: _lib.CreateEmbedWithText("Source Code", "https://github.com/MonstahGames/EdgyBot \nIf you have a GitHub account, be sure to :star: the Repository!"));
        
        [Command("say")][Name("say")][Summary("Have EdgyBot send your message.")]
        public async Task SayCmd ([Remainder]string input)
            => await ReplyAsync(input);
        
        [Command("sayd")][Name("sayd")][Alias("saydelete")][Summary("This does the same thing as 'sayd', but deletes the original message.")]
        public async Task SaydCmd ([Remainder]string input)
        {
            await ReplyAsync(input);
            try { await Context.Message.DeleteAsync(); } catch
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError("Sayd Command", "Could not delete the message. Does the bot have the permissions to do so?"));
            }      
        }    

        [Command("e")]
        public async Task ECmd ()
        {
            //This is just a fun little inside joke, That's why it's only meant for one server.
            if (Context.Guild.Id != 424929039237775361)
                return;

            await ReplyAsync("monstah is not gay german");
        }
        
        [Command("suggest", RunMode = RunMode.Async)][Alias("sg", "sugg")][Name("suggest")][Summary("Sends your suggestion to the owner of the bot.")]
        public async Task SuggestCmd ([Remainder]string msg = null)
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("New Suggestion!", msg);
            EmbedFooterBuilder efb = new EmbedFooterBuilder
            {
                Text = Context.User.Username + $"#{Context.User.Discriminator}" + " in " + Context.Guild.Name + " at " + DateTime.Now.ToLongTimeString()
            };
            eb.Footer = efb;
            await EdgyCore.Handler.EventHandler.OwnerUser.SendMessageAsync("", embed: eb.Build());
            await ReplyAsync("Your message has been sent!");
        }

        [Command("bugreport")][Alias("reportbug")][Name("bugreport")][Summary("Submits a bug to the owner of the bot. Please use this command wisely. The command, steps to reproduce, etc...")]
        public async Task BugReportCmd ([Remainder]string msg = null)
        {
            if (msg == null)
            {
                await ReplyAsync("", embed: _lib.CreateEmbedWithError("Bug Report Error", "Please enter a message. The command, steps to reproduce, etc..."));
                return;
            }
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("New Bug!", msg);
            EmbedFooterBuilder efb = new EmbedFooterBuilder
            {
                Text = Context.User.Username + $"#{Context.User.Discriminator}" + " in " + Context.Guild.Name + " at " + DateTime.Now.ToLongTimeString()
            };
            eb.Footer = efb;
            await EdgyCore.Handler.EventHandler.OwnerUser.SendMessageAsync("", embed: eb.Build());
            await ReplyAsync("", embed: _lib.CreateEmbedWithText("Success", "Your Bug Report has been sent!"));
        }

        [Command("support")][Name("support")][Alias("helpedgy")][Summary("Tells you how you can support the development of EdgyBot")]
        public async Task SupportCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Discord Bot Lists", "Upvoting EdgyBot on the Discord Bot List helps EdgyBot get into more servers for even more fun.");
            eb.AddField("DBL", "https://discordbots.org/bot/373163613390897163");
            eb.AddField("Discord Bots", "https://bots.discord.pw/bots/373163613390897163");
            eb.AddField("BFD", "https://botsfordiscord.com/bot/373163613390897163");
            eb.AddField("botlist.space", "https://botlist.space/view/373163613390897163");

            eb.AddField("Moneys!11!", "Having a concurrent earning of a bit under 10$, I can keep EdgyBot running 24/7");
            eb.AddField("Support Link", "https://www.paypal.me/monstahhh");          
            eb.WithFooter(new EmbedFooterBuilder{Text = "Thanks for taking your time to even look at this command. People like you keep EdgyBot running."});
            await ReplyAsync("", embed: eb.Build());
        }
    }
}