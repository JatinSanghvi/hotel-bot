using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HotelBot
{
    [Serializable]
    internal sealed class GreetingsDialog : IDialog
    {
        private const string IsFirstMessageKey = "IsFirstMessage";
        private const string IsNameMessageKey = "IsNameMessage";
        private const string UserNameKey = "UserName";

        public Task StartAsync(IDialogContext context)
        {
            context.UserData.SetValue(IsFirstMessageKey, true);
            context.Wait(MessageReceivedAsync);
            return Task.FromResult(0);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> awaitableMessageActivity)
        {
            IMessageActivity messageActivity = await awaitableMessageActivity;

            string userName = GetValue<string>(context, UserNameKey);

            if (CheckAndResetValue(context, IsFirstMessageKey))
            {
                context.UserData.SetValue(IsFirstMessageKey, false);

                if (string.IsNullOrEmpty(userName))
                {
                    await context.PostAsync("Hi there, I am John Bot.");
                    await context.PostAsync("Please specify you name.");
                    context.UserData.SetValue(IsNameMessageKey, true);
                }
                else
                {
                    await context.PostAsync($"Hi {userName}, I am John Bot. How can I help you?");
                }
            }
            else if (CheckAndResetValue(context, IsNameMessageKey))
            {
                userName = messageActivity.Text;
                context.UserData.SetValue(UserNameKey, userName);

                await context.PostAsync($"Hi {userName}, how can I help you?");
            }
            else
            {
                await context.PostAsync("Sorry, I am yet to be implemented.");
            }

            context.Wait(MessageReceivedAsync);
        }

        private bool CheckAndResetValue(IDialogContext context, string key)
        {
            if (GetValue<bool>(context, key))
            {
                context.UserData.SetValue(key, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static TValue GetValue<TValue>(IDialogContext context, string key)
        {
            TValue value;
            context.UserData.TryGetValue(key, out value);
            return value;
        }
    }
}