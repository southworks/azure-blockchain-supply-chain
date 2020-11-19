# Retailer API

Here is the API endpoints documentation for Retailer contract.

## Retailer API Endpoints:


[Postman](https://www.postman.com/): [Retailer Collection](./Retailer.postman_collection) 


#### POST/GetOrderItem
This is a post request which gets a specific order's item and the quantity of that item. The response should return the item id and the item's quantity.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
|itemId |uint32  | yes | item id 
| contractAddress | string | yes | target contract address


Example Body:

    {
        "orderId":  0,
        "itemId": 0,
        "contractAddress":  Retailer Contract Address
    }
        
#### Post/GetNewOrders
This is a post request which responds with an array of orders that have the status "New". 

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
| contractAddress | string | yes | target contract address

Example Body:

    {
        "contractAddress":  Retailer Contract Address
    }

#### POST/AcceptOrder
This is a post request which accepts an order if the the order status is "New".

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address


Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address
    }

#### POST/CheckOrderItemsAvailability
This is a post request which checks if an item exits.  

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address

Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address
    }

#### POST/DeliverOrder
This is a post request which changes the order status to "In Transit" only if the the order status is "Accepted".

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address

Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address
    }

#### POST/RejectOrder
This is a post request which rejects the order by setting the order status to "Rejected" only if the order status is "New".

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address

Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address
    }

#### POST/UpdateMinItemsPerOrder
This is a post request which updates the minimum amount allowed of an item.  

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | minItems   | yes | order id|
| contractAddress | string | yes | target contract address

Example Body:

    {
        "minItems":  3,
        "contractAddress":  Retailer Contract Address
    }

#### POST/UpdateStockContract
This is a post requests that updates the stock contract by passing it a new contract address.  

JSON BODY:

|Field | Type     | Required | Description |
|:-: | :-: | :-:|:-: |
|   address  | newContract   | yes |the address of the new contract|
| contractAddress | string | yes | target contract address

Example Body:

    {
        "newContract":  new contract address,
        "contractAddress":  Retailer Contract Address
    }
