// Author: Gabriel Ellebrink, gabell-2@student.ltu.se, L0002B, Uppgift 1 Console-variant

namespace SellerStatsConsole
{
  // Seller class to create sellers containing their information.
  class Seller : IComparable<Seller>
  {
    // Seller's name.
    private string name;
    // Seller's social security number.
    private int ssn;
    // Seller's district.
    private string district;
    // Seller's number of sold articles.
    private int numSold;
    // Seller's level (based on sold articles).
    // Level 0: <50
    // Level 1: 50-99
    // Level 2: 100-199
    // Level 3: >199
    private int level;

    public Seller (string name, int ssn, string district, int numSold)
    {
      this.name = name;
      this.ssn = ssn;
      this.district = district;
      this.numSold = numSold;
      setLevel(numSold);
    }

    // Sets the sellers level from sold count.
    public void setLevel(int numSold)
    {
      // Level 0.
      if (numSold < 50)
      {
        this.level = 0;
      }
      // Level 1.
      else if (numSold < 100)
      {
        this.level = 1;
      }
      // Level 2.
      else if (numSold < 200)
      {
        this.level = 2;
      }
      else
      {
        // Level 3.
        this.level = 3;
      }
    }

    // Gets the seller's level.
    public int getLevel()
    {
      return this.level;
    }

    // Compare two sellers for sorting.
    int IComparable<Seller>.CompareTo (Seller other)
    {
      return other.numSold - this.numSold;

      // alternative
      if (this.numSold > other.numSold)
      {
        return -1;
      }
      else if (this.numSold < other.numSold)
      {
        return 1;
      }
      return 0;
    }

    // Nicely displays a seller.
    public override string ToString()
    {
      string result = $"Name: \t\t{this.name}\n";
      result += $"SSN: \t\t{this.ssn}\n";
      result += $"District: \t{this.district}\n";
      result += $"Sold: \t\t{this.numSold}\n";
      return result;
    }
  }

  // Book of Seller objects with sort and print functionality.
  class SellerBook
  {
    // Collection of sellers.
    private Seller[] sellers;
    // Current number of sellers in the seller book.
    private int sellerCount = 0;
    // Array containing number of sellers at each level.
    // Position i=0 corresponds to level 0, i=1 to level 1, etc.
    private int[] levelCount = new int[] { 0, 0, 0, 0 };

    public SellerBook (int numSellers)
    {
      this.sellers = new Seller[numSellers];
    }

    // Adds a seller to the seller book.
    public void add(string name, int ssn, string district, int numSold)
    {
      // Create a seller object.
      Seller seller = new Seller(name, ssn, district, numSold);
      // Add seller to the collection of sellers.
      this.sellers[sellerCount] = seller;
      // Increment seller count.
      sellerCount++;
      // Increment level count.
      this.levelCount[seller.getLevel()]++;
    }

    // Sorts the seller book on number of sold articles.
    public void sort()
    {
      Array.Sort(this.sellers);
    }

    // Nicely displays a seller book divided in levels.
    public override string ToString()
    {
      string result = "";
      Seller[] foundSellers;

      // Iterate over each level.
      for (int i = 0; i < 4; i++)
      {
        // Find sellers at current level.
        foundSellers = Array.FindAll(this.sellers, e => e.getLevel() == i);

        // Print level info.
        if (foundSellers.Length > 0)
        {
          result += $"-- Level {i} --";
        }

        // Iterate over found sellers.
        foreach (Seller seller in foundSellers)
        {
          // Print seller info.
          result += "\n";
          result += seller.ToString();
        }

        // Print level info.
        if (foundSellers.Length > 0)
        {
          result += $"{this.levelCount[i]} sellers have reached level {i}.\n\n";
        }
      }
      return result;
    }
  }

  class Program
  {
    static async Task Main(string[] args)
    {
      bool quit = false;

      // Print welcome info.
      Console.WriteLine("----------------------------------\n");
      Console.WriteLine("--- Welcome to Seller Stats! -----\n");
      Console.WriteLine("--- Made by: Gabriel Ellebrink ---\n");
      Console.WriteLine("----------------------------------\n");

      while (!quit)
      {
        string numSellersInput;
        int numSellers;
        string name;
        string ssnInput;
        int ssn;
        string district;
        string numSoldInput;
        int numSold;

        // Ask how many sellers to add.
        Console.Write("How many sellers do you want to enter? ");
        numSellersInput = Console.ReadLine();

        // Convert input to an int, or ask again if invalid.
        while (!int.TryParse(numSellersInput, out numSellers) || numSellers <= 0)
        {
          Console.Write("Invalid input. Please enter a positive nonzero number: ");
          numSellersInput = Console.ReadLine();
        }

        // Create seller book.
        SellerBook sellerBook = new SellerBook(numSellers);

        // Loop numSellers times.
        for (int i = 0; i < numSellers; i++)
        {
          Console.Write($"- Seller {i+1} -\n");

          // Get seller name.
          Console.Write("Name: ");
          name = Console.ReadLine();

          // Get SSN.
          Console.Write("Social security number: ");
          ssnInput = Console.ReadLine();

          // Convert input to an int, or ask again if invalid.
          while (!int.TryParse(ssnInput, out ssn))
          {
            Console.Write("Invalid input. Please enter a number: ");
            ssnInput = Console.ReadLine();
          }

          // Get district.
          Console.Write("District: ");
          district = Console.ReadLine();

          // Get number of articles sold.
          Console.Write("Number of sold articles: ");
          numSoldInput = Console.ReadLine();

          // Convert input to an int, or ask again if invalid.
          while (!int.TryParse(numSoldInput, out numSold))
          {
            Console.Write("Invalid input. Please enter a number: ");
            numSoldInput = Console.ReadLine();
          }

          // Add seller.
          sellerBook.add(name, ssn, district, numSold);
        }

        Console.Write("\n");

        // Sort sellers.
        sellerBook.sort();

        // Write stats to file.
        await File.WriteAllTextAsync("result.txt", sellerBook.ToString());

        // Print stats to console.
        Console.Write(sellerBook);

        // Ask whether to quit or run again.
        Console.Write("Write 'q' to quit, or any other key to restart: ");
        if (Console.ReadLine() == "q")
        {
          quit = true;
        }
        Console.WriteLine("\n");
      }
      return;
    }
  }
}