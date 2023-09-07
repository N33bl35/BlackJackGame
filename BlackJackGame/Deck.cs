//******************************************************************************
// DEFINING CLASSES
// When creating these classes and removing this block, the methods break

namespace BlackJackGame
{
    //Class
    class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue faceValue in Enum.GetValues(typeof(FaceValue)))
                {
                    Cards.Add(new Card { Suit = suit.ToString(), FaceValue = faceValue.ToString() });
                }
            }
        }

        //Method
        public void Shuffle(Random rng)
        {
            //Random rng = new Random();
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }
        //Method
        public Card DealCard()
        {
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }
    }
}