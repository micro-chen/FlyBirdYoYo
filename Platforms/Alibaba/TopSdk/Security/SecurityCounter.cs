using System;
using System.Threading;
using System.Collections.Generic;


namespace Top.Api.Security
{

    /// <summary>
    /// 计数器
    /// </summary>
    public class SecurityCounter : SecurityConstants
    {
        private static readonly object Lock = new object();
        private static readonly IDictionary<string, Counter> AppCounter = new Dictionary<string, Counter>();
        private static readonly IDictionary<string, IDictionary<string, Counter>> AppUserCounter = new Dictionary<string, IDictionary<string, Counter>>();
        private string appkey;
        private static readonly object InitLock = new object();

        public SecurityCounter(string appkey)
        {
            this.appkey = appkey;
            lock (InitLock)
            {
                Counter appCounter = GetAppCounter(appkey);
                if (appCounter == null)
                {
                    appCounter = new Counter();
                    AppCounter.Add(appkey, appCounter);
                }

                IDictionary<string, Counter> userCounter = GetUserCounter(appkey);
                if (userCounter == null)
                {
                    userCounter = new Dictionary<string, Counter>();
                    AppUserCounter.Add(appkey, userCounter);
                }
            }
        }

        public static Counter GetAppCounter(string appkey)
        {
            Counter appCounter = null;
            AppCounter.TryGetValue(appkey, out appCounter);
            return appCounter;
        }

        public static IDictionary<string, Counter> GetUserCounter(string appkey)
        {
            IDictionary<string, Counter> userCounter = null;
            AppUserCounter.TryGetValue(appkey, out userCounter);
            return userCounter;
        }

        public static void AddEncryptCount(string type, Counter counter)
        {
            if (counter == null)
            {
                return;
            }

            if (PHONE.Equals(type))
            {
                Interlocked.Increment(ref counter.EncryptPhoneNum);
            }
            else if (NICK.Equals(type))
            {
                Interlocked.Increment(ref counter.EncryptNickNum);
            }
            else if (RECEIVER_NAME.Equals(type))
            {
                Interlocked.Increment(ref counter.EncryptReceiverNameNum);
            }
            else if (SIMPLE.Equals(type))
            {
                Interlocked.Increment(ref counter.EncryptSimpleNum);
            }
            else if (SEARCH.Equals(type))
            {
                Interlocked.Increment(ref counter.EncryptSearchNum);
            }
        }

        public static void AddDecryptCount(string type, Counter counter)
        {
            if (counter == null)
            {
                return;
            }

            if (PHONE.Equals(type))
            {
                Interlocked.Increment(ref counter.DecryptPhoneNum);
            }
            else if (NICK.Equals(type))
            {
                Interlocked.Increment(ref counter.DecryptNickNum);
            }
            else if (RECEIVER_NAME.Equals(type))
            {
                Interlocked.Increment(ref counter.DecryptReceiverNameNum);
            }
            else if (SIMPLE.Equals(type))
            {
                Interlocked.Increment(ref counter.DecryptSimpleNum);
            }
            else if (SEARCH.Equals(type))
            {
                Interlocked.Increment(ref counter.DecryptSearchNum);
            }
        }

        public static void AddSearchCount(string type, Counter counter)
        {
            if (counter == null)
            {
                return;
            }

            if (PHONE.Equals(type))
            {
                Interlocked.Increment(ref counter.SearchPhoneNum);
            }
            else if (NICK.Equals(type))
            {
                Interlocked.Increment(ref counter.SearchNickNum);
            }
            else if (RECEIVER_NAME.Equals(type))
            {
                Interlocked.Increment(ref counter.SearchReceiverNameNum);
            }
            else if (SIMPLE.Equals(type))
            {
                Interlocked.Increment(ref counter.SearchSimpleNum);
            }
            else if (SEARCH.Equals(type))
            {
                Interlocked.Increment(ref counter.SearchSearchNum);
            }
        }

        public void AddEncryptCount(string type, string session)
        {
            AddEncryptCount(type, GetCounter(session));
        }

        public void AddDecryptCount(string type, string session)
        {
            AddDecryptCount(type, GetCounter(session));
        }

        public void AddSearchCount(string type, string session)
        {
            AddSearchCount(type, GetCounter(session));
        }

        public static void CleanUserCounter(string appkey)
        {
            IDictionary<string, Counter> userCounter = GetUserCounter(appkey);
            if (userCounter != null)
            {
                lock (Lock)
                {
                    userCounter.Clear();
                }
            }
        }

        public static IDictionary<string, Counter> CloneUserCounter(string appkey)
        {
            IDictionary<string, Counter> targetDictionary = new Dictionary<string, Counter>();
            IDictionary<string, Counter> userCounter = GetUserCounter(appkey);
            if (userCounter == null)
            {
                return targetDictionary;
            }
          
            lock (Lock)
            {
                foreach (KeyValuePair<string, Counter> currentPair in userCounter)
                {
                    targetDictionary.Add(currentPair.Key, currentPair.Value);
                }
            }
            return targetDictionary;
        }


        private Counter GetCounter(string session)
        {
            Counter counter = null;
            if (session == null)
            {
                counter = GetAppCounter(appkey);
            }
            else
            {
                IDictionary<string, Counter> userCounter = GetUserCounter(appkey);
                if (userCounter == null)
                {
                    return null;
                }
                lock (Lock)
                {
                    userCounter.TryGetValue(session, out counter);
                    if (counter == null)
                    {
                        counter = new Counter();
                        userCounter.Add(session, counter);
                    }
                }
            }
            return counter;
        }
    }
}
