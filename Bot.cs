using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CustomProject.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using CustomProject.Slash_Commands;
using CustomProject.YoutubeAnnouncementAutomatic;
using System.Timers;
using CustomProject.External_Classes;
using CustomProject.Image_Handler;

namespace CustomProject
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        private YoutubeVideo _video = new YoutubeVideo();
        private YoutubeVideo temp = new YoutubeVideo();
        private AnnouncementDetectAndEngine _YouTubeEngine = new AnnouncementDetectAndEngine();

        private static int ImageIDCounter = 0;
        public static GoogleImageHandler imageHandler;

        public async Task RunAsync()
        {
            imageHandler = GoogleImageHandler.Instance;

            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(config);

            Client.ComponentInteractionCreated += ButtonCommandResponse;

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            }
            );

            var commandsConfig = new CommandsNextConfiguration()
            {

                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            var slashCommandConfig = Client.UseSlashCommands();


            Commands.RegisterCommands<FunCommands>();

            Commands.RegisterCommands<GameCommands>();

            Commands.RegisterCommands<UserRequestedCommands>();

            slashCommandConfig.RegisterCommands<FunSlashCommands>();

            slashCommandConfig.RegisterCommands<UserRequestSlashCommand>();

            slashCommandConfig.RegisterCommands<ModerationSlashCommand>();

            slashCommandConfig.RegisterCommands<AnnouncementSlashCommand>();

            Commands.CommandErrored += OnCommandError;

            await Client.ConnectAsync();

            await YoutubeUploadNotifier(_YouTubeEngine.channelId, _YouTubeEngine.apiKey, Client, 857557402512261122); // The Channel ID should be made by you.

            await Task.Delay(-1);

        }

        private static async Task MessageSendHandler(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (e.Message.Content == "!image")
            {
                ImageIDCounter = 0; //Reset the counter when someone uses this command
            }

        }

        private async Task ButtonCommandResponse(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            if (e.Interaction.Data.CustomId == "1")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You pressed the 1st Button"));
            }
            else if (e.Interaction.Data.CustomId == "2")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent("You pressed the 2nd Button"));
            }
            else if (e.Interaction.Data.CustomId == "funButton")
            {

                string funList = "**!hello** -> Hello Hello! \n" +
                                 "**!add/subtract/multiply/divide (number 1) (number 2)** -> Calculate \n" +
                                 "**!poll (time limit) (option 1) (option 2) (option 3) (option 4) (topic)** -> Starts a poll dummy!";

                var FunCommandList = new DiscordInteractionResponseBuilder()
                {
                    Title = "Fun Command List",
                    Content = funList,
                };

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, FunCommandList);
            }
            else if (e.Interaction.Data.CustomId == "gameButton")
            {
                string gamesList = "**!cardgame** -> Play a simple card game. Whoever draws the highest wins the game, if same number, whoever gets the strongest suit wins! \n";

                var GamesCommandList = new DiscordInteractionResponseBuilder()
                {
                    Title = "Game Command List",
                    Content = gamesList,
                };

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, GamesCommandList);
            }
            else if (e.Interaction.Data.CustomId == "slashButton")
            {
                string slashList = "**!hello** -> this is just a test command xd \n" +
                                   "**!poll** -> poll poll on the wall! \n" +
                                   "**!caption** -> Caption this, you may need it!";

                var SlashCommandList = new DiscordInteractionResponseBuilder()
                {
                    Title = "Game Command List",
                    Content = slashList,
                };

                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, SlashCommandList);

            }
            else if (e.Interaction.Data.CustomId == "previousButton")
            {
                ImageIDCounter--;
                string imageURL = Program.imageHandler.GetImageAtId(ImageIDCounter); 

                //Initialise the Buttons again

                var previousEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":track_previous:"));
                var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "previousButton", "Previous", false, previousEmoji);

                var nextEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":track_next:"));
                var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "nextButton", "Next", false, nextEmoji);

                

                var imageMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Results")
                    .WithImageUrl(imageURL)
                    .WithFooter("Page " + ImageIDCounter)
                    )
                    .AddComponents(previousButton, nextButton);
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(imageMessage.Embed).AddComponents(imageMessage.Components));
            }
            else if (e.Interaction.Data.CustomId == "nextButton")
            {
                ImageIDCounter++;
                string imageURL = Program.imageHandler.GetImageAtId(ImageIDCounter);

                var previousEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":track_previous:"));
                var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "previousButton", "Previous", false, previousEmoji);

                var nextEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(Client, ":track_next:"));
                var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "nextButton", "Next", false, nextEmoji);

                var imageMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Results")
                    .WithImageUrl(imageURL)
                    .WithFooter("Page " + ImageIDCounter)
                    )
                    .AddComponents(previousButton, nextButton);
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(imageMessage.Embed).AddComponents(imageMessage.Components));
            }
        }

        private Task ButtonCommandResponse(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task OnCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException)
            {
                var castedException = (ChecksFailedException)e.Exception;
                string cooldownTimer = string.Empty;

                foreach (var check in castedException.FailedChecks)
                {
                    var Cooldown = (CooldownAttribute)check;
                    TimeSpan timeLeft = Cooldown.GetRemainingCooldown(e.Context);
                    cooldownTimer = timeLeft.ToString(@"hh\:mm\:ss");
                }
                var cooldownMessage = new DiscordEmbedBuilder()
                {
                    Title = "Cutie, you are so imaptient!",
                    Description = "You need to wait for: " + cooldownTimer,
                    Color = DiscordColor.Violet,
                };
                await e.Context.Channel.SendMessageAsync(embed: cooldownMessage);
            }
        }

        private Task OnClientReady(DiscordClient Sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }

        public async Task YoutubeUploadNotifier(string channelId, string apiKey, DiscordClient client, ulong channelIdToNotify)
        {
            var Timeforsearching = new Timer(300000); 
            Timeforsearching.Elapsed += async (sender, e) => 
            {
                _video = _YouTubeEngine.GetLatestVideo(channelId, apiKey); //API goes into work
                DateTime lastCheckedAt = DateTime.Now;

                if (_video != null)
                {
                    if (temp.videoTitle == _video.videoTitle) //This is just a quick debug to see if it runs properly
                    {
                        Console.WriteLine("Same name buddy.");
                    }
                    else if (_video.PublishedAt < lastCheckedAt) //If the new video is actually a new video
                    {
                        var message = $"@everyone Ayo! We just got a new video guyss!! Come and check it out! | **{_video.videoTitle}** \n" +
                                      "Da URL: " + _video.videoUrl;

                        await client.GetChannelAsync(channelIdToNotify).Result.SendMessageAsync(message);
                        temp = _video;
                    }
                }
            };
            Timeforsearching.Start();
        }
    }
}

