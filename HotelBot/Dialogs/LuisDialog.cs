using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace HotelBot
{
    // Get programmatic key from https://www.luis.ai/users/settings.
    [LuisModel("1b7dabfc-46c9-4ffd-a317-6c464626c1b6", "00000000000000000000000000000000")]
    [Serializable]
    internal sealed class LuisDialog : LuisDialog<RoomReservation>
    {
        private readonly BuildFormDelegate<RoomReservation> buildRoomReservationForm;

        public LuisDialog(BuildFormDelegate<RoomReservation> buildRoomReservationForm)
        {
            this.buildRoomReservationForm = buildRoomReservationForm;
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneAsync(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am sorry. I do not know what you mean.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Greet")]
        public Task GreetAsync(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingsDialog(), this.ResumeAfter);
            return Task.CompletedTask;
        }

        [LuisIntent("Request Reservation")]
        public Task RequestReservationAsync(IDialogContext context, LuisResult result)
        {
            FormDialog<RoomReservation> reservationForm =
                new FormDialog<RoomReservation>(new RoomReservation(), this.buildRoomReservationForm, FormOptions.PromptInStart);

            context.Call<RoomReservation>(reservationForm, this.ResumeAfter);
            return Task.CompletedTask;
        }

        [LuisIntent("Query Amenities")]
        public async Task QueryAmenitiesAsync(IDialogContext context, LuisResult result)
        {
            string amenityName = result.Entities.FirstOrDefault(e => e.Type == "Amenity")?.Entity?.ToLower() ?? "";

            if (amenityName == "pool" || amenityName == "gym" || amenityName == "wifi" || amenityName == "towels")
            {
                await context.PostAsync("Yes, we have that!");
            }
            else
            {
                await context.PostAsync("I am sorry, we do not have that.");
            }

            context.Wait(this.MessageReceived);
        }

        private Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(this.MessageReceived);
            return Task.CompletedTask;
        }
    }
}