using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Google.Apis.YouTube.v3.Data;

namespace CustomProject.Slash_Commands
{
    public class AnnouncementSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("Announcement", "Announce everything you want!")]
        [Cooldown(1, 60, CooldownBucketType.User)]
        public async Task AnnouncementCommand(InteractionContext ctx, [Option("Channel", "A channel to notify")] DiscordChannel ChanneltoNotify,
                                                                        [Option("text", "The text to announce (separate paragraphs with <p>)")] string text2)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Time to Send!"));
            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                string[] paragraphs = text2.Split(new string[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);

                var essay = new StringBuilder();
                foreach (string paragraph in paragraphs)
                {
                    // Add indentation to the paragraph
                    string indentedParagraph = "    " + paragraph.Replace("\n", "\n    ");

                    // Add the paragraph to the essay
                    essay.AppendLine(indentedParagraph);
                }

                string formattedEssay = essay.ToString();

                var announcement = new DiscordMessageBuilder()
                              .AddEmbed(new DiscordEmbedBuilder()
                              .WithColor(DiscordColor.Purple)
                              .WithTitle("Announcement!")
                              .WithDescription(formattedEssay));

                var Done = new DiscordMessageBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                            .WithColor(DiscordColor.Purple)
                            .WithTitle("Message Sent!")
                            );

                await ctx.Channel.SendMessageAsync(Done);
                await ChanneltoNotify.SendMessageAsync("@everyone");
                await ChanneltoNotify.SendMessageAsync(announcement);
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Aww, how sweet!",
                    Description = "Oh you naughty cutie, sneaky you are trying to use that command on my watch?",
                    Color = DiscordColor.Violet
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }


    }
}
