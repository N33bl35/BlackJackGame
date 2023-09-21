//I think maybe this is going to work. "You’re making changes in a project you don’t have write access to. Submitting a change will write it to a new branch in your fork N33bl35/BlackJackGame, so you can send a pull request."
using BlackJackGame;

Deck deck = new();

deck.Shuffle(new Random());

List<BlackJackGame.Card> playerHand = new();
List<Card> dealerHand = new();

Console.WriteLine("Hello, what is your name?");
string playerName = Console.ReadLine();
Console.WriteLine("Welcome to Blackjack!");

// I would like to work on seeing the CORRECT formatting for this entire code. Want to see it so I can practice
// Not dealing with Hard or Soft 17, Splitting, int-Downs, etc.
//******************************************************************************
//******************************************************************************
//******************************************************************************

// NEEBS NEEBS NEEBS NEEBS

// Once you get to this screen, at the top right hand corner there should be a drop menu or a pencil to edit this file. 
// If you are able, select EDIT IN PLACE. This allows you to edit MY code right here on gitHub. I am able to commit changes, but I'm not sure what will happen if you do.

// Go ahead and add some notes in here when/if you can. Then I will look at how it actually effects my file.

//******************************************************************************
//******************************************************************************
//******************************************************************************

bool readyToPlay = PromptUserForDecision(playerName);

if (readyToPlay)
{
    Console.WriteLine("Let's play BlackJack!");
}
else
{
    Console.WriteLine("Maybe next time. Goodbye!");
    return;
}

//******************************************************************************
//  Display current local time and UTC to user.

        // Get the current local time
        DateTime localTime = DateTime.Now;

        // Get the current UTC time
        DateTime utcTime = DateTime.UtcNow;

        // Display the local time
        Console.WriteLine("Local Time: " + localTime.ToString("dd-MM-yyyy HH:mm:ss"));

        // Display the UTC time
        Console.WriteLine("UTC Time: " + utcTime.ToString("dd-MM-yyyy HH:mm:ss"));

        // Wait for user input before closing the console window
        Console.ReadLine();

//******************************************************************************

static bool PromptUserForDecision(string playerName)
{
    Console.WriteLine($"Are you ready to play?");
    Console.WriteLine("[Y] To Play.");
    Console.WriteLine("[N] To Exit.");


    while (true)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
        char keyChar = char.ToUpper(keyInfo.KeyChar);

        if (keyChar == 'Y')
        {
            Console.WriteLine();
            return true;

        }
        else if (keyChar == 'N')
        {
            Console.WriteLine();
            return false;

        }
        else
        {
            Console.WriteLine("\nInvalid input. Please press [Y] to play or [N] to exit.");
        }
    }
}
//******************************************************************************
// Round 1 Start, Buy-In

Console.WriteLine("Do you want to buy in at $10, $25, $50, or $100?");
int buyInValue;

while (true)
{
    if (!int.TryParse(Console.ReadLine(), out buyInValue) || (buyInValue != 10 && buyInValue != 25 && buyInValue != 50 && buyInValue != 100))
    {
        Console.WriteLine("Invalid response. Please enter [10], [25], [50], or [100].");
    }
    else
    {
        break;
    }
}

int roundOutcome = 0;
int bankRollAmount = buyInValue + -roundOutcome;

Console.WriteLine($"Great! You've bought in at {buyInValue}. Your Bankroll is ${bankRollAmount}.");
bool playAgain = true;

//******************************************************************************
//******************************************************************************
//******************************************************************************
// Any continuous round start

