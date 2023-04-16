using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProject.External_Classes
{
    internal class CardBuilder
    {
        public int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        public string[] cardSuits = { "Spades", "Clubs", "Diamonds", "Hearts" };

        public int SelectedNumber { get; internal set; }
        public string SelectedSuit { get; internal set; }
        public string SelectedCard { get; internal set; }

        public CardBuilder() 
        {
            var Random = new Random();
            int indexNumber = Random.Next(0, this.cardNumbers.Length - 1);

            var Random1 = new Random();
            int indexSuit = Random1.Next(0, this.cardSuits.Length - 1);

            this.SelectedNumber = this.cardNumbers.ElementAt(indexNumber);
            this.SelectedSuit = this.cardSuits.ElementAt(indexSuit);
            this.SelectedCard = this.cardNumbers.ElementAt(indexNumber) + " of " + this.cardSuits.ElementAt(indexSuit);
        }


    }
}
