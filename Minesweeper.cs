using System;
using System.Collections.Generic;
using System.Linq;


namespace Minesweeper
{
    class Minesweeper
    {

        static string BOMB = "*";
        static List<Tuple<int, int>> bombs = new List<Tuple<int, int>>();

        static void Main()
        {
            Console.Write("Please enter the board size:");
            int boardsize = int.Parse(Console.ReadLine());
            string[,] matrix = new string[boardsize, boardsize];

            Console.WriteLine("Do you want to place bombs? Y/N?");
            ConsoleKeyInfo userInput = Console.ReadKey(true);
            bool bombing = true;

            if (userInput.Key == ConsoleKey.Y)
            {
                while (bombing)
                {

                    Console.Write("Cell value row, column:");

                    string bombsValue = Console.ReadLine();

                    int bombRow = 0;
                    int bombCol = 0;

                    ParseUserInput(bombsValue, out bombRow, out bombCol);

                    if (bombRow >= 0 && bombCol >= 0)
                        bombs.Add(new Tuple<int, int>(bombRow, bombCol));

                    Console.WriteLine("Press S to exit inserting bombs, otherwise any key to continue...");
                    userInput = Console.ReadKey();
                    
                    if (userInput.Key == ConsoleKey.S)                    {

                        bombing = false;
                    }

                    Console.WriteLine("Initial board:");
                }

            }

            if (bombs.Count == 0)
            {

                SetRandomBombs(boardsize, matrix);
                Console.WriteLine("Initial board, bombs are randomly placed:");
            }

            InitMatrix(matrix);
            PrintMatrix(matrix);
            CalculateMatrixValue(matrix);

            Console.WriteLine("Calculated board:");

            PrintMatrix(matrix);

            Console.WriteLine("Press CTRL+C to exit, otherwise press any key, to continue with value retrieve.");

            userInput = Console.ReadKey(true);
            while (userInput.KeyChar != 27)
            {

                Console.Write("Cell value to be retrieved row, column:");

                int row = -1;
                int col = -1;

                string cellValue = Console.ReadLine();
                ParseUserInput(cellValue, out row, out col);

                if (row >= 0 && col >= 0)
                    RetrieveCellValue(row, col, matrix);


            }
        }

        static void ParseUserInput(string userInput, out int row, out int col)
        {
            row = -1;
            col = -1;

            var input = userInput.Split(',');
            if (input.Length != 2)
            {
                Console.WriteLine("Wrong row,column value.");

            }
            else
            {

                if (!Int32.TryParse(input[0], out row))
                    Console.WriteLine("Wrong values for row. Row value must be int.");

                if (!Int32.TryParse(input[1], out col))
                    Console.WriteLine("Wrong values for column. Column value must be int.");

            }
        }

