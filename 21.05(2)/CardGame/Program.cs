using System;
using System.Collections.Generic;

// Класс Карта
class Card
{
    public string Rank { get; private set; }
    public string Suit { get; private set; }

    public Card(string rank, string suit)
    {
        Rank = rank;
        Suit = suit;
    }

    public int GetValue()
    {
        switch (Rank)
        {
            case "6":
             return 6;
            case "7":
             return 7;
            case "8":
             return 8;
            case "9":
             return 9;
            case "10":
             return 10;
            case "Валет":
             return 11;
            case "Дама":
             return 12;
            case "Король":
             return 13;
            case "Туз":
             return 14;
            default:
            return 0;
        }
    }

    public bool IsHigherThan(Card other)
    {
        // Шестёрка не забирает туза
        if (this.Rank == "6" && other.Rank == "Туз")
            return false;
        if (other.Rank == "6" && this.Rank == "Туз")
            return true;
        
        return GetValue() > other.GetValue();
    }

    public override string ToString()
    {
        return $"{Rank}{Suit}";
    }
}

// Класс Игрок
class Player
{
    public string Name { get; private set; }
    private Queue<Card> _cards = new Queue<Card>();

    public Player(string name)
    {
        Name = name;
    }

    public int CardCount => _cards.Count;

    public void AddCard(Card card)
    {
        _cards.Enqueue(card);
    }

    public void AddCards(List<Card> cards)
    {
        foreach (Card c in cards)
            _cards.Enqueue(c);
    }

    public Card PlayCard()
    {
        return _cards.Dequeue();
    }

    public void ShowCards()
    {
        Console.Write($"{Name} ({CardCount} карт): ");
        foreach (Card c in _cards)
            Console.Write(c + " ");
        Console.WriteLine();
    }
}

// Класс Игра
class CardGame
{
    private List<Player> _players = new List<Player>();
    private List<Card> _deck = new List<Card>();

    public CardGame(string[] playerNames)
    {
        foreach (string name in playerNames)
            _players.Add(new Player(name));

        CreateDeck();
        Shuffle();
        Deal();
    }

    private void CreateDeck()
    {
        string[] ranks = { "6", "7", "8", "9", "10", "Валет", "Дама", "Король", "Туз" };
        string[] suits = { "♠", "♥", "♣", "♦" };

        foreach (string suit in suits)
            foreach (string rank in ranks)
                _deck.Add(new Card(rank, suit));
    }

    private void Shuffle()
    {
        Random rnd = new Random();
        for (int i = _deck.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            Card temp = _deck[i];
            _deck[i] = _deck[j];
            _deck[j] = temp;
        }
    }

    private void Deal()
    {
        int cardsPerPlayer = _deck.Count / _players.Count;
        int index = 0;

        foreach (Player p in _players)
        {
            for (int i = 0; i < cardsPerPlayer; i++)
                p.AddCard(_deck[index++]);
        }
    }

    public void Play()
    {
        int round = 1;
        int totalCards = _deck.Count;

        while (true)
        {
            // Проверяем победителя
            foreach (Player p in _players)
            {
                if (p.CardCount == totalCards)
                {
                    Console.WriteLine($"\n*** ПОБЕДИТЕЛЬ: {p.Name} ***");
                    return;
                }
            }

            Console.WriteLine($"\nРаунд {round++}");

            List<Card> playedCards = new List<Card>();
            List<Card> stake = new List<Card>();

            // Каждый игрок кладёт карту
            foreach (Player p in _players)
            {
                if (p.CardCount > 0)
                {
                    Card card = p.PlayCard();
                    playedCards.Add(card);
                    stake.Add(card);
                    Console.WriteLine($"{p.Name} кладёт {card}");
                }
            }

            // Определяем победителя раунда
            int winnerIndex = 0;
            for (int i = 1; i < playedCards.Count; i++)
            {
                if (playedCards[i].IsHigherThan(playedCards[winnerIndex]))
                    winnerIndex = i;
            }

            Console.WriteLine($"Забирает {_players[winnerIndex].Name}");
            _players[winnerIndex].AddCards(stake);
        }
    }
}

class Program
{
    static void Main()
    {
        string[] names = { "Игрок 1", "Игрок 2" };
        CardGame game = new CardGame(names);
        game.Play();
    }
}