namespace Cameo
{
    public enum ESuits
    {
        spade = 1,
        diamond,
        clubs,
        heart,
        blank = 0
    }

    public enum EValues
    {
        ace = 1,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king,
        joker_1,
        joker_2,
        defaultNull = 0
    }

    public enum EPile
    {
        DiscardPile,
        DeckPile
    }

    public enum EPlay
    {
        DrawCard,
        DiscardCard,
        UsePower
    }

    public enum ERoundPhase
    {
        Start,
        Running,
        Cameo,
        End
    }

}