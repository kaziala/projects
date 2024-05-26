using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HighlanderSimulator
{
    public sealed partial class MainPage : Page
    {
        private Random random;
        private int MAX_ITERATIONS = 15;
        private GameResultsRepository.GameResultRepository gameResultRepository;

        public MainPage()
        {
            this.InitializeComponent();
            this.random = new Random();
            this.gameResultRepository = new GameResultsRepository.GameResultRepository("Server=(local);Database=Group Project;User=GroupProject;Password=groupproject");
        }

        private async void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(SizeTextBox.Text);
            int goodCount = 0;
            int badCount = 0;

            if (UserProvidedRadioButton.IsChecked == true)
            {
                goodCount = int.Parse(GoodCountTextBox.Text);
                badCount = int.Parse(BadCountTextBox.Text);
            }
            else if (ProgramDeterminedRadioButton.IsChecked == true)
            {
                goodCount = size / 2;
                badCount = size / 2;
            }

            var grid = InitializeGrid(size, goodCount, badCount);
            DisplayGrid(grid);

            int iterations = 0;
            bool isOption1 = true;
            while (true)
            {
                iterations++;
                MoveHighlanders(grid);
                HandleInteractions(grid);
                DisplayGrid(grid);

                if (isOption1)
                {
                    if (CheckOption1Termination(grid))
                    {
                        var winner = grid.Cast<Highlander>().FirstOrDefault(h => h != null);
                        var victim = grid.Cast<Highlander>().FirstOrDefault(h => h != null && h != winner);
                        await DisplayOption1Outcome(grid, winner, victim, iterations);
                        break;
                    }
                }
                else
                {
                    if (CheckOption2Termination(iterations))
                    {
                        await DisplayOption2Outcome(grid, iterations);
                        break;
                    }
                }
            }
        }

        private Highlander[,] InitializeGrid(int size, int goodCount, int badCount)
        {
            Highlander[,] grid = new Highlander[size, size];

            for (int i = 0; i < goodCount; i++)
            {
                PlaceHighlander(grid, new GoodHighlander($"Good Highlander {i + 1}", random.Next(1, size + 1), random.Next(1, size + 1)));
            }

            for (int i = 0; i < badCount; i++)
            {
                PlaceHighlander(grid, new BadHighlander($"Bad Highlander {i + 1}", random.Next(1, size + 1), random.Next(1, size + 1)));
            }

            return grid;
        }

        private async void StartSimulation(int size, int goodCount, int badCount)
        {
            Highlander[,] grid = new Highlander[size, size];

            for (int i = 0; i < goodCount; i++)
            {
                PlaceHighlander(grid, new GoodHighlander($"Good Highlander {i + 1}", random.Next(1, size + 1), random.Next(1, size + 1)));
            }

            for (int i = 0; i < badCount; i++)
            {
                PlaceHighlander(grid, new BadHighlander($"Bad Highlander {i + 1}", random.Next(1, size + 1), random.Next(1, size + 1)));
            }

            DisplayGrid(grid);

            int iterations = 0;
            bool isOption1 = true;
            while (true)
            {
                iterations++;
                MoveHighlanders(grid);
                HandleInteractions(grid);
                DisplayGrid(grid);

                if (isOption1)
                {
                    if (CheckOption1Termination(grid))
                    {
                        var winner = grid.Cast<Highlander>().FirstOrDefault(h => h != null);
                        var victim = grid.Cast<Highlander>().FirstOrDefault(h => h != null && h != winner);
                        await DisplayOption1Outcome(grid, winner, victim, iterations);
                        if (gameResultRepository != null)
                        {
                            if (grid != null && grid[0, 0] != null && grid[1, 0] != null)
                            {
                                await gameResultRepository.InsertOption1Result(grid[0, 0].Name, grid[1, 0].Name, iterations, grid[1, 0].PowerLevel);
                                break;
                            }
                            else
                            {
                                await ShowMessage("Grid is null");
                                break;
                            }
                        }
                        else
                        {
                            await ShowMessage("gameResultRepo is null");
                            break;
                        }
                    }
                }
                else
                {
                    if (CheckOption2Termination(iterations))
                    {
                        await DisplayOption2Outcome(grid, iterations);
                        await gameResultRepository.InsertOption2Result(iterations);
                        break;
                    }
                }
            }
        }

        private void MoveHighlanders(Highlander[,] grid)
        {
            int size = grid.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Highlander highlander = grid[i, j];
                    if (highlander != null)
                    {
                        int direction = random.Next(8);
                        switch (direction)
                        {
                            case 0: // North 
                                if (i > 0) grid[i - 1, j] = highlander;
                                break;
                            case 1: // Northeast
                                if (i > 0 && j < size - 1) grid[i - 1, j + 1] = highlander;
                                break;
                            case 2: // East
                                if (j < size - 1) grid[i, j + 1] = highlander;
                                break;
                            case 3: // Southeast
                                if (i < size - 1 && j < size - 1) grid[i + 1, j + 1] = highlander;
                                break;
                            case 4: // South
                                if (i < size - 1) grid[i + 1, j] = highlander;
                                break;
                            case 5: // Southwest
                                if (i < size - 1 && j > 0) grid[i + 1, j - 1] = highlander;
                                break;
                            case 6: // West
                                if (j > 0) grid[i, j - 1] = highlander;
                                break;
                            case 7: // Northwest
                                if (i > 0 && j > 0) grid[i - 1, j - 1] = highlander;
                                break;
                        }

                        grid[i, j] = null;
                    }
                }
            }
        }

        private void HandleInteractions(Highlander[,] grid)
        {
            int size = grid.GetLength(0);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Highlander currentHighlander = grid[i, j];
                    if (currentHighlander != null)
                    {
                        for (int x = Math.Max(0, i - 1); x <= Math.Min(size - 1, i + 1); x++)
                        {
                            for (int y = Math.Max(0, j - 1); y <= Math.Min(size - 1, j + 1); y++)
                            {
                                Highlander neighborHighlander = grid[x, y];
                                if (neighborHighlander != null && neighborHighlander != currentHighlander)
                                {
                                    currentHighlander.Interact(neighborHighlander);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CheckOption1Termination(Highlander[,] grid)
        {
            int remainingCount = 0;
            foreach (Highlander highlander in grid)
            {
                if (highlander != null)
                {
                    remainingCount++;
                }
            }

            return remainingCount == 2;
        }

        private bool CheckOption2Termination(int iterations)
        {
            return iterations >= MAX_ITERATIONS;
        }

        private async Task InsertOption1ResultIntoDatabase(Highlander[,] grid, int iterations)
        {
            try
            {
                var winner = grid.Cast<Highlander>().FirstOrDefault(h => h != null);
                var victim = grid.Cast<Highlander>().FirstOrDefault(h => h != null && h != winner);
                if (winner != null)
                {
                    await gameResultRepository.InsertOption1Result(winner.Name, victim?.Name, iterations, victim?.PowerLevel ?? 0);
                }
            }
            catch (Exception ex)
            {
                await ShowMessage("Error inserting option 1 result: " + ex.Message);
            }
        }


        private async Task InsertOption2ResultIntoDatabase(int iterations)
        {
            try
            {
                await gameResultRepository.InsertOption2Result(iterations);
            }
            catch (Exception ex)
            {
                await ShowMessage("Error inserting option 2 result: " + ex.Message);
            }
        }


        private async Task ShowMessage(string message)
        {
            if (Window.Current.Content is Frame frame)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Simulation Parameters",
                    Content = message,
                    CloseButtonText = "OK"
                };

                await dialog.ShowAsync();
            }
        }

        private void PlaceHighlander(Highlander[,] grid, Highlander highlander)
        {
            int x, y;
            do
            {
                x = random.Next(grid.GetLength(0));
                y = random.Next(grid.GetLength(1));
            } while (grid[x, y] != null);

            grid[x, y] = highlander;
        }

        private void DisplayGrid(Highlander[,] grid)
        {
            SimulationGrid.Children.Clear();
            SimulationGrid.RowDefinitions.Clear();
            SimulationGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                SimulationGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                SimulationGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Text = grid[i, j] != null ? (grid[i, j].IsGood ? "G" : "B") : ".",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(grid[i, j] != null ? (grid[i, j].IsGood ? Colors.Green : Colors.Red) : Colors.Black)
                    };
                    Grid.SetRow(textBlock, i);
                    Grid.SetColumn(textBlock, j);
                    SimulationGrid.Children.Add(textBlock);
                }
            }
        }
        private async Task DisplayOption1Outcome(Highlander[,] grid, Highlander winner, Highlander victim, int iterations)
        {
            // Display outcome and insert into database
            await ShowMessage($"Option 1 outcome: Winner determined after {iterations} iterations.");

            // Insert the game result into the database
            using (var gameResultRepository = new GameResultsRepository.GameResultRepository("Server=(local);"))
            {
                // Insert game result into the database
                await gameResultRepository.InsertOption1Result(winner.Name, victim?.Name, iterations, victim?.PowerLevel ?? 0);
            }
        }

        private async Task DisplayOption2Outcome(Highlander[,] grid, int iterations)
        {
            // Display outcome and insert into database
            await ShowMessage("Option 2 outcome");

            // Insert the game result into the database
            using (var gameResultRepository = new GameResultsRepository.GameResultRepository("Server=(local);Database=Group Project;User=GroupProject;Password=groupproject"))
            {
                // Insert game result into the database
                await gameResultRepository.InsertOption2Result(iterations);
            }
        }

        private void UserProvidedRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
