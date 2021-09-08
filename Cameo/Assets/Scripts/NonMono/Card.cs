using Cameo.Mono;
using Cameo.Utils;

namespace Cameo.NonMono
{
    public class Card : I_Power
    {
        public e_suits suit;
        public e_values value;
        public int actualValue = 0;

        /// <summary>
        /// Creates a card object
        /// </summary>
        /// <param name="suit">Target suit</param>
        /// <param name="value">Target value of the card</param>
        public Card(e_suits suit, e_values value)
        {
            this.suit = suit;
            this.value = value;
            if (value != e_values.king)
                actualValue = (int)value;
            else if (suit != e_suits.diamond || suit != e_suits.heart)
                actualValue = (int)value;
        }

        /// <summary>
        /// Interface implementation; Call this method to check if the car has power or not
        /// </summary>
        /// <returns></returns>
        public bool hasPower()
        {
            bool flag = false;
            switch (value)
            {
                case e_values.king:
                    switch (suit)
                    {
                        case e_suits.clubs:
                        case e_suits.spade:
                            flag = true;
                            break;
                    }
                    break;
                case e_values.seven:
                case e_values.eight:
                case e_values.nine:
                case e_values.ten:
                case e_values.jack:
                case e_values.queen:
                    flag = true;
                    break;
            }
            return flag;
        }

        //TODO: usePower not implemented

        /// <summary>
        /// Call this method if the function hasPower() returns true
        /// </summary>
        /// <param name="player">Target player using the power card</param>
        public void usePower(Player player)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Overloaded operator ==
        /// </summary>
        public static bool operator ==(Card card_1, Card card_2)
        {
            bool flag = false;
            flag = card_1.suit == card_2.suit;
            flag = card_1.value == card_2.value;
            return flag;
        }

        /// <summary>
        /// Overloaded operator !=
        /// </summary>
        public static bool operator !=(Card card_1, Card card_2)
        {
            bool flag = false;
            flag = card_1.suit != card_2.suit;
            flag = card_1.value != card_2.value;
            return flag;
        }
    }
}