using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;


namespace CustomProject.Commands
{
    public class FunCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommands(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Hello");
        }

        [Command("add")]
        public async Task Addition(CommandContext ctx, int number1, int number2)
        {
            int answer = number1 + number2;
            await ctx.Channel.SendMessageAsync(answer.ToString());
        }
    }
}
