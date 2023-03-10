(ushort x, ushort y)[] SnakeLength(string str, int length)
{
    str += ' ';
    int value = default;
    for (; value < str.Length && length >= 0; value++)
    {
        if (str[value] == ' ')
        {
            length--;
        }
    }

    str = str.Substring(0, value - 1);

    return str.Split(' ')
                .Select(symbol => symbol.Split(','))
                .Select(crd => (x: ushort.Parse(crd[0]), y: ushort.Parse(crd[1])))
                .ToArray();
}

void Print(char[][] plc, ushort lvl, ushort scr)
{
    Console.SetCursorPosition(0,0);
    Console.Write(plc[0]);
    Console.WriteLine($" Level: {lvl}");
    Console.Write(plc[1]);
    Console.WriteLine($" Score: {scr}");
    for (byte j = 2; j < plc.Length - 1; j++)
    {
        Console.WriteLine(plc[j]);
    }
    Console.WriteLine(plc[plc.Length - 1]);
}

char[] Framing(char symbol, byte length)
{
    char[] symbols = new char[length];
    symbols[0] = '*';
    for (byte j = 1; j < symbols.Length - 1; j++)
    {
        symbols[j] = symbol;
    }
    symbols[symbols.Length - 1] = '*';
    return symbols;
}

/////////////////////////////////////////////////////////////////////////////////////
Console.CursorVisible = false;
Console.Clear();

byte width = 20,
        height = 10;

ushort appleX = (ushort)new Random().Next(1, width - 2),
        appleY = (ushort)new Random().Next(1, height - 2),
        level = default,
        score = default,
        leng = default;
string snake = $"{new Random().Next(1, width - 2)}," +
                $"{new Random().Next(1, height - 2)}";
(ushort x, ushort y)[] coordinates;

ConsoleKeyInfo keyLast = new ConsoleKeyInfo(),
                key = new ConsoleKeyInfo();

char[][] field = new char[height][];

field[0] = Framing('*',width);
for (byte j = 1; j < field.Length - 1; j++)
{
    field[j] = Framing(' ',width);
}
field[field.Length - 1] = Framing('*',width);

/////////////////////////////////////////////////////////////////////////////////

while (keyLast.Key != ConsoleKey.Escape)
{
    coordinates = SnakeLength(snake, leng);

    for (int i = 0; i < coordinates.Length; i++)
    {
        field[coordinates[i].y][coordinates[i].x] = 'o';
    }
    field[appleY][appleX] = '$';

    Print(field, level, score);

    field[coordinates[coordinates.Length - 1].y]
         [coordinates[coordinates.Length - 1].x] = ' ';

    Thread.Sleep(500);
    while (Console.KeyAvailable)
        key = Console.ReadKey(true);

    switch (key.Key)
    {
        case ConsoleKey.W:
            if (keyLast.Key == ConsoleKey.S)
            {
                key = keyLast;
                goto case ConsoleKey.S;
            }
            coordinates[0].y--;
            break;

        case ConsoleKey.S:
            if (keyLast.Key == ConsoleKey.W)
            {
                key = keyLast;
                goto case ConsoleKey.W;
            }
            coordinates[0].y++;
            break;

        case ConsoleKey.A:
            if (keyLast.Key == ConsoleKey.D)
            {
                key = keyLast;
                goto case ConsoleKey.D;
            }
            coordinates[0].x--;
            break;

        case ConsoleKey.D:
            if (keyLast.Key == ConsoleKey.A)
            {
                key = keyLast;
                goto case ConsoleKey.A;
            }
            coordinates[0].x++;
            break;
    }
    keyLast = key;
    if (field[coordinates[0].y][coordinates[0].x] == '*' || field[coordinates[0].y][coordinates[0].x] == 'o')
    {

        field[4] = "********GAME********".ToCharArray();
        field[5] = "********************".ToCharArray();
        field[6] = "********OVER********".ToCharArray();
        Print(field, level, score);
        break;
    }

    snake = $"{coordinates[0].x},{coordinates[0].y} " + snake;

    if (coordinates[0].y == appleY && coordinates[0].x == appleX)
    {
        leng++;
        level++;
        appleX = (ushort)new Random().Next(1, field[0].Length - 2);
        appleY = (ushort)new Random().Next(1, field.Length - 2);
    }

    if (key.Key != 0) score++;
}
