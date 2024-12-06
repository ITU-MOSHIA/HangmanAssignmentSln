using System;

namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private string _wordToGuess; // Word to guess
        private char[] _guessedWord; // Tracks guessed letters
        private int _incorrectGuesses = 0; // Number of incorrect guesses
        private const int MaxAttempts = 7; // Max wrong attempts before game ends
        private readonly string[] _wordList = { "MAUI", "DEVELOPER", "CORRECT", "GUESS", "PROGRAM", "HANGMAN" }; // Word list

        public HangmanGamePage()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            Random random = new Random();
            _wordToGuess = _wordList[random.Next(_wordList.Length)]; // Select a random word
            _guessedWord = new string('_', _wordToGuess.Length).ToCharArray(); // Initialize blanks
            _incorrectGuesses = 0;

            HangmanImage.Source = "hang1.png"; // Start with the first image
            UpdateWordDisplay();
            UpdateTriesLeft();
        }

        private void OnGuessButtonClicked(object sender, EventArgs e)
        {
            string guess = GuessEntry.Text?.ToUpper().Trim();

            if (string.IsNullOrEmpty(guess) || guess.Length != 1 || !char.IsLetter(guess[0]))
            {
                DisplayAlert("Invalid Input", "Please enter a valid single letter.", "OK");
                return;
            }

            char guessedChar = guess[0];

            if (_wordToGuess.Contains(guessedChar))
            {
                // Correct guess: update the guessed word
                for (int i = 0; i < _wordToGuess.Length; i++)
                {
                    if (_wordToGuess[i] == guessedChar)
                    {
                        _guessedWord[i] = guessedChar;
                    }
                }
            }
            else
            {
                // Incorrect guess: increment incorrect guesses
                _incorrectGuesses++;
                UpdateHangmanImage();
            }

            UpdateWordDisplay();
            UpdateTriesLeft();
            CheckGameOver();
            GuessEntry.Text = string.Empty; // Clear input
        }

        private void UpdateWordDisplay()
        {
            WordDisplay.Text = string.Join(" ", _guessedWord); // Display word with spaces
        }

        private void UpdateHangmanImage()
        {
            // Update the hangman image based on the number of incorrect guesses
            if (_incorrectGuesses <= MaxAttempts)
            {
                HangmanImage.Source = $"hang{_incorrectGuesses + 1}.png"; // Images: hang1.png to hang8.png
            }
        }

        private void UpdateTriesLeft()
        {
            TriesLeftLabel.Text = $"Tries Left: {MaxAttempts - _incorrectGuesses}";
        }

        private void CheckGameOver()
        {
            if (_incorrectGuesses >= MaxAttempts)
            {
                // Game Over: Player failed to guess the word
                HangmanImage.Source = "hang8.png"; // Show the final hang image
                DisplayAlert("Game Over", $"You died! The word was: {_wordToGuess}", "OK");
                StartNewGame();
            }
            else if (!_guessedWord.Contains('_'))
            {
                // Game Over: Player guessed the word successfully
                DisplayAlert("Congratulations!", $"You survived! The word was: {_wordToGuess}", "OK");
                StartNewGame();
            }
        }
    }
}
