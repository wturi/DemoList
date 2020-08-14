using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace Demo1
{
    internal class Program
    {
        /// <summary>
        /// 马婧
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            NewTest();

            Console.ReadLine();
        }

        private static void NewTest()
        {
            string str = File.ReadAllText(@"../../RD100220070299.txt", Encoding.GetEncoding("GB2312"));

            var allRowName = new string[] { "ATM取款发卡", "ATM取款受理", "ATM存款发卡", "ATM转账转入方", "ATM转账转出方", "ATM转账受理方", "ATM查询发卡", "ATM查询受理", "网上联机退货发卡", "网上消费受理", "柜面取款发卡方", "柜面取款受理方", "柜面存款发卡方", "柜面存款受理方",
                "柜面转账转入方", "柜面转账转出方", "柜面转账受理方","农民工取款发款方","现金充值发卡","ATM调账发卡","ATM调账受理","柜面通调账受理等","POS消费发卡","POS消费撤销发卡","POS预授权完成发卡","POS退货发卡","POS查询发卡方","手工预授权完成发卡方","固话消费发卡",
            "其他转账转入","其它转账转出","助农取款发卡方","网上调账发卡","指定账户圈存发卡","指定账户圈存受理","例外交易","其它消费发卡","其它查询发卡方","其它查询受理方","自助消费发卡","自助消费撤销发卡","自助预授权完成发卡","批量代付发卡","批量代收发卡","品牌服务费",
            "周期计费发卡","账户信息验证发卡","借记签约发卡","手工退货发卡方","POS调账发卡","脱机消费发卡","收付费","网上消费受理","间联POS消费受理","间联自助消费受理","网上联机退货受理","间联POS退货受理","直联POS消费受理"};

            var rowNameList = new List<string>();
            List<List<string>> numList = new List<List<string>>();

            foreach (var name in allRowName)
            {
                if (str.Contains(name))
                {
                    rowNameList.Add(name);
                }
            }

            var d2 = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 1; i < d2.Length; i++)
            {
                var dd = new List<string>();
                var d1 = d2[i].Split(new string[] { " ", "\n", "\r", "-", "小", "计" }, StringSplitOptions.RemoveEmptyEntries);

                dd = d1.ToList();

                numList.AddRange(rowNameList.Where(rowName => d2[i].Contains(rowName)).Select(rowName => dd));
            }
            //交易类型1
            decimal jiaohuanjie3 = 0;
            decimal zhaunjieqingsuanjie = 0;
            decimal jiesum = 0;
            decimal jiaohuandai3 = 0;
            decimal zhaunjieqingsuandai = 0;
            decimal daisum = 0;

            //交易类型2
            decimal jiaohuanjie = 0;
            decimal zhuanjieqingsuanjie1 = 0;
            decimal jiesum1 = 0;
            decimal daisum1 = 0;
            decimal jiaohuandai = 0;
            decimal zhuanjieqingsuandai1 = 0;
            decimal sum1 = 0;

            //交易类型3
            decimal jiaoyijie1 = 0;
            decimal jiaohuanjie2 = 0;
            decimal jiaohuandai2 = 0;
            decimal zhuanjieqingsuanjie2 = 0;
            decimal zhuanjieqingsuandai2 = 0;
            decimal jiesum3 = 0;
            decimal daisum3 = 0;
            decimal sum3 = 0;
            //交易类型4
            decimal shoufufei = 0;

            //交易类型5
            decimal zijinqingsuanjie = 0;
            decimal zijinqingsuandai = 0;
            decimal cha = 0;
            //交易类型6
            decimal jiaohuanjie1 = 0;
            decimal jiaohuandai1 = 0;
            decimal zijinqingsuanjie1 = 0;
            decimal zijinqingsuandai1 = 0;
            decimal jiesum2 = 0;
            decimal daisum2 = 0;
            decimal sum2 = 0;
            foreach (var item in numList)
            {
                string rowname = item[0];
                if (rowname.StartsWith("ATM") || rowname.StartsWith("柜面") || rowname.Equals("网上转账转入") || rowname.Equals("网上联机退货发卡") || rowname.Equals("农民工取款发款方") || rowname.Equals("现金充值发卡"))
                {
                    jiaohuanjie3 += Convert.ToDecimal(item[4]);
                    zhaunjieqingsuanjie += Convert.ToDecimal(item[6]);
                    jiesum = jiaohuanjie3 + zhaunjieqingsuanjie;
                    jiaohuandai3 += Convert.ToDecimal(item[5]);
                    zhaunjieqingsuandai += Convert.ToDecimal(item[7]);
                    daisum = jiaohuandai3 + zhaunjieqingsuandai;
                }
                else if (rowname.StartsWith("POS") || rowname.StartsWith("其它") || rowname.StartsWith("手工") || rowname.Equals("固话消费发卡") || rowname.Equals("助农取款发卡方") || rowname.Equals("网上调账发卡") ||
                    rowname.StartsWith("指定") || rowname.StartsWith("自助") || rowname.StartsWith("批量") ||
                    rowname.Equals("例外交易") || rowname.Equals("品牌服务费") || rowname.Equals("周期计费发卡") || rowname.Equals("账户信息验证发卡") || rowname.Equals("借记签约发卡") || rowname.Equals("手工退货发卡方"))
                {
                    jiaohuanjie += Convert.ToDecimal(item[4]);
                    zhuanjieqingsuanjie1 += Convert.ToDecimal(item[6]);
                    jiaohuandai += Convert.ToDecimal(item[5]);
                    zhuanjieqingsuandai1 += Convert.ToDecimal(item[7]);
                    jiesum1 = jiaohuanjie + zhuanjieqingsuanjie1;
                    daisum1 = jiaohuandai + zhuanjieqingsuandai1;
                    sum1 = jiesum1 + daisum1;
                }
                else if (rowname.Equals("脱机消费发卡"))
                {
                    jiaoyijie1 += Convert.ToDecimal(item[2]);
                    jiaohuanjie2 += Convert.ToDecimal(item[4]);
                    zhuanjieqingsuanjie2 += Convert.ToDecimal(item[6]);
                    jiesum3 = jiaohuanjie2 + zhuanjieqingsuanjie2;
                    jiaohuandai2 += Convert.ToDecimal(item[5]);
                    zhuanjieqingsuandai2 += Convert.ToDecimal(item[7]);
                    daisum3 = jiesum3 + zhuanjieqingsuandai2;
                }
                else if (rowname.StartsWith("收付费"))
                {
                    if (Convert.ToDecimal(item[2]) == 0)
                    {
                        shoufufei += shoufufei;
                    }
                    else
                    {
                        shoufufei = Convert.ToDecimal(item[3]);
                    }
                }
                else if (rowname.Equals("收付费") || rowname.Equals("间联POS消费受理") || rowname.Equals("间联自助消费受理") || rowname.Equals("网上联机退货受理") || rowname.Equals("间联POS退货受理"))
                {
                    zijinqingsuanjie += Convert.ToDecimal(item[8]);
                    zijinqingsuandai += Convert.ToDecimal(item[9]);
                    cha = zijinqingsuanjie - zijinqingsuandai;
                }
                else if (rowname.Equals("直联POS消费受理"))
                {
                    jiaohuanjie1 += Convert.ToDecimal(item[4]);
                    zijinqingsuanjie1 += Convert.ToDecimal(item[8]);
                    jiesum2 = jiaohuanjie1 + zijinqingsuanjie1;
                    jiaohuandai1 += Convert.ToDecimal(item[5]);
                    zijinqingsuandai1 += Convert.ToDecimal(item[9]);
                    daisum2 = jiaohuandai1 + zijinqingsuandai1;
                    sum2 = jiesum2 + daisum2;
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(numList));

            Console.WriteLine(JsonConvert.SerializeObject(numList.Where(i => i.Contains("消费受理"))));
        }
    }
}