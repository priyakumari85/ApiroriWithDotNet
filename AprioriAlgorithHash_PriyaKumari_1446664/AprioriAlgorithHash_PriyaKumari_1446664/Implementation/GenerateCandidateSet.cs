using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AprioriAlgorithHash_PriyaKumari_1446664.Class;

namespace AprioriAlgorithHash_PriyaKumari_1446664.Implementation
{
    public class GenerateCandidateSet
    {
        // generates candidate item set for k=2. Logic for K=2 is different as it doesnt need subset to be checked
        public List<CandidateItemSet> GenerateCandidate(List<HashMap> hashitems, List<TransactionSet> hashedTransactionSet, int currentTupple,int support)
        {
            List<CandidateItemSet> candidateSet = new List<CandidateItemSet>();
            if (hashitems.Count() != 0)
            {
                    for (var i = 0; i <= hashitems.Count; i++)
                    {
                        for (var j = i + 1; j < hashitems.Count; j++)
                        {
                            List<int> currentTuppleList = new List<int>();
                            List<int> transactionList = new List<int>();
                            currentTuppleList.Add(hashitems[i].Order);
                            currentTuppleList.Add(hashitems[j].Order);
                            transactionList = (from list1 in hashitems[i].TransactionId
                                           join list2 in hashitems[j].TransactionId on list1 equals list2
                                           select list1).ToList();

                          if (transactionList.Count() >= support)
                            {
                                currentTuppleList.Sort();
                                CandidateItemSet x = new CandidateItemSet();
                                x.Values = new List<int>();
                                x.Values.AddRange(currentTuppleList);
                                x.Count = transactionList.Count();
                                x.TransactionID = transactionList;
                                candidateSet.Add(x);
                            }
                        }
                    }
            }
            return candidateSet;
        }

        // generates candidate item set for k= 3+
        public List<CandidateItemSet> GenerateCandidate(List<FrequentItemSet> frequentSet, List<TransactionSet> hashedTransactionSet, int currentTupple, int support)
        {
            List<CandidateItemSet> candidateSet = new List<CandidateItemSet>();
            if (frequentSet.Count() != 0)
            {
                    for (var i = 0; i <= frequentSet.Count; i++)
                    {
                        for (var j = i + 1; j < frequentSet.Count; j++)
                        {
                            int item1 = frequentSet[i].Values[currentTupple - 2];
                            int item2 = frequentSet[j].Values[currentTupple - 2];
                            frequentSet[i].Values.RemoveAt(currentTupple - 2);
                            frequentSet[j].Values.RemoveAt(currentTupple - 2);
                            bool contains = true;
                            for (int k = 0;k<currentTupple-2;k++) {
                            if (frequentSet[i].Values[k] != frequentSet[j].Values[k])
                            {
                                contains = false;
                            }
                            }
                            frequentSet[i].Values.Add(item1);
                            frequentSet[j].Values.Add(item2);
                            if (contains && item1<item2)
                            {
                                List<int> currentTuppleList = new List<int>();
                                currentTuppleList.AddRange(frequentSet[i].Values);
                                currentTuppleList.Add(item2);
                                currentTuppleList.Sort();
                                if (CreateSubset(currentTuppleList, frequentSet, currentTupple))
                                {
                                    currentTuppleList.Sort();
                                    CandidateItemSet x = new CandidateItemSet();
                                    x.Values = new List<int>();
                                    x.Values.AddRange(currentTuppleList);
                                    candidateSet.Add(x);
                                }
                            }
                       }
                 }

               }
            return candidateSet;
        }

        // get support count of each candidate item set at level K=2 by traversing through transaction list of each item
        //public int GetSupportCountFork2(List<int> tupple, List<TransactionSet> transaction,List<int> transactionList, int levelCount)
        //{
        //    int count = 0;
        //    var currentTran = transaction.Where(t => t.Values.Count() >= levelCount).ToList();
        //    foreach (var c in currentTran)
        //    {
        //        if(transactionList.Contains(c.TransactionID))
        //        {
        //            if (!tupple.Except(c.Values.ToList()).Any())
        //            {
        //                count++;
        //            }
        //        }
        //    }

        //    return count;
        //}

        // For k= 3+ ,function creates subsets and checks if those subsets are present in frequent item set at k-1 level
        // Pruning Step
        public bool CreateSubset(List<int> currentTuppleList, List<FrequentItemSet> frequentSet, int levelCount)
        {
            bool found = false;
            for(int i=0; i<=levelCount-3;i++)
            {
                int removedItem = currentTuppleList[i];
                currentTuppleList.RemoveAt(i);
                foreach(var c in frequentSet)
                {
                   if (!currentTuppleList.Except(c.Values).Any())
                        {
                            found = true;
                        }
                       
                }
                    currentTuppleList.Add(removedItem);

            }
            return found;
        }
    }
}
