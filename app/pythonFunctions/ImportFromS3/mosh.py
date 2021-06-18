import logging
import pandas as pd
import numpy as np
from signalboxclient import SignalBoxClient, Credentials, Configuration

signalbox_timestamp_format = '%Y-%m-%dT%H:%M:%S.%f%z'


class Category:
    # Define the kinds of events that can be produced in this file
    def __init__(self, kind: str, event_types):
        self.name = kind
        self.event_types = {}
        for e in event_types:
            self.event_types[e] = e


class Categories:
    def __init__(self):
        self.billing = Category('BILLING', [
                                'SUBSCRIPTION_ACTIVATED', 'SUBSCRIPTION_CANCELED', 'SUBSCRIPTION_TERM_STARTED'])
        self.ticket = Category('TICKET', ['TICKET_CREATED', 'TICKET_CLOSED'])


categories = Categories()


def handle(input: dict, df: pd.DataFrame):
    # the entry function for this file
    logging.info(f'MOSH is handling dataframe with columns: {df.columns}')
    source_config = input.get('source')
    client = create_signalbox_client(input.get('signalbox'))
    file_name = source_config['file']
    if 'subscription' in file_name:
        return handle_subscriptions(client, df)
    elif 'contact' in file_name:
        return handle_contacts(client, df)
    elif 'ticket' in file_name:
        return handle_tickets(client, df)
    else:
        raise Exception(f'Unknown conversion for file: {file_name}')


def create_signalbox_client(config):
    return SignalBoxClient(Credentials(config['apiKey']), Configuration(config['host'], config['port']))


def get_subscription_activated_event(row, common_id_key):
    commonId = row[common_id_key]

    return pd.Series({
        'commonUserId': commonId,
        'eventId': str(row['subscription_id']) + '_ACTIVATED',
        'kind': categories.billing.name,
        'eventType': categories.billing.event_types['SUBSCRIPTION_ACTIVATED'],
        'timestamp': row['activated_at'],
        'properties': {
            'subscription_id': row['subscription_id'],
            'plan': row['plan'],
            'plan_price': row['plan_price'],
        }
    })


def get_subscription_term_start_event(row, common_id_key):
    commonId = row[common_id_key]

    return pd.Series({
        'commonUserId': commonId,
        'eventId': str(row['subscription_id']) + str(row['current_term_end'].strftime(signalbox_timestamp_format)) + '_TERM_STARTED',
        'kind': categories.billing.name,
        'eventType': categories.billing.event_types['SUBSCRIPTION_TERM_STARTED'],
        'timestamp': row['current_term_start'],
        'properties': {
            'subscription_id': row['subscription_id'],
            'plan': row['plan'],
            'plan_price': row['plan_price'],
            'current_term_end': row['current_term_end'].strftime(signalbox_timestamp_format)
        }
    })


def get_subscription_cancel_event(row, common_id_key):
    commonId = row[common_id_key]

    return pd.Series({
        'commonUserId': commonId,
        'eventId': str(row['subscription_id']) + '_CANCELED',
        'kind': categories.billing.name,
        'eventType': categories.billing.event_types['SUBSCRIPTION_CANCELED'],
        'timestamp': row['cancelled_at'],
        'properties': {
            'subscription_id': row['subscription_id'],
            'plan': row['plan'],
            'plan_price': row['plan_price'],
        }
    })


def handle_subscriptions(client: SignalBoxClient, df: pd.DataFrame):
    logging.info('Handling subscriptions DataFrame')
    common_user_id_key = 'common_user_id'
    df = df.loc[(df[common_user_id_key] != None) &
                (df[common_user_id_key] != "")]
    df['activated_at'] = pd.to_datetime(df['activated_at'], unit='s')
    df['cancelled_at'] = pd.to_datetime(df['cancelled_at'], unit='s')
    df['current_term_start'] = pd.to_datetime(
        df['current_term_start'], unit='s')
    df['current_term_end'] = pd.to_datetime(df['current_term_end'], unit='s')
    activated_events_df = df.apply(lambda row: get_subscription_activated_event(
        row, common_user_id_key), axis=1)
    term_started_events_df = df.apply(lambda row: get_subscription_term_start_event(
        row, common_user_id_key), axis=1)
    term_started_events_df = term_started_events_df.loc[term_started_events_df.timestamp > pd.to_datetime(
        '2018-01-01')]  # remove any 1970 datetimes
    canceled_events_df = df.apply(lambda row: get_subscription_cancel_event(
        row, common_user_id_key), axis=1)
    # clean them up
    logging.info(
        f'subs canceled len before datetime cleaning, {len(canceled_events_df)}')
    # fixing min datetimes
    canceled_events_df = canceled_events_df.loc[canceled_events_df.timestamp > pd.to_datetime(
        '1971-01-01')]
    logging.info(
        f'subs len canceled after datetime cleaning, {len(canceled_events_df)}')

    logging.info(f'Converted types. DataFrame had length: {len(df)}')
    activate_events = []
    for index, row in activated_events_df.iterrows():
        e = client.construct_event(row['commonUserId'], row['eventId'], row['eventType'], row['kind'],
                                   row['properties'], row['timestamp'].strftime(signalbox_timestamp_format), None)
        activate_events.append(e)
    print('Uploading', len(activate_events), ' activate events')
    client.log_events(activate_events)

    term_start_events = []
    for index, row in term_started_events_df.iterrows():
        e = client.construct_event(row['commonUserId'], row['eventId'], row['eventType'], row['kind'],
                                   row['properties'], row['timestamp'].strftime(signalbox_timestamp_format), None)
        term_start_events.append(e)
    print('Uploading', len(term_start_events), ' term start events')
    client.log_events(term_start_events)

    cancel_events = []
    for index, row in canceled_events_df.iterrows():
        e = client.construct_event(row['commonUserId'], row['eventId'], row['eventType'], row['kind'],
                                   row['properties'], row['timestamp'].strftime(signalbox_timestamp_format), None)
        cancel_events.append(e)
    print('Uploading', len(cancel_events), ' cancel events')
    client.log_events(cancel_events)

    logging.info('Completed tickets import.')
    return {
        'Processed': len(activate_events) + len(cancel_events)
    }


