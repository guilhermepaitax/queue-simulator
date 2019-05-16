using System;


namespace MarketSimulator.Models
{
    public class MyQueue<T> // fila generica
    {
        private T[] arrayQueue;
        private int tail, head, size;

        static T[] emptyArray = new T[0];
        private const int minGrow = 4;
        private const int growFactor = 200;

        public int Tail { get => tail; }
        public int Head { get => head; }
        public int Size { get => size; }

        public MyQueue()
        {
            arrayQueue = emptyArray;
            head = 0;
            tail = 0;
            size = 0;
        }

        public int Count()
        {
            return size;
        }

        private void SetCapacity(int capacity)
        {
            T[] newArray = new T[capacity];
            if(size > 0)
            {
                if(head < tail)
                {
                    Array.Copy(arrayQueue, head, newArray, 0, size);
                }
                else
                {
                    Array.Copy(arrayQueue, head, newArray, 0, arrayQueue.Length - head);
                    Array.Copy(arrayQueue, 0, newArray, arrayQueue.Length - head, tail);
                }
            }
            arrayQueue = newArray;
            head = 0;
            tail = (size == capacity) ? 0 : size;
        }

        public void Enqueue(T item) //adiciona
        {
            if(size == arrayQueue.Length)
            {
                int newcapacity = (int)((long)arrayQueue.Length * (long)growFactor / 100);
                if(newcapacity < arrayQueue.Length + minGrow)
                {
                    newcapacity = arrayQueue.Length + minGrow;
                }
                SetCapacity(newcapacity);
            }

            arrayQueue[tail] = item;
            tail = (tail + 1) % arrayQueue.Length;
            size++;
        }

        public void Dequeue() // remove
        {
            if(size > 0)
            {
                T removed = arrayQueue[head];
                arrayQueue[head] = default(T);
                head = (head + 1) % arrayQueue.Length;
                size--;
            }
        }

        public T Peek() // mostra o primeiro
        {
            return arrayQueue[head];
        }

    }
}
