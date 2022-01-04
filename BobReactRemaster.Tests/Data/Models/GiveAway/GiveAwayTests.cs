using System;
using System.Collections.Generic;
using System.Text;

namespace BobReactRemaster.Tests.Data.Models.GiveAway
{
    internal class GiveAwayTests
    {
        //Test
        //AddOrUpdateGift(Gift)
        //RemoveGift(Gift)
        //NextGift() - Randomly gets the next Gift in line for Raffling considering current "turn"
        //Increment "Turn" on all Gifts with the same Link (trim and clean for parameters)
        // Raffle() - Randomly choose a Winner from the List of Particpants excluding all but the participants with the least amount of won gifts
        // RaffleFFA() - Randomly choose a winner from the List of Particpants
        // RaffleWeightedFFA() - Randomly Choose a winner from the List of Participants Weighting people better with less Gifts won

        //Clear current Participants and or just use 1 List of Participants that gets cleared on raffle/NextGift

        //For Last because I need to figure out how Websockets work
        //GetParticipants() returns all current Participants with Info how many Gifts someone won already

    }
}
