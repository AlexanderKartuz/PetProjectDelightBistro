namespace FirstConsoleApp.BullsAndCowsGameBySleepaidyAndYato
{
    public class BotPlayer : Player
    {
        private BullsAndCowsGame _game { get; set; }
        private List<string> _uniqueDigitNumbers { get; set; }
        private string _lastGuess { get; set; }

        public BotPlayer()
        {
            GenerateUniqueDigitNumbers();
        }
        public void SetGame(BullsAndCowsGame game)
        {
            _game = game;
        }

        public override string MakeGuess()
        {
            var guessNumber = "";
            if (this._game.Round == 1)
            {
                guessNumber = "1234";
            }
            else if (this._game.Round == 2)
            {
                guessNumber = "5678";
            }
            else if (this._game.Round == 3 && _uniqueDigitNumbers[_uniqueDigitNumbers.Count - 1].Contains("9"))
            {
                    guessNumber = "9" + _uniqueDigitNumbers[0][0] + _uniqueDigitNumbers[0][1] + _uniqueDigitNumbers[0][2];
            }
            else
            {
                guessNumber = _uniqueDigitNumbers[0];
            }
            _lastGuess = guessNumber;
            return guessNumber;
        }
        public override void ProcessGuessResult(int bullsCount, int cowsCount)
        {
            FilterNumberCandidates(bullsCount, cowsCount);
        }

        private void FilterNumberCandidates(int bullsCount, int cowsCount)
        {
            int bullsCountNumberCandidates = 0;
            int cowsCountNumberCandidates = 0;
            for (int indexUniqueDigitNumbers = _uniqueDigitNumbers.Count - 1; indexUniqueDigitNumbers >= 0; indexUniqueDigitNumbers--)
            {
                this._game.SearchForBullsAndCows(_uniqueDigitNumbers[indexUniqueDigitNumbers], this._lastGuess, out bullsCountNumberCandidates, out cowsCountNumberCandidates);
                if (bullsCountNumberCandidates != bullsCount || cowsCountNumberCandidates != cowsCount)
                {
                    _uniqueDigitNumbers.RemoveAt(indexUniqueDigitNumbers);
                }
            }
        }
        private void GenerateUniqueDigitNumbers()
        {
            _uniqueDigitNumbers = new List<string>();
            for (int firstDigit = 0; firstDigit < 10; firstDigit++)
            {
                for (int secondDigit = 0; secondDigit < 10; secondDigit++)
                {
                    if (secondDigit == firstDigit)
                    {
                        continue;
                    }
                    for (int thirdDigit = 0; thirdDigit < 10; thirdDigit++)
                    {
                        if (thirdDigit == firstDigit || thirdDigit == secondDigit)
                        {
                            continue;
                        }
                        for (int fourthDigit = 0; fourthDigit < 10; fourthDigit++)
                        {
                            if (fourthDigit == firstDigit || fourthDigit == secondDigit || fourthDigit == thirdDigit)
                            {
                                continue;
                            }
                            this._uniqueDigitNumbers.Add(firstDigit.ToString() + secondDigit.ToString() +
                                thirdDigit.ToString() + fourthDigit.ToString());
                        }
                    }
                }
            }
        }
    }
}
