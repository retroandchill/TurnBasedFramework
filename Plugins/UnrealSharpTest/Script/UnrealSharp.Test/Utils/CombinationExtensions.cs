namespace UnrealSharp.Test.Utils;

public static class CombinationExtensions
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        return sequences.Aggregate(Enumerable.Empty<IEnumerable<T>>(),
            (a, b) => a.SelectMany(_ => b, 
                (x, y) => x.Concat([y])));
    }

    public static IEnumerable<IEnumerable<T?>> SequentialGrouping<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        var enumerators = sequences.Select(x => x.GetEnumerator()).ToList();
        var currentValues = new T[enumerators.Count];
        var hasValue = new bool[enumerators.Count];
        
        try
        {
            while (true)
            {
                var anyMoved = false;
            
                for (var i = 0; i < enumerators.Count; i++)
                {
                    if (enumerators[i].MoveNext())
                    {
                        currentValues[i] = enumerators[i].Current;
                        hasValue[i] = true;
                        anyMoved = true;
                    }
                    else if (!hasValue[i])
                    {
                        currentValues[i] = default!;
                    }
                }

                if (!anyMoved)
                    break;

                yield return currentValues.ToArray();

            }
        }
        finally
        {
            foreach (var enumerator in enumerators)
            {
                enumerator.Dispose();
            }
        }
    }

    public static IEnumerable<IEnumerable<T>> PairwiseGrouping<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        var parameterValues = sequences.Select(seq => seq.ToArray()).ToArray();
        if (parameterValues.Length == 0) return [];
        if (parameterValues.Length == 1) return parameterValues[0].Select(v => new[] { v });

        var parameterSizes = parameterValues.Select(p => p.Length).ToArray();

        var testCases = new List<int[]>();

        var coveredPairs = new HashSet<(int, int, int, int)>();
        
        var firstCase = new int[parameterSizes.Length];
        testCases.Add(firstCase);

        var totalPairsNeeded = 0;
        for (var i = 0; i < parameterSizes.Length - 1; i++)
        {
            for (var j = i + 1; j < parameterSizes.Length; j++)
            {
                totalPairsNeeded += parameterSizes[i] * parameterSizes[j];
            }
        }

        while (coveredPairs.Count < totalPairsNeeded)
        {
            var bestCase = GenerateBestTestCase(parameterSizes, coveredPairs);
            if (bestCase == null) break;
        
            testCases.Add(bestCase);
        
            // Add all pairs from this test case to covered pairs
            for (var i = 0; i < bestCase.Length - 1; i++)
            {
                for (var j = i + 1; j < bestCase.Length; j++)
                {
                    coveredPairs.Add((i, bestCase[i], j, bestCase[j]));
                }
            }
        }
        
        return testCases.Select(testCase =>
            testCase.Select((value, paramIndex) => parameterValues[paramIndex][value]));
    }
    
    private static int[]? GenerateBestTestCase(int[] parameterSizes, HashSet<(int, int, int, int)> coveredPairs)
    {
        var bestCase = new int[parameterSizes.Length];
        var maxNewPairs = -1;
    
        // Try different combinations to find the one that covers the most new pairs
        for (var attempts = 0; attempts < 50; attempts++)
        {
            var candidate = new int[parameterSizes.Length];
            for (var i = 0; i < candidate.Length; i++)
            {
                candidate[i] = Random.Shared.Next(parameterSizes[i]);
            }

            var newPairs = CountNewPairs(candidate, coveredPairs);
            if (newPairs > maxNewPairs)
            {
                maxNewPairs = newPairs;
                Array.Copy(candidate, bestCase, candidate.Length);
            }
        }

        return maxNewPairs > 0 ? bestCase : null;
    }

    private static int CountNewPairs(int[] candidate, HashSet<(int, int, int, int)> coveredPairs)
    {
        var newPairs = 0;
        for (var i = 0; i < candidate.Length - 1; i++)
        {
            for (var j = i + 1; j < candidate.Length; j++)
            {
                if (!coveredPairs.Contains((i, candidate[i], j, candidate[j])))
                {
                    newPairs++;
                }
            }
        }
        return newPairs;
    }

}