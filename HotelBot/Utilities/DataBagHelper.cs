using Microsoft.Bot.Builder.Dialogs;

namespace HotelBot
{
    internal static class DataBagHelper
    {
        public static bool CheckAndResetValue(IBotContext context, DataBagKey key)
        {
            if (GetValue<bool>(context, key))
            {
                context.UserData.SetValue(key.ToString(), false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static TValue GetValue<TValue>(IBotContext context, DataBagKey key)
        {
            TValue value;
            context.UserData.TryGetValue(key.ToString(), out value);
            return value;
        }

        public static void SetValue<TValue>(IBotContext context, DataBagKey key, TValue value)
        {
            context.UserData.SetValue<TValue>(key.ToString(), value);
        }
    }
}