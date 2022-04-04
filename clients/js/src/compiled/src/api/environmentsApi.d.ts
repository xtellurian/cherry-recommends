export function fetchEnvironmentsAsync({ token, page }: {
    token: any;
    page: any;
}): Promise<any>;
export function createEnvironmentAsync({ token, environment }: {
    token: any;
    environment: any;
}): Promise<any>;
export function deleteEnvironmentAsync({ token, id }: {
    token: any;
    id: any;
}): Promise<any>;
export const setDefaultEnvironmentId: (e: number) => void;
