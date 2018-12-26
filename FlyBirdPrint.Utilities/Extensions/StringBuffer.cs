
//using System;
//using System.Text;
//using System.Collections.Generic;

//namespace System
//{
//    /// <summary>
//    /// 线程安全的字符串Buffer
//    /// </summary>
//    public class StringBuffer
//    {
//        private StringBuilder m_InnerBuilder = new StringBuilder();
//        private object syncLock = new object();


//        public StringBuffer()
//        {
//            m_InnerBuilder = new StringBuilder();
//        }
//        public StringBuffer(int capacity)
//        {
//            m_InnerBuilder = new StringBuilder(capacity);
//        }

//        public StringBuffer(string value)
//        {
//            m_InnerBuilder = new StringBuilder(value);
//        }

//        public StringBuffer(int capacity, int maxCapacity)
//        {
//            m_InnerBuilder = new StringBuilder(capacity, maxCapacity);
//        }

//        public StringBuffer(string value, int capacity)
//        {
//            m_InnerBuilder = new StringBuilder(value, capacity);
//        }

//        public StringBuffer(string value, int startIndex, int length, int capacity)
//        {
//            m_InnerBuilder = new StringBuilder(value, startIndex, length, capacity);
//        }

//        public StringBuffer(StringBuilder innerBuilder)
//        {
//            m_InnerBuilder = innerBuilder;
//        }



//        public StringBuilder InnerBuilder
//        {
//            get { return m_InnerBuilder; }
//        }

//        internal object SyncLock
//        {
//            get
//            {
//                return this.syncLock;
//            }
//        }

//        public int Length
//        {
//            get
//            {
//                return m_InnerBuilder.Length;
//            }
//        }

//        public void Remove(int startIndex, int length)
//        {
//            lock (syncLock)
//            {
//                m_InnerBuilder.Remove(startIndex, length);
//            }

//        }

//        public void Replace(string oldValue, string newValue)
//        {
//            lock (syncLock)
//            {
//                m_InnerBuilder.Replace(oldValue, newValue);
//            }
//        }




//        public static StringBuffer operator +(StringBuffer buffer, bool value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, byte value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, char value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, char[] value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }

//        public static StringBuffer operator +(StringBuffer buffer, decimal value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, double value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, float value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, int value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, long value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, object value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, sbyte value)
//        {
//            buffer.InnerBuilder.Append(value);

//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, short value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, string value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, uint value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, ulong value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }
//        public static StringBuffer operator +(StringBuffer buffer, ushort value)
//        {
//            lock (buffer.SyncLock)
//            {
//                buffer.InnerBuilder.Append(value);
//            }
//            return buffer;
//        }

//        public override string ToString()
//        {
//            lock (syncLock)
//            {
//                return InnerBuilder.ToString();
//            }
//        }
//    }
//}