        /// <summary>
        /// Init matrix,board values. 
        /// </summary>
        /// <param name="matrix">Board, matrix to be initialised</param>
        static void InitMatrix(string[,] matrix)
        {
            
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (bombs.Count > 0)
                    {
                        matrix[row, col] = "0";

                        for (int b = 0; b < bombs.Count(); b++)
                        {
                            if (bombs[b].Item1 == row && bombs[b].Item2 == col)
                            {
                                matrix[row, col] = BOMB;
                            }

                        }
                    }
                    else//randomly bombed
                    {
                        if (matrix[row, col] != BOMB)
                            matrix[row, col] = "0";
                    }

                }
            }
        }

        /// <summary>
        /// Show matrix values
        /// </summary>
        /// <param name="matrix">Provided matrix</param>
        static void PrintMatrix(string[,] matrix)
        {
            Console.WriteLine();
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write("{0} ", matrix[row, col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Check if given cell is BOMB or not.
        /// </summary>
        /// <param name="row">Row value</param>
        /// <param name="col">Column value</param>
        /// <param name="matrix">board, matrix</param>
        /// <returns>int</returns>
        static int CheckBomb(int row, int col, string[,] matrix)
        {
            if (IsInRange(row, col, matrix) && matrix[row, col] == BOMB)
            {
                return 1;
            }
            else
                return 0;
        }

        /// <summary>
        /// Calulates bomb count arround each cell
        /// </summary>
        /// <param name="row">Row value</param>
        /// <param name="col">Column values</param>
        /// <param name="matrix">Matrix, board</param>
        /// <returns>Total bombs arround each cell.</returns>
        static int CalulateBombs(int row, int col, string[,] matrix)
        {
            var bombCount = 0;

            bombCount += CheckBomb(row, col + 1, matrix);
            bombCount += CheckBomb(row - 1, col + 1, matrix);
            bombCount += CheckBomb(row + 1, col + 1, matrix);
            bombCount += CheckBomb(row, col - 1, matrix);
            bombCount += CheckBomb(row - 1, col - 1, matrix);
            bombCount += CheckBomb(row + 1, col - 1, matrix);
            bombCount += CheckBomb(row - 1, col, matrix);
            bombCount += CheckBomb(row + 1, col, matrix);

            return bombCount;
        }
        /// <summary>
        /// Place values arround bombs.
        /// </summary>
        /// <param name="matrix">board, matrix</param>
        static void CalculateMatrixValue(string[,] matrix)
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] != BOMB)
                    {
                        var calulatedValue = CalulateBombs(row, col, matrix);
                        matrix[row, col] = calulatedValue.ToString();
                    }

                }
            }
        }

        /// <summary>
        /// Check if given row and column values is inside the board, matrix.
        /// </summary>
        /// <param name="row">Row value</param>
        /// <param name="col">Column value</param>
        /// <param name="matrix">Matrix, board to be checked.</param>
        /// <returns>If both row and column are in range.</returns>
        static bool IsInRange(int row, int col, string[,] matrix)
        {
            bool isRowInRange = row >= 0 && row < matrix.GetLength(0);
            bool isColInRange = col >= 0 && col < matrix.GetLength(1);

            return isRowInRange && isColInRange;
        }
        /// <summary>
        /// Check if row is in range.
        /// </summary>
        /// <param name="row">Row value.</param>
        /// <param name="matrix">Board to be checked.</param>
        /// <returns>If row is in range</returns>
        static bool IsRowInRange(int row, string[,] matrix)
        {
            bool isRowInRange = row >= 0 && row < matrix.GetLength(0);

            return isRowInRange;
        }
        /// <summary>
        /// Check if column is in range.
        /// </summary>
        /// <param name="col">Column to be cheked.</param>
        /// <param name="matrix">Board to be checked.</param>
        /// <returns>If column is in range.</returns>
        static bool IsColInRange(int col, string[,] matrix)
        {
            bool isColInRange = col >= 0 && col < matrix.GetLength(1);

            return isColInRange;
        }

        /// <summary>
        /// Retrieve value for a given cell.
        /// </summary>
        /// <param name="row">Row value</param>
        /// <param name="col">Column value for a cell.</param>
        /// <param name="matrix">Board to be checked.</param>
        static void RetrieveCellValue(int row, int col, string[,] matrix)
        {
            int rowCount = matrix.GetLength(0) - 1;
            int colCount = matrix.GetLength(1) - 1;

            if (!IsRowInRange(row, matrix))
            {
                Console.WriteLine("Row value {0} is out of range!", row);
                Console.WriteLine("Row value must be in a range [0,{0}] and column value must be in a range [0,{1}]", rowCount, colCount);

            }
            else if (!IsColInRange(col, matrix))
            {
                Console.WriteLine("Column value {0} is out of range!", col);
                Console.WriteLine("Row value must be in a range [0,{0}] and column value must be in a range [0,{1}]", rowCount, colCount);

            }
            else
            {
                Console.WriteLine("Value for the cell {0}, {1} is: {2}", row, col, matrix[row, col]);
            }
        }
        /// <summary>
        /// Place random bombs if they are not placed. Number of bombs = boardSize. Can be changed.
        /// </summary>
        /// <param name="boardSize">Boardsize value</param>
        /// <param name="matrix">Matrix, board where bombs can be placed.</param>
        static void SetRandomBombs(int boardSize, string[,] matrix)
        {
            int bombCount = 0;
            int maxBombCount = boardSize;

            Random randGenerator = new Random();

            while (bombCount < maxBombCount)
            {
                int row = randGenerator.Next(0, boardSize - 1);
                int col = randGenerator.Next(0, boardSize - 1);

                matrix[row, col] = BOMB;
                bombCount++;
            }
        }

    }
}
