
def is_valid_invoke_payload(payload):
    if ('features' not in payload):
        return (True, "payload:features is required.")
    if ('items' not in payload):
        return (False, "payload:items is required.")
    else:
        return (True, None)

def is_valid_invoke_optimiser_request_body(req_body):

    if 'payload' not in req_body:
        return (False, "the payload property is required")
    
    return is_valid_invoke_payload(req_body['payload'])

def is_valid_create_optimiser_request_body(req_body: dict):
    if 'id' not in req_body:
        return (False, "the id property is required")
    if 'name' not in req_body:
        return (False, "the name property is required")
    if 'items' not in req_body:
        return (False, "the items property is required")
    if 'defaultItem' not in req_body:
        return (False, "the defaultItem property is required")
    if 'nItemsToRecommend' not in req_body:
        return (False, "the nItemsToRecommend property is required")
    else:
        return (True, None)
