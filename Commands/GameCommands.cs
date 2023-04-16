using System.Threading.Tasks;
using CustomProject.External_Classes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace CustomProject.Commands
{
    public class GameCommands : BaseCommandModule
    {
        [Command("cardgame")]
        [Cooldown(1, 3, CooldownBucketType.User)]
        public async Task SimpleCardGame(CommandContext ctx)
        {
            //user
            var UserCard = new CardBuilder();
            
            var userCardMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Purple)
                .WithTitle("Your Card:")
                .WithDescription("You drew a: " + UserCard.SelectedCard)
                );

            await ctx.Channel.SendMessageAsync(userCardMessage);

            //bot
            var BotCard = new CardBuilder();
            var botCardMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Purple)
                .WithTitle("My Card:")
                .WithDescription("I drew a: " + BotCard.SelectedCard)
                );

            await ctx.Channel.SendMessageAsync(botCardMessage);

            //The card game's condition
            if ((UserCard.SelectedNumber) > (BotCard.SelectedNumber))
            {
                var WinningMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    
                    .WithColor(DiscordColor.Green)
                    .WithTitle("Oh, **You Win!**")
                    
                    );

                await ctx.Channel.SendMessageAsync(WinningMessage);
                await ctx.Channel.SendMessageAsync("Aww, I just want some cash for an ice cream!");
            }
            else if ((UserCard.SelectedNumber) < (BotCard.SelectedNumber))
            {
                var LosingMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()

                    .WithColor(DiscordColor.Red)
                    .WithTitle("You Lose!")
                    );

                await ctx.Channel.SendMessageAsync(LosingMessage);
                await ctx.Channel.SendMessageAsync("Teehee, I will be asking for my Ice Cream!");
            }
            else if ((UserCard.SelectedNumber) == BotCard.SelectedNumber)
            {
                if ((UserCard.SelectedSuit.Length) > (BotCard.SelectedSuit.Length))
                {
                    var Winning2Message = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()

                        .WithColor(DiscordColor.Green)
                        .WithTitle("Oh, **You Win!**")

                    );

                    await ctx.Channel.SendMessageAsync(Winning2Message);
                    await ctx.Channel.SendMessageAsync("Aww, I just want some cash for an ice cream!");
                }
                else if ((UserCard.SelectedSuit.Length) < (BotCard.SelectedSuit.Length))
                {
                    var Losing2Message = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()

                        .WithColor(DiscordColor.Red)
                        .WithTitle("You Lose!")
                    );

                    await ctx.Channel.SendMessageAsync(Losing2Message);
                    await ctx.Channel.SendMessageAsync("Teehee, I will be asking for my Ice Cream!");
                }
                else if ((UserCard.SelectedSuit.Length) == (BotCard.SelectedSuit.Length))
                {
                    var DrawMessage = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()

                        .WithColor(DiscordColor.Yellow)
                        .WithTitle("Draw!")
                    );

                    await ctx.Channel.SendMessageAsync(DrawMessage);
                    await ctx.Channel.SendMessageAsync("Oh, half an ice cream then! Your treat, ok?");
                }
            }
        }


    }
}
