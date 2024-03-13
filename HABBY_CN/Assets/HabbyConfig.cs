//no use

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HabbyConfig
{
         
    public readonly string GameName;
    public readonly bool HasLicense;
    public readonly int AgeRange;
    public readonly string Description;
    public readonly string ServerURL;
        

    public HabbyConfig(string gameName,bool hasLicense,int applicableRange,string description,string serverURL)
    {
        GameName = gameName;
        HasLicense = hasLicense;
        AgeRange = applicableRange;
        Description = description;
        ServerURL = serverURL;
    }
         
    public class Builder
    {
        private string _gameName;
        private bool _hasLicense;
        private int _ageRange;
        private string _description;
        private string _serverURL;


        public Builder()
        {
        }
             
        public Builder GameName(string gameName)
        {
            _gameName = gameName;
            return this;
        }
             
        public Builder HasLicense(bool hasLicense)
        {
            _hasLicense = hasLicense;
            return this;
        }
             
        public Builder AgeRange(int ageRange)
        {
            _ageRange = ageRange;
            return this;
        }
             
        public Builder Description(string description)
        {
            _description = description;
            return this;
        }

        public Builder ServerURL(string serverURL)
        {
            _serverURL = serverURL;
            return this;
        }
             

        public HabbyConfig ConfigBuilder()
        {
            return new HabbyConfig(_gameName,_hasLicense,_ageRange,_description,_serverURL);
        }
    }
}

/// <summary>
/// Twitter的分布式自增ID雪花算法
/// </summary>
public class IdWorker
{
    //起始的时间戳
    private static long START_STMP = 1480166465631L;

    //每一部分占用的位数
    private static int SEQUENCE_BIT = 12; //序列号占用的位数
    private static int MACHINE_BIT = 5;   //机器标识占用的位数
    private static int DATACENTER_BIT = 5;//数据中心占用的位数

    //每一部分的最大值
    private static long MAX_DATACENTER_NUM = -1L ^ (-1L << DATACENTER_BIT);
    private static long MAX_MACHINE_NUM = -1L ^ (-1L << MACHINE_BIT);
    private static long MAX_SEQUENCE = -1L ^ (-1L << SEQUENCE_BIT);

    //每一部分向左的位移
    private static int MACHINE_LEFT = SEQUENCE_BIT;
    private static int DATACENTER_LEFT = SEQUENCE_BIT + MACHINE_BIT;
    private static int TIMESTMP_LEFT = DATACENTER_LEFT + DATACENTER_BIT;

    private long datacenterId = 1;  //数据中心
    private long machineId = 1;     //机器标识
    private long sequence = 0L; //序列号
    private long lastStmp = -1L;//上一次时间戳

    #region 单例:完全懒汉
    private static readonly Lazy<IdWorker> lazy = new Lazy<IdWorker>(() => new IdWorker());
    public static IdWorker Singleton { get { return lazy.Value; } }
    private IdWorker() { }
    #endregion

    public IdWorker(long cid, long mid)
    {
        if (cid > MAX_DATACENTER_NUM || cid < 0) throw new Exception($"中心Id应在(0,{MAX_DATACENTER_NUM})之间");
        if (mid > MAX_MACHINE_NUM || mid < 0) throw new Exception($"机器Id应在(0,{MAX_MACHINE_NUM})之间");
        datacenterId = cid;
        machineId = mid;
    }

    /// <summary>
    /// 产生下一个ID
    /// </summary>
    /// <returns></returns>
    public long nextId()
    {
        long currStmp = getNewstmp();
        if (currStmp < lastStmp) throw new Exception("时钟倒退，Id生成失败！");

        if (currStmp == lastStmp)
        {
            //相同毫秒内，序列号自增
            sequence = (sequence + 1) & MAX_SEQUENCE;
            //同一毫秒的序列数已经达到最大
            if (sequence == 0L) currStmp = getNextMill();
        }
        else
        {
            //不同毫秒内，序列号置为0
            sequence = 0L;
        }

        lastStmp = currStmp;

        return (currStmp - START_STMP) << TIMESTMP_LEFT       //时间戳部分
                      | datacenterId << DATACENTER_LEFT       //数据中心部分
                      | machineId << MACHINE_LEFT             //机器标识部分
                      | sequence;                             //序列号部分
    }

