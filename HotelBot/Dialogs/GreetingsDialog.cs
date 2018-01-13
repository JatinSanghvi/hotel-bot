using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

using static HotelBot.DataBagKey;

namespace HotelBot
{
    [Serializable]
    internal sealed class GreetingsDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await RequestNameAsync(context);
            context.Wait(GreetAsync);
        }

        private static async Task RequestNameAsync(IDialogContext context)
        {
            string userName = DataBagHelper.GetValue<string>(context, UserName);

            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("Hi there, I am John Bot. Please specify you name.");
                DataBagHelper.SetValue(context, IsNameMessage, true);
            }
            else
            {
                await context.PostAsync($"Hi {userName}, I am John Bot.");
            }
        }

        private async Task GreetAsync(IDialogContext context, IAwaitable<IMessageActivity> awaitableMessageActivity)
        {
            IMessageActivity messageActivity = await awaitableMessageActivity;

            string userName = DataBagHelper.GetValue<string>(context, UserName);

            if (DataBagHelper.CheckAndResetValue(context, IsNameMessage))
            {
                userName = messageActivity.Text;
                DataBagHelper.SetValue(context, UserName, userName);
            }

            context.Done(messageActivity);
        }
    }
}