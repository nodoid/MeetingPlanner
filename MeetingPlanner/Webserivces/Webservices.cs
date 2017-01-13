using System;
using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeetingPlanner
{
    public static class Webservices
    {
        public static async Task<T> GetData<T>(string method, string para, string id)
        {
            var url = string.Format("{0}/{1}?{2}={3}", Constants.WorkplaceUrl, method, para, id);
            var obj = Activator.CreateInstance<T>();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var locationJson = response.Content.ReadAsStringAsync().Result;
                    obj = JsonConvert.DeserializeObject<T>(locationJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }
            return obj;
        }

        public static async Task<T> GetData<T>(string method, string paraOne, string idOne, string paraTwo, string idTow)
        {
            var url = string.Format("{0}/{1}?{2}={3}&{4}={5}", Constants.WorkplaceUrl, method, paraOne, idOne, paraTwo, idTow);
            var obj = Activator.CreateInstance<T>();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var departmentsJson = response.Content.ReadAsStringAsync().Result;
                    obj = JsonConvert.DeserializeObject<T>(departmentsJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return obj;
        }

        public static async Task<T> GetData<T>(string method, string paraOne, string idOne, string paraTwo, string idTow, string paraThree, string idThree)
        {
            var url = string.Format("{0}/{1}?{2}={3}&{4}={5}&{6}={7}", Constants.WorkplaceUrl, method, paraOne, idOne, paraTwo, idTow, paraThree, idThree);
            var obj = Activator.CreateInstance<T>();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var departmentsJson = response.Content.ReadAsStringAsync().Result;
                    obj = JsonConvert.DeserializeObject<T>(departmentsJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return obj;
        }

        public static async Task<U> GetListData<U>(string method, params string[] data)
        {
            var url = string.Format("{0}/{1}?", Constants.WorkplaceUrl, method);
            if (data.Length != 0)
            {
                foreach (var v in data)
                    url += string.Format("/{0}", v.ToLowerInvariant());
            }

            U dta = default(U);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var departmentsJson = response.Content.ReadAsStringAsync().Result;

                    dta = JsonConvert.DeserializeObject<U>(departmentsJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetListData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return dta;
        }

        public static async Task<U> GetListData<U>(string method, string para, string id)
        {
            var url = string.Format("{0}/{1}?{2}={3}", Constants.WorkplaceUrl, method, para, id);
            U dta = default(U);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var departmentsJson = response.Content.ReadAsStringAsync().Result;

                    dta = JsonConvert.DeserializeObject<U>(departmentsJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetListData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return dta;
        }

        public static async Task<U> GetListData<U>(string method)
        {
            var url = string.Format("{0}/{1}", Constants.WorkplaceUrl, method);
            U dta = default(U);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    var departmentsJson = response.Content.ReadAsStringAsync().Result;
                    dta = JsonConvert.DeserializeObject<U>(departmentsJson);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception in GetListData {0}-{1}", ex.Message, ex.InnerException);
#endif
            }

            return dta;
        }

        public static async Task<bool> SendData(string method, string json)
        {
            var success = false;
            var url = string.Format("{0}/{1}", Constants.WorkplaceUrl, method);
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await client.PostAsync(url, content);
                success = response.IsSuccessStatusCode;
            }
            return success;
        }
    }
}
