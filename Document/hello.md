# SDKFramework


## 开发背景 & 设计初衷

- 背景
由于各个项目或业务需求的差异，HABBY技术中台在为项目组提供技术支持的过程中，
不得不考虑各种定制化因素，甚至更改原有的技术实现。然而这个过程看似琐碎繁杂，却又有共性。

- 初衷
开发人员想要避免重复的体力劳动，必须快速梳理、分解、抽象业务需求，实现一套通用的技术架构。
从产品和业务的角度出发，我们会根据实际需求以及项目组在使用过程中的意见与反馈，对产品进行迭代。


## 功能

- 版署  `国内申请版号用到的 实名认证，防沉迷..`
- 登录  `暂未实现`
- 支付  `暂未实现`
- 广告  `暂未实现`
- 聚合渠道SDK  `最终愿景`

## 快速开始



## XMind & 目录结构

```
SDKFramework/
├── Account/    账号，官网登录      \
                                    为了提升可读性,以后把这两个模块和SDK相关模块放在另一个SDK外观中，不再和Framework共用一个外观
├── Analytics/  TGA,云控, ...      /

├── Config/     这其中的代码文件对应 Resources/SDKConfig 中的json文件

├── Editor/     编辑器扩展、AssetsBundle构建脚本等

├── Example/    示例场景以及脚本文件

├── Framework/  基础框架,基于MonoBehavior实现了一套执行顺序可控的生命周期，除了UI必须依赖Assets,其他子模块都可以独立运行
    ├── Assets
    ├── UI
    ├── Network
    ├── Message

├── Resources/  以后尽量打成AssetsBundle
    ├── SDKConfig   这里的配置文件是自动生成的，通过EditorWindow配置，详细请看使用说明
    ├── SDKPrefab   所有预制体
    ├── Fonts
    ├── Textures

├── UI/         UI功能目录,开发规范：一个View对应一个Mediator,
                View中存放UI的基本控件,Mediator中处理UI功能,
                开发时通过编辑器菜单栏 SDKFramework/Generator ViewMediator 来生成脚本模版

├── Utils/      通用工具类，扩展方法...
```




<!-- ```cs
if (!InputFully()) return;
HabbyUserClient.Instance.RegisterWithAccount(userId, passward, (response) =>
{
    HLogger.Log($"Register Response Code：{response.code}");
    if (response.code == 0)
    {
        HabbyTextHelper.Instance.ShowTip("注册成功！");
        //Login();
    }
    else
    {
        HabbyTextHelper.Instance.ShowTip("无法创建该用户，请重试");
    }
});
```
我们将不再为项目组单独接入版署SDK，如有使用上的弊端和修改建议随时联系 孟瑞卿 wx：mrq617470300 -->


