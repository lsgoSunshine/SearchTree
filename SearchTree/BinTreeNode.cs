using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTree
{
    class BinTreeNode<T>
    {
        private T data;
        private BinTreeNode<T> lchild;
        private BinTreeNode<T> rchild;
        private int bf;                     //平衡因子
        private BinTreeNode<T> parent;
        public BinTreeNode<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int Bf
        {
            get { return bf; }
            set { bf = value; }
        }

        public BinTreeNode<T> Lchild
        {
            get { return lchild; }
            set { lchild = value; } 
        }
        public BinTreeNode<T> Rchild
        {
            get { return rchild; }
            set { rchild = value; }
        }
        public T Data
        {
            get { return data; }
            set {
                    if (value == null)
                    {
                        throw new Exception("传入值为null");
                    }
                    else
                    {
                        data = value;
                    }
                }
        }
        public BinTreeNode(T key) {
            this.data = key;
            this.rchild = this.lchild = null;
            this.bf = 0;
        }
    }
}
