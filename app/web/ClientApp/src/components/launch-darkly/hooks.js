import { useLDClient } from "launchdarkly-react-client-sdk";

export const useFeatureFlag = (key, defaultValue) => {
  const ld = useLDClient();
  if (!ld) {
    console.warn("Launch Darkly flags not initialised");
  }
  const result = ld?.variation(key, defaultValue) ?? defaultValue;
  console.debug(`Feature Flag ${key} evaluated ${result}`);
  return result;
};
