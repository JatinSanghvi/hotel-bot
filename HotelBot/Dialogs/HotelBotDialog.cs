using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

using static HotelBot.DataBagKey;

namespace HotelBot
{
    internal static class HotelBotDialog
    {
        public static IDialog<string> Dialog()
        {
            IDialog<string> dialog = Chain.PostToChain()
                .Select(message => message.Text)
                .Switch(
                    new RegexCase<IDialog<string>>(new Regex(@"^hi(?:\s|$)", RegexOptions.IgnoreCase), GreetingSelector),
                    new DefaultCase<string, IDialog<string>>(RoomReservationSelector))
                .Unwrap()
                .PostToUser();

            return dialog;
        }


        private static readonly ContextualSelector<string, IDialog<string>> GreetingSelector =
            (context, text) => Chain.ContinueWith(new GreetingsDialog(), GreetingContinuation);

        private static readonly ContextualSelector<string, IDialog<string>> RoomReservationSelector =
            (context, text) => Chain.ContinueWith(
                   FormDialog.FromForm(RoomReservation.BuildForm, FormOptions.PromptInStart), RoomReservationContinuation);

        private async static Task<IDialog<string>> GreetingContinuation(IBotContext context, IAwaitable<object> item)
        {
            await item;

            return Chain.Return<string>("How can I help you?");
        }

        private async static Task<IDialog<string>> RoomReservationContinuation(IBotContext context, IAwaitable<object> item)
        {
            await item;

            string userName = DataBagHelper.GetValue<string>(context, UserName);

            if (string.IsNullOrEmpty(userName))
            {
                return Chain.Return<string>("Thanks. We have booked reservation for you.");
            }
            else
            {
                return Chain.Return<string>($"Thanks, {userName}. We have booked reservation for you.");
            }
        }
    }
}