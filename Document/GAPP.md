## 版署SDK接入流程

### 一、根据游戏类型从EditorWindow配置：

#### 拖入PureCN.unitypackage之后打开编辑器菜单 SDKFramework/Configuration Application

![](C:\Users\WIN11\Desktop\57386a5672cdafd3fad95c3025738b9.png)

### 二、国内申请版号版本所需的功能接口：

#### 1、初始化，运行SDK（在下方代码注释位置填写进入游戏的逻辑）（可以在入口脚本的Start方法中调用）
 ```cs
SDK MRQ = SDK.New();
MRQ.Run(new SDK.ProcedureOption()
{
    Splash = () =>
    {
        HabbyFramework.UI.OpenUI(UIViewID.SplashAdviceUI);
    },
    Login = () =>
    {
        HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
    },
    EnterGame = () =>
    {
        //Write your logic for entering the game
        HLogger.Log("宿主程序进入成功!!!");
    },
});
```

#### 2、开始登录版署服务器流程（当游戏启动Loading结束后调用）
```cs
SDK.Procedure?.Login();
```

#### 3、在商店界面中添加一个按钮，点击按钮调用如下接口
```cs
HabbyFramework.UI.OpenUI(UIViewID.PurchaseRulesUI);
```

#### 4、刷新玩家当前月消费额度（从游戏服拿取数据，传入以下接口中）（可以在游戏服登录成功之后调用）
```cs
HabbyFramework.Account.RefreshMonthlyExpense(/*传入商品金额,以分为单位*/);
```

#### 5、每次商品支付成功之后调用
```cs
HabbyFramework.Account.CanPurchase(/*传入商品金额,以分为单位*/);
```

#### 6、切换账号（注意调用之前客户端清除游戏服数据）
```cs
HabbyFramework.Account.Logout(1);//调用该接口传入1，会退出当前账号并打开登录界面
```



### 如果需要从lua层调用需要注册 typeof(SDK)，typeof(HabbyFramework)

-------------------------------------------------------------------

### 使用过程中有任何问题，联系孟瑞卿，wx：mrq617470300