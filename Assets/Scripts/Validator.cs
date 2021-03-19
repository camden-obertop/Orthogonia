using System.Collections.Generic;

public static class Validator
{
    public static bool IsValid(VoxelManager.VoxelState[,,] solution, Clue[,] frontClues, Clue[,] sideClues, Clue[,] topClues)
    {
        int length = solution.GetLength(0);
        int height = solution.GetLength(1);
        int width = solution.GetLength(2);

        VoxelManager.VoxelState[,,] workingSolution = new VoxelManager.VoxelState[length, height, width];

        // for every front line
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < length; i++)
            {
            }
        }

        // for every side line
        for (int j = 0; j < height; j++)
        {
            for (int k = 0; k < width; k++)
            {
            }
        }

        // for every top line
        for (int k = 0; k < width; k++)
        {
            for (int i = 0; i < length; i++)
            {
            }
        }

        return false;
    }

    private static void LineEmptyCheck(ref VoxelManager.VoxelState[] line, Clue lineClue)
    {
        if (lineClue.VoxelCount == 0)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = VoxelManager.VoxelState.Cleared;
            }
        }
    }

    private static void LineFullCheck(ref VoxelManager.VoxelState[] line, Clue lineClue)
    {
        if (lineClue.VoxelCount == line.Length)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = VoxelManager.VoxelState.Marked;
            }
        }
        else if (lineClue.VoxelCount + lineClue.GapCount == line.Length && lineClue.GapCount == lineClue.VoxelCount - 1)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = voxelIndex % 2 == 0 ? VoxelManager.VoxelState.Marked : VoxelManager.VoxelState.Cleared;
            }
        }
    }

    private static (List<int>, List<int>) CalculateLeftRightPossibleSolutions(ref VoxelManager.VoxelState[] line, Clue lineClue)
    {
        List<int> leftPossibleSolutions = new List<int>();
        List<int> rightPossibleSolutions = new List<int>();



        return (leftPossibleSolutions, rightPossibleSolutions);
    }
}
