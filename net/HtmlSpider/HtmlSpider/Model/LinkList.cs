using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSpider.Model
{
    public class MyLinkList<T>
    {
        public MyLinkListNode<T> FirstNode;

        public int Count
        {
            get
            {
                int count = 0;
                if (FirstNode != null)
                {
                    MyLinkListNode<T> curNode = FirstNode;
                    while (curNode != null)
                    {
                        count++;
                        curNode = curNode.Next;
                    }
                }
                return count;
            }
        }

        public void Add(T item)
        {
            MyLinkListNode<T> newNode = new MyLinkListNode<T>(item);
            if (FirstNode == null)
            {
                FirstNode = newNode;
            }
            else
            {
                var currNode = FirstNode;
                while (currNode.Next != null)
                {
                    currNode = currNode.Next;
                }
                currNode.Next = newNode;
                newNode.Pre = currNode;
            }
        }

        public void Remove(T item)
        {
            var node = GetNode(item);
            Remove(node);
        }

        public void Remove(MyLinkListNode<T> node)
        {
            if (node == FirstNode)
            {
                FirstNode = node.Next;
            }
            else
            {
                node.Pre.Next = node.Next;
                if (node.Next != null)
                    node.Next.Pre = node.Pre;
            }
        }

        public MyLinkListNode<T> GetNode(T item)
        {
            MyLinkListNode<T> curNode = FirstNode;
            while (curNode != null)
            {
                if (curNode.Data.Equals(item))
                    return curNode;
                else
                    curNode = curNode.Next;
            }
            return null;
        }

    }

    public class MyLinkListNode<T>
    {
        public T Data;
        public MyLinkListNode<T> Pre;
        public MyLinkListNode<T> Next;

        public MyLinkListNode(T item)
        {
            Data = item;
        }
    }
}