    private long getNextMill()
    {
        long mill = getNewstmp();
        while (mill <= lastStmp)
        {
            mill = getNewstmp();
        }
        return mill;
    }

    private long getNewstmp()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }
    
}

// void Machine1()
// {
//     for (int j = 0; j < 500; j++)
//     {
//         Task.Run(() =>
//         {
//             IdWorker idworker = IdWorker.Singleton;
//             for (int i = 0; i < 10; i++)
//             {
//                 Debug.Log(idworker.nextId());
//             }
//         });
//     }
// }
//
// void Machine5()
// {
//     List<IdWorker> workers = new List<IdWorker>();
//     System.Random random = new System.Random();
//     for (int i = 0; i < 5; i++)
//     {
//         workers.Add(new IdWorker(1, i + 1));
//     }         
//     for (int j = 0; j < 500; j++)
//     {
//Task.Run(() =>
//         {
//             for (int i = 0; i < 10; i++)
//             {
//                 int mid = random.Next(0, 5);
//                 Debug.Log(workers[mid].nextId());
//             }
//         });
//     }
// } v


namespace ChainofResponsibility
{
    // 采购请求
    public class PurchaseRequest
    {
        // 金额
        public double Amount { get; set; }
        // 产品名字
        public string ProductName { get; set; }
        public PurchaseRequest(double amount, string productName)
        {
            Amount = amount;
            ProductName = productName;
        }
    }

    // 审批人,Handler
    public abstract class Approver
    {
        public Approver NextApprover { get; set; }
        public string Name { get; set; }
        public Approver(string name)
        {
            this.Name = name;
        }
        public abstract void ProcessRequest(PurchaseRequest request);
    }

    // ConcreteHandler
    public class Manager : Approver
    {
        public Manager(string name)
            : base(name)
        { }

        public override void ProcessRequest(PurchaseRequest request)
        {
            if (request.Amount < 10000.0)
            {
                Debug.LogFormat("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
            }
            else if (NextApprover != null)
            {
                NextApprover.ProcessRequest(request);
            }
        }
    }

    // ConcreteHandler,副总
    public class VicePresident : Approver
    {
        public VicePresident(string name)
            : base(name)
        { 
        }
        public override void ProcessRequest(PurchaseRequest request)
        {
            if (request.Amount < 25000.0)
            {
                Debug.LogFormat("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
            }
            else if (NextApprover != null)
            {
                NextApprover.ProcessRequest(request);
            }
        }
    }

    // ConcreteHandler，总经理
    public class President :Approver
    {
        public President(string name)
            : base(name)
        { }
        public override void ProcessRequest(PurchaseRequest request)
        {
            if (request.Amount < 100000.0)
            {
                Debug.LogFormat("{0}-{1} approved the request of purshing {2}", this, Name, request.ProductName);
            }
            else
            {
                Debug.LogFormat("Request需要组织一个会议讨论");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PurchaseRequest requestTelphone = new PurchaseRequest(4000.0, "Telphone");
            PurchaseRequest requestSoftware = new PurchaseRequest(10000.0, "Visual Studio");
            PurchaseRequest requestComputers = new PurchaseRequest(40000.0, "Computers");

            Approver manager = new Manager("LearningHard");
            Approver Vp = new VicePresident("Tony");
            Approver Pre = new President("BossTom");

            // 设置责任链
            manager.NextApprover = Vp;
            Vp.NextApprover = Pre;

            // 处理请求
            manager.ProcessRequest(requestTelphone);
            manager.ProcessRequest(requestSoftware);
            manager.ProcessRequest(requestComputers);
            Console.ReadLine();
        }
    }
}