using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using OpenAI_API;
using System;
using System.Linq;
using System.Threading.Tasks;
using CustomProject.Image_Handler;

namespace CustomProject.Commands
{
    public class UserRequestedCommands : BaseCommandModule
    {
        [Command("image")]
        [Description("Searches Google Images for the given query.")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task ImageSearch(CommandContext ctx, [RemainingText] string query)
        {
            Program.imageHandler.images.Clear();
            int IDCount = 0;

            string cseId = "873a0cae5a426459e";
            string apiKey = "AIzaSyDsJ2USVqYrmO0G-NWhClI1H8FA3kSfOes";

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Discord Bot",
                ApiKey = apiKey,
            });

            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = cseId;
            listRequest.Num = 10;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = query;

            var search = await listRequest.ExecuteAsync();
            var results = search.Items;

            foreach (var result in results)
            {
                Program.imageHandler.images.Add(IDCount, result.Link);
                IDCount++;
            }

            if (results == null || !results.Any())
            {
                await ctx.RespondAsync("I can't find the result, aww boy...");
                return;
            }
            else
            {
                //Create the buttons for this embed
                var previousEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":track_previous:"));
                var previousButton = new DiscordButtonComponent(ButtonStyle.Primary, "previousButton", "Previous", false, previousEmoji);

                var nextEmoji = new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":track_next:"));
                var nextButton = new DiscordButtonComponent(ButtonStyle.Primary, "nextButton", "Next", false, nextEmoji);


                var random = new Random();

                var randomResult = random.Next(0, results.Count);
                var imageMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Purple)
                    .WithTitle("Results for: " + query)
                    .WithImageUrl(results[randomResult].Link)
                    )
                    .AddComponents(previousButton, nextButton);

                await ctx.Channel.SendMessageAsync(imageMessage);
            }
        }

        [Command("gpt")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task ChatGPT(CommandContext ctx, params string[] message)
        {
            var api = new OpenAIAPI("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");

            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage("Type in a query");

            chat.AppendUserInput(string.Join(" ", message));

            string response = await chat.GetResponseFromChatbot();

            var responseMsg = new DiscordEmbedBuilder()
            {
                Title = string.Join(" ", message),
                Description = response,
                Color = DiscordColor.Purple
            };

            await ctx.Channel.SendMessageAsync(embed: responseMsg);
        }
    }
}