while (playAgain == true)
{
    deck.Shuffle(new Random());
    playerHand.Clear();
    dealerHand.Clear();

    // Deal 2 new cards to player and dealer
    playerHand.Add(deck.DealCard());
    dealerHand.Add(deck.DealCard());
    playerHand.Add(deck.DealCard());
    dealerHand.Add(deck.DealCard());

    if (bankRollAmount <= 0)
    {
        Console.WriteLine("Sorry! You have bust out. Better luck next time!");
        return;
    }

    int playerTotal = CalculateHandValue(playerHand);
    int dealerTotal = CalculateHandValue(dealerHand);

    // Introduce betting mechanic
    // No decimals

    int betValue;

    while (true)
    {
        Console.WriteLine("How much would you like to bet? Min ($1) Max ($10000)");
        if (!int.TryParse(Console.ReadLine(), out betValue) || betValue < 0 || betValue > 10000)
        {
            Console.WriteLine("Invalid bet amount. Please enter an amount between $00.01 and $10000.");
        }
        else
        {
            break;
        }
    }
    Console.WriteLine($"You're betting ${betValue}. Good Luck!");
    Console.WriteLine("");
    Console.WriteLine($"Player's hand: {DisplayHand(playerHand, true)} ({playerTotal})");
    Console.WriteLine($"Dealer's hand: {DisplayHand(dealerHand, true)} ({dealerTotal})");  //one facedown until all players stand dealerShownTotal

    //******************************************************************************
    //******************************************************************************
    //******************************************************************************
    // Actual Gameplay

    while (playerTotal < 21)
    {
        Console.WriteLine("Do you want to hit [H] or stand [S]?");
        char choice = char.ToUpper(Console.ReadKey().KeyChar);

        if (choice == 'H' && playerTotal < 21)
        {
            Card newCard = deck.DealCard();
            playerHand.Add(newCard);
            playerTotal = CalculateHandValue(playerHand);
            Console.WriteLine($"\nYou drew {DisplayCard(newCard)}");
            Console.WriteLine($"Player's hand: {DisplayHand(playerHand, true)} ({playerTotal})");

            if (dealerTotal < 17)
            {
                Card newCardDealer = deck.DealCard();
                dealerHand.Add(newCardDealer);
                dealerTotal = CalculateHandValue(dealerHand);
                Console.WriteLine($"\nDealer has to hit. Dealer draws {DisplayCard(newCardDealer)}");
                Console.WriteLine($"Dealer hand: {DisplayHand(dealerHand, true)} ({dealerTotal})");
                continue;
            }
            continue;
        }
        if (choice == 'S')
        {
            Console.WriteLine($"\nYou Stand. {DisplayHand(playerHand, true)} ({playerTotal})");

            while (dealerTotal <= 17) //"Hit on Soft 17, Stand on Hard 17" when implemented
            {
                Card newCard = deck.DealCard();
                dealerHand.Add(newCard);
                dealerTotal = CalculateHandValue(dealerHand);
                Console.WriteLine($"\nDealer has to hit. Dealer draws {DisplayCard(newCard)}");
                Console.WriteLine($"Dealer hand: {DisplayHand(dealerHand, true)} ({dealerTotal})");
            }
            break;
        }
        else if (choice != 'H' && choice != 'S')
        {
            Console.WriteLine("Invalid Response.\nPlease enter 'H' to Hit or 'S' to Stand.");
            continue;
        }
    }

    //******************************************************************************
    // Round over, determine winner
    // Retrieve cards first

    DetermineWinner(playerTotal, dealerTotal);

    string DisplayHand(List<Card> hand, bool revealAll = false)
    {
        List<string> cardStrings = new();

        for (int i = 0; i < hand.Count; i++)
        {
            if (i == 0 && !revealAll)
            {
                cardStrings.Add("Hidden");
            }
            else
            {
                cardStrings.Add(DisplayCard(hand[i]));
            }
        }

        return string.Join(", ", cardStrings);
    }

    // Calculate hand value

    int CalculateHandValue(List<Card> hand)
    {
        int total = 0;
        int numAces = 0;

        foreach (var card in hand)
        {
            if (Enum.TryParse(card.FaceValue, out FaceValue faceValue))
            {
                if (faceValue == FaceValue.Ace)
                {
                    numAces++;
                    total += 11; // Assume Ace is 11 initially
                }
                else if (faceValue >= FaceValue.Two && faceValue <= FaceValue.Ten)
                {
                    total += (int)faceValue;
                }
                else
                {
                    total += 10; // Jack, Queen, and King are all worth 10
                }
            }
        }

        // "Adjust the value of Aces if needed" - ChatGPT

        while (numAces > 0 && total > 21)
        {
            total -= 10; // Change the value of one Ace from 11 to 1
            numAces--;
        }

        return total;
    }

    string DisplayCard(BlackJackGame.Card card)
    {
        return $"{card.FaceValue} of {card.Suit}";
    }

    //******************************************************************************
    // Tell user outcome of round
    // I could probably make this a function to figure this out, adjust bankroll, etc. and use a switch statement?
    // ISSUE: Not handling rules for a tie when both have 21
    // with player and dealer classes, this function would be round outcome

    void DetermineWinner(int playerTotal, int dealerTotal)
    {
        if (playerTotal > 21)                                       //Player loses
        {
            Console.WriteLine("Player busts! Dealer wins!");
            bankRollAmount = bankRollAmount - betValue;
            Console.WriteLine($"- ${betValue}\nBankroll: ${bankRollAmount}");
        }
        else if (dealerTotal > 21)                                  //Player wins
        {
            Console.WriteLine($"Dealer busts! {playerName} wins!");
            bankRollAmount = bankRollAmount + (betValue * 2);
            Console.WriteLine($"+ ${betValue * 2}\nBankroll: ${bankRollAmount}");
        }
        else if (playerTotal == 21 && playerTotal != dealerTotal)    //Player wins
        {
            Console.WriteLine($"{playerName} has Blackjack!");
            bankRollAmount = bankRollAmount + (betValue * 2);
            Console.WriteLine($"+ ${betValue * 2}\nBankroll: ${bankRollAmount}");
        }
        else if (dealerTotal == 21)                                 //Player loses
        {
            Console.WriteLine("Dealer has Blackjack!");
            bankRollAmount = bankRollAmount - betValue;
            Console.WriteLine($"- ${betValue}\nBankroll: ${bankRollAmount}");
        }
        else if (playerTotal > dealerTotal)                         //Player wins
        {
            Console.WriteLine($"{playerName} wins!");
            bankRollAmount = bankRollAmount + (betValue * 2);
            Console.WriteLine($"+ ${betValue * 2}\nBankroll: ${bankRollAmount}");
        }
        else if (playerTotal < dealerTotal)                         //Player loses
        {
            Console.WriteLine("Dealer wins!");
            bankRollAmount = bankRollAmount - betValue;
            Console.WriteLine($"- ${betValue}\nBankroll: ${bankRollAmount}");
        }
        else if (playerTotal == dealerTotal)
        {
            Console.WriteLine("It's a tie!");                       // TIE
            Console.WriteLine($"All bets returned. BankRoll : {bankRollAmount}");
        }
    }

    //******************************************************************************
    // Can/Does user want to play another round?

    if (bankRollAmount > 0)
    {
        Console.WriteLine("Would you like to play another round? [Y] [N]");
        string anotherRound = Console.ReadLine();
        string anotherRoundUpper = anotherRound.ToUpper();

        bool playAnotherRound = anotherRoundUpper == "Y";

        // The bool isn't working properly here. We're not catching invalid responses, they are treated as false
        if (playAnotherRound == true)
        {
            playAgain = true;
            continue;
        }
        if (playAnotherRound != true && playAnotherRound)
        {
            Console.WriteLine("Invalid Response. Please enter [Y] to play another round or [N] to quit the game.");
        }
        if (playAnotherRound == false)
        {
            Console.WriteLine("Okay, see ya again next time!");
            break;
        }
        if (bankRollAmount <= 0)
        {
            Console.WriteLine($"Sorry, you have busted out of the game. You are at {bankRollAmount}. Better luck next time!");
            break;
        }
    }
}
