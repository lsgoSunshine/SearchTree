using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SearchTree.RedBlackTree
{
    class RedBlackTree<T>where T:IComparable
    {
        private RBNode<T> root;
        private string orderString;
        private Pen blackPen = new Pen(Color.Black, 1);
        private Font drawFont = new Font("Arial", 8);//显示的字符串使用的字体
        private SolidBrush drawBrush = new SolidBrush(Color.Black);//写字符串用的刷子

        /*************对红黑树节点x进行左旋操作 ******************/
        /* 
         * 左旋示意图：对节点x进行左旋 
         *     p                       p 
         *    /                       / 
         *   x                       y 
         *  / \                     / \ 
         * lx  y      ----->       x  ry 
         *    / \                 / \ 
         *   ly ry               lx ly 
         * 左旋做了三件事： 
         * 1. 将y的左子节点赋给x的右子节点,并将x赋给y左子节点的父节点(y左子节点非空时) 
         * 2. 将x的父节点p(非空时)赋给y的父节点，同时更新p的子节点为y(左或右) 
         * 3. 将y的左子节点设为x，将x的父节点设为y 
         */
        public void LeftRotate(RBNode<T> node)
        {
            RBNode<T> y = node.Rchild;
            node.Rchild = y.Lchild;
            if (y.Lchild != null)
                y.Lchild.Parent = node;
            y.Parent = node.Parent;
            if (node.Parent != null)
            {
                if (node == node.Parent.Lchild)
                    node.Parent.Lchild = y;
                else
                    node.Parent.Rchild = y;
            }else if (node.Parent == null)
                this.root = y;
            y.Lchild = node;
            node.Parent = y;
        }
        /*************对红黑树节点y进行右旋操作 ******************/
        /* 
         * 左旋示意图：对节点y进行右旋 
         *        p                   p 
         *       /                   / 
         *      y                   x 
         *     / \                 / \ 
         *    x  ry   ----->      lx  y 
         *   / \                     / \ 
         * lx  rx                   rx ry 
         * 右旋做了三件事： 
         * 1. 将x的右子节点赋给y的左子节点,并将y赋给x右子节点的父节点(x右子节点非空时) 
         * 2. 将y的父节点p(非空时)赋给x的父节点，同时更新p的子节点为x(左或右) 
         * 3. 将x的右子节点设为y，将y的父节点设为x 
         */
         public void RightLotate(RBNode<T> node)
        {
            RBNode<T> x = node.Lchild;
            node.Lchild = x.Rchild;
            if(x.Rchild!=null)
                x.Rchild.Parent = node;
            x.Parent = node.Parent;
            if (node.Parent == null)
                this.root = x;
            else
            {
                if (node == node.Parent.Lchild)
                    node.Parent.Lchild = x;
                else
                    node.Parent.Rchild = x;
            }
            x.Rchild = node;
            node.Parent = x.Rchild;
        }
        public void insert(T key)
        {
            RBNode<T> node = new RBNode<T>(key);
            insert(node);
        }
        public void insert(RBNode<T> node)
        {
            RBNode<T> current = null;//表示最后node的父节点
            RBNode<T> x = this.root;
            while (x != null)
            {
                current = x;
                int cmp = node.Data.CompareTo(x.Data);
                if (cmp < 0)
                    x = x.Lchild;
                else
                    x = x.Rchild;
            }
            node.Parent = current;
            if (current != null)
            {
                int cmp = node.Data.CompareTo(current.Data);
                if (cmp < 0)
                    current.Lchild = node;
                else
                    current.Rchild = node;
            }
            else
                this.root = node;
            insertFixUp(node);
        }
        private void insertFixUp(RBNode<T> node)
        {
            RBNode<T> parent, gparent;
            while (((parent = parentOf(node)) != null) && parent.Color)
            {
                gparent = parentOf(parent);
                if (parent == gparent.Lchild)
                {
                    RBNode<T> uncle = gparent.Rchild;
                    if (uncle != null && uncle.Color)
                    {
                        parent.Color = false;
                        uncle.Color = false;
                        gparent.Color = true;
                        //setBlack(parent);
                        //setBlack(uncle);
                        //setRed(gparent);
                        node = gparent;
                        continue;
                    }
                    if (node == parent.Rchild)
                    {
                        LeftRotate(parent);
                        RBNode<T> tmp = parent;
                        parent = node;
                        node = tmp;
                    }
                    parent.Color = false;
                    gparent.Color = true;
                    //setBlack(parent);
                    //setRed(gparent);
                    RightLotate(gparent);
                }
                else
                {
                    RBNode<T> uncle = gparent.Lchild;
                    if (uncle != null && uncle.Color)
                    {
                        parent.Color = false;
                        uncle.Color = false;
                        gparent.Color = true;
                        //setBlack(parent);
                        //setBlack(uncle);
                        //setRed(parent);
                        node = gparent;
                        continue;
                    }
                    if (node == parent.Lchild)
                    {
                        RightLotate(parent);
                        RBNode<T> tmp = parent;
                        parent = node;
                        node = tmp;
                    }
                    parent.Color = false;
                    gparent.Color = true;
                    //setBlack(parent);
                    //setRed(gparent);
                    LeftRotate(gparent);
                }
            }
            this.root.Color = false;
            //setBlack(this.root);
        }
        private void remove(RBNode<T> node)
        {
            RBNode<T> child, parent;
            Boolean color;
            //被删除的节点  左右子节点都不为空 的情况
            if ((node.Lchild != null) && (node.Rchild != null))
            {
                //先找到被删除节点的后继节点，用它来取代被删除节点的位置  
                RBNode<T> replace = node;
                //1)获取后继节点
                replace = replace.Rchild;
                while (replace.Lchild != null)
                    replace = replace.Lchild;
                //  2). 处理“后继节点”和“被删除节点的父节点”之间的关系  
                if (parentOf(node) != null)
                {
                    if (node == parentOf(node).Lchild)
                        parentOf(node).Rchild = replace;
                    else
                        parentOf(node).Rchild = replace;
                }
                else
                    this.root = replace;

                //3）处理 后继节点的子节点 和 被删除节点的子节点 之间的关系
                child = replace.Rchild;//后继节点肯定不存在左子节点
                parent = parentOf(replace);
                color = replace.Color;
                if (parent == node)
                    parent = replace;
                else
                {
                    if (child != null)
                        child.Parent = parent;
                    parent.Lchild = child;
                    replace.Rchild = node.Rchild;
                    node.Rchild.Parent = replace;     
                }
                replace.Parent = node.Parent;
                replace.Color = node.Color;
                replace.Lchild = node.Lchild;
                node.Lchild = node.Lchild;
                node.Lchild.Parent = replace;
                if (!color)
                    removeFixUp(child, parent);
                node = null;
                return;
                
                
                
                    
            }
        }
        public RBNode<T> parentOf(RBNode<T> node)
        { //获得父节点  
            return node != null ? node.Parent : null;
        }

        //node表示待修正的节点，即后继节点的子节点（因为后继节点被挪到删除节点的位置去了）  
        private void removeFixUp(RBNode<T> node, RBNode<T> parent)
        {
            RBNode<T> other;

            while ((node == null || !node.Color) && (node != this.root))
            {
                if (parent.Lchild == node)
                { //node是左子节点，下面else与这里的刚好相反  
                    other = parent.Rchild; //node的兄弟节点  
                    if (other.Color)
                    { //case1: node的兄弟节点other是红色的  
                        other.Color = false;
                        parent.Color = true;
                        LeftRotate(parent);
                        other = parent.Rchild;
                    }

                    //case2: node的兄弟节点other是黑色的，且other的两个子节点也都是黑色的  
                    if ((other.Lchild == null || !other.Lchild.Color) &&
                            (other.Rchild == null || !other.Rchild.Color))
                    {
                        other.Color = true;
                        node = parent;
                        parent = parentOf(node);
                    }
                    else
                    {
                        //case3: node的兄弟节点other是黑色的，且other的左子节点是红色，右子节点是黑色  
                        if (other.Rchild == null || !other.Rchild.Color)
                        {
                            other.Lchild.Color = false;
                            other.Color = true;
                            RightLotate(other);//RightRotate(other);
                            other = parent.Rchild;
                        }

                        //case4: node的兄弟节点other是黑色的，且other的右子节点是红色，左子节点任意颜色  
                        other.Color = parent.Color;
                        parent.Color = false;
                        other.Rchild.Color = false;
                        LeftRotate(parent);
                        node = this.root;
                        break;
                    }
                }
                else
                { //与上面的对称  
                    other = parent.Lchild;

                    if (other.Color)
                    {
                        // Case 1: node的兄弟other是红色的    
                        other.Color = false;
                        parent.Color = true;
                        RightLotate(parent);
                        other = parent.Lchild;
                    }

                    if ((other.Lchild == null || !other.Lchild.Color) &&
                        (other.Rchild == null || other.Rchild.Color))
                    {
                        // Case 2: node的兄弟other是黑色，且other的俩个子节点都是黑色的    
                        other.Color=true;
                        node = parent;
                        parent = parentOf(node);
                    }
                    else
                    {

                        if (other.Lchild == null || !other.Lchild.Color)
                        {
                            // Case 3: node的兄弟other是黑色的，并且other的左子节点是红色，右子节点为黑色。    
                            other.Rchild.Color = false;
                            other.Color = true;
                            LeftRotate(other);
                            other = parent.Lchild;
                        }

                        // Case 4: node的兄弟other是黑色的；并且other的左子节点是红色的，右子节点任意颜色  
                        other.Color = parent.Color;
                        parent.Color = false;
                        other.Lchild.Color = false;
                        RightLotate(parent);
                        node = this.root;
                        break;
                    }
                }
            }
            if (node != null)
                node.Color = false;
        }
        private void MidOrder(RBNode<T> current)
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
        public void drawTree(PaintEventArgs e, Graphics dc)
        {
            draw(e, dc, this.root, 300, 10, 0);
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
        private void draw(PaintEventArgs e, Graphics dc, RBNode<T> node, int x, int y, int delt_f)
        {
            if (node == null)
            {
                throw new Exception("空字符串");
            }
            dc.DrawEllipse(blackPen, x, y, 25, 20);
            String drawString = node.Data.ToString();//要显示的字符串
            PointF drawPoint = new PointF(x + 1, y + 3);//显示的字符串左上角的坐标
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
            // delt /= 2;
            if (node.Parent != null)
                dc.DrawLine(blackPen, x + 10, y, delt_f + 10, y - 20);
            if (node.Lchild != null)
            {
                draw(e, dc, node.Lchild, x - System.Math.Abs(x - delt_f) / 2, y + 40, x);
            }

            if (node.Rchild != null)
            {
                draw(e, dc, node.Rchild, x + System.Math.Abs(x - delt_f) / 2, y + 40, x);
            }

        }
    }
}
