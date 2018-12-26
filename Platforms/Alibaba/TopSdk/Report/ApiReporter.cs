using System;
using Top.Api.Util;
using System.Threading;
using System.Collections.Generic;
using Top.Api.Security;
using System.Text;

namespace Top.Api.Report
{

    public class ApiReporter
    {
        private static readonly ITopLogger Log = Top.Api.Log.Instance;
        private static readonly object InitSecretLock = new object();
        private static readonly int SleepTime = 1000 * 60 * 1;
        private static readonly int FlushInterval = 1000 * 60 * 5;// 5分钟
        private static readonly int MinFlushInterval = 1000 * 60 * 1;// 1分钟
        private static readonly string AppSecretType = "2";
        private static readonly string AppUserSecretType = "3";
        private static readonly IDictionary<string, object> AppInitStatus = new Dictionary<string, object>();
        private DefaultTopClient topClient;


        public void InitSecret(DefaultTopClient defaultTaobaoClient)
        {
            string appkey = defaultTaobaoClient.appKey;
            this.topClient = defaultTaobaoClient;
            lock (InitSecretLock)
            {
                if (AppInitStatus.ContainsKey(appkey))
                {
                    return;
                }

                InitSecretThread();
                AppInitStatus.Add(appkey, true);
            }
        }

        private void InitSecretThread()
        {
            Thread uploadThread = new Thread(o =>
            {
                DoUploadBiz();
            });
            uploadThread.IsBackground = true;
            uploadThread.Name = "flushSecretApiReporter-thread";
            uploadThread.Start();
        }

        private void DoUploadBiz()
        {
            long uploadTime = TopUtils.GetCurrentTimeMillis() + FlushInterval;
            while (true)
            {
                try
                {
                    Thread.Sleep(SleepTime);
                    IDictionary<string, Counter> appUserCounterMap = SecurityCounter.GetUserCounter(topClient.appKey);
                    if (TopUtils.GetCurrentTimeMillis() >= uploadTime || (appUserCounterMap.Count * 4 > 65536))
                    {
                        StringBuilder counterBuilder = new StringBuilder();
                        IDictionary<string, Counter> cloneAppUserCounter = SecurityCounter.CloneUserCounter(topClient.appKey);
                        SecurityCounter.CleanUserCounter(topClient.appKey);

                        int count = 0;
                        foreach (KeyValuePair<string, Counter> currentPair in cloneAppUserCounter)
                        {
                            Counter counter = currentPair.Value;
                            if (counterBuilder.Length > 0)
                            {
                                counterBuilder.Append(";");
                            }
                            counterBuilder.Append(currentPair.Key);
                            GenerateCounter(counterBuilder, counter);

                            if ((++count) % 100 == 0)
                            {
                                DoUpload(counterBuilder.ToString(), AppUserSecretType);
                                counterBuilder = new StringBuilder();
                            }
                        }
                        if (counterBuilder.Length > 0)
                        {
                            DoUpload(counterBuilder.ToString(), AppUserSecretType);
                            counterBuilder = new StringBuilder();
                        }

                        Counter appCounter = SecurityCounter.GetAppCounter(topClient.appKey);
                        counterBuilder.Append(SecurityCore.GetAppUserSecretCache().Count);
                        GenerateCounter(counterBuilder, appCounter);
                        appCounter.Reset();

                        long uploadInterval = DoUpload(counterBuilder.ToString(), AppSecretType);
                        uploadTime = TopUtils.GetCurrentTimeMillis() + uploadInterval;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(string.Format("flushSecretApiReporter error: {0}", e.Message));
                }
            }
        }

        private IDictionary<string, Counter> CloneAppUserCounter(IDictionary<string, Counter> sourceDictionary)
        {
            IDictionary<string, Counter> targetDictionary = new Dictionary<string, Counter>();
            foreach (KeyValuePair<string, Counter> currentPair in sourceDictionary)
            {
                targetDictionary.Add(currentPair.Key, currentPair.Value);
            }
            return targetDictionary;
        }

        private int DoUpload(string contentJson, string type)
        {
            int uploadInterval = FlushInterval;
            TopSdkFeedbackUploadRequest request = new TopSdkFeedbackUploadRequest();
            request.Type = type;
            request.Content = contentJson;

            TopSdkFeedbackUploadResponse response = topClient.Execute(request, null);
            if (!response.IsError)
            {
                uploadInterval = response.UploadInterval;
                if (uploadInterval < MinFlushInterval)
                {
                    uploadInterval = FlushInterval;
                }
            }
            return uploadInterval;
        }


        private void GenerateCounter(StringBuilder counterBuilder, Counter counter)
        {

            // encrypt
            if (counterBuilder.Length > 0)
            {
                counterBuilder.Append(",");
            }
            if (SecurityCounter.GetAppCounter(topClient.appKey).GetEncryptPhoneNum() != 0)
            {
                counterBuilder.Append(counter.GetEncryptPhoneNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetEncryptNickNum() != 0)
            {
                counterBuilder.Append(counter.GetEncryptNickNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetEncryptReceiverNameNum() != 0)
            {
                counterBuilder.Append(counter.GetEncryptReceiverNameNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetEncryptSimpleNum() != 0)
            {
                counterBuilder.Append(counter.GetEncryptSimpleNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetEncryptSearchNum() != 0)
            {
                counterBuilder.Append(counter.GetEncryptSearchNum());
            }
            else
            {
                counterBuilder.Append("0");
            }

            // decrypt
            counterBuilder.Append(",");
            if (counter.GetDecryptPhoneNum() != 0)
            {
                counterBuilder.Append(counter.GetDecryptPhoneNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetDecryptNickNum() != 0)
            {
                counterBuilder.Append(counter.GetDecryptNickNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetDecryptReceiverNameNum() != 0)
            {
                counterBuilder.Append(counter.GetDecryptReceiverNameNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetDecryptSimpleNum() != 0)
            {
                counterBuilder.Append(counter.GetDecryptSimpleNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetDecryptSearchNum() != 0)
            {
                counterBuilder.Append(counter.GetDecryptSearchNum());
            }
            else
            {
                counterBuilder.Append("0");
            }

            // search
            counterBuilder.Append(",");
            if (counter.GetSearchPhoneNum() != 0)
            {
                counterBuilder.Append(counter.GetSearchPhoneNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetSearchNickNum() != 0)
            {
                counterBuilder.Append(counter.GetSearchNickNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetSearchReceiverNameNum() != 0)
            {
                counterBuilder.Append(counter.GetSearchReceiverNameNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetSearchSimpleNum() != 0)
            {
                counterBuilder.Append(counter.GetSearchSimpleNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
            counterBuilder.Append(",");
            if (counter.GetSearchSearchNum() != 0)
            {
                counterBuilder.Append(counter.GetSearchSearchNum());
            }
            else
            {
                counterBuilder.Append("0");
            }
        }
    }
}
