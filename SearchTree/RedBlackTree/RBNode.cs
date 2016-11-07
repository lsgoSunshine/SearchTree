using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTree.RedBlackTree
{
    class RBNode<T> where T : IComparable
    {
       //true red
       //false black
        public bool Color
        {
            get;
            set;
        }
        public T Data
        {
            get;
            set;
        }
        public RBNode<T> Rchild
        {
            get;
            set;
        }
        public RBNode<T> Lchild
        {
            get;
            set;
        }
        public RBNode<T> Parent
        {
            get;
            set;
        }
        public RBNode(T key)
        {
            Data = key;
            Color = true;
            Lchild = Rchild = Parent = null;
        }
    }
}
