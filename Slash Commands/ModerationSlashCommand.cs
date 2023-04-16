using System;
using System.Globalization;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace CustomProject.Slash_Commands
{
    public class ModerationSlashCommand : ApplicationCommandModule
    {


        [SlashCommand("kick", "Yeehaw! Yer got kicked out!")]
        public async Task Kick(InteractionContext ctx, [Option("user", "The user you want to kick")] DiscordUser user,
            [Option("reason", "Reason for kick")] string reason = null)


        {
            await ctx.DeferAsync();

            var member = (DiscordMember)user;


            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator) && member.Permissions.HasPermission(Permissions.Administrator))
            {
                var CantKickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " can't be pulled out! ",
                    Description = "User is a mod or admin you know, tee hee~",
                    Color = DiscordColor.Purple
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(CantKickMessage));
            }
            else if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                await member.RemoveAsync();
                var kickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " got pulled out of the server! ",
                    Description = "Pulled by " + ctx.User.Username + "\nBecause of: " + reason,
                    Color = DiscordColor.Purple
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(kickMessage));
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

        [SlashCommand("timeout", "Shhhh! Be quiet!")]
        public async Task Timeout(InteractionContext ctx, [Option("user", "The user you want to shut up")] DiscordUser user,
                                                          [Option("duration", "Duration of the timeout in seconds")] long duration)
        {
            await ctx.DeferAsync();

            var member = (DiscordMember)user;

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator) && member.Permissions.HasPermission(Permissions.Administrator))
            {
                var CantKickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " can't be pulled out! ",
                    Description = "User is a mod or admin you know, tee hee~",
                    Color = DiscordColor.Purple
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(CantKickMessage));
            }
            else if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var TimeDuration = DateTime.Now + TimeSpan.FromSeconds(duration);

                await member.TimeoutAsync(TimeDuration);

                var timeoutMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " has been jebaited and got hooked on the mouth!!",
                    Description = "Duration: " + TimeSpan.FromSeconds(duration).ToString(),
                    Color = DiscordColor.Purple
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
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

        [SlashCommand("ban", "The greatest hook of all time!")]
        [Cooldown(1, 600, CooldownBucketType.User)]
        public async Task Ban(InteractionContext ctx, [Option("user", "The user you want to ban")] DiscordUser user,
                                      [Option("reason", "Reason for ban")] string reason = null)
        {
            await ctx.DeferAsync();

            var member = (DiscordMember)user;

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator) && member.Permissions.HasPermission(Permissions.Administrator))
            {
                var CantKickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " can't be pulled out! ",
                    Description = "User is a mod or admin you know, tee hee~",
                    Color = DiscordColor.Purple
                };
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(CantKickMessage));
            }
            else if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                await ctx.Guild.BanMemberAsync(member, 0, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = "Teehee, Rope unleashed Rope's greatest weapon! The hook of ban! Super effective against user: " + member.Username,
                    Description = "Reason: " + reason,
                    Color = DiscordColor.Purple
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
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
        [SlashCommand("createrole", "Creates a role")]
        public async Task CreateRoleCommand(InteractionContext ctx, [Option("RoleName", "The name of the role")] string name, [Option("Colour", "The hex number of the color")] string color,
        [Option("Reason", "Reason for creating the role")] string reason)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Creating Role..."));

            var newColor = DiscordColor.Black;
            if (!string.IsNullOrWhiteSpace(color) && color.StartsWith("#") && int.TryParse(color.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var colorValue))
            {
                newColor = new DiscordColor(colorValue);
            }


            await ctx.Guild.CreateRoleAsync(name, null, newColor, null, null, reason);

            await ctx.Channel.SendMessageAsync("Role Created Successfully");
        }
    }
}
