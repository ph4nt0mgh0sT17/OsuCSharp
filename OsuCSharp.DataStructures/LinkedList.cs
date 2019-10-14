using System.Diagnostics;

/// <summary>
/// A namespace for all kinds data structures that exist.
/// </summary>
namespace OsuCSharp.DataStructures
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a list of objects that can be accessed with an index. Can be manipulated with via Add, Search, Remove operations.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class LinkedList<T>
    {
        #region Member fields

        internal LinkedListNode<T> mHead;
        internal int mCount;
        internal int mVersion;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty and null <see cref="LinkedList{T}"/>.
        /// </summary>
        public LinkedList()
        { 
        }

        /// <summary>
        /// Creates the <see cref="LinkedList{T}"/> with values from given <b>collection</b>.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public LinkedList(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                // Adds as the last item
                AddLast(item);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The count of items in the list.
        /// </summary>
        public int Count
        {
            get => mCount;
        }

        /// <summary>
        /// The first node in the list.
        /// </summary>
        public LinkedListNode<T> First
        {
            get => mHead;
        }

        /// <summary>
        /// The last node in the list.
        /// </summary>
        public LinkedListNode<T> Last
        {
            get => mHead == null ? null : mHead.Previous;
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts a new node in the last place of the list.
        /// </summary>
        /// <param name="value">The value of new node.</param>
        public void Add(T value)
        {
            // Adds as the last item in the list
            AddLast(value);
        }

        /// <summary>
        /// Creates a new node and inserts it after the given node.
        /// </summary>
        /// <param name="node">The given node.</param>
        /// <param name="value">The value of new node.</param>
        /// <returns>The created new node.</returns>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);

            // Creates new node and inserts it
            LinkedListNode<T> newNode = new LinkedListNode<T>(node.List, value);
            InternalInsertNodeBefore(node.Next!, newNode);

            return newNode;
        }

        /// <summary>
        /// Creates a new node and inserts it after the given node.
        /// </summary>
        /// <param name="node">The given node.</param>
        /// <param name="newNode">The new node.</param>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);

            // Creates new node and inserts it
            InternalInsertNodeBefore(node.Next!, newNode);
        }

        /// <summary>
        /// Creates a new node and inserts it before the given node.
        /// </summary>
        /// <param name="node">The given node.</param>
        /// <param name="value">The value of new node.</param>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            ValidateNode(node);

            // Creates a new node and inserts it before the given node
            LinkedListNode<T> newNode = new LinkedListNode<T>(this, value);
            InternalInsertNodeBefore(node, newNode);

            if (node == mHead)
            {
                mHead = newNode;
            }

            return newNode;
        }

        /// <summary>
        /// Creates a new node and inserts it before the given node.
        /// </summary>
        /// <param name="node">The given node.</param>
        /// <param name="newNode">The new node.</param>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);

            InternalInsertNodeBefore(node, newNode);

            // Attach this list to the new node
            newNode.mList = this;

            if (node == mHead)
            {
                mHead = newNode;
            }
        }

        /// <summary>
        /// Inserts the new node with given value as first item of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value">The value of the new node.</param>
        /// <returns>The create new node.</returns>
        public LinkedListNode<T> AddFirst(T value)
        {
            // Creates the new node
            LinkedListNode<T> newNode = new LinkedListNode<T>(this, value);

            if (mHead == null)
            {
                InternalInsertNodeToEmptyList(newNode);
            }

            else
            {
                InternalInsertNodeBefore(mHead, newNode);
                mHead = newNode;
            }

            return newNode;
        }

        /// <summary>
        /// Inserts the given new node as first item of the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="newNode">The given new node.</param>
        public void AddFirst(LinkedListNode<T> newNode)
        {
            ValidateNewNode(newNode);

            if (mHead == null)
            {
                InternalInsertNodeToEmptyList(newNode);
            }

            else
            {
                InternalInsertNodeBefore(mHead, newNode);
                mHead = newNode;
            }

            newNode.mList = this;
        }

        /// <summary>
        /// Adds the new node with given <b>value</b> as last item in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The new node.</returns>
        public LinkedListNode<T> AddLast(T value)
        {
            // Creates the new node
            LinkedListNode<T> newNode = new LinkedListNode<T>(this, value);

            if (mHead == null)
            {
                InternalInsertNodeToEmptyList(newNode);
            }

            else
            {
                InternalInsertNodeBefore(mHead, newNode);
            }

            return newNode;
        }

        /// <summary>
        /// Adds the <b>newNode</b> as last item in the <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="newNode">The new created node.</param>
        public void AddLast(LinkedListNode<T> newNode)
        {
            ValidateNewNode(newNode);

            if (mHead == null)
            {
                InternalInsertNodeToEmptyList(newNode);
            }

            else
            {
                InternalInsertNodeBefore(mHead, newNode);
            }

            // Attaches this list to the new node
            newNode.mList = this;
        }

        /// <summary>
        /// Clears the <see cref="LinkedList{T}"/>. O(n)
        /// </summary>
        public void Clear()
        {
            LinkedListNode<T> current = mHead;

            while (current != null)
            {
                DeleteNode(ref current);
            }

            mHead = null;
            mCount = 0;
            mVersion++;
        }



        /// <summary>
        /// Checks if the <see cref="LinkedList{T}"/> does containt the given <b>value</b>.
        /// </summary>
        /// <param name="value">The value in the list.</param>
        /// <returns>The state.</returns>
        public bool Contains(T value)
        {
            return Find(value) != null;
        }

        /// <summary>
        /// <para>
        /// Finds the <see cref="LinkedListNode{T}"/> by given <b>value</b>.
        /// </para>
        /// 
        /// <para>
        /// If <see cref="LinkedListNode{T}"/> is not found, <b>null</b> is returned.
        /// </para>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LinkedListNode<T> Find(T value)
        {
            LinkedListNode<T> node = mHead;

            // Comparer for the values
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            if (node != null)
            {
                do
                {
                    // We can compare the values via EqualityComparer
                    if (comparer.Equals(node!.mItem, value))
                    {
                        return node;
                    }

                    // Iterates to the next node
                    node = node.mNext;

                } while (node != mHead);
            }

            
            return null;

        }

        /// <summary>
        /// Gets the value of <see cref="LinkedListNode{T}"/> at given <b>index</b>.
        /// </summary>
        /// <param name="index">The index of the <see cref="LinkedListNode{T}"/>.</param>
        /// <returns>The value.</returns>
        public T Get(int index)
        {

            LinkedListNode<T> node = mHead;

            // Internal index for iterating through whole list
            int internalIndex = 0; 

            if (node != null)
            {
                do
                {
                    
                    if (internalIndex == index)
                    {
                        return node.Value;
                    }

                    // Iterates to the next node
                    node = node.mNext;
                    internalIndex++;

                }                 
                while (node != mHead); // Circular list
            }

            
            throw new IndexOutOfRangeException("The current index is not in the list.");
        }

        #region Helper methods

        /// <summary>
        /// Validates the node if not null or has attached wrong <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        internal void ValidateNode(LinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node.List != this)
            {
                throw new InvalidOperationException("The LinkedList node does not belong to the current LinkedList.");
            }
        }

        /// <summary>
        /// Validates the node if not null or has attached wrong <see cref="LinkedList{T}"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        internal void ValidateNewNode(LinkedListNode<T> newNode)
        {
            if (newNode == null)
            {
                throw new ArgumentNullException(nameof(newNode));
            }

            if (newNode.List != this)
            {
                throw new InvalidOperationException("The LinkedList node already belongs to a LinkedList.");
            }
        }

        /// <summary>
        /// Inserts the NewNode before the Node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="newNode">The new created node.</param>
        private void InternalInsertNodeBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            // Inserts the new node between the node and previous node
            newNode.mNext = node;
            newNode.mPrevious = node.mPrevious;
            node.mPrevious!.mNext = newNode;
            node.mPrevious = newNode;

            mVersion++;
            mCount++;
        }

        private void InternalInsertNodeToEmptyList(LinkedListNode<T> newNode)
        {
            Debug.Assert(mHead == null && mCount == 0, "LinkedList must be empty when this method is called!");

            // Makes a head of the list
            newNode.mNext = newNode;
            newNode.mPrevious = newNode;
            mHead = newNode;

            mVersion++;
            mCount++;
        }

        private void DeleteNode(ref LinkedListNode<T> current)
        {
            LinkedListNode<T> temp = current;
            current = current.Next;
            temp.Invalidate();
        }

        #endregion

        #endregion

    }

    /// <summary>
    /// An item that is part of the <see cref="LinkedList{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LinkedListNode<T>
    {
        #region Member fields

        internal LinkedList<T> mList;
        internal LinkedListNode<T> mNext;
        internal LinkedListNode<T> mPrevious;
        internal T mItem;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a linked list node with no siblings and with given value.
        /// </summary>
        /// <param name="value">The given value.</param>
        public LinkedListNode(T value)
        {
            mItem = value;
        }

        /// <summary>
        /// Creates a linked list node with a given list and value.
        /// </summary>
        /// <param name="list">The given list.</param>
        /// <param name="value">The given value.</param>
        internal LinkedListNode(LinkedList<T> list, T value)
        {
            mList = list;
            mItem = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list where the <see cref="LinkedListNode{T}"/> is stored.
        /// </summary>
        public LinkedList<T> List
        {
            get => mList;
        }

        /// <summary>
        /// Gets the next <see cref="LinkedListNode{T}"/> of the current <see cref="LinkedListNode{T}"/>.
        /// </summary>
        public LinkedListNode<T> Next
        {
            get => mNext == null || mNext == mList!.mHead ? null : mNext;
        }

        /// <summary>
        /// Gets the previous <see cref="LinkedListNode{T}"/> of the current <see cref="LinkedListNode{T}"/>.
        /// </summary>
        public LinkedListNode<T> Previous
        {
            get => mPrevious == null || this == mList!.mHead ? null : mPrevious;
        }

        /// <summary>
        /// Gets the value of the current <see cref="LinkedListNode{T}"/>.
        /// </summary>
        public T Value
        {
            get => mItem;
            set => mItem = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invalidates the <see cref="LinkedListNode{T}"/>.
        /// </summary>
        internal void Invalidate()
        {
            mList = null;
            mNext = null;
            mPrevious = null;
        }

        #endregion
    }
}
