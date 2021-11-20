namespace Cameo.NonMono
{
    [System.Serializable]
    public struct Card
    {
        // Constructor

        public Card(ESuits suit = ESuits.blank, EValues value = EValues.defaultNull)
        {
            this.Suit = suit;
            this.Value = value;

            if ((suit == ESuits.diamond || suit == ESuits.heart) && value == EValues.king)
                this.CardValue = 0;
            else if (value == EValues.joker_1 || value == EValues.joker_2)
                this.CardValue = -1;
            else
                this.CardValue = (int)value;
        }

        // Public fields

        public ESuits Suit;
        public EValues Value;
        public int CardValue;


        //Public methods

        public bool IsValid() => Value != EValues.defaultNull && Suit != ESuits.blank;
        public static bool operator ==(Card card_A, Card card_B)=> (card_A.Suit == card_B.Suit) && (card_A.Value == card_B.Value);
        public static bool operator !=(Card card_A, Card card_B) => (card_A.Suit != card_B.Suit) && (card_A.Value != card_B.Value);
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => $"{Suit.ToString()}-{Value.ToString()}";
    }
}