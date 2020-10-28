using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
        }



    }

    public class ListNode
    {
        public int Val;
        public ListNode Next;
        public ListNode(int x) { Val = x; }
    }

    public class Solution
    {
        public bool IsPalindrome(ListNode head)
        {
            if (head == null) return true;
            var nodeList = new List<int>();

            while (head != null)
            {
                nodeList.Add(head.Val);
                head = head.Next;
            }


            for (int i = 0, j = nodeList.Count - 1; i < j; i++, j--)
            {
                if (nodeList[i] != nodeList[j])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
