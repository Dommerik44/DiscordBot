using System;
using Discord;
using Discord.Commands;
using Youtube;

namespace DiscordBot
{
    class Bot
    {
        public Bot()
        {
            DiscordClient discord = new DiscordClient(x => { x.LogLevel = LogSeverity.Info; x.LogHandler = Log; });

            discord.UsingCommands(x => { x.PrefixChar = '!'; x.AllowMentionPrefix = true;});

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("add").Do(e => 
            {
                Add(e.Message.ToString());
            });

            discord.ExecuteAndWait(async () => { await discord.Connect("MzAwODk4ODAzOTc0OTMwNDMy.C8z0Gw.hzvS5UVcuDq4XlgKbg69NvbhDlg", TokenType.Bot); });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void Add(string videoID)
        {
            // Need to add code to substring the video id from the link
            YouTube yt = new YouTube(videoID);
        }
    }
}
