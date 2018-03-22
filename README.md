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

###### In-App Generator
In-App Generator contract is published on Ethereum Main Net and Rinkeby, Ropsten test networks. You can access its functionality with [Contract ABI from this repo](https://github.com/smurz/dinapp/blob/master/Abi/InAppGeneratorAbi.json) and addresses:

* Main Net `0xdb178152ae492bee5b4f4f5f40d1befbfdfc064b`
* Rinkeby  `0x31aaf76e08ca427eebb987c9e15d6aef0068e722`
* Ropsten  `0x703bb0c92e5839d482dbf4bc387dceef14206a98`

#### DINAPP client library
Is developed targeting netstandard 1.1, hence it is compatible with all the operating systems (Windows, Linux, MacOS, Android and OSX).

###### Client library features:
1) Check the subscription status or the number of in-app products bought.
2) Access all the In-App product info.
3) Simplifies payment process with QR code and ethereum Uri scheme.
4) Allows to acquire In-App product directly with user wallet key storage and password. 
(Though it is not recommended and should be done only if you fully trust the source)

Creating In-App using Ethereum Wallet
===============
[WatchGeneratorContract]: /Screenshots/WatchGeneratorContract.png "Watch Generator Contract"
[GeneratorInfo]: /Screenshots/GeneratorInfo.png "Generator Info"
[SelectGeneratorFunction]: /Screenshots/SelectGeneratorFunction.png "Generator's Functions List"
[CreateSubscribtion]: /Screenshots/CreateSubscribtion.png "Create Subscription In-App"
[ExecuteCreation]: /Screenshots/ExecuteCreation.png "Execute Creation of Subscription In-App"
[CheckTransaction]: /Screenshots/CheckTransaction.png "Check Transaction"
[DevCountInApps]: /Screenshots/DevCountInApps.png "Developer's In-Apps amount"
[InAppInfo]: /Screenshots/InAppInfo.png "In-App Information"
[WatchInApp]: /Screenshots/WatchInApp.png "Watch In-App Contract"
[InAppFunctions]: /Screenshots/InAppFunctions.png "In-App Contract's Functions"

For creating In-App using Wallet you should watch Generator Contract in "Contracts" section in application.
Take address of In-App Generator and [Contract ABI of In-AppGenerator](/Abi/InAppGeneratorAbi.json) for fields of watching.

![WatchGeneratorContract]

Open generator info in list of your watching contracts.

![GeneratorInfo]

In a field "Select function" select type of In-App. There are 3 types: Subscription, Permanent and Consumable In-Apps.

![SelectGeneratorFunction]

Paste all needed information in fields. Be careful: Set buy price in wei, not in ether. `1000000000000000000 Wei == 1 Ether`.
For more information read information about ERC20 protocol.

![CreateSubscribtion]

Confirm executing of creation transaction.

![ExecuteCreation]

Wait while this transaction will be confirmed.

![CheckTransaction]

Back to the In-App Generator information and find count of your In-Apps.

![DevCountInApps]

You can see information of your In-App by developer address and index.

![InAppInfo]

Watch info by In-App address and ABI of In-App for [Subscribtion and Permanent In-App](/Abi/SubscriptionInAppAbi.json) or [Consumable In-App](/Abi/ConsumableInAppAbi.json)

![WatchInApp]

You can manage your In-App by In-Apps Functions

![InAppFunctions]


