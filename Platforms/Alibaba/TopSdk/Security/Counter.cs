using System;
using System.Threading;


namespace Top.Api.Security
{

    /// <summary>
    /// 计数器
    /// </summary>
    public class Counter
    {
        public long EncryptPhoneNum = 0;
        public long EncryptNickNum = 0;
        public long EncryptReceiverNameNum = 0;
        public long EncryptSimpleNum = 0;
        public long EncryptSearchNum = 0;

        public long DecryptPhoneNum = 0;
        public long DecryptNickNum = 0;
        public long DecryptReceiverNameNum = 0;
        public long DecryptSimpleNum = 0;
        public long DecryptSearchNum = 0;

        public long SearchPhoneNum = 0;
        public long SearchNickNum = 0;
        public long SearchReceiverNameNum = 0;
        public long SearchSimpleNum = 0;
        public long SearchSearchNum = 0;

        public long GetSearchSearchNum()
        {
            return SearchSearchNum;
        }

        public long GetSearchSimpleNum()
        {
            return SearchSimpleNum;
        }

        public long GetDecryptSimpleNum()
        {
            return DecryptSimpleNum;
        }

        public long GetDecryptSearchNum()
        {
            return DecryptSearchNum;
        }

        public long GetEncryptSimpleNum()
        {
            return EncryptSimpleNum;
        }

        public long GetEncryptSearchNum()
        {
            return EncryptSearchNum;
        }

        public long GetEncryptPhoneNum()
        {
            return EncryptPhoneNum;
        }

        public long GetEncryptNickNum()
        {
            return EncryptNickNum;
        }

        public long GetEncryptReceiverNameNum()
        {
            return EncryptReceiverNameNum;
        }

        public long GetDecryptPhoneNum()
        {
            return DecryptPhoneNum;
        }

        public long GetDecryptNickNum()
        {
            return DecryptNickNum;
        }

        public long GetDecryptReceiverNameNum()
        {
            return DecryptReceiverNameNum;
        }

        public long GetSearchPhoneNum()
        {
            return SearchPhoneNum;
        }

        public long GetSearchNickNum()
        {
            return SearchNickNum;
        }

        public long GetSearchReceiverNameNum()
        {
            return SearchReceiverNameNum;
        }

        public void Reset()
        {
            EncryptPhoneNum = 0;
            EncryptNickNum = 0;
            EncryptReceiverNameNum = 0;
            EncryptSimpleNum = 0;
            EncryptSearchNum = 0;

            DecryptPhoneNum = 0;
            DecryptNickNum = 0;
            DecryptReceiverNameNum = 0;
            DecryptSimpleNum = 0;
            DecryptSearchNum = 0;

            SearchPhoneNum = 0;
            SearchNickNum = 0;
            SearchReceiverNameNum = 0;
            SearchSimpleNum = 0;
            SearchSearchNum = 0;
        }

    }
}
