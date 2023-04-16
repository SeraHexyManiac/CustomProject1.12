using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.CommandsNext;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using OpenAI_API;
using System.Linq;

namespace CustomProject.Slash_Commands
{
    public class UserRequestSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("image","Find an image that you want.")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task Image(InteractionContext ctx, [Option("name", "Type anything you want to find.")] string text)
        {
            string cseId = "873a0cae5a426459e";
            string apiKey = "AIzaSyDsJ2USVqYrmO0G-NWhClI1H8FA3kSfOes";

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Discord Bot",
                ApiKey = apiKey,
            });

            var listRequest = customSearchService.Cse.List();
            listRequest.Cx = cseId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = text;

            var search = await listRequest.ExecuteAsync();
            var results = search.Items;

            var random = new Random();
            var randomIndex = random.Next(0, results.Count);
            var randomResult = results[randomIndex];

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Here is what you search, teehee~ (I'm still trying to become better in searching!)"));
            if (results == null || !results.Any())
            {
                await ctx.Channel.SendMessageAsync("I can't find the result, aww boy...");
                return;
            }
            else
            {
                var captionMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()

                    .WithColor(DiscordColor.Purple)
                    .WithFooter()
                    .WithImageUrl(randomResult.Link)
                );
                await ctx.Channel.SendMessageAsync(captionMessage);
            }


        }



    }
}
