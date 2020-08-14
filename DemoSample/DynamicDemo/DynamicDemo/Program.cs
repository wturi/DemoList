using System;

using Newtonsoft.Json;

namespace DynamicDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var returnObj = GetDynamicClassByDataTable();
            //Console.WriteLine(JsonConvert.SerializeObject(returnObj));

            Run1();

            Console.ReadLine();
        }

        private static void Run1()
        {
            var data = new
            {
                result = 1,
                response = new
                {
                    data = new
                    {
                        data = new
                        {
                            num = 1,
                        },
                        version = "2020",
                        result = 1
                    }
                },
            };

            var json = "{\"result\":1,\"response\":{\"data\":{\"version\":\"20200603\",\"result\":1,\"timestamp\":1597039219,\"message\":\"success\",\"id\":\"f6197efdef280309dd4b206db6a44d82\",\"sha1\":\"6c560be5d659ebc3842a7468fcaea559ef96169b\",\"time_cost\":\"627\",\"identify_results\":[{\"type\":\"10506\",\"orientation\":0,\"details\":{\"user_name\":\"周泓\",\"user_id\":\"310104198509180893\",\"number\":\"7812333426553\",\"check_code\":\"4073\",\"date\":\"2020年06月02日\",\"agentcode\":\"SHA777,860909\",\"issue_by\":\"障部东方航空股心有票心)(桥保\",\"fare\":\"305.00\",\"tax\":\"\",\"fuel_surcharge\":\"\",\"caac_development_fund\":\"50.00\",\"insurance\":\"XXX\",\"total\":\"355.00\",\"flights\":[{\"from\":\"T2上海虹桥\",\"to\":\"温州\",\"flight_number\":\"FM9515\",\"date\":\"2020年06月02日\",\"time\":\"08:10\",\"seat\":\"V\",\"carrier\":\"上航\"}],\"kind\":\"交通\",\"international_flag\":\"\",\"print_number\":\"13525840731\"},\"extra\":{\"check_code_candidates\":[\"4073\"]}}]}}}";
            Console.WriteLine(json);

            var dynamic = JsonConvert.DeserializeObject<dynamic>(json);

            Console.WriteLine(dynamic.response.data.timestamp);

            Console.ReadLine();
        }

        /// <summary>
        /// 使用dynamic根据DataTable的列名自动添加属性并赋值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static Object GetDynamicClassByDataTable()
        {
            dynamic d = new System.Dynamic.ExpandoObject();
            (d as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>).Add(new System.Collections.Generic.KeyValuePair<string, object>("numA", 1));

            return d;
        }
    }
}