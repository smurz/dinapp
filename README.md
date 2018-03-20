# DINAPP

###### .net standard 1.1 sdk

[![NuGet version](https://badge.fury.io/nu/DINAPP.Libs.Client.svg)](https://badge.fury.io/nu/DINAPP.Libs.Client)

What is DINAPP?
===============
DINAPP is the monetization platform based on the Ethereum blockchain. It allows to sell digital contents from inside of your application and acquire revenue in ethereum coins. You can use this platform to sell a wide range of content, including downloadable content such as media files or photos, virtual content such as game levels or potions, premium services and features, and more.

##### Supported types of in-app purchases:
* Permanent subscription 
* Time-limited subscription
* Standard in-app products

When you use the DINAPP platform to sell an item, whether it's an in-app product or a subscription, DINAPP handles all checkout details so your application never has to directly process any ethereum transactions.

How it works?
=============
##### DINAPP smart contracts
DINAPP includes a smart contract called In-App Generator that is used to create in-app products and store info about created ones. All created In-App products are smart contracts with their own addresses on ethereum blockchain. You don't have to publish them by yourself, just send transaction to Generator and In-App purchase contract will be published with the specified parameters (name, type, price, duration, etc...) and ready to work. 

All In-App contracts have fallback functions to handle payment and getMoney function to retrieve developers revenue. Fallback functions are designed to give out one in-app purchase token at once and return the change if user sends more ethereum than required by the price.

**In-App Generator**
In-App Generator contract is published on Ethereum Main Net and Rinkeby, Ropsten test networks. You can access its functionality with [Contract ABI from this repo](https://github.com/smurz/dinapp/blob/master/Abi/InAppGeneratorAbi.json)
| **Network** | **Address** |
|---------:|:---------|
|Main Net |`0xdb178152ae492bee5b4f4f5f40d1befbfdfc064b`|
|Rinkeby|`0x31aaf76e08ca427eebb987c9e15d6aef0068e722`|
|Ropsten|`0x703bb0c92e5839d482dbf4bc387dceef14206a98`|

#### DINAPP client library
Is developed targeting netstandard 1.1, hence it is compatible with all the operating systems (Windows, Linux, MacOS, Android and OSX).

###### Client library features:
1) Check the subscription status or the number of in-app products bought.
2) Access all the In-App product info.
3) Simplifies payment process with QR code and ethereum Uri scheme.
4) Allows to acquire In-App product directly with user wallet key storage and password. 
(Though it is not recommended and should be done only if you fully trust the source)


# Roadmap
* Documentation for creating and managing In-App products
* Client library integration code examples
* UI In-App purchase dialog for UWP
* Developers client to simplify in-app creation process and track sales
* UI In-App purchase dialog for Xamarin Forms, WPF
* Ethereum Wallet with Ethereum Uri scheme support and owned In-App Tokens tracking
* ...