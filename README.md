# WechatSdk

## Qc.WechatSdk

`Qc.WechatSdk` 是一个基于 `.NET Standard 2.0` 构建，对微信公众号平台平台的常用接口进行了封装。

### 已实现接口
- 获取AccessToken
- 微信公众号授权登录
- 消息模板发送
- JSSDK

### 使用 WechatSdk


#### 一.安装程序包

[![Nuget](https://img.shields.io/nuget/v/Qc.WechatSdk)](https://www.nuget.org/packages/Qc.WechatSdk/)

- dotnet cli  
  `dotnet add package Qc.ProjectSdk`
- 包管理器  
  `Install-Package Install-Package Qc.WechatSdk`

#### 二.添加配置

> 如需实现自定义存储 AccessToken，动态获取应用配置，可自行实现接口 `IWechatSdkHook`  
> 默认提供 `DefaultWechatSdkHook`，存储 AccessToken 等信息到指定目录(默认./AppData)

```cs
using WechatSdk;
public void ConfigureServices(IServiceCollection services)
{
  //...
  services.AddWechatSdk<WechatSdk.DefaultWechatSdkHook>(opt =>
  {
      opt.AppId = "Wechat AppId";
      opt.Appsecret = "Wechat Appsecret";
  });
  //...
}
```

#### 三.代码中使用

在需要地方注入`WechatService`后即可使用

### WechatConfig 配置项

Wechat文档地址: 

## 示例说明

`Qc.WechatSdk.Sample` 为示例项目，可进行测试
