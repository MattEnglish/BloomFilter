using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace BloomFilter
{
    class Program
    {
        static void Main(string[] args)
        {


            var s = "hello";
            var bloomFilter = new BloomFilter();
            bloomFilter.AddWord("hello");

            var words = File.ReadAllLines("wordlist.txt");
            Stopwatch st = new Stopwatch();
            st.Start();
            foreach (var word in words)
            {
                bloomFilter.AddWord(word);
            }
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds);
            Console.WriteLine(words.Length);
            var r = new Random();
            int count = 0;
            /*
            for (int i = 0; i < 1000000; i++)
            {

                if (bloomFilter.WordAlreadyInFilter(r.Next().ToString()))
                {
                    count++;
                }

            }
            Console.WriteLine(count);
            */
            
        }






        
    }

    public class BloomFilter
    {
        public bool[] bloomArray = new bool[(int)Math.Pow(256, 3)];
        private MD5 md5 = MD5.Create();


        public bool WordAlreadyInFilter(string word)
        {
            var wordAsbytes = BitConverter.GetBytes(word.GetHashCode());
            var newHash = md5.ComputeHash(wordAsbytes);
            var newArray = GetArray(newHash);

            foreach (var i in newArray)
            {
                if (!bloomArray[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void AddWord(string word)
        {
            var wordAsbytes = BitConverter.GetBytes(word.GetHashCode());
            var newHash = md5.ComputeHash(wordAsbytes);
            var newArray = GetArray(newHash);

            foreach (var i in newArray)
            {
                bloomArray[i] = true;
            }
        }

        private int[] GetArray(Byte[] digitHash)
        {
            var newArray = new int[5];

            for (int i = 0; i < 5; i++)
            {
                newArray[i] = Combine(digitHash[i], digitHash[i + 5]);
                newArray[i] = Combine(digitHash[i+10], newArray[i]);
            }
            return newArray;
        }


        public int Combine(byte b1, byte b2)
        {
            int combined = b1 << 8 | b2;
            return combined;
        }

        public int Combine(byte b, int i)
        {
            int combined = i << 8 | b;
            return combined;
        }
    }
}
