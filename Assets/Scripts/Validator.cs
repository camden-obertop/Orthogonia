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

    private static void LineSolverDriver(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        // run all the methods and check if the line is complete in-between


    }

    #region Line solver methods
    private static void LineEmptyCheck(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.VoxelCount == 0)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = VoxelManager.VoxelState.Cleared;
            }

            lineClue.Complete = true;
        }
    }

    private static void LineFullCheck(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.VoxelCount == line.Length)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = VoxelManager.VoxelState.Marked;
            }

            lineClue.Complete = true;
        }
        else if (lineClue.VoxelCount + lineClue.GapCount == line.Length && lineClue.GapCount == lineClue.VoxelCount - 1)
        {
            for (int voxelIndex = 0; voxelIndex < line.Length; voxelIndex++)
            {
                line[voxelIndex] = voxelIndex % 2 == 0 ? VoxelManager.VoxelState.Marked : VoxelManager.VoxelState.Cleared;
            }

            lineClue.Complete = true;
        }
    }

    private static void DeduceOverlaps(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.GapCount == 0)
        {
            int rightOverlapIndex = lineClue.VoxelCount - 1;
            int leftOverlapIndex = line.Length - lineClue.VoxelCount;

            // Find right overlap index
            int counted = 0;
            for (int i = 0; i < line.Length; i++)
            {
                DeduceOverlapsHelper(ref line, ref lineClue, ref rightOverlapIndex, ref counted, i);
            }

            // Find left overlap index
            counted = 0;
            for (int i = line.Length; i >= 0; i--)
            {
                DeduceOverlapsHelper(ref line, ref lineClue, ref leftOverlapIndex, ref counted, i);
            }

            if (rightOverlapIndex >= leftOverlapIndex)
            {
                for (int i = leftOverlapIndex; i <= rightOverlapIndex; i++)
                {
                    line[i] = VoxelManager.VoxelState.Marked;
                }
            }
        }
    }

    private static void DeduceOverlapsHelper(ref VoxelManager.VoxelState[] line, ref Clue lineClue, ref int overlapIndex, ref int counted, int index)
    {
        if (line[index] == VoxelManager.VoxelState.Unmarked || line[index] == VoxelManager.VoxelState.Marked)
        {
            counted++;

            if (counted >= lineClue.VoxelCount)
            {
                overlapIndex = index;
                return;
            }
        }
        else if (line[index] == VoxelManager.VoxelState.Cleared)
        {
            counted = 0;
        }
    }

    private static void DeduceByTracing(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.GapCount == 0)
        {
            // Trace left side
            bool started = false;
            int counted = 0;
            for (int i = 0; i < line.Length; i++)
            {
                DeduceByTracingHelper(ref line, ref lineClue, ref started, ref counted, i);

                if (counted >= lineClue.VoxelCount)
                {
                    break;
                }
            }

            // Trace right side
            started = false;
            counted = 0;
            for (int i = line.Length; i >= 0; i--)
            {
                DeduceByTracingHelper(ref line, ref lineClue, ref started, ref counted, i);

                if (counted >= lineClue.VoxelCount)
                {
                    break;
                }
            }
        }
    }

    private static void DeduceByTracingHelper(ref VoxelManager.VoxelState[] line, ref Clue lineClue, ref bool started, ref int counted, int index)
    {
        if (line[index] == VoxelManager.VoxelState.Cleared)
        {
            started = false;
            counted = 0;
        }
        else if (line[index] == VoxelManager.VoxelState.Unmarked)
        {
            counted++;
            if (started && counted <= lineClue.VoxelCount)
            {
                line[index] = VoxelManager.VoxelState.Marked;
            }
        }
        else if (line[index] == VoxelManager.VoxelState.Marked)
        {
            started = true;
            counted++;
        }
    }

    private static void DeduceClears(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.GapCount == 0)
        {
            bool started = false;
            int counted = 0;
            for (int i = 0; i < line.Length; i++)
            {
                DeduceClearsHelper(ref line, ref lineClue, ref started, ref counted, i);   
            }

            started = false;
            counted = 0;
            for (int i = line.Length; i >= 0; i--)
            {
                DeduceClearsHelper(ref line, ref lineClue, ref started, ref counted, i);
            }
        }
    }

    private static void DeduceClearsHelper(ref VoxelManager.VoxelState[] line, ref Clue lineClue, ref bool started, ref int counted, int index)
    {
        if (line[index] == VoxelManager.VoxelState.Unmarked)
        {
            if (started)
            {
                counted++;
                if (counted > lineClue.VoxelCount)
                {
                    line[index] = VoxelManager.VoxelState.Cleared;
                }
            }
        }
        else if (line[index] == VoxelManager.VoxelState.Marked)
        {
            counted++;
            if (!started)
            {
                started = true;
            }
        }
    }

    private static void DeduceJoins(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.GapCount == 0)
        {
            int leftIndex = int.MaxValue;
            int rightIndex = int.MaxValue;
            bool leftFound = false;
            bool rightFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == VoxelManager.VoxelState.Marked)
                {
                    if (!leftFound)
                    {
                        leftFound = true;
                        leftIndex = i;
                    }
                    else if (leftFound && !rightFound)
                    {
                        rightFound = true;
                        rightIndex = i;
                    }
                }
            }

            if (leftFound && rightFound)
            {
                for (int i = leftIndex + 1; i < rightIndex; i++)
                {
                    line[i] = VoxelManager.VoxelState.Marked;
                }
            }
        }
    }

    private static void DeduceGapEdgeMarks(ref VoxelManager.VoxelState[] line, ref Clue lineClue)
    {
        if (lineClue.VoxelCount + lineClue.GapCount == line.Length && lineClue.VoxelCount - lineClue.GapCount > 1)
        {
            line[0] = VoxelManager.VoxelState.Marked;
            line[line.Length - 1] = VoxelManager.VoxelState.Marked;
        }
    }

    private static bool CheckLineCompletion(VoxelManager.VoxelState[] line, VoxelManager.VoxelState[] solutionLine, ref Clue lineClue)
    {
        if (!lineClue.Complete)
        {
            bool correct = true;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != solutionLine[i])
                {
                    correct = false;
                }
            }

            if (correct)
            {
                lineClue.Complete = true;
            }

            return correct;
        }

        return true;
    }
    #endregion
}
