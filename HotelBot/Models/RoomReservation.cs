using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.FormFlow;

namespace HotelBot
{
    internal enum BedSizeOption
    {
        King,
        Queen,
        Single,
        Double
    };

    internal enum AmenitiesOption
    {
        Kitchen,
        ExtraTowels,
        GymAccess,
        Wifi
    };

    [Serializable]
    internal sealed class RoomReservation
    {
        public BedSizeOption? BedSize;
        public int? NumberOfOccupants;
        public DateTime? CheckInDate;
        public int? NumberOfDaysToStay;
        public List<AmenitiesOption> Amenities;

        public static IForm<RoomReservation> BuildForm()
        {
            return new FormBuilder<RoomReservation>()
                .Message("Please provide room reservation details.")
                .Build();
        }
    }
}