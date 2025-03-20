using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wildfrost_Archipelago.Managers
{
    public class RewardManager
    {

        private void PrepItemPools()
        {
            throw new NotImplementedException();
        }
        private (string name, string desription, int locationId) GetCardData()
        {
            throw new NotImplementedException();
        }
        private void GetCardList(int count, bool isUnit)
        {
            throw new NotImplementedException();
        }
        private void UseUnlockCard()
        {
            throw new NotImplementedException();
        }
    }
}

//Item node opened

//Determine cards to show
//-Figure out ratio of unlocked items to remaining locked items
//--Get total unlocked items for each possible pool
//--Get total AP items for each possible pool
//--Check for all/most/no checks remaining
//--Add forced cards/checks
//--Add remaining cards based on unlock ratio

//Cards generated
//-Get card title, card description, and location ID
//-Build card

//Card selected
//-Remove card from deck
//-Update unlock locations
//-Send location ID to server