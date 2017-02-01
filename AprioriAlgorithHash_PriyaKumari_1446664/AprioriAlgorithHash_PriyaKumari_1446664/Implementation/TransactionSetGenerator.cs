using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AprioriAlgorithHash_PriyaKumari_1446664.Implementation
{
    public class TransactionSetGenerator
    {
        List<TransactionSet> transactionSet = new List<TransactionSet>();
        List<HashMap> hashMapofUniqueItems = new List<HashMap>();

        // Gets all the unique items from transaction and does intial count
        // all the string items are given unique Integer value which helps in faster search
        public List<HashMap> GenerateHashMap(List<string> source)
        {
            if (source.Count() != 0)
            {
                int transactionId = 0;
                int order = 0;

                foreach (var s in source) {
                    transactionId++;
                    List<string> srcStringList = s.Split().ToList();
                    if (srcStringList.Count() != 0) {
                        foreach (string str in srcStringList)
                        {
                            if (hashMapofUniqueItems.Count == 0)
                            {
                                HashMap hsm = new HashMap();
                                hsm.UniqueString = str;
                                hsm.TransactionId = new List<int>();
                                hsm.TransactionId.Add(transactionId);
                                hsm.Order = order++;
                                hsm.Count = 1;
                                hashMapofUniqueItems.Add(hsm);

                            }
                            else if(hashMapofUniqueItems.Count > 0) {
                                var searchedItem = hashMapofUniqueItems.Where(h => h.UniqueString == str).FirstOrDefault();
                                if (searchedItem != null)
                                {
                                    searchedItem.Count = searchedItem.Count+1;
                                    var temp = searchedItem.TransactionId.Where(t => t == transactionId).FirstOrDefault();
                                    if (temp == 0)
                                    {
                                        searchedItem.TransactionId.Add(transactionId);
                                    }

                                }
                                else if (searchedItem == null) {
                                    HashMap hsm = new HashMap();
                                    hsm.UniqueString = str;
                                    hsm.TransactionId = new List<int>();
                                    hsm.TransactionId.Add(transactionId);
                                    hsm.Order = order++;
                                    hsm.Count = 1;
                                    hashMapofUniqueItems.Add(hsm);
                                }

                            }
                        }

                    }

                } 
            }
            return hashMapofUniqueItems;
        }

        // creates hashed transaction set. Each string in transaction is converted into the interger equivalent of it
        // hashmap created with unique set of items is used to transaction hashing
        public List<TransactionSet> GenerateHashedTransactionSet(List<HashMap> hashedItems, List<string> source)
        {
            if (source.Count() != 0 && hashedItems.Count() !=0)
            {
                int transactionId = 0;
                foreach (var s in source) {
                    transactionId++;
                    TransactionSet ts = new TransactionSet();
                    ts.Values = new List<int>();
                    List<string> srcStringList = s.Split().ToList();
                    if (srcStringList.Count() != 0) {
                        foreach(var str in srcStringList) {
                            var searchedItem = hashedItems.Where(h => h.UniqueString == str).FirstOrDefault();
                            ts.TransactionID = transactionId;
                            ts.Values.Add(searchedItem.Order);
                        }
                        ts.Values.Sort();
                        transactionSet.Add(ts);
                    }


                }

            }
            var x = transactionSet;
            return transactionSet;
        }

    }
}
