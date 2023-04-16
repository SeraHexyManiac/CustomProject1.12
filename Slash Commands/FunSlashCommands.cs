using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace CustomProject.Slash_Commands
{
    public class FunSlashCommands : ApplicationCommandModule
    {

        [SlashCommand("hello", "This is the test slash command")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task TestSlashCommand(InteractionContext ctx,  [Choice("Pre-Defined Text","Just a random string.")]
                                                                    [Option("string", "type anything you want.")] string text)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Starting Slash Command"));

            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = text
            };

            await ctx.Channel.SendMessageAsync(embed: embedMessage);
        }

        [SlashCommand("poll", "Oh, you want to create a poll? Okie teehee~")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task PollCommand(InteractionContext ctx, [Option("question", "The main poll subject/question")] string Question,
                                                      [Option("timelimit", "The time set on this poll (second)")] long TimeLimit,
                                                      [Option("option1", "Option 1")] string Option1,
                                                      [Option("option2", "Option 1")] string Option2,
                                                      [Option("option3", "Option 1")] string Option3,
                                                      [Option("option4", "Option 1")] string Option4)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                .WithContent(Question));

            var interactvity = ctx.Client.GetInteractivity(); 
            TimeSpan timer = TimeSpan.FromSeconds(TimeLimit); 
            DiscordEmoji[] optionEmojis = { DiscordEmoji.FromName(ctx.Client, ":one:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":two:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":three:", false),
                                            DiscordEmoji.FromName(ctx.Client, ":four:", false) }; 

            string optionsString = optionEmojis[0] + " | " + Option1 + "\n" +
                                   optionEmojis[1] + " | " + Option2 + "\n" +
                                   optionEmojis[2] + " | " + Option3 + "\n" +
                                   optionEmojis[3] + " | " + Option4; 

            var pollMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Purple)
                .WithTitle(string.Join(" ", Question))
                .WithDescription(optionsString)
                ); 

            var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage); 

            foreach (var emoji in optionEmojis)
            {
                await putReactOn.CreateReactionAsync(emoji); 
            }

            var result = await interactvity.CollectReactionsAsync(putReactOn, timer); 

            int count1 = 0; 
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in result)
            {
                if (emoji.Emoji == optionEmojis[0])
                {
                    count1++;
                }
                if (emoji.Emoji == optionEmojis[1])
                {
                    count2++;
                }
                if (emoji.Emoji == optionEmojis[2])
                {
                    count3++;
                }
                if (emoji.Emoji == optionEmojis[3])
                {
                    count4++;
                }
            }

            int totalVotes = count1 + count2 + count3 + count4;

            string resultsString = optionEmojis[0] + ": " + count1 + " Votes \n" +
                       optionEmojis[1] + ": " + count2 + " Votes \n" +
                       optionEmojis[2] + ": " + count3 + " Votes \n" +
                       optionEmojis[3] + ": " + count4 + " Votes \n\n" +
                       "The total number of votes is " + totalVotes;

            var resultsMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Violet)
                .WithTitle("Results of Poll")
                .WithDescription(resultsString)
                );

            await ctx.Channel.SendMessageAsync(resultsMessage);        
        }

        [SlashCommand("caption", "Give any image a Caption")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task CaptionCommand(InteractionContext ctx, [Option("caption", "The caption you want the image to have")] string caption,
                                                                 [Option("image", "The image you want to upload")] DiscordAttachment picture)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                    .WithContent(caption));

            var captionMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Purple)
                .WithFooter()
                .WithImageUrl(picture.Url)
                );

            await ctx.Channel.SendMessageAsync(captionMessage);

        }

        [SlashCommand("help", "Help help? Help with commands!")]
        
        public async Task HelpCommand(InteractionContext ctx, [Choice("Fun Commands","prefix (!) Fun Commands.")]
                                                              [Choice("Game Commands","prefix (!) Game Commands.")]
                                                              [Choice("User Request Commands","prefix User Request Commands.")]
                                                              [Choice("Fun Slash Commands","A lot of Fun Commands")]
                                                              [Choice("Moderation Slash Commands","Moderation~~")]
                                                              [Choice("User Request Slash Commands","User Request Slash please!")]
                                                              [Option("Choice", "type anything you want.")] string text)

        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            .WithContent("I will show you what I have!"));

            if (text == "prefix (!) Fun Commands.")
            {
                var resultsMessage = new DiscordMessageBuilder()
               .AddEmbed(new DiscordEmbedBuilder()

               .WithColor(DiscordColor.Violet)
               .WithTitle("Fun Command!")
               .WithDescription("**!hello**\n -> Hello Hello! \n" +
                                 "\n**!add/subtract/multiply/divide (number 1) (number 2)**\n -> Calculate \n" +
                                 "\n**!poll (time limit) (option 1) (option 2) (option 3) (option 4) (topic)**\n -> Starts a poll dummy!")
               );

                await ctx.Channel.SendMessageAsync(resultsMessage);
            }
            else if (text == "prefix (!) Game Commands.")
            {
                var resultsMessage = new DiscordMessageBuilder()

                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Violet)
                .WithTitle("Game Commands")
                .WithDescription("**!cardgame**\n -> Play a simple card game. Whoever draws the highest wins the game, if same number, whoever gets the strongest suit wins! \n")

                );

                await ctx.Channel.SendMessageAsync(resultsMessage);
            }
            else if (text == "prefix User Request Commands.")
            {
                var resultsMessage = new DiscordMessageBuilder()

                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Violet)
                .WithTitle("User Request Command (Noted: Commands may not work so report me when it happens.)")
                .WithDescription("**!image (name with description)**\n -> Get yourself the image you want!\n" +
                                 "\n**!gpt (your question)**\n -> GG! GPT!")

                );

                await ctx.Channel.SendMessageAsync(resultsMessage);
            }
            else if (text == "A lot of Fun Commands")
            {
                var resultsMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Violet)
                .WithTitle("Fun Slash Command!")
                .WithDescription("**Hello**\n -> Hello Hello! \n" +
                                 "\n**Caption**\n -> Cap the Caption! \n" +
                                 "\n**Poll**\n -> Starts a poll dummy!")
);

                await ctx.Channel.SendMessageAsync(resultsMessage);
            }
            else if (text == "User Request Slash please!")
            {
                var resultsMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Violet)
                    .WithTitle("User Request Slash Command")
                    .WithDescription("**Image**\n -> Images? Get you one!")

                    );
                await ctx.Channel.SendMessageAsync(resultsMessage);
            }
            else if (text == "Moderation~~")
            {
                if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
                {
                    var resultsMessage = new DiscordMessageBuilder()
                   .AddEmbed(new DiscordEmbedBuilder()
                   .WithColor(DiscordColor.Violet)
                   .WithTitle("Moderation Command")
                   .WithDescription("**Timeout**\n -> Hooked! Your mouth gets mocked!\n" + 
                                    "\n**Kick**\n -> Bye bye! You will be back some time... I think?\n" +
                                    "\n**Ban**\n -> One and a two and a three, gone!")                                                          

                   );
                    await ctx.Channel.SendMessageAsync(resultsMessage);
                }
                else
                {
                    var resultsMessage = new DiscordMessageBuilder()
                  .AddEmbed(new DiscordEmbedBuilder()
                  .WithColor(DiscordColor.Violet)
                  .WithTitle("Moderation Command")
                  .WithDescription("Teehee, you are a sneaky little cute member!")

                  );
                    await ctx.Channel.SendMessageAsync(resultsMessage);
                }
            }
        }

        [SlashCommand("Countdown", "Counting!")]
        [Cooldown(1, 5, CooldownBucketType.Channel)]
        public async Task CountingCommand(InteractionContext ctx, [Option("Countdown", "Time that you want to put in (seconds)")] long Countdown)
        
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
           .WithContent("Let's Count down!"));

            
            while (Countdown > 0)
            {
                string CountdownMessage = Countdown.ToString();
                var CountingMessage = new DiscordMessageBuilder()

                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Violet)
                    .WithTitle(CountdownMessage)
                    .WithDescription("You have " + CountdownMessage + " seconds left!")

                    );

                await ctx.Channel.SendMessageAsync(CountingMessage);
                await Task.Delay(1000);
                Countdown -= 1;
                
            }
            await ctx.Channel.SendMessageAsync("Time is up! Time is UP!");


        }


        /*[SlashCommand("rockpaperscissors","A small game of rock paper scissors!")]
        public async Task RockPaperScissorsCommand(InteractionContext ctx, [Option("user","The user you want ")] DiscordUser user)
        {
            DiscordButtonComponent button1 = new DiscordButtonComponent(ButtonStyle.Primary, "scissors", "Scissors");
            DiscordButtonComponent button2 = new DiscordButtonComponent(ButtonStyle.Primary, "rock", "Rock");
            DiscordButtonComponent button3 = new DiscordButtonComponent(ButtonStyle.Primary, "paper", "Paper");

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Purple)
                .WithTitle("This is a message with buttons")
                .WithDescription("Please select a button")
                    )
                .AddComponents(button1)
                .AddComponents(button2)
                .AddComponents(button3);

            await ctx.Member.SendMessageAsync(message);

        }*/

    }
}
