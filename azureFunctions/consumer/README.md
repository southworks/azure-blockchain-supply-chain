# Consumer API

Here is the documentation for the Consumer API endpoints.

## Consumer API Endpoints:


[Postman](https://www.postman.com/): [Profile Collection](https://dev.azure.com/southworks/_git/dothraki?path=%2Fdev%2Fsupply_chain%2FazureFunctions%2FuserProfileService%2FProfile.postman_collection.json&version=GBPostman_collections_added) 

#### POST/CreateUser
This is a post request which creates a user. The response should return the user's id.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|name |string  | yes | user's name
 email | string      | yes | user's email

Example Body:

    {
        "name":"myName",
        "email":"test@test.com"
    }

#### POST/GetUserProfile
This is a post request which gets a specific user's profile. The response should return the user's id and their address.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   userId  | string    | yes | user's id|

Example Body:

    {
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/UpdateUserProfile
This is a post request which updates the user profile.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   userId  | string    | yes | user's id|
|name |string  | yes | user's name
 email | string      | yes | user's email

Example Body:

    {
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12",
        "name":"myNameUpdates",
        "email":"updatedTest@test.com"
    }

#### POST/DeleteUser
This is a post request which deletes the user.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   userId  | string    | yes | user's id|

Example Body:

    {
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/CreateOrder
This is a post request which creates an order in the retailer contract. Using item ids from the stock contract. The response should return the order's id.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   itemIds  | uint32[]    | yes | An array of item ids from the stock contract|
|itemQuantities |uint32[]  | yes | An array of amounts of each item
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|

Example Body:

    {
        "itemIds":  [0,1],
        "itemQuantities": [10,20],
        "contractAddress":  Retailer Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/CancelOrder
This is a post request which cancels an order by setting the order status to "Cancelled". It requires that the order status is "New" and the client that made the order is the one to cancel it.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|

Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/ConfirmDelivery
This is a post request which confirms an order by setting the order status to "Delivered". This can only happen if the order status is set to "InTransit" and it can only be done by the client who placed the order.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|

Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/GetOrderStatus
This is a post request which gets a specific order's status by the order's id. The response will return an object with the status number and the label of the status.

JSON Body:  

|Field | Type     | Required | Description
|:-: | :-: | :-:|:-:
|   orderId  | uint32   | yes | order id|
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|


Example Body:

    {
        "orderId":  0,
        "contractAddress":  Retailer Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/GetItem
This is a post request which gets the item's details by it's id. The response should return the item's name, description, category and stock.

JSON Body:  

|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item Id  |
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|

Example Body Body :

    {
        "id":0,
        "contractAddress":  Stock Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }

#### POST/GetItemStock
This is a post request which gets the item's stock by its item id and should return the stock amount.

JSON Body:  


|Field | Type     | Required       | Description
|:-: | :-: | :-:|:-:
|   id  | uint32    | yes | item id  |
| contractAddress | string | yes | target contract address
|   userId  | string    | yes | user's id|

Example Body:

    {
        "id":0,
        "contractAddress":  Stock Contract Address,
        "userId": "6fa9aab1-0651-4425-a238-aba627524b12"
    }