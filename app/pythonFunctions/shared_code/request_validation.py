
def is_duplicate_common_ids(items):
    seen = []
    for i in items:
        common_id = i['commonId']
        if common_id in seen:
            return (True, f'Duplicate common id {common_id}')
        else:
            seen.append(common_id)
    return (False, None)


def is_valid_invoke_payload(payload):
    if ('features' not in payload):
        return (False, "payload:features is required.")
    if ('items' not in payload):
        return (False, "payload:items is required.")

    dup_items, message = is_duplicate_common_ids(payload['items'])
    if dup_items:
        return (False, message)
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
