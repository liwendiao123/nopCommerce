using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Infrastructure
{

    public class OptimizeTree<K, T>
    {
        public Node<K, T> Root;
        public int UpdataId;
    }

    public class Node<K, T>
    {
        private static Queue<Node<K, T>> _queue = new Queue<Node<K, T>>();

        private T _obj;
        private K _key;
        private LinkedList<Node<K, T>> _childs = new LinkedList<Node<K, T>>();

        public delegate void TraversalNode(Node<K, T> tarver, out bool stop);

        public class TraversalFlag
        {
            public bool Stop = false;
            public void StopTraversal()
            {
                Stop = true;
            }
        }

        public IEnumerable<Node<K, T>> Childs
        {
            get
            {
                IEnumerable<Node<K, T>> iEnumerable = _childs;
                return iEnumerable;
            }
        }

        public Node<K, T> Parent;

        public T Value
        {
            get
            {
                return _obj;
            }
            set
            {
                _obj = value;
            }
        }
        public K Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public Node(K k, T t)
        {
            Key = k;
            Value = t;
        }

        public Node(K k)
        {
            Key = k;
        }

        public bool IsLeaf()
        {
            return (CountChilds() == 0);
        }

        public int CountChilds()
        {
            return _childs.Count;
        }

        public void AddChild(K k, T t)
        {
            AddChild(new Node<K, T>(k, t));
        }

        public virtual void AddChild(Node<K, T> child)
        {
            _childs.AddLast(child);
            child.Parent = this;
        }

        public virtual void DeleteChild(K k)
        {
            Node<K, T> node = FindChilds(k);
            if (node != null)
            {
                _childs.Remove(node);
                node.Parent = null;
            }
        }

        public Node<K, T> FindChilds(K k)
        {
            foreach (Node<K, T> node in _childs)
            {
                if (node.Key.Equals(k))
                {
                    return node;
                }
            }

            return null;
        }

        public virtual Node<K, T> Find(K k)
        {
            Node<K, T> result = null;
            Node<K, T>.TraversalNode fun;
            fun = delegate (Node<K, T> tarver, out bool stop)
            {
                if (tarver.Key.Equals(k))
                {
                    result = tarver;
                    stop = true;
                }
                else
                {
                    stop = false;
                }
            };
            this.BreadthFirst(fun);

            return result;
        }

        public virtual void BreadthFirst(TraversalNode fun)
        {
            Queue<Node<K, T>> queue = new Queue<Node<K, T>>();
            bool flag;
            Node<K, T> v;
            Node<K, T> visitNode = this;
            fun(visitNode, out flag);
            if (flag == true)
                goto FunOut;
            queue.Enqueue(this);
            while (queue.Count != 0)
            {
                //Debug.Log("BreadthFirst");
                v = queue.Dequeue();
                if (v.CountChilds() > 0)
                {
                    foreach (Node<K, T> w in v.Childs)
                    {
                        visitNode = w;
                        fun(visitNode, out flag);
                        if (flag == true)
                        {
                            goto FunOut;
                        }
                        queue.Enqueue(w);
                    }
                }
            }

            FunOut:
            queue.Clear();
        }

        public virtual LinkedList<Node<K, T>> BreadthFirst()
        {
            LinkedList<Node<K, T>> list = new LinkedList<Node<K, T>>();
            LinkedListNode<Node<K, T>> head;
            list.AddLast(this);
            head = list.First;
            while (head != null)
            {
                if (head.Value.CountChilds() > 0)
                {
                    foreach (Node<K, T> w in head.Value.Childs)
                    {
                        list.AddLast(w);
                    }
                }
                head = head.Next;
            }

            return list;
        }

    }


    public class NodeFast<K, T> : Node<K, T>
    {
        private OptimizeTree<K, T> _optimizeTree;
        private int _oldId;
        private LinkedList<Node<K, T>> _listBreadthFirst;

        public NodeFast(K k, T t, OptimizeTree<K, T> optimizeTree) : base(k, t)
        {
            this._optimizeTree = optimizeTree;
        }

        public override void AddChild(Node<K, T> child)
        {
            base.AddChild(child);
            _optimizeTree.UpdataId++;
        }

        public override void DeleteChild(K k)
        {
            base.DeleteChild(k);
            _optimizeTree.UpdataId++;
        }

        public override LinkedList<Node<K, T>> BreadthFirst()
        {
            if (_listBreadthFirst == null || _oldId != _optimizeTree.UpdataId)
            {
                _listBreadthFirst = base.BreadthFirst();
            }

            return _listBreadthFirst;
        }
    }
}
