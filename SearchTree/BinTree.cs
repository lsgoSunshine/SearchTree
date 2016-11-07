using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTree
{
    class BinTree<T>
    {
        private string orderString = string.Empty;
        private BinTreeNode<T> root;
        public BinTree(BinTreeNode<T> root)
        {
            this.root = root;
        }
        
        private void PreOrder(BinTreeNode<T> current)
        {
            if (current == null)
                return;
            orderString += current.Data.ToString()+" ";
            PreOrder(current.Lchild);
            PreOrder(current.Rchild);
        }
        public string PreOrderTraversal()
        {
            orderString = string.Empty;
            PreOrder(root);
            return orderString.Trim();
        }
        private void MidOrder(BinTreeNode<T> current)
        {
            if (current == null)
                return;            
            MidOrder(current.Lchild);
            orderString += current.Data.ToString()+" ";
            MidOrder(current.Rchild);
        }
        public string MidOrderTraversal()
        {
            orderString = string.Empty;
            MidOrder(root);
            return orderString.Trim();
        }
        private void PostOrder(BinTreeNode<T> current)
        {
            if (current == null)
                return;
            PostOrder(current.Lchild);
            orderString += current.Data.ToString() + " ";
            PostOrder(current.Rchild);
        }
        public string PostOrderTraversal()
        {
            orderString = string.Empty;
            PostOrder(root);
            return orderString.Trim();
        }
        public string LevelTraversal()
        {
            orderString = string.Empty;
            if (root != null)
            {
                Queue<BinTreeNode<T>> LQ = new Queue<BinTreeNode<T>>();
                LQ.Enqueue(root);
                while (LQ.Count == 0)
                {
                    BinTreeNode<T> temp = LQ.Dequeue();
                    orderString += temp.Data.ToString() +" " ;
                    if (temp.Lchild != null)
                        LQ.Enqueue(temp.Lchild);
                    if (temp.Rchild != null)
                        LQ.Enqueue(temp.Rchild);
                }
            }
            return orderString.Trim();
        }
        private BinTreeNode<T> FindParent(BinTreeNode<T> current, BinTreeNode<T> find)
        {
            if (find == null)
                throw new Exception("传入参数find为空");
            if (current == null)
                return null;
            if (current.Lchild != null && current.Lchild.Equals(find) == true)
                return current;
            if (current.Rchild != null && current.Rchild.Equals(find) == true)
                return current;
            BinTreeNode<T> temp = FindParent(current.Lchild, find);
            if (temp != null)
                return temp;
            else
                return FindParent(current.Rchild, find);
        }
        public BinTreeNode<T> GetParent(BinTreeNode<T> find)
        {
            if (find == null)
                throw new Exception("传入参数find为null");
            return FindParent(root, find);
        }
    }
}
