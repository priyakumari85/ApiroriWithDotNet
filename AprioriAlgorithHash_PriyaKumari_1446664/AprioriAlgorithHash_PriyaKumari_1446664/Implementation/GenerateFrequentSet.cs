using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AprioriAlgorithHash_PriyaKumari_1446664.Class;

namespace AprioriAlgorithHash_PriyaKumari_1446664.Implementation
{
    public class GenerateFrequentSet
    {
        // Returns unique items with minimum support count. Initial Pass
        public List<HashMap> InitialFrequentItemSetK1(List<HashMap> hashedItems,int support)
        {
            List<HashMap> frequentItemAtK1 =  hashedItems.Where(l=>l.Count>= support).ToList();
            return frequentItemAtK1;

        }

        // generates frequent item set for k=2. Logic for K= is different as it doesnt need subset to be checked
        public List<FrequentItemSet> GenerateFrequentItemSet(List<HashMap> hashitems, List<TransactionSet> hashedTransactionSet,int currentTupple, int support)
        {
            List<FrequentItemSet> fisATlevelk = new List<FrequentItemSet>();
            GenerateCandidateSet gcs = new GenerateCandidateSet();
            List<CandidateItemSet> frequentItemSetAtLevel = gcs.GenerateCandidate(hashitems,hashedTransactionSet,currentTupple,support);
            if (frequentItemSetAtLevel.Count>0)
            {
                foreach (var c in frequentItemSetAtLevel)
                {
                    FrequentItemSet fs = new FrequentItemSet();
                    fs.Values = new List<int>();
                    fs.Values.AddRange(c.Values);
                    fs.Count = c.Count;
                    fs.TransactionID = c.TransactionID;
                    fs.Values.Sort();
                    fisATlevelk.Add(fs);
                }
            }
            return fisATlevelk;

        }

        // generates frequent item set for k= 3+
        public List<FrequentItemSet> GenerateFrequentItemSet(List<FrequentItemSet> frequentItem, List<TransactionSet> hashedTransactionSet, int currentTupple, int support)
        {
            List<FrequentItemSet> frequentitemSet = new List<FrequentItemSet>();
            GenerateCandidateSet gcs = new GenerateCandidateSet();
            List<CandidateItemSet> candidateItemSetAtLevel = gcs.GenerateCandidate(frequentItem, hashedTransactionSet, currentTupple, support);
            if (candidateItemSetAtLevel.Count() > 0)
            {
                foreach (var c in candidateItemSetAtLevel)
                {
                    int count = GetSupportCount(c, hashedTransactionSet, currentTupple);
                    if (count >= support)
                    {
                        FrequentItemSet fs = new FrequentItemSet();
                        fs.Values = new List<int>();
                        fs.Values.AddRange(c.Values);
                        fs.Count = count;
                        fs.Values.Sort();
                        frequentitemSet.Add(fs);
                    }
                }
            }
       
            return frequentitemSet;
        }


        // get support count of each candidate item set at level K=3+ by traversing through whole transaction
        public int GetSupportCount(CandidateItemSet candidateSet, List<TransactionSet> hashedTransactionSet,int currentTupple)
        {
            var currentList = hashedTransactionSet.Where(h => h.Values.Count() >= currentTupple).ToList();
            int count = 0;
                foreach (var t in currentList)
                {
                    if (!candidateSet.Values.Except(t.Values).Any())
                    {
                    count++;
                    }
                }

            return count++;
        }
    }
}
