﻿using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Services.BruteForce
{
    public class DemandDistributions
    {
        public int DemandId { get; }
        private int _currentN;
        private int _currentK;
        public List<List<int>> Distributions { get; private set; }

        public DemandDistributions(int demandId)
        {
            this.DemandId = demandId;
            this.Distributions = new List<List<int>>();
        }

        private void Subsets(int n, int k, List<int> chosen)
        {
            if (k == 0)
            {
                PrepareResult(chosen);
                return;
            }
            if (n == 0)
            {
                return;
            }
            Subsets(n - 1, k, new List<int>(chosen));
            chosen.Insert(0, n);
            Subsets(n - 1, k - 1, new List<int>(chosen));
        }

        private void ReturnResult(List<int> data)
        {
            Distributions.Add(data);
        }

        private void PrepareResult(List<int> data)
        {
            data.Add(_currentN + _currentK);
            ReturnResult(new List<int>(data.Select(x => x - (data.IndexOf(x) > 0 ? data[data.IndexOf(x) - 1] : 0) - 1).ToList()));
            data.RemoveAt(data.Count - 1);
        }

        public void FindAllDistributions(int distributionVolume, int numberOfPaths)
        {
            List<int> chosen = new List<int>();
            _currentN = distributionVolume;
            _currentK = numberOfPaths;
            Subsets(distributionVolume + numberOfPaths - 1, numberOfPaths - 1, chosen);
        }
    }
}
