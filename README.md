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
