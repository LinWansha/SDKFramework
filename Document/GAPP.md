## 版署SDK接入流程

### 一、引入、下载SDK：

##### 1. [下载unitypackage](https://raw.githubusercontent.com/LinWansha/SDKFramework/main/Product/PureCN_GAPP.unitypackage)
##### 2. 通过 UPM (Unity Package Manager) 引入：com.habby.sdkframework


### 二、根据游戏内容填充配置：

#### 引入SDK之后打开编辑器菜单 SDKFramework/Configuration Application

![ConfigurationApp](https://raw.githubusercontent.com/LinWansha/SDKFramework/main/DocImg/ConfigurationApp.png)


### 三、国内申请版号版本所需的功能接口：

#### 1、初始化，运行SDK（在下方代码注释位置填写进入游戏的逻辑）（可以在入口脚本的Start方法中调用）
 ```cs
SDK MRQ = SDK.New();
MRQ.Run(new SDK.ProcedureOption()
{
    Splash = () =>
    {
        HabbyFramework.UI.OpenUI(UIViewID.SplashAdviceUI);
    },
    Login = (token) =>
    {
        //use this token to login game server
        Debug.Log($"版署服务器登录成功,Persistent token: {token}");
    },
    EnterGame = () =>
    {
        //Write your logic for entering the game
        Debug.Log("宿主程序进入成功!!!");
    },
});
```

#### 2、打开登录界面UI（当游戏启动Loading结束后调用）
```cs
HabbyFramework.UI.OpenUI(UIViewID.EntryUI);
```

#### 3、在商店界面中添加一个按钮，点击按钮调用如下接口
```cs
HabbyFramework.UI.OpenUI(UIViewID.PurchaseRulesUI);
```

#### 4、刷新玩家当前月消费额度（可以在游戏服登录成功之后调用）
```cs
HabbyFramework.Account.RefreshMonthlyExpense(/*传入商品金额,以分为单位*/);
```

#### 5、用户点击商品拉起订单之前调用
```cs
HabbyFramework.Account.CanPurchase(/*传入商品金额,以分为单位*/);
```

#### 6、切换账号（退出登录，回到登录界面）
```cs
HabbyFramework.Account.Logout(1);//调用该接口传入1，会退出当前账号并打开登录界面
```



### 如果需要从lua层调用，请按需注册以上类型到lua虚拟栈
-------------------------------------------------------------------

### 使用过程中有任何问题，联系孟瑞卿，wx：mrq617470300
### 期待您的意见与反馈！！！