using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SearchTree
{
    class AVLTree
    {
        private BinTreeNode<int> root = default(BinTreeNode<int>);
        private string orderString = "";
        public int Length { get; private set; }
        private int GetValue(BinTreeNode<int> current , int key)
        {
            if (current == null) throw new Exception("节点为空");
            if (current.Data > key) { return GetValue(current.Lchild, key); }
            if (current.Data < key) { return GetValue(current.Rchild, key); }
            return current.Data;
        }
        private Pen blackPen = new Pen(Color.Black, 1);
        private Font drawFont = new Font("Arial", 8);//显示的字符串使用的字体
        private SolidBrush drawBrush = new SolidBrush(Color.Black);//写字符串用的刷子
        public void Insert(int key)
        {
            //if (key == null) throw new ArgumentException(nameof(key));
            if (root == null)
            {
                root = new BinTreeNode<int>(key);
                return;
            }
            BinTreeNode<int> current = new BinTreeNode<int>(key);
            FindPosition(root, current);
            BinTreeNode<int> parent = current.Parent;
            if (parent != null)
            {
                Length++;
                //如果新插入的节点是做孩子 父亲节点平衡因子+1
                //如果新插入的节点是右孩子 父亲节点平衡因子-1
                if (current == parent.Lchild)
                    parent.Bf++;
                else
                    parent.Bf--;
                if (parent.Bf != 0)
                    AdjustInsert(parent);

            }

        }
        //寻找node节点的父亲节点
        public void FindPosition(BinTreeNode<int>current,BinTreeNode<int> node)
        {
            if (node.Data < current.Data)
            {
                if (current.Lchild == null)
                {
                    current.Lchild = node;
                    node.Parent = current;
                }
                else
                    FindPosition(current.Lchild, node);

            }else if (node.Data > current.Data)
            {
                if (current.Rchild == null)
                {
                    current.Rchild = node;
                    node.Parent = current;
                }
                else
                    FindPosition(current.Rchild, node);
            }
            else
            {
                current.Data = node.Data;
            }
        }
        private void AdjustInsert(BinTreeNode<int> node)
        {
            if (node.Bf > 1)
                BalanceRightRotate(node);
            else if (node.Bf < -1)
                BalanceLeftRotate(node);
            else
            {
                BinTreeNode<int> root = node.Parent;
                if (root != null)
                {
                    if (node == root.Lchild)
                        root.Bf++;
                    else
                        root.Bf--;
                    if (root.Bf != 0)
                        AdjustInsert(root);
                }
            }    
        }
        private BinTreeNode<int> BalanceRightRotate(BinTreeNode<int> current)
        {
            BinTreeNode<int> child = current.Lchild;
            if (child.Bf < 0)
            {
                BinTreeNode<int> grand = child.Rchild;
                LeftRotate(grand, child);
                if (grand.Bf > 0)
                {
                    grand.Bf = 2;
                    child.Bf = 0;
                }
                else
                {
                    int temp = grand.Bf;
                    grand.Bf = -1 * child.Bf;
                    child.Bf = -1 * temp;
                }
                child = grand;
            }
            RightRotate(child, current);
            current.Bf = -1 * child.Bf + 1;
            child.Bf = child.Bf > 1 ? 0 : child.Bf - 1;
            return child;
        }
        private BinTreeNode<int> BalanceLeftRotate(BinTreeNode<int> current)
        {
            BinTreeNode<int> child = current.Rchild;
            if (child.Bf>0)
            {
                BinTreeNode<int> grand = child.Lchild;
                RightRotate(grand, child);
                if (grand.Bf < 0)
                {
                    grand.Bf = -2;
                    child.Bf = 0;
                }
                else
                {
                    int temp = grand.Bf;
                    grand.Bf = -1 * child.Bf;
                    child.Bf = -1 * temp;
                }
                child = grand;
            }
            LeftRotate(child, current);
            current.Bf = -1 * child.Bf - 1;
            child.Bf = child.Bf < -1 ? 0 : child.Bf + 1;
            return child;
        }
        private void RightRotate(BinTreeNode<int> node,BinTreeNode<int> parent)
        {
            OperateForBothRotate(node, parent);
            parent.Lchild = node.Rchild;
            if (node.Rchild != null)
            {
                node.Rchild.Parent = parent;
            }
            node.Rchild = parent;
        }
        private void LeftRotate(BinTreeNode<int> node, BinTreeNode<int> parent)
        {
            OperateForBothRotate(node, parent);
            parent.Rchild = node.Lchild;
            if (node.Lchild != null)
            {
                node.Lchild.Parent = parent;
            }
            node.Lchild = parent;
        }
        private void OperateForBothRotate(BinTreeNode<int> node, BinTreeNode<int> parent)
        {
            BinTreeNode<int> grand = parent.Parent;
            node.Parent = grand;
            parent.Parent = node;
            if (grand == null)
                root = node;
            else if (parent == grand.Rchild)
                grand.Rchild = node;
            else
                grand.Lchild = node;
        }
        public void Delete(int key)
        {
            //if (key == null)
            //    throw new ArgumentNullException(nameof(key));
            BinTreeNode<int> current = root;
            while (current != null)
            {
                if (key.CompareTo(current.Data) == 0)
                {
                    BinTreeNode<int> node = current;
                    if (current.Lchild != null)
                        node = GetMaxNode(current.Lchild);
                    else if (current.Rchild != null)
                        node = GetMinNode(current.Rchild);
                    current.Data = node.Data;
                    Remove(node);
                    Length--;
                }
                else if (key.CompareTo(current.Data) < 0) current = current.Lchild;
                else current = current.Rchild;
            }
        }
        private void Remove(BinTreeNode<int> node)
        {
            if (node == root)
            {
                root = null;
                return;
            }
            BinTreeNode<int> parent = node.Parent;
            if (node == parent.Lchild)
            {
                parent.Lchild = node.Lchild;
                if (node.Lchild != null)
                {
                    node.Lchild.Parent = parent;
                }
                parent.Bf--;
            }
            else
            {
                //parent.Rchild = null;
                parent.Rchild = node.Lchild;
                if (node.Lchild != null)
                    node.Lchild.Parent = parent;
                parent.Bf++;
            }
            if (parent.Bf != 1 && parent.Bf != -1)
                AdjustRemove(parent);
        }
        private void AdjustRemove(BinTreeNode<int> node)
        {
            if (node.Bf > 1)
                node = BalanceRightRotate(node);
            else if (node.Bf < -1)
                node = BalanceLeftRotate(node);
            BinTreeNode<int> parent = node.Parent;
            if (node.Bf == 0 && parent != null)
            {
                if (node == parent.Lchild)
                    parent.Bf--;
                else
                    parent.Bf++;
                if (parent.Bf != 1 && parent.Bf != -1)
                    AdjustRemove(parent);
            }
        }
        private BinTreeNode<int>GetMaxNode(BinTreeNode<int> current)
        {
            if (current.Rchild == null)
                return current;
            return GetMaxNode(current.Rchild);
        }
        private BinTreeNode<int> GetMinNode(BinTreeNode<int> current)
        {
            if (current.Lchild == null)
                return current;
            return GetMinNode(current.Lchild);
        }
        //private BinTree<int> binTree;
        //public AVLTree(int data)
        //{
        //    BinTreeNode<int> root = new BinTreeNode<int>();
        //    root.Data = data;
        //    binTree = new BinTree<int>(root);
        //}

        ////右旋处理
        //public void RRotate(BinTreeNode<int> P)
        //{
        //    BinTreeNode<int> L;
        //    L = P.Lchild;
        //    P.Lchild = L.Rchild;
        //    L.Rchild = P;
        //    P = L;
        //}
        ////左旋处理
        //public void LRotate(BinTreeNode<int> P)
        //{
        //    BinTreeNode<int> L;
        //    L = P.Rchild;
        //    P.Rchild = L.Lchild;
        //    L.Lchild = P;
        //    P = L;
        //}
        //public void LeftBalance(BinTreeNode<int> T)
        //{
        //    BinTreeNode<int> L, Lr;
        //    L = T.Lchild;            //L指向T左子树根节点
        //    switch (L.Bf)
        //    {
        //        case 1:
        //            T.Bf = L.Bf = 0;
        //            RRotate(T);
        //            break;
        //        case -1:
        //            Lr = L.Rchild;
        //            switch (Lr.Bf)
        //            {
        //                case 1:
        //                    T.Bf = -1;
        //                    L.Bf = 0;
        //                    break;
        //                case 0:
        //                    T.Bf = L.Bf = 0;
        //                    break;
        //                case -1:
        //                    T.Bf = 0;
        //                    L.Bf = 1;
        //                    break;
        //            }
        //            Lr.Bf = 0;
        //            LRotate(T.Lchild);
        //            RRotate(T);
        //            break;
        //    }
        //}
        private void MidOrder(BinTreeNode<int> current)
        {
            if (current == null)
                return;
            MidOrder(current.Lchild);
            orderString += current.Data.ToString() + " ";
            MidOrder(current.Rchild);
        }
        public string MidOrderTraversal()
        {
            orderString = string.Empty;
            MidOrder(root);
            return orderString.Trim();
        }
        
        
        public void drawTree(Graphics dc)
        {
            draw( dc, this.root,300, 10,0);
        }
        //private void draw(PaintEventArgs e,Graphics dc,BinTreeNode<int> node,int x,int y)
        //{
        //    if (node == null)
        //    {
        //        throw new Exception("空字符串");
        //    }
        //    dc.DrawEllipse(blackPen, x, y, 25, 20);
        //    String drawString = node.Data.ToString();//要显示的字符串
        //    PointF drawPoint = new PointF(x+1, y+3);//显示的字符串左上角的坐标
        //    e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
        //    delt /= 2;
        //    if (node.Lchild != null)
        //        draw(e, dc, node.Lchild,  delt, y + 40);
        //    if (node.Rchild != null)
        //        draw(e, dc, node.Rchild, x + delt, y + 40);


        //}
        private void draw( Graphics dc, BinTreeNode<int> node, int x, int y, int delt_f)
        {
            if (node == null)
            {
                return;
            }
            dc.DrawEllipse(blackPen, x, y, 25, 20);
            String drawString = node.Data.ToString();//要显示的字符串
            PointF drawPoint = new PointF(x + 1, y + 3);//显示的字符串左上角的坐标
            dc.DrawString(drawString, drawFont, drawBrush, drawPoint);
            // delt /= 2;
            if (node.Parent != null)
                dc.DrawLine(blackPen, x + 10, y, delt_f + 10, y - 20);
            if (node.Lchild != null)
            {
                draw( dc, node.Lchild, x - System.Math.Abs(x - delt_f) / 2, y + 40, x);
            }

            if (node.Rchild != null)
            {
                draw( dc, node.Rchild, x + System.Math.Abs(x - delt_f) / 2, y + 40, x);
            }

        }
    }
}
