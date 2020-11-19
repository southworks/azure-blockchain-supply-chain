# Stock API

Here is the API endpoints documentation for Stock contract.

## Stock API Endpoints:

[Postman](https://www.postman.com/): [Stock Collection](./Stock.postman_collection)

#### POST/CreateItem
This is a post request which creates an item in the stock contract. The response should return the item id.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   name  | string    | yes | name of item  |
|description |string  | yes | description of item
category | string      | yes | category of item
| stock |  uint32 | yes | amount of stock
| contractAddress | string | yes | target contract address

Example Body:

    {
        "name":  "itemName",
        "description": "description of item",
        "category": "item category",
        "stock": 500,
        "contractAddress":  Stock Contract Address
    }

#### POST/RemoveItem
This is a post request which removes an item specified by the item id.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "contractAddress":  Stock Contract Address
    }

#### POST/IncreaseItemStock
This is a post request which allows you to increase the stock amount on an item specified by the item id.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id |
|  amount  | uint32    | yes | amount to increase the stock by  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "amount":20,
        "contractAddress":  Stock Contract Address
    }

#### POST/DecreaseItemStock
This is a post request which allows you to decrease the stock amount on an item specified by the item id.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id  |
|  amount  | uint32    | yes | amount to decrease the stock by  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "amount":20,
        "contractAddress":  Stock Contract Address
    }

#### POST/UpdateItemCategory
This is a post request which allows you to update the category of an item.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :- | :-:|:-:
|   id  | uint32    | yes | item id  |
|  category  | string    | yes | new category name  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "category": "new category of item",
        "contractAddress":  Stock Contract Address
    }

#### POST/UpdateItemDescription
This is a post request which allows you to update the description of an item.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id |
|  description  | string    | yes | new description  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "description":"new description of item",
        "contractAddress":  Stock Contract Address
    }

#### POST/UpdateItemName
This is a post request which allows you to update the name of an item.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id  |
|  name  | string    | yes | new name of item  |
| contractAddress | string | yes | target contract address

Example Body:

    {
        "id":0,
        "name":"newName",
        "contractAddress":  Stock Contract Address
    }
