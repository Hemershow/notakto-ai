public struct Data 
{
    public ulong White { get; } = (1 << 27) + (1 << 36);
    public ulong Black { get; } = (1 << 28) + (1 << 35);

    public Data(ulong white, ulong black)
    {
        White = white;
        Black = black;
    }
}
public class Othello
{
    private const ulong u = 1;
    Data data = new Data();
    int whiteCount = 0;
    int blackCount = 0;
    public Othello(Data data, int white, int black)
    {
        this.data = data;
        whiteCount = white;
        blackCount = black;
    }
    public void Play(int board, int posit)
    {
        lastBoard = board;
        lastPosition = posit;

        var tableIndex = 9 * board;
        var sumsIndex = 8 * board;
        data[tableIndex + posit] = true;
        sums[sumsIndex + (posit / 3)]++;
        sums[sumsIndex + (posit % 3) + 3]++;
        if (posit % 4 == 0)
            sums[sumsIndex + 6]++;
        if (posit % 2 == 0 && posit > 0 && posit < 8)
            sums[sumsIndex + 7]++;
    }
    public bool CanPlay(int board)
    {
        int boardIndex = 8 * board;
        for (int i = 0; i < 8; i++)
        {
            if (sums[boardIndex + i] == 3)
                return false;
        }
        return true;
    }
    public bool GameEnded()
    {
        for (int i = 0; i < boards; i++)
        {
            if (CanPlay(i))
                return false;
        }
        return true;
    }
    public Notakto Clone()
    {
        Notakto copy = new Notakto(boards);
        Array.Copy(
            this.data, 
            copy.data, 
            this.data.Length
        );
        Array.Copy(
            this.sums, 
            copy.sums, 
            this.sums.Length
        );
        return copy;
    }
    public IEnumerable<Notakto> Next()
    {
        var clone = this.Clone();
        for (int b = 0; b < boards; b++)
        {
            if (!CanPlay(b))
                continue;
            
            for (int p = 0; p < 9; p++)
            {
                if (data[b * 9 + p])
                    continue;
                
                clone.Play(b, p);
                yield return clone;
                clone = this.Clone();
            }
        }
    }
}