Creating In-App using Web3
===============
Getting generator contract.
```js
var AbiOfContract = /* Paste here json abi of Generator */
var generatorAbi = web3.eth.contract(AbiOfContract);
var contractAddress = /* In-App Generator address of current Network */
var generatorContract = generatorAbi.at(contractAddress);
```
For deploying contracts you should sign transaction. About signing read [documentation of Web3](http://web3js.readthedocs.io/en/1.0/web3-eth.html?highlight=signTransaction#signtransaction)
#### Permanent In-App
Creating permanent In-App using Web3.
```js
var buyPriceInWei = /* Price in Wei. It should be more than 1000000 */;
var inAppName = /* Name of new In-App */;
var symbol = /* ERC20 symbol of contract */;
var description = /* Description of In-App */;
var projectName = /* Name of project */;
var addressOfDeveloper = /* You will deploy In-App from this address */;
var gasAmount = /* Amount of gas for deploy contract. We recomend use ~2000000 */

generatorContract.createPermanent(buyPriceInWei, inAppName, symbol, description, projectName).send({from: addressOfDeveloper, gas: gasAmount}); 
```
#### Subscription In-App
Creating subscription In-App using Web3.
```js
var buyPriceInWei = /* Price in Wei. It should be more than 1000000 */;
var amountOfDays = /* Subscription duration in days */
var inAppName = /* Name of new In-App */;
var symbol = /* ERC20 symbol of contract */;
var description = /* Description of In-App */;
var projectName = /* Name of project */;
var addressOfDeveloper = /* You will deploy In-App from this address */;
var gasAmount = /* Amount of gas for deploy contract. We recomend use ~2000000 */

generatorContract.createSubscription(buyPriceInWei, amountOfDays, inAppName, symbol, description, projectName).send({from: addressOfDeveloper, gas: gasAmount}); 
```
#### Consumable In-App
Creating Consumable In-App using Web3.
```js
var buyPriceInWei = /* Price in Wei. It should be more than 1000000 */;
var inAppName = /* Name of new In-App */;
var symbol = /* ERC20 symbol of contract */;
var description = /* Description of In-App */;
var projectName = /* Name of project */;
var addressOfDeveloper = /* You will deploy In-App from this address */;
var gasAmount = /* Amount of gas for deploy contract. We recomend use ~2000000 */

generatorContract.createConsumable(buyPriceInWei, inAppName, symbol, description, projectName).send({from: addressOfDeveloper, gas: gasAmount}); 
```
After the creating of In-App you should wait while contract will be confirm. You can get address of new contract using this code.
```js
var developerAddress = /* Address of developer who deploy the contract */;
generatorContract.inAppInfoCount(developerAddress); // In console you will see count of your InApps
generatorContract.inAppInfos(developerAddress, /* Count_of_your_InApps */ - 1);
```
You can get all information about your InApp including address. Use this address in your application.


# DINAPP.Libs.Client integration
Take a look at [our code samples here](https://github.com/smurz/dinapp/tree/master/NetSamples) for better understanding of client library integration process with UWP app. There is also [an awsome in-app purchase dialog](https://github.com/smurz/dinapp/tree/master/NetSamples/DINAPP.Libs.Client.UWP) example there too. Feel free to just add it to your UWP app and modify its style as you wish.

#### Install and initialize
 Add DINAPP.Libs.Client reference via nuget
`Install-Package DINAPP.Libs.Client -Version 1.0.0`

Create an instance of `InAppSeviceBuilder`. It takes `EthereumNetwork` enum as parameter, choose `EthereumNetwork.MainNet` for your real-life products or one of the test networks (Rinkeby or Ropsten) for tests or debug builds. Then build an instance of `InAppService`.

``` C#
    private IInAppService _inAppService;

    var inAppServiceBuilder = new InAppSeviceBuilder(EthereumNetwork.MainNet);
    _inAppService = inAppServiceBuilder.Build();
```

#### Read in-app product info
Before you charge your user some ethereum it is recommended to show some product info.
You can get all in-app product info with `IInAppInfoReader`
```C#
    var inAppInfo = await _inAppService.InAppInfoReader
                                       .GetProductInfoAsync("<paste your in-app contract address>");
```
If your in-app product is correctly published on the selected ethereum network you will get an object with this interface as a result.
```C#
  public interface IInAppInfo
  {
    string Name { get; }
    string ProjectName { get; }
    BigInteger PriceInWei { get; }
    BigInteger DurationInDays { get; }
    uint InAppType { get; }
    string ContractAdress { get; }
    string Description { get; }
  }
```
You can also view the In-App contract publisher address and in-app product index for publisher in Generator
```C#
var publisherInfo = await _inAppService.InAppInfoReader
                                       .GetInAppRegisteredByAddressAsync("<paste your in-app contract address>");
```
#### Check in-app purchase status
In-App purchase status can be checked with `ILicenseChecker`. All status checks require User Wallet Address, that user used to buy an in-app product from. Prompt user to enter it somewhere and store it for further use.
Check Time-Limited and Permanent subscriptions status with `GetSubscriptionStatusAsync`
```C#
            var subscriptionStatus =
                await _inAppService.LicenseChecker.GetSubscriptionStatusAsync(
                    "0x... subscription in-app address",
                    "0x... user wallet address");
            if(subscriptionStatus.IsActive)
            {
                // unlock some cool features for the subscribed user
            }
```
Check the number of simple in-app products bought with `GetConsumableStatusAsync`
```C#
            var consumableStatus = await _inAppService.LicenseChecker.GetConsumableStatusAsync(
                "0x... consumable in-app address",
                "0x... user wallet address");
            var productsCount = consumableStatus.Count;
```
Ethereum blockchain has some restrictions, so we can't track how user spends your in-app consumables with our contract (otherwise he would have to pay for each transaction), so you should store the initial Count somewhere and track the change. We will have some examples on how to implement this later.

#### Payment methods
The most simple way to let user acquire **In-App product** is to show the **contract address** and its **price in Ether**. You can get both from `InAppInfoReader` as shown in example above. After that user can go to their wallet and use **In-App contract fallback** or **handlePayment** function to acquire in-app token. 

Our client library provides some methods to simplify payment process such as
* ethereum URI scheme
``` C#
var uriScheme = await _inAppService.Payment.GetPaymentUriSchemeAsync("0x... in-app address");
```
* QR code
``` C#
var qrCode = await _inAppService.Payment.GetQrCodeMatrixAsync("0x... in-app address");
```
* Direct authorized payment. (Not recommended, if you don't fully trust the source)
```C#
                //user wallet password
                var password = "";
                //read user wallet keystore .json file
                var keystore = "";

                //create new instance of KeyStorageInfo
                var keyStorageInfo = new KeyStorageInfo(password, keystore);

                //gas price = 2 Gwei. set it to what ever price is apropriate.
                var gasPrice = new BigInteger(2000000000);

                //handle payment
                //PayAuthorizedAsync method return true if payment was successfull
                var result = await _inAppService.Payment.PayAuthorizedAsync(
                                                            InAppAddress, 
                                                            UserWalletAddress, 
                                                            keyStorageInfo, 
                                                            gasPrice);

                if (result)
                {
                    //check in app purchase status
                    CheckInAppStatus();
                }
```

# Roadmap
* Documentation for creating and managing In-App products
* UI In-App purchase dialog for UWP
* Developers client to simplify in-app creation process and track sales
* UI In-App purchase dialog for Xamarin Forms, WPF
* Ethereum Wallet with Ethereum Uri scheme support and owned In-App Tokens tracking
* ...
