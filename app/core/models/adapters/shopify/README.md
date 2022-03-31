# Shopify Integration

## Library

We are using https://github.com/nozzlegear/ShopifySharp

## Application Model

Models were extracted from the ShopifySharp library. Some model properties were left as 'object' type for brevity and to lessen the chance of deserialization error. If needed, please refer to the library to get the required models.

* decimal? are converted to string
