菜鸟打印的ISV sdk  的 .net Core 移植
文档：
http://pac.i56.taobao.com/portal/portalDoc.htm?spm=0.0.0.0.WKpsBf&id=225&tdsourcetag=s_pcqq_aiomsg&qq-pf-to=pcqq.group
菜鸟ISV接入应用-控制台：
https://global.link.cainiao.com/manager?spm=a2d6n.11224607.0.0.7ae648c7bli2eu#/?_k=spwgzb

获取电子面单单号：
http://pac.i56.taobao.com/apiinfo/showDetail.htm?spm=0.0.0.0.mTnFcJ&apiId=TMS_WAYBILL_APPLY_NEW


 1授权  
 2电子面单云打印接口 
 3电子面单云打印更新接口 
 4获取发货地，CP开通状态，账户的使用情况
5 ISV电子面单取消接口   就这几个吧；
参考快递助手的菜鸟接口代码，； 申请单号好像用到了打印，更新，和发货地；



string respResult = string.Empty;
                HttpWebRequest reqObj = WebRequest.CreateHttp(targetUri);
                reqObj.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                reqObj.Method = "POST";

                var data = Encoding.UTF8.GetBytes(postData);
                using (Stream stream = reqObj.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse respObj = (await reqObj.GetResponseAsync()) as HttpWebResponse;
                using (var sm=respObj.GetResponseStream())
                {
                    using (var reader=new StreamReader(sm))
                    {
                        respResult = await reader.ReadToEndAsync();
                    }
                }
