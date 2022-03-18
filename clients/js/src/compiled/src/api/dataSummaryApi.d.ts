export function fetchEventSummaryAsync({ token }: {
    token: any;
}): Promise<any>;
export function fetchEventKindNamesAsync({ token }: {
    token: any;
}): Promise<any>;
export function fetchEventKindSummaryAsync({ token, kind }: {
    token: any;
    kind: any;
}): Promise<any>;
export function fetchEventTimelineAsync({ token, kind, eventType }: {
    token: any;
    kind: any;
    eventType: any;
}): Promise<any>;
export function fetchDashboardAsync({ token, scope }: {
    token: any;
    scope: any;
}): Promise<any>;
export function fetchLatestActionsAsync({ token }: {
    token: any;
}): Promise<any>;