def get_ticket_create_event(row, common_id_key: str):
    commonId = row[common_id_key]

    return pd.Series({
        'commonUserId': commonId,
        'eventId': str(row['hsObjectId']) + '_CREATED',
        'kind': categories.ticket.name,
        'eventType': categories.ticket.event_types['TICKET_CREATED'],
        'timestamp': row['closed_data'],
        'properties': row.replace({np.nan: None}).to_dict()
    })


def get_ticket_close_event(row, common_id_key: str):
    commonId = row[common_id_key]

    return pd.Series({
        'commonUserId': commonId,
        'eventId': str(row['hsObjectId']) + '_CLOSED',
        'kind': categories.ticket.name,
        'eventType': categories.ticket.event_types['TICKET_CLOSED'],
        'timestamp': row['closed_data'],
        'properties': row.replace({np.nan: None}).to_dict()
    })


def handle_tickets(client: SignalBoxClient, df: pd.DataFrame):
    logging.info('Handling tickets DataFrame')
    common_user_id_key = 'common_user_id'
    df = df.loc[(df[common_user_id_key] != None) &
                (df[common_user_id_key] != "")]

    # dates are millisecond unix
    df['createdate'] = pd.to_datetime(
        pd.to_numeric(df['createdate']) / 1000, unit='s').dt.strftime(signalbox_timestamp_format)
    df['closed_data'] = pd.to_datetime(
        pd.to_numeric(df['closed_data']) / 1000, unit='s').dt.strftime(signalbox_timestamp_format)

    ticket_create_events = df.apply(lambda x: get_ticket_create_event(
        x, common_user_id_key), axis=1).dropna(subset=['commonUserId']).replace({np.nan: None})
    logging.info(f'Created {len(ticket_create_events)} ticket create events')

    ticket_close_events = df.apply(lambda x: get_ticket_close_event(x, common_user_id_key), axis=1).dropna(
        subset=['commonUserId', 'timestamp']).replace({np.nan: None})
    logging.info(f'Created {len(ticket_close_events)} ticket close events')

    create_events = []
    for index, row in ticket_create_events.iterrows():
        e = client.construct_event(row['commonUserId'], row['eventId'], row['eventType'],
                                   row['kind'], row['properties'], row['timestamp'], None)
        create_events.append(e)
    logging.info(f'Uploading {len(create_events)} create events')
    client.log_events(create_events)

    close_events = []
    for index, row in ticket_close_events.iterrows():
        e = client.construct_event(row['commonUserId'], row['eventId'], row['eventType'],
                                   row['kind'], row['properties'], row['timestamp'], None)
        close_events.append(e)
    logging.info(f'Uploading {len(close_events)} close events')
    client.log_events(close_events)

    logging.info('Completed tickets import.')
    return {
        'Processed': len(create_events) + len(close_events)
    }


hubspotIntegratedSystemId = 8


def handle_contacts(client: SignalBoxClient, df: pd.DataFrame):
    logging.info('Handling contacts dataframe')
    common_user_id_key = 'chargebeecustomerid'
    df = df.loc[(df[common_user_id_key] != None) &
                (df[common_user_id_key] != "")].drop_duplicates(subset=[common_user_id_key]).set_index(common_user_id_key)
    users = []
    for index, row in df.iterrows():
        users.append(client.construct_user(index, None, row.to_dict(),
                     hubspotIntegratedSystemId, row['hsObjectId']))
    logging.info(f'Constructed {len(users)} user objects. Uploading...')
    results = client.create_or_update_users(users)
    logging.info('Completed contacts import.')
    return {
        'Processed In': len(users),
        'Processed Out': len(results),
    }
