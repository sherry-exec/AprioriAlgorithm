using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriAlgo
{
    struct Support<T>
    {
        public Support(Set<T> s, int _sup)
        {
            set = new Set<T>(s);
            support = _sup;
        }

        public Set<T> set;
        public int support;
    }

    class Program
    {
        static Set<int>Items;       // Contains all the items
        static List<Set<int>> Trans = new List<Set<int>>();     // Contains all the transaction
        static List<Support<int>> Subsets = new List<Support<int>>();      // All the proper subsets of Items and their support(frequency)

        static List<Support<int>> subsetsOfLength(int length)
        {
            List<Support<int>> subs = new List<Support<int>>();

            for (int i = 0; i < Subsets.Count; i++)
            {
                if (Subsets[i].set.Count == length)
                    subs.Add(Subsets[i]);
            }

            return subs;
        }

        static List<Support<int>> subsetsOfLength(List<Support<int>> s, int length)
        {
            List<Support<int>> subs = new List<Support<int>>();

            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].set.Count == length)
                    subs.Add(s[i]);
            }

            return subs;
        }

        static void Init(int min_support)
        {
            int[] t1 = { 1, 3, 4 };
            int[] t2 = { 2, 3, 5 };
            int[] t3 = { 1, 2, 3, 5 };
            int[] t4 = { 2, 5 };
            int[] i = { 1, 2, 3, 4, 5 };

            Items = new Set<int>(i);
            Trans.Add(new Set<int>(t1));
            Trans.Add(new Set<int>(t2));
            Trans.Add(new Set<int>(t3));
            Trans.Add(new Set<int>(t4));

            calcSubsetsAndSupport(min_support);
        }

        static void calcSubsetsAndSupport(int min_support)
        {
            List<Set<int>> s = Items.Subsets();

            int supportCount = 0;
            for (int i = 0; i < s.Count; i++)
            {
                for (int j = 0; j < Trans.Count; j++)
                {
                    if(Trans[j].Contains(s[i]))
                    {
                        supportCount++;
                    }
                }

                if (supportCount >= min_support)
                    Subsets.Add(new Support<int>(s[i], supportCount));
                supportCount = 0;
            }
            return;
        }

        // Pruning
        static List<Support<int>> Pruning(List<Support<int>> clist_sets, List<Support<int>> flist_sets, int len)
        {
            List<Support<int>> sets = new List<Support<int>>();
            bool freqSetFound = true;

            for (int i = 0; i < clist_sets.Count; i++)
            {
                freqSetFound = false;
                List<Set<int>> freqSubsets = clist_sets[i].set.Subsets(len);

                for (int x = 0; x < freqSubsets.Count; x++)
                {
                    if(freqSubsets[x].Count == len)
                    {
                        for (int y = 0; y < flist_sets.Count; y++)
                        {
                            if(freqSubsets[x].Equals(flist_sets[y].set))
                            {
                                freqSetFound = true;
                                break;
                            }
                        }
                    }

                    if(!freqSetFound)
                        break;
                }

                if(freqSetFound)
                {
                    sets.Add(clist_sets[i]);
                }
            }

            return sets;
        }

        static void Main(string[] args)
        {
            List<List<Support<int>>> CList = new List<List<Support<int>>>();
            List<List<Support<int>>> FList = new List<List<Support<int>>>();
            int k = 0,          // no. of iteration, also corresponds to no. of items in sets in a candidate list of kth iteration
                min_sup = 2;

            Init(min_sup);

            do
            {
                // Developing Candidate List
                List<Support<int>> tempFreqList = subsetsOfLength(k+1);       //Frequent sets of length k and their supports
                List<Support<int>> tempCList = new List<Support<int>>();    //CList for kth iteration
                for (int i = 0; i < tempFreqList.Count; i++)
                {
                    Support<int> freqSet = new Support<int>(tempFreqList[i].set, tempFreqList[i].support);
                    tempCList.Add(freqSet);
                }
                CList.Add(tempCList);   //Candidate List developed for kth itertion

                // Pruning
                // keep those sets from CList's kth iteration list whose all subsets are in previous FList i.e have support >= sup_count
                if (k > 1)
                    CList[k] = Pruning(CList[k], FList[k - 1], k + 1);    //Prune the current kth CList list

                // Developing Frequent List
                List<Support<int>> tempFList = new List<Support<int>>();    //FList for kth iteration
                for (int i = 0; i < CList[k].Count; i++)      // extracting sets from kth list whose support >= min_sup
                {
                    if(CList[k][i].support >= min_sup)
                    {
                        tempFList.Add(CList[k][i]);
                    }
                }
                FList.Add(tempFList);   //Frequent List developed for kth itertion

                k++;

            } while (CList[k-1].Count > 0 && FList[k-1].Count > 0);

            for (int i = 0; i < FList[k-1].Count; i++)
            {
                Console.WriteLine(FList[k-1][i].ToString());
            }

            Console.ReadKey();
        }
    }
}
