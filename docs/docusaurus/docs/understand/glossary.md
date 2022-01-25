---
sidebar_position: 1
---

# Glossary

## Common Entity
An entity with a common ID.

## Common ID
An identifier recognisable by a name/number within the Cherry database to uniquely identify each common entity, for eg customers/items/recommenders. 

## Consuming a recommendation
Consuming a recommendation implies that the recommendation has been shown or applied to a Customer. 
By default, Cherry assumes that all recommendations are consumed.

## Customer
The customer entity represents a person who is a potential or active customer using your product/service. A customer is a type of common entity. The Common ID is the same as the Customer ID.

## Entity
A resource available on the Cherry REST API.

## Environment
A way to group Cherry entities. Environments are commonly used to separate dev/test/prod data.

## Event
An event is a tracked activity of a customer while they were using your product/service, for eg: every time a customer purchases from you an event is logged in.

## Event source
A data source to gather customer data.

## Integrations
External tools connected to Cherry that can either be source of customer data or sink for metric values and recommendations.

## Invokation
An invokation is the process by which a recommender produces a recommendation.

## Metric
A customer feature/metric is a value derived from a customers event history and properties. Metric values are provided to recommenders to help them make decisions.

## Metric generator
A metric generator defines how a metric value is calculated. A generator uses filter(events) select(properties) aggregate pattern to calculate a value from a set of customer events.

## Parameters
A parameter is a numeric variable. Each parameter represents a value that can be varied within a process. 

_For eg: Varying the total number of products shown to a customer. In that case the parameter should be called 'number of products', values for the parameter are recommendeded during invocation._

## Recommender
A recommender chooses an item for a customer when it is invoked. Represents the process by which we chose items for customers. Over time a recommender runs an optimisation routine that maximises revenue weighted conversion. 

_For eg: A recommender finds the smallest discount maximise revenue without giving too much away._

## Recommendation
An entity that contains an item that has been recommended for a customer.

## (Recommendable) Item
An item should represent the variations you wish to offer your customers.

_For eg: It could be an offer/discount/product._ 

## Recommendation Destination
A place where the recommendations will be sent. Destinations are set per recommender.

## Tenant
A container for all your Cherry environments and entities.
