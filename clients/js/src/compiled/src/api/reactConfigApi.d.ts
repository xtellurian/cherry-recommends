import { components } from "../model/api";
declare type Auth0ReactConfig = components["schemas"]["Auth0ReactConfig"] | undefined;
export declare const fetchAuth0ConfigurationAsync: () => Promise<Auth0ReactConfig>;
declare type ReactConfig = components["schemas"]["ReactConfig"] | undefined;
export declare const fetchConfigurationAsync: () => Promise<ReactConfig>;
export {};
