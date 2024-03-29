﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprioriAlgo
{
    public class Set<T>
    {
        List<T> elements;
        List<Set<T>> subsets;

        // Constructors

        public Set()
        {
            elements = new List<T>();
        }

        public Set(int capacity)
        {
            elements = new List<T>(capacity);
        }
        
        public Set(Set<T> set)
        {
            elements = new List<T>();
            for (int i = 0; i < set.Count; i++)
            {
                elements.Add(set[i]);
            }
            elements.Sort();
        }

        public Set(T[] arr)
        {
            elements = new List<T>();
            for (int i = 0; i < arr.Length; i++)
            {
                elements.Add(arr[i]);
            }
            elements.Sort();
        }

        public Set(List<T> list)
        {
            elements = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                elements.Add(list[i]);
            }
            elements.Sort();
        }

        // Manipulators

        public bool Add(T item)
        {
            if (elements.Contains(item))
                return false;
            else
                elements.Add(item);
            
            elements.Sort();
            return true;
        }

        public int Add(Set<T> set)
        {
            int elementsAdded = 0;

            for (int i = 0; i < set.Count; i++)
            {
                if (!elements.Contains(set[i]))
                {
                    elements.Add(set[i]);
                    elementsAdded++;
                }
            }

            elements.Sort();
            return elementsAdded;
        }

        public bool Remove(T item)
        {
            if (elements.Remove(item))
                return true;
            else
                return false;
        }

        public void RemoveAt(int index)
        {
            elements.RemoveAt(index);
        }

        public void RemoveAll()
        {
            Clear();
        }

        public void Sort()
        {
            elements.Sort();
        }

        public void Clear()
        {
            elements.Clear();
        }

        public List<Set<T>> Subsets(int element_count = 0)
        {
            subsets = new List<Set<T>>();
            List<Set<T>> tempset = new List<Set<T>>();
            Set<T> temp;

            // generate all subsets on length 1
            for (int i = 0; i < elements.Count; i++)
            {
                temp = new Set<T>();
                temp.Add(elements[i]);
                tempset.Add(temp);
            }


            generateSubsets(tempset, 1, element_count <= 0 ? elements.Count : element_count);

            return subsets;
        }

        private void generateSubsets(List<Set<T>> _set, int count, int limit)
        {
            // Add the subsets generated by previous function to subsets list
            for (int i = 0; i < _set.Count; i++)
            {
                subsets.Add(_set[i]);
            }

            // Decide whether to make more subsets or not
            if (subsets.Count >= Convert.ToInt16(Math.Pow(2, elements.Count)) - 2 || count >= limit)
                return;

            List<Set<T>> tempset = new List<Set<T>>();
            Set<T> temp;

            for (int i = 0; i < _set.Count; i++)
            {
                for (int x = 0; x < elements.Count; x++)
                {
                    if (!_set[i].Contains(elements[x]))
                    {
                        temp = new Set<T>();
                        temp.Add(_set[i]);
                        temp.Add(elements[x]);
                        temp.Sort();

                        if (!tempset.Contains(temp) && !subsets.Contains(temp))
                        {
                            tempset.Add(temp);
                        }
                    }
                }
            }

            generateSubsets(tempset, ++count, limit);
        }

        // Utility

        public int Match(Set<T> itemSet)
        {
            int matches = 0;
            for (int i = 0; i < itemSet.Count; i++)
            {
                if (elements.Contains(itemSet[i]))
                    matches++;
            }

            return matches;
        }

        public bool Contains(Set<T> itemSet)
        {
            for (int i = 0; i < itemSet.Count; i++)
            {
                if (!elements.Contains(itemSet[i]))
                    return false;
            }

            return true;
        }

        public bool Contains(T item)
        {
            return elements.Contains(item);
        }

        public override bool Equals(object obj)
        {
            Set<T> itemSet = (Set<T>)obj;
            itemSet.Sort();

            if (itemSet.Count == elements.Count)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (!itemSet[i].Equals(elements[i]))
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public static Set<T> operator +(Set<T> s1, Set<T> s2)   //Union
        {
            Set<T> set = new Set<T>();

            for (int i = 0; i < s1.Count; i++)
            {
                set.Add(s1[i]);
            }

            for (int i = 0; i < s2.Count; i++)
            {
                if (!set.Contains(s2[i]))
                    set.Add(s2[i]);
            }

            set.Sort();

            return set;
        }

        public static Set<T> operator ^(Set<T> s1, Set<T> s2)   //Intersection
        {
            Set<T> set = new Set<T>();

            for (int i = 0; i < s1.Count; i++)
            {
                if (s2.Contains(s1[i]))
                    set.Add(s1[i]);
            }

            set.Sort();

            return set;
        }

        public static Set<T> operator -(Set<T> s1, Set<T> s2)   //Subtraction
        {
            Set<T> set = new Set<T>();

            for (int i = 0; i < s1.Count; i++)
            {
                if (!s2.Contains(s1[i]))
                    set.Add(s1[i]);
            }

            return set;
        }

        // Accessors

        public int Count
        {
            get { return elements.Count; }
        }

        public T this[int index]
        {
            get
            {
                return elements[index];
            }
            set
            {
                elements[index] = value;
            }
        }

        public List<T> toList()
        {
            List<T> arraySet = new List<T>(elements.Count);

            for (int i = 0; i < elements.Count; i++)
            {
                arraySet.Add(elements[i]);
            }

            return arraySet;
        }

        public T[] toArray()
        {
            T[] arraySet = new T[elements.Count];

            for (int i = 0; i < elements.Count; i++)
            {
                arraySet[i] = elements[i];
            }

            return arraySet;
        }

        public override string ToString()
        {
            string strSet = "";

            strSet = "{";

            for (int i = 0; i < elements.Count; i++)
            {
                strSet += elements[i].ToString();
                if (i != elements.Count - 1)
                    strSet += ",";
            }


            strSet += "}";

            return strSet;
        }
    }
}
