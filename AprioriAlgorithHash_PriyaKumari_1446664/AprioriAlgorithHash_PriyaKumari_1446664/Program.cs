using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AprioriAlgorithHash_PriyaKumari_1446664.Implementation;
using AprioriAlgorithHash_PriyaKumari_1446664.Class;


namespace AprioriAlgorithHash_PriyaKumari_1446664
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your text file path");
            String a = Console.ReadLine();
            Console.WriteLine("Please enter support count");
            string b = Console.ReadLine();
            Console.WriteLine("Please enter tuple length");
            String tupp = Console.ReadLine();
            StreamReader input;
            if (File.Exists(a))
            {
                Console.WriteLine("Processing Started.....");
                TransactionSetGenerator ts = new TransactionSetGenerator();
                GenerateFrequentSet gfs = new GenerateFrequentSet();
                List<FrequentItemSet> fisATlevelk = new List<FrequentItemSet>();
                List<FrequentItemSet> finalFrequentSet = new List<FrequentItemSet>();
                int supportCount = Convert.ToInt32(b);
                int targetTuppleLenght = Convert.ToInt32(tupp);
                int currTuppleLenght = 1;
                bool terminate = false;

                // Read the input file
                input = File.OpenText(a);

                // break input file at each line end to identify the transactions
                List<string> source = input.ReadToEnd().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //This line generates string to interger hash. also gets the count of each individual item.
                //This represents itemSet 'I'
                List<HashMap> hashedUniqueItems = ts.GenerateHashMap(source);

                // This represents itemset 'T' all transactions
                List<TransactionSet> hashedTransactionSet = ts.GenerateHashedTransactionSet(hashedUniqueItems, source);

                //Initial pass. K=1 and gives items which has count> support count
                List<HashMap> frequentItemAtLevel1 = gfs.InitialFrequentItemSetK1(hashedUniqueItems, supportCount);

                // logic to loops till the candidate set is empty
                if (targetTuppleLenght == 1)
                {
                    if (frequentItemAtLevel1.Count() !=0)
                    {
                        List<FrequentItemSet> fisATlevel1 = new List<FrequentItemSet>();
                        foreach (var c in frequentItemAtLevel1)
                        {
                            FrequentItemSet fs = new FrequentItemSet();
                            fs.Values = new List<int>();
                            fs.Values.Add(c.Order);
                            fs.Count = c.Count;
                            fs.Values.Sort();
                            fisATlevel1.Add(fs);
                        }
                        //terminate = true;
                        finalFrequentSet.AddRange(fisATlevel1);
                    }


                }

                while (terminate != true)
                {
                    currTuppleLenght++;
                    if (currTuppleLenght == 2)
                    {
                        GenerateFrequentSet cg = new GenerateFrequentSet();
                        fisATlevelk = cg.GenerateFrequentItemSet(frequentItemAtLevel1, hashedTransactionSet, currTuppleLenght,supportCount);
                        if (fisATlevelk.Count == 0)
                        {
                            terminate = true;
                        }
                        if (currTuppleLenght >= targetTuppleLenght && fisATlevelk.Count != 0)
                        {
                            finalFrequentSet.AddRange(fisATlevelk);
                        }
                    }

                    else if (currTuppleLenght >= 2)
                    {
                        GenerateFrequentSet cg = new GenerateFrequentSet();
                        fisATlevelk = cg.GenerateFrequentItemSet(fisATlevelk, hashedTransactionSet, currTuppleLenght, supportCount);
                        if (fisATlevelk.Count == 0)
                        {
                            terminate = true;
                        }
                        if (currTuppleLenght >= targetTuppleLenght && fisATlevelk.Count != 0)
                        {
                            finalFrequentSet.AddRange(fisATlevelk);
                        }
                    }
                }

                // this section takes the final frequent set and writes it to a text file
                StringBuilder fileContents = new StringBuilder();
                foreach (var c in finalFrequentSet)
                {
                    StringBuilder final = new StringBuilder();
                    foreach (var str in c.Values)
                    {
                        string orderValue = hashedUniqueItems.Where(h => h.Order == str).Select(h => h.UniqueString).FirstOrDefault();
                        final.Append(orderValue).Append(" ");
                    }
                    final.Append("(").Append(c.Count).Append(")");
                    fileContents.AppendLine(final.ToString());
                }
                Console.WriteLine("Please enter your output text file path");
                String outputpath = Console.ReadLine();
                System.IO.StreamWriter file = new System.IO.StreamWriter(outputpath);
                file.WriteLine(fileContents);
                file.Dispose();

            }

        }
    }
